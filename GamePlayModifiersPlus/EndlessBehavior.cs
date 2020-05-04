using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Reflection;
using System.IO;
using GamePlayModifiersPlus.Utilities;
using GamePlayModifiersPlus.TwitchStuff;
using UnityEngine.Networking;
using System.IO.Compression;
namespace GamePlayModifiersPlus
{
    internal class EndlessBehavior : MonoBehaviour
    {
        public static IPreviewBeatmapLevel[] LastLevelCollection = new IPreviewBeatmapLevel[0];

        private IPreviewBeatmapLevel[] levelCollection = null;
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private SongProgressUIController progessController;
        internal AudioClip nextSong;
        private BeatmapData nextBeatmap;
        private CustomPreviewBeatmapLevel nextSongInfo;
        private StandardLevelInfoSaveData.DifficultyBeatmap nextMapDiffInfo;
        private PauseMenuManager pauseManager;
        private float switchTime = float.MaxValue;

        private BeatmapObjectCallbackController callbackController;
        private BeatmapObjectSpawnMovementData originalSpawnMovementData;
        private NoteCutSoundEffectManager seManager;
        private BeatmapDataLoader dataLoader = new BeatmapDataLoader();

        private List<IPreviewBeatmapLevel> ToPlay;
        private bool _allow360 = false;
        private bool FoundValidSong = false;
        System.Random random = new System.Random();

        void Awake()
        {

            if (LastLevelCollection != null && LastLevelCollection.Length > 0 && Config.EndlessUseCurrentLevelCollection)
                levelCollection = LastLevelCollection;
            else
                levelCollection = SongCore.Loader.CustomLevels.Values.ToArray();
            StartCoroutine(Setup());
            _allow360 = Config.EndlessAllow360;

            ResetToPlay();
        }

        private void ResetToPlay()
        {
            FoundValidSong = false;
            ToPlay = levelCollection.ToList();
        }

        public void SongEnd()
        {
            switchTime = nextSong.length - 1f;
            SwitchToNextMap();
        }
        private IEnumerator Setup()
        {
            yield return new WaitForSeconds(0.1f);
            callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            originalSpawnMovementData = GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData");
            seManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().First();
            progessController = Resources.FindObjectsOfTypeAll<SongProgressUIController>().First();
            pauseManager = Resources.FindObjectsOfTypeAll<PauseMenuManager>().First();
            // switchTime = 20f;
            switchTime = GameObjects.songAudio.clip.length - 1f;
            Task.Run(PrepareNextSong);
        }

        void Update()
        {

            if (GameObjects.songAudio.time >= switchTime && nextSong != null)
            {
                switchTime = nextSong.length - 1f;
                SwitchToNextMap();
            }

        }

        private void SwitchToNextMap()
        {
            if (nextSong == null || nextBeatmap == null || nextMapDiffInfo == null) return;
            if (BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings.staticLights)
                nextBeatmap.SetProperty<BeatmapData>("beatmapEventData", new BeatmapEventData[0]);

            AudioClip oldClip = GameObjects.songAudio.clip;
            TwitchPowers.ResetTimeSync(nextSong, 0f, nextSongInfo.songTimeOffset, 1f);
            TwitchPowers.ManuallySetNJSOffset(GameObjects.spawnController, nextMapDiffInfo.noteJumpMovementSpeed,
                nextMapDiffInfo.noteJumpStartBeatOffset, nextSongInfo.beatsPerMinute);
            //    TwitchPowers.ClearCallbackItemDataList(callBackDataList);
            // DestroyNotes();
            TwitchPowers.DestroyObjectsRaw();
            TwitchPowers.ResetNoteCutSoundEffects(seManager);
            callbackController.SetField("_spawningStartTime", 0f);
            callbackController.SetNewBeatmapData(nextBeatmap);
            UpdatePauseMenu();
            ClearSoundEffects();
            //Destroying audio clip is actually bad idea
            //   IPA.Utilities.Async.UnityMainThreadTaskScheduler.Factory.StartNew(() => { oldClip.UnloadAudioData(); AudioClip.Destroy(oldClip); });
            Task.Run(PrepareNextSong);
            CheckIntroSkip();
            ResetProgressUI();
        }


        private async Task PrepareNextSong()
        {
            try
            {
                bool validSong = false;


                while (!validSong)
                {
                    await Task.Yield();
                    IPreviewBeatmapLevel previewLevel = null;
                    IPreviewBeatmapLevel requestLevel = null;

                    if (Config.EndlessPrioritizeSongRequests)
                        requestLevel = await GetSongRequestSong();
                    if (requestLevel != null)
                    {
                        previewLevel = requestLevel;
                    }
                    else
                    {
                        if (ToPlay.Count == 0)
                        {
                            if (!FoundValidSong) break;
                            else
                                ResetToPlay();
                        }

                        int nextSongIndex = random.Next(0, ToPlay.Count);

                        previewLevel = ToPlay[nextSongIndex];
                        ToPlay.RemoveAt(nextSongIndex);
                    }

                    validSong = IsValid(previewLevel, out nextMapDiffInfo);

                    if (validSong)
                    {
                        nextSongInfo = previewLevel as CustomPreviewBeatmapLevel;
                    }
                    // Plugin.Log("Removing song, new count: " + ToPlay.Count);
                }

                if (nextMapDiffInfo == null) return;

                FoundValidSong = true;

                await IPA.Utilities.Async.UnityMainThreadTaskScheduler.Factory.StartNew(
                    async () => nextSong = await nextSongInfo.GetPreviewAudioClipAsync(CancellationTokenSource.Token));

                //   bool loaded;
                //  await Task.Run(() => loaded = nextSong.LoadAudioData());

                string path = Path.Combine(nextSongInfo.customLevelPath, nextMapDiffInfo.beatmapFilename);
                string json = File.ReadAllText(path);
                nextBeatmap = dataLoader.GetBeatmapDataFromJson(json, nextSongInfo.beatsPerMinute, nextSongInfo.shuffle, nextSongInfo.shufflePeriod);
                Plugin.Log($"Next Song: {nextSongInfo.songName} - Mapped by {nextSongInfo.levelAuthorName}, is Ready");
            }
            catch (Exception ex)
            {
                Plugin.Log(ex.ToString());
            }
        }

        private bool IsValid(IPreviewBeatmapLevel testLevel, out StandardLevelInfoSaveData.DifficultyBeatmap nextDiff)
        {

            nextDiff = null;
            if (testLevel.levelID == nextSongInfo?.levelID) return false;
            if (!(testLevel is CustomPreviewBeatmapLevel)) return false;

            CustomPreviewBeatmapLevel level = testLevel as CustomPreviewBeatmapLevel;
            var extraData = SongCore.Collections.RetrieveExtraSongData(level.levelID, level.customLevelPath);
            if (extraData == null)
            {
                Plugin.Log("Null Extra Data");
                return false;
            }
            //  Add To Config
            BeatmapDifficulty preferredDiff = Config.EndlessPrefDifficulty;
            BeatmapDifficulty minDiff = Config.EndlessMinDifficulty;
            BeatmapDifficulty maxDiff = Config.EndlessMaxDifficulty;

            // Randomly Choose Characteristic?
            // BeatmapCharacteristicSO selectedCharacteristic = level.previewDifficultyBeatmapSets.First().beatmapCharacteristic;
            BeatmapCharacteristicSO selectedCharacteristic = level.previewDifficultyBeatmapSets[
                UnityEngine.Random.Range(0, level.previewDifficultyBeatmapSets.Length - 1)].beatmapCharacteristic;

            foreach (var set in level.standardLevelInfoSaveData.difficultyBeatmapSets)
            {
                if (set.beatmapCharacteristicName != selectedCharacteristic.serializedName) continue;

                foreach (var diff in set.difficultyBeatmaps)
                {
                    BeatmapDifficulty difficulty = (BeatmapDifficulty)Enum.Parse(typeof(BeatmapDifficulty), diff.difficulty);
                    if ((difficulty == preferredDiff))
                    {
                        if (ValidateDifficulty(diff, extraData, selectedCharacteristic, difficulty))
                        {
                            nextDiff = diff;
                            return true;
                        }
                    }

                }
                foreach (var diff in set.difficultyBeatmaps)
                {
                    BeatmapDifficulty difficulty = (BeatmapDifficulty)Enum.Parse(typeof(BeatmapDifficulty), diff.difficulty);
                    if ((int)difficulty <= (int)minDiff || (int)difficulty >= (int)maxDiff) continue;

                    if (ValidateDifficulty(diff, extraData, selectedCharacteristic, difficulty))
                    {
                        nextDiff = diff;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ValidateDifficulty(StandardLevelInfoSaveData.DifficultyBeatmap diffBeatmap, SongCore.Data.ExtraSongData data, BeatmapCharacteristicSO characteristic, BeatmapDifficulty beatmapDifficulty)
        {
            var diffData = data._difficulties.FirstOrDefault(x => x._beatmapCharacteristicName == characteristic.serializedName && x._difficulty == beatmapDifficulty);
            if (diffData == null) return false;

            var requirements = diffData.additionalDifficultyData._requirements;

            if (requirements.Length > 0)
            {
                foreach (string req in requirements)
                {
                    //Make sure requirement is actually present
                    if (!SongCore.Collections.capabilities.Contains(req))
                    {
                        Plugin.Log($"Missing difficulty requirement {req}");
                        return false;
                    }
                    switch (req)
                    {
                        case "MappingExtensions":
                            MappingExtensions.Plugin.ForceActivateForSong();
                            break;

                        //Don't allow difficulties with requirements that we aren't sure how to handle to be played
                        default:
                            Plugin.Log($"Unsure how to handle difficulty requirement {req}");
                            return false;
                    }
                }
            }

            //Add Option to allow 360 in endless mode that forces 360 UI and allows characteristics with rotation events
            if (characteristic.containsRotationEvents && !_allow360)
            {
                Plugin.Log("360 maps not enabled.");
                return false;
            }
            return true;
        }

        private void ClearSoundEffects()
        {
            seManager.GetField<NoteCutSoundEffect.Pool>("_noteCutSoundEffectPool").Clear();
        }

        private void ResetProgressUI()
        {
            progessController.Start();
            var cPlusCounter = GameObject.Find("Counters+ | Progress Counter");
            if (cPlusCounter != null)
            {
                ResetCountersPlusCounter(cPlusCounter);
            }

        }

        private void UpdatePauseMenu()
        {
            var currInitData = pauseManager.GetField<PauseMenuManager.InitData>("_initData");
            PauseMenuManager.InitData newData = new PauseMenuManager.InitData(currInitData.backButtonText, nextSongInfo.songName, nextSongInfo.songSubName, nextMapDiffInfo.difficulty);
            pauseManager.SetField("_initData", newData);
            pauseManager.Start();
        }
        private void ResetCountersPlusCounter(GameObject counter)
        {
            counter.GetComponent<CountersPlus.Counters.ProgressCounter>().SetField("length", nextSong.length);
        }

        private void CheckIntroSkip()
        {
            var skip = GameObject.Find("IntroSkip Behavior");
            if (skip != null)
                ResetIntroSkip(skip);
        }

        private void ResetIntroSkip(GameObject skip)
        {
            bool practice = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.practiceSettings != null;
            if (practice || BS_Utils.Gameplay.Gamemode.IsIsolatedLevel) return;

            var skipBehavior = skip.GetComponent<IntroSkip.SkipBehavior>();
            skipBehavior.ReInit();
        }





        private async Task<CustomPreviewBeatmapLevel> GetSongRequestSong()
        {
            if (!Plugin.songRequestPluginInstalled)
                return null;
            else
            {
                //      var result = await GetSongRequestSRM();
                //      return result;
                return null;
            }


        }
        /*
        private async Task<CustomPreviewBeatmapLevel> GetSongRequestSRM()
        {
            if (SongRequestManager.RequestQueue.Songs?.Count == 0)
                return null;
            try
            {
                var next = SongRequestManager.RequestQueue.Songs.First();
                //Need to wait for to update to do more
                SongRequestManager.RequestBot.DequeueRequest(next, false);
                //Check if song exists
                if (SongCore.Collections.songWithHashPresent(next.song["hash"]))
                {
                    var level = SongCore.Loader.CustomLevels.FirstOrDefault(x => SongCore.Utilities.Hashing.GetCustomLevelHash(x.Value).ToLower() == next.song["hash"].Value.ToLower()).Value;
                    return level;
                }
                else
                {
                    string path = string.Concat(new string[]
                    {
                        next.song["id"].Value,
                        " (",
                        next.song["songName"].Value,
                        " - ",
                        next.song["levelAuthor"].Value,
                        ")"
                    });
                    path = SongRequestManager.RequestBot.normalize.RemoveDirectorySymbols(ref path);
                    string currentSongDirectory = Path.Combine(Environment.CurrentDirectory, "Beat Saber_Data\\CustomLevels", path);
                    if (!Directory.Exists(currentSongDirectory))
                        Directory.CreateDirectory(currentSongDirectory);
                    else
                    {
                        try
                        {
                            var level2 = SongCore.Loader.LoadSong(SongCore.Loader.GetStandardLevelInfoSaveData(currentSongDirectory), currentSongDirectory, out var songhash1);
                            if(level2 != null)
                                return level2;
                        }
                        catch(Exception ex)
                        {
                            Plugin.Log("Failed to get song request from existing directory, attempting to download");
                        }
                    }
                    string downloadURL = "https://beatsaver.com" + next.song["downloadURL"].Value;
                    var request = await Plugin.client.GetAsync(downloadURL);
                    if(!request.IsSuccessStatusCode)
                    {
                        Plugin.Log($"Failed to get song request song. {request.StatusCode} | {request.ReasonPhrase}");
                        return null;
                    }

                    byte[] data = await request.Content.ReadAsByteArrayAsync();


                    Stream dataStream = new MemoryStream(data);
                    ZipArchive archive = new System.IO.Compression.ZipArchive(dataStream, System.IO.Compression.ZipArchiveMode.Read);
                    await Task.Run(() =>
                    {
                        archive.ExtractToDirectory(currentSongDirectory);
                        foreach (var entry in archive.Entries)
                        {
                            var entryPath = Path.Combine(currentSongDirectory, entry.Name); 
                                entry.ExtractToFile(entryPath, true);

                        }
                    }).ConfigureAwait(false);

                    var level = SongCore.Loader.LoadSong(SongCore.Loader.GetStandardLevelInfoSaveData(currentSongDirectory), currentSongDirectory, out var songhash2);
                    if (level != null)
                        return level;
                    else
                        return null;

                }
            }
            catch (Exception ex)
            {
                Plugin.Log($"Failed to get song request song. {ex}");
                return null;
            }
        }

        */
    }
}
