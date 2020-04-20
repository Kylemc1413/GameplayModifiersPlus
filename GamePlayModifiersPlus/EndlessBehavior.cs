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
namespace GamePlayModifiersPlus
{
    internal class EndlessBehavior : MonoBehaviour
    {
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private SongProgressUIController progessController;
        private AudioClip nextSong;
        private BeatmapData nextBeatmap;
        private CustomPreviewBeatmapLevel nextSongInfo;
        private StandardLevelInfoSaveData.DifficultyBeatmap nextMapDiffInfo;
        private float switchTime = float.MaxValue;

        private BeatmapObjectCallbackController callbackController;
        private BeatmapObjectSpawnMovementData originalSpawnMovementData;
        private NoteCutSoundEffectManager seManager;
        private BeatmapDataLoader dataLoader = new BeatmapDataLoader();

        void Awake()
        {
            StartCoroutine(Setup());
        }
        private IEnumerator Setup()
        {
            yield return new WaitForSeconds(0.1f);
            callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            originalSpawnMovementData = Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData");
            seManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().First();
            progessController = Resources.FindObjectsOfTypeAll<SongProgressUIController>().First();
            // switchTime = 20f;
            switchTime = Plugin.songAudio.clip.length - 1f;
            Task.Run(PrepareNextSong);
        }
        void Update()
        {
            if (Plugin.songAudio.time >= switchTime)
            {
                //   switchTime = 20f;
                switchTime = nextSong.length - 1f;
                SwitchToNextMap();
            }
        }

        private void SwitchToNextMap()
        {
            if (BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings.staticLights)
                nextBeatmap.SetProperty<BeatmapData>("beatmapEventData", new BeatmapEventData[0]);

            TwitchPowers.ResetTimeSync(nextSong, 0f, nextSongInfo.songTimeOffset, 1f);
            TwitchPowers.ManuallySetNJSOffset(Plugin.spawnController, nextMapDiffInfo.noteJumpMovementSpeed,
                nextMapDiffInfo.noteJumpStartBeatOffset, nextSongInfo.beatsPerMinute);
            //    TwitchPowers.ClearCallbackItemDataList(callBackDataList);
            // DestroyNotes();
            TwitchPowers.DestroyObjectsRaw();
            TwitchPowers.ResetNoteCutSoundEffects(seManager);
            callbackController.SetField("_spawningStartTime", 0f);
            callbackController.SetNewBeatmapData(nextBeatmap);
            ResetProgressUI();
            Task.Run(PrepareNextSong);
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

        private void ResetCountersPlusCounter(GameObject counter)
        {
            counter.GetComponent<CountersPlus.Counters.ProgressCounter>().SetField("length", nextSong.length);
        }
        private async Task PrepareNextSong()
        {
            bool validSong = false;

            while(!validSong)
            {
                await Task.Yield();
                int nextSongIndex = UnityEngine.Random.Range(0, SongCore.Loader.CustomLevels.Count);
                nextSongInfo = SongCore.Loader.CustomLevels.ElementAt(nextSongIndex).Value;
                validSong = IsValid(nextSongInfo);
            }

            nextMapDiffInfo = nextSongInfo.standardLevelInfoSaveData.difficultyBeatmapSets[0].difficultyBeatmaps.Last();
            nextSong = await nextSongInfo.GetPreviewAudioClipAsync(CancellationTokenSource.Token);
            string path = Path.Combine(nextSongInfo.customLevelPath, nextMapDiffInfo.beatmapFilename);
            string json = File.ReadAllText(path);
            nextBeatmap = dataLoader.GetBeatmapDataFromJson(json, nextSongInfo.beatsPerMinute, nextSongInfo.shuffle, nextSongInfo.shufflePeriod);
        }

        private bool IsValid(CustomPreviewBeatmapLevel level)
        {
            bool result = true;
            var extraData = SongCore.Collections.RetrieveExtraSongData(level.levelID);
            if (extraData == null)
                result = false;
            List<string> requirements = new List<string>();

            foreach (var diff in extraData._difficulties)
                requirements.AddRange(diff.additionalDifficultyData._requirements);

            if (requirements.Any(x => !SongCore.Collections.capabilities.Contains(x)))
                result = false;

            if (level.previewDifficultyBeatmapSets.Any(x => x.beatmapCharacteristic.containsRotationEvents))
                result = false;

            return result;
        }


    }
}
