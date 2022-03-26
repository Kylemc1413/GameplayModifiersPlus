namespace GamePlayModifiersPlus.TwitchStuff
{
    using ChatCore;
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine.Networking;
    using GamePlayModifiersPlus.Utilities;
    using IPA.Utilities;
    using Zenject;
    public class TwitchPowers : MonoBehaviour
    {
        public static IEnumerator ChargeOverTime()
        {
            yield return new WaitForSeconds(Config.timeForCharges);
            GameModifiersController.charges += Config.chargesOverTime;
            if (GameModifiersController.charges > Config.maxCharges) GameModifiersController.charges = Config.maxCharges;
            Plugin.twitchPowers.StartCoroutine(ChargeOverTime());
        }


        public static IEnumerator TempDA(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " DA | ";
            var initData = GameObjects.beatmapObjectManager.GetField<BasicBeatmapObjectManager.InitData, BasicBeatmapObjectManager>("_initData");
            GameObjects.beatmapObjectManager.SetField("_initData", new BasicBeatmapObjectManager.InitData(true, false, initData.cutAngleTolerance, initData.notesUniformScale));
            ResetGameNoteStates(NoteVisualModifierType.DisappearingArrow);
            yield return new WaitForSeconds(length);
            GameObjects.beatmapObjectManager.SetField("_initData", new BasicBeatmapObjectManager.InitData(false, false, initData.cutAngleTolerance, initData.notesUniformScale));
            ResetGameNoteStates(NoteVisualModifierType.Normal);
            text.text = text.text.Replace(" DA | ", "");
        }
        public static void ResetGameNoteStates(NoteVisualModifierType state)
        {
            if (GameObjects.beatmapObjectManager == null) return;
            try
            {
                var notepool = GameObjects.beatmapObjectManager
                .GetField<MemoryPoolContainer<GameNoteController>, BasicBeatmapObjectManager>("_gameNotePoolContainer");

                MemoryPoolBase<GameNoteController> noteBaseMemoryPool = notepool
                    .GetField<IMemoryPool<GameNoteController>, MemoryPoolContainer<GameNoteController>>("_memoryPool")
                    as MemoryPoolBase<GameNoteController>;
                List<GameNoteController> notes = new List<GameNoteController>();
                foreach (var note in notepool.activeItems)
                {
                    notes.Add(note);
                }
                foreach (var inactiveNote in noteBaseMemoryPool.InactiveItems)
                {
                    notes.Add(inactiveNote);
                }
                foreach (var note in notes)
                {
                    note.SetField("_noteVisualModifierType", state);
                    ResetGameNoteDA(note, state);
                }
            }
            catch (System.Exception ex)
            {
                Plugin.log.Error("Error resetting note states: " + ex);
            }

        }

        public static void ResetGameNoteDA(GameNoteController note, NoteVisualModifierType state)
        {
            var daController = note.gameObject.GetComponent<DisappearingArrowControllerBase<GameNoteController>>();
            daController.InvokeMethod<System.Object, DisappearingArrowControllerBase<GameNoteController>>("OnDestroy");
            daController.InvokeMethod<System.Object, DisappearingArrowControllerBase<GameNoteController>>("Awake");
            daController.InvokeMethod<System.Object, DisappearingArrowControllerBase<GameNoteController>>("HandleGameNoteControllerDidInit", note);
        }
        public static IEnumerator CoolDown(float waitTime, string cooldown, string message)
        {

            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
            text.text += " " + cooldown + " | ";
            Plugin.cooldowns.SetCooldown(true, cooldown);
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (Config.showCooldownOnMessage)
                {
                    if (Config.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false)
                    {
                        ChatMessageHandler.TryAsyncMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds." + "Global Command Cooldown Active for " + Config.globalCommandCooldown + " seconds.");
                    }
                    else
                        ChatMessageHandler.TryAsyncMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds");
                }
                else
                    ChatMessageHandler.TryAsyncMessage(message);
            }



            yield return new WaitForSeconds(waitTime);
            Plugin.cooldowns.SetCooldown(false, cooldown);
            text.text = text.text.Replace(" " + cooldown + " | ", "");
        }

        public static IEnumerator GlobalCoolDown()
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;

            Plugin.cooldowns.SetCooldown(true, "Global");
            yield return new WaitForSeconds(Config.globalCommandCooldown);
            Plugin.cooldowns.SetCooldown(false, "Global");
            text.text = text.text.Replace(" " + "Global" + " | ", "");
        }

        public static IEnumerator TempInstaFail(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " InstaFail | ";
            Image energyBar = GameObjects.energyPanel.GetField<Image, GameEnergyUIPanel>("_energyBar");
            energyBar.color = Color.red;
            GameObjects.energyCounter.SetField("_badNoteEnergyDrain", 1f);
            GameObjects.energyCounter.SetField("_missNoteEnergyDrain", 1f);
            GameObjects.energyCounter.SetField("_hitBombEnergyDrain", 1f);
            GameObjects.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 1f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            GameObjects.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            GameObjects.energyCounter.SetField("_missNoteEnergyDrain", 0.15f);
            GameObjects.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            GameObjects.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
            text.text = text.text.Replace(" InstaFail | ", "");
        }

        public static IEnumerator TempInvincibility(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Invincible | ";
            Image energyBar = GameObjects.energyPanel.GetField<Image, GameEnergyUIPanel>("_energyBar");
            energyBar.color = Color.yellow;
            GameObjects.energyCounter.SetField("_badNoteEnergyDrain", 0f);
            GameObjects.energyCounter.SetField("_missNoteEnergyDrain", 0f);
            GameObjects.energyCounter.SetField("_hitBombEnergyDrain", 0f);
            GameObjects.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            GameObjects.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            GameObjects.energyCounter.SetField("_missNoteEnergyDrain", 0.15f);
            GameObjects.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            GameObjects.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
            text.text = text.text.Replace(" Invincible | ", "");
        }

        public static IEnumerator TempPoison(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Poison | ";
            Image energyBar = GameObjects.energyPanel.GetField<Image, GameEnergyUIPanel>("_energyBar");
            energyBar.color = Color.magenta;
            GameObjects.energyCounter.SetField("_goodNoteEnergyCharge", 0f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            GameObjects.energyCounter.SetField("_goodNoteEnergyCharge", 0.01f);

            text.text = text.text.Replace(" Poison | ", "");
        }




        public static IEnumerator TestingGround(float length)
        {
            yield return new WaitForSeconds(5f);
            Plugin.twitchPowers.StartCoroutine(TwitchPowers.MadScience(99f));
        }

        public static IEnumerator LeftRotation()
        {
            yield return new WaitForSeconds(0f);
            float eventTime = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            BeatmapEventData newEvent = new SpawnRotationBeatmapEventData(eventTime, SpawnRotationBeatmapEventData.SpawnRotationEventType.Early, -30);

            GameObjects.callbacksController.AddEventsToBeatmap(new List<BeatmapEventData> { newEvent });
        }

        public static IEnumerator RandomRotation(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " RandomRotation | ";
            float startTime = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float endTime = startTime + length;
            float marker = startTime;
            List<BeatmapEventData> data = new List<BeatmapEventData>();
            while (marker < endTime)
            {
                marker += 1f;
                int state = UnityEngine.Random.Range(0, 3);
                switch (state)
                {
                    case 0:
                        Plugin.Log($"Left insertion at {marker}");
                        BeatmapEventData leftEvent = new SpawnRotationBeatmapEventData(marker, SpawnRotationBeatmapEventData.SpawnRotationEventType.Early, -30);
                        data.Add(leftEvent);
                        break;
                    case 1:
                        Plugin.Log($"right insertion at {marker}");
                        BeatmapEventData rightEvent = new SpawnRotationBeatmapEventData(marker, SpawnRotationBeatmapEventData.SpawnRotationEventType.Early, 30);
                        data.Add(rightEvent);
                        break;
                    case 2:
                        Plugin.Log($"Skip at {marker}");
                        break;
                    default:
                        break;

                }
            }
            GameObjects.callbacksController.AddEventsToBeatmap(data);
            yield return new WaitForSeconds(length);
            text.text = text.text.Replace(" RandomRotation | ", "");
        }


        public static IEnumerator RightRotation()
        {
            yield return new WaitForSeconds(0f);
            float startTime = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            BeatmapEventData newEvent = new SpawnRotationBeatmapEventData(startTime, SpawnRotationBeatmapEventData.SpawnRotationEventType.Early, 30);

            GameObjects.callbacksController.AddEventsToBeatmap(new List<BeatmapEventData> { newEvent });
        }

        public static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }

        public static IEnumerator ScaleNotes(float scale, float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Smaller/Larger | ";
            GameModifiersController.altereddNoteScale = scale;
            yield return new WaitForSeconds(length);
            GameModifiersController.altereddNoteScale = 1f;
            text.text = text.text.Replace(" Smaller/Larger | ", "");
        }

        public static IEnumerator RandomNotes(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Random | ";
            GMPUI.randomSize = true;
            yield return new WaitForSeconds(length);
            GMPUI.randomSize = false;
            text.text = text.text.Replace(" Random | ", "");
        }

        public static IEnumerator NjsRandom(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " NJSRandom | ";
            GMPUI.njsRandom = true;
            Plugin.twitchPowers.StartCoroutine(RandomNjsOrOffset());
            yield return new WaitForSeconds(length);
            GMPUI.njsRandom = false;
            AdjustNjsOrOffset();
            text.text = text.text.Replace(" NJSRandom | ", "");
        }

        public static IEnumerator OffsetRandom(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Random Offset | ";
            GMPUI.offsetrandom = true;
            Plugin.twitchPowers.StartCoroutine(RandomNjsOrOffset());
            yield return new WaitForSeconds(length);
            GMPUI.offsetrandom = false;
            AdjustNjsOrOffset();
            text.text = text.text.Replace(" Random Offset | ", "");
        }

        public static IEnumerator RandomNjsOrOffset()
        {
            yield return new WaitForSeconds(0.33f);
            AdjustNjsOrOffset();
            if ((GMPUI.njsRandom || GMPUI.offsetrandom) && Plugin.isValidScene)
                Plugin.twitchPowers.StartCoroutine(RandomNjsOrOffset());

        }

        public static void AdjustNjsOrOffset()
        {
            if (GameObjects.spawnController == null) return;
            if (Plugin.levelData.GameplayCoreSceneSetupData == null) return;

            float njs = Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed;
            if (njs == 0)
                njs = BeatmapDifficultyMethods.NoteJumpMovementSpeed(Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.difficulty);
            if (GMPUI.njsRandom)
                njs = UnityEngine.Random.Range(Config.njsRandomMin, Config.njsRandomMax);
            if (GMPUI.reverse)
                njs *= -1;
            float noteJumpStartBeatOffset = Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset;
            if (GMPUI.offsetrandom)
                noteJumpStartBeatOffset += UnityEngine.Random.Range((float)Config.offsetrandomMin, (float)Config.offsetrandomMax);
            var spawnController = GameObjects.spawnController;
            var callbacksController = GameObjects.callbacksController;

            var spawnMovementData = spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData");
            var initData = spawnController.GetField<BeatmapObjectSpawnController.InitData, BeatmapObjectSpawnController>("_initData");

            var bpm = GameObjects.bpmController.currentBpm;
            var oldAheadTime = spawnMovementData.spawnAheadTime;
            var lastProcessedNode = callbacksController.GetLastNode(oldAheadTime);

            callbacksController.RemoveBeatmapCallback(spawnController.GetField<BeatmapDataCallbackWrapper, BeatmapObjectSpawnController>("_obstacleDataCallbackWrapper"));
            callbacksController.RemoveBeatmapCallback(spawnController.GetField<BeatmapDataCallbackWrapper, BeatmapObjectSpawnController>("_noteDataCallbackWrapper"));
            callbacksController.RemoveBeatmapCallback(spawnController.GetField<BeatmapDataCallbackWrapper, BeatmapObjectSpawnController>("_sliderDataCallbackWrapper"));
            callbacksController.RemoveBeatmapCallback(spawnController.GetField<BeatmapDataCallbackWrapper, BeatmapObjectSpawnController>("_spawnRotationCallbackWrapper"));
            initData.Update(njs, noteJumpStartBeatOffset);
            spawnController.SetField("_isInitialized", false);
            spawnController.Start();
            var newAheadTime = spawnMovementData.spawnAheadTime;
            if(lastProcessedNode != null)
                callbacksController.SetNewLastNodeForCallback(lastProcessedNode, newAheadTime);
        }

        public static IEnumerator Pause()
        {
            yield return new WaitForSeconds(0f);
            PauseController pauseManager = Resources.FindObjectsOfTypeAll<PauseController>().LastOrDefault();
            pauseManager?.Pause();
        }

        public static IEnumerator TempNoArrows(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " NoArrows | ";
            yield return new WaitForSeconds(0f);
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    note.SetNoteToAnyCutDirection();
                    note.TransformNoteAOrBToRandomType();
                    return note;
                }
                return x;

            }, start, end);
            yield return new WaitForSeconds(length + 2f);
            text.text = text.text.Replace(" NoArrows | ", "");
            //    dataModel.beatmapData = beatmapData;
        }

        public static IEnumerator RandomBombs(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Bombs | ";


            yield return new WaitForSeconds(0f);
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    try
                    {
                        //                        Plugin.Log("Attempting to Convert to Bomb");
                    
                        int randMax = (int)((1 / Config.bombsChance) * 100);
                        int randMin = 100;
                        int random = Random.Range(1, randMax);

                        //                Plugin.Log("Min: " + randMin + " Max: " + randMax + " Number: " + random);

                        if (random <= randMin || Config.bombsChance == 1)
                            note.SetProperty("gameplayType", NoteData.GameplayType.Bomb);
                    }
                    catch (System.Exception ex)
                    {
                        Plugin.Log(ex.ToString());
                    }
                    return note;
                }
                return x;

            }, start, end);

            yield return new WaitForSeconds(length + 2f);
            text.text = text.text.Replace(" Bombs | ", "");
        }

        public static IEnumerator SpeedChange(float length, float pitch)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Speed | ";

            float songspeedmul = Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;

            GameModifiersController.SetTimeScale(pitch);


            yield return new WaitForSeconds(length);


            GameModifiersController.SetTimeScale(songspeedmul);
            text.text = text.text.Replace(" Speed | ", "");
        }



        public static IEnumerator NoArrows()
        {
            yield return new WaitForSeconds(0f);
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    note.SetNoteToAnyCutDirection();
                    note.TransformNoteAOrBToRandomType();
                    return note;
                }
                return x;

            });
            //    dataModel.beatmapData = beatmapData;
        }
        public static AudioClip RealityClip = null;
        public static BeatmapData realityCheckData = null;
        public IEnumerator RealityCheck(float duration = 10f)
        {
            yield break;
            /*
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " RCTTS | ";
            if (realityCheckData == null)
            {
                BeatmapDataLoader dataLoader = new BeatmapDataLoader();
                string json = new System.IO.StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GamePlayModifiersPlus.Resources.RealityCheck.ExpertPlus.dat")).ReadToEnd();
                realityCheckData = dataLoader.GetBeatmapDataFromJson(json, 260f, 0f, 0.5f);
            }


            StartCoroutine(SwitchMap(realityCheckData, RealityClip, 260f, 0f, 17f, 0f, duration, Config.rcttsRandomizeStart));
            yield return new WaitForSeconds(duration);
            text.text = text.text.Replace(" RCTTS | ", "");
            */
        }

        public static AudioClip WorkoutClip = null;
        public static BeatmapData workoutData = null;
        public IEnumerator Workout(float duration = 30f)
        {
            yield break;
            /*
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Workout | ";
            if (workoutData == null)
            {
                BeatmapDataLoader dataLoader = new BeatmapDataLoader();
                string json = new System.IO.StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GamePlayModifiersPlus.Resources.Workout.ExpertPlus.dat")).ReadToEnd();
                workoutData = dataLoader.GetBeatmapDataFromJson(json, 200f, 0f, 0.5f);
            }


            StartCoroutine(SwitchMap(workoutData, WorkoutClip, 200f, 0f, 10f, 0f, duration));
            yield return new WaitForSeconds(duration);
            text.text = text.text.Replace(" Workout | ", "");
            */
        }

        public IEnumerator SwitchMap(BeatmapData newDataBase, AudioClip newAudio, float newBpm, float newTimeOffset, float newNjs, float newSpawnOffset, float duration, bool randomizeStartTime = true)
        {
            yield break;
            /*
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().Last();
            BeatmapObjectSpawnMovementData originalSpawnMovementData = GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData");
            BeatmapCallbackItemDataList callBackDataList = GameObjects.spawnController.GetField<BeatmapCallbackItemDataList, BeatmapObjectSpawnController>("_beatmapCallbackItemDataList");
            NoteCutSoundEffectManager seManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().LastOrDefault();
            float startTime = 0f;
            float startOffset = originalSpawnMovementData.spawnAheadTime + 0.1f;
            //Get initial Map data
            float originalTime = GameObjects.songAudio.time;
            float originalBPM = GameObjects.spawnController.currentBpm;
            float originalTimeOffset = GameObjects.AudioTimeSync.GetField<float, AudioTimeSyncController>("_songTimeOffset");
            float originalNJS = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed;
            float originalSpawnOffset = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset;
            AudioClip originalClip = GameObjects.songAudio.clip;
            BeatmapData originalData = callbackController.GetField<IReadonlyBeatmapData, BeatmapObjectCallbackController>("_beatmapData") as BeatmapData;
            try
            {


                //Switch To Reality Check

                BeatmapData newData = newDataBase.GetCopy();
                if (BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings.environmentEffectsFilterDefaultPreset == EnvironmentEffectsFilterPreset.NoEffects)
                    newData.SetField<BeatmapData, List<BeatmapEventData>>("_beatmapEventsData", new List<BeatmapEventData>());
                if (randomizeStartTime)
                {
                    startTime = UnityEngine.Random.Range(0f, (newAudio.length / 2f));
                    duration = Mathf.Min(newAudio.length - 1f - startTime, duration);
                    List<BeatmapObjectData>[] data3 = new List<BeatmapObjectData>[4];
                    for (int i = 0; i < newData.beatmapLinesData.Count; i++)
                    {
                        data3[i] = new List<BeatmapObjectData>();
                        data3[i].AddRange(newData.beatmapLinesData[i].beatmapObjectsData);
                        data3[i].RemoveAll(x => x.time <= startTime + startOffset);
                        var linedata = newData.GetField<BeatmapLineData[], BeatmapData>("_beatmapLinesData")[i];
                        linedata.SetField<BeatmapLineData, List<BeatmapObjectData>>("_beatmapObjectsData", data3[i]);

                    }
                }

                ResetTimeSync(newAudio, startTime, newTimeOffset, 1f);
                ManuallySetNJSOffset(GameObjects.spawnController, newNjs, newSpawnOffset, newBpm);
                ClearCallbackItemDataList(callBackDataList);
                // DestroyNotes();
                DestroyObjectsRaw();
                ResetNoteCutSoundEffects(seManager);
                callbackController.SetField("_spawningStartTime", 0f);
                callbackController.SetNewBeatmapData(newData);
            }
            catch (System.Exception ex)
            {
                Plugin.log.Error("Exception switching Map: " + ex);
            }
            yield return new WaitForSeconds(duration - 0.2f);
            //Restore Original Map
            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < originalData.beatmapLinesData.Count; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(originalData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time <= originalTime + startOffset);
                var linedata = originalData.GetField<BeatmapLineData[], BeatmapData>("_beatmapLinesData")[i];
                linedata.SetField<BeatmapLineData, List<BeatmapObjectData>>("_beatmapObjectsData", data2[i]);
            }
            ResetTimeSync(originalClip, originalTime, originalTimeOffset, BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul);
            ManuallySetNJSOffset(GameObjects.spawnController, originalNJS, originalSpawnOffset, originalBPM);
            ClearCallbackItemDataList(callBackDataList);
            //  DestroyNotes();
            DestroyObjectsRaw();
            ResetNoteCutSoundEffects(seManager);
            callbackController.SetNewBeatmapData(originalData);
            */
        }
        public static void ResetNoteCutSoundEffects(NoteCutSoundEffectManager manager)
        {
            manager.SetField("_prevNoteATime", -1f);
            manager.SetField("_prevNoteBTime", -1f);

        }
        /*
        public static void ClearCallbackItemDataList(BeatmapCallbackItemDataList list)
        {
            list.GetField<List<BeatmapObjectData>, BeatmapCallbackItemDataList>("_beatmapObjectDataList").Clear();
            list.GetField<List<BeatmapEventData>, BeatmapCallbackItemDataList>("_beatmapEventDataList").Clear();
            var noteLists = list.GetField<Dictionary<ColorType, List<NoteData>>, BeatmapCallbackItemDataList>("_notesByColorType").Values;
            foreach (var noteList in noteLists)
                noteList.Clear();
            list.GetField<List<ObstacleData>, BeatmapCallbackItemDataList>("_obstacles").Clear();
            list.GetField<List<NoteData>, BeatmapCallbackItemDataList>("_bombNotes").Clear();
            list.GetField<List<BeatmapEventData>, BeatmapCallbackItemDataList>("_beatmapEarlyEvents").Clear();
            list.GetField<List<BeatmapEventData>, BeatmapCallbackItemDataList>("_beatmapLateEvents").Clear();

        }
        */
        public static void ResetTimeSync(AudioClip clip, float time, float timeOffset, float timeScale)
        {
            AudioTimeSyncController.InitData initData =
                GameObjects.AudioTimeSync.GetField<AudioTimeSyncController.InitData, AudioTimeSyncController>("_initData");
            AudioTimeSyncController.InitData newInitData = new AudioTimeSyncController.InitData(clip,
                            time, timeOffset, 1f);
            GameObjects.AudioTimeSync.SetField("_initData", newInitData);
            GameObjects.AudioTimeSync.SetField("_startSongTime", time);
            GameObjects.AudioTimeSync.SetField("_songTimeOffset", timeOffset
                + GameObjects.AudioTimeSync.GetField<FloatSO, AudioTimeSyncController>("_audioLatency"));
            GameObjects.AudioTimeSync.SetField("_audioStarted", false);
            GameObjects.songAudio.clip = clip;
            GameObjects.AudioTimeSync.StartSong();
        }
        /*
        public static void ManuallySetNJSOffset(BeatmapObjectSpawnController _spawnController, float njs, float offset, float bpm)
        {
            BeatmapObjectSpawnMovementData spawnMovementData =
  _spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData");

            spawnMovementData.SetField("_startNoteJumpMovementSpeed", njs);
            spawnMovementData.SetField("_noteJumpStartBeatOffset", offset);

            spawnMovementData.Update(bpm,
                _spawnController.GetField<float, BeatmapObjectSpawnController>("_jumpOffsetY"));
        }
        */
        public static IEnumerator ExtraLanes()
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float beatTime = 60f / GameObjects.bpmController.currentBpm;
            float sliderTime = beatTime / 6f;
            float centerDistTime = beatTime / 2f;
            float claimedCenterTime = 0f;
            Plugin.Log("Grabbed BeatmapData");
            //        List<System.Tuple<NoteType, float>> noteTimes = new List<System.Tuple<NoteType, float>>();
            //        List<float> doubleTimes = new List<float>();
            Dictionary<System.Tuple<int, float, ColorType>, int> laneShifts = new Dictionary<System.Tuple<int, float, ColorType>, int>();
            GameObjects.callbacksController.ModifyBeatmap(beatmapObject =>
            {
                if (beatmapObject is NoteData note)
                {
                    int newIndex = -1413;
                    System.Tuple<int, float, ColorType> noteTuple = new System.Tuple<int, float, ColorType>(note.lineIndex, note.time, note.colorType);
                    var existingTuple = laneShifts.Keys.FirstOrDefault(x => Mathf.Abs(x.Item2 - note.time) <= sliderTime
                    && x.Item1 == noteTuple.Item1 && x.Item3 == noteTuple.Item3);

                    if (GMPUI.sixLanes)
                    {
                        if (laneShifts.ContainsKey(noteTuple))
                            newIndex = laneShifts[noteTuple];
                        else if (existingTuple != null)
                        {
                            newIndex = laneShifts[existingTuple];
                            laneShifts.Add(noteTuple, newIndex);
                        }
                        else
                        {
                            if (note.lineIndex == 0 && Random.Range(1, 4) >= 2)
                            {
                                newIndex = -1;
                                laneShifts.Add(noteTuple, newIndex);
                            }
                            if (note.lineIndex == 3 && Random.Range(1, 4) >= 2)
                            {
                                newIndex = 4;
                                laneShifts.Add(noteTuple, newIndex);
                            }
                        }
                    }
                    else if (GMPUI.fiveLanes)
                    {
                        if (laneShifts.ContainsKey(noteTuple))
                            newIndex = laneShifts[noteTuple];
                        else if (existingTuple != null)
                        {
                            newIndex = laneShifts[existingTuple];
                            laneShifts.Add(noteTuple, newIndex);
                        }
                        else
                            switch (note.lineIndex)
                            {

                                case 0:
                                    newIndex = -1500;
                                    break;

                                case 1:
                                    newIndex = UnityEngine.Random.Range(0, 10) <= 3 && (Mathf.Abs(claimedCenterTime - note.time) > centerDistTime) ? 2500 : 1500;
                                    laneShifts.Add(noteTuple, newIndex);
                                    break;

                                case 2:
                                    newIndex = UnityEngine.Random.Range(0, 10) <= 3 && (Mathf.Abs(claimedCenterTime - note.time) > centerDistTime) ? 2500 : 3500;
                                    laneShifts.Add(noteTuple, newIndex);
                                    break;

                                case 3:
                                    newIndex = 4500;
                                    break;

                                default:
                                    if (note.lineIndex < 0)
                                    {
                                        Plugin.log.Warn("Map is already Extra Lanes, not allowing 5 lanes");
                                        return note;
                                    }
                                    if (note.lineIndex > 3)
                                    {
                                        Plugin.log.Warn("Map is already Extra Lanes, not allowing 5 lanes");
                                        return note;
                                    }
                                    break;
                            }
                        if (newIndex == 2500)
                        {
                            claimedCenterTime = note.time;
                        }
                    }

                    if (GMPUI.fourLayers)
                    {
                        if (note.noteLineLayer == NoteLineLayer.Top && Random.Range(1, 4) > 2 && IsUpward(note.cutDirection))
                            note.SetProperty<NoteData, NoteLineLayer>("noteLineLayer", (NoteLineLayer)3);
                    }

                    if (GMPUI.angleShift && !((int)note.cutDirection >= 1000))
                    {
                        int angle = 1000;
                        switch (note.cutDirection)
                        {
                            case NoteCutDirection.Any:
                                angle = -1;
                                break;
                            case NoteCutDirection.Down:
                                angle = 1000;
                                break;
                            case NoteCutDirection.DownLeft:
                                angle = 1045;
                                break;
                            case NoteCutDirection.Left:
                                angle = 1090;
                                break;
                            case NoteCutDirection.UpLeft:
                                angle = 1135;
                                break;
                            case NoteCutDirection.Up:
                                angle = 1180;
                                break;
                            case NoteCutDirection.UpRight:
                                angle = 1225;
                                break;
                            case NoteCutDirection.Right:
                                angle = 1270;
                                break;
                            case NoteCutDirection.DownRight:
                                angle = 1315;
                                break;
                        }
                        if (angle >= 1000)
                        {
                            angle += Random.Range(-20, 20);
                            angle = Mathf.Clamp(angle, 1000, 1360);
                            note.SetProperty<NoteData, NoteCutDirection>("cutDirection", (NoteCutDirection)angle);
                        }
                    }

                    if (GMPUI.laneShift)
                    {
                        if (newIndex == -1413) newIndex = note.lineIndex;
                        if (!(newIndex >= 1000 || newIndex <= -1000))
                        {
                            int shiftedIndex = (newIndex * 1000) + 1000 + (UnityEngine.Random.Range(-5, 5) * 45);
                            if (newIndex < 0)
                                shiftedIndex = (newIndex * 1000) + 1000 - (UnityEngine.Random.Range(-5, 5) * 45);
                            if (newIndex == 0)
                                shiftedIndex = 1000 + (UnityEngine.Random.Range(-5, 5) * 45);
                            if (shiftedIndex < 1000 && shiftedIndex > -1000)
                                shiftedIndex -= 2000;
                            newIndex = shiftedIndex;
                        }
                        else if (newIndex >= 1000 && newIndex <= 4500)
                        {

                            int shiftedIndex = newIndex + (UnityEngine.Random.Range(-5, 5) * 45);
                            if (shiftedIndex < 1000 && shiftedIndex > -1000)
                                shiftedIndex -= 2000;
                        }
                    }

                    if (newIndex != -1413)
                    {
                        note.SetProperty<NoteData, int>("lineIndex", newIndex);
                        note.SetProperty<NoteData, int>("flipLineIndex", newIndex);
                    }

                    return note;
                }
                return beatmapObject;

            }, start);
        }

        public static bool IsUpward(NoteCutDirection direction)
        {
            return direction == NoteCutDirection.Up || direction == NoteCutDirection.UpLeft || direction == NoteCutDirection.UpRight;
        }

        public static IEnumerator Reverse(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Reverse | ";
            float wait = GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float start = GameObjects.songAudio.time + wait * 2f;
            float end = start + length;
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    note.SetProperty("colorType", note.colorType.Opposite());
                    return note;
                }
                return x;

            }, start, end);
            GMPUI.reverse = true;
            AdjustNjsOrOffset();
            GameModifiersController.reverseSound.Play();
            DestroyObjectsRaw();
            //   DestroyNotes();
            //Code to show some kinda prompt here maybe
            yield return new WaitForSeconds(length);
            GMPUI.reverse = false;
            AdjustNjsOrOffset();
            //Code to destroy notes here
            GameModifiersController.reverseSound.Play();
            //   DestroyNotes();
            DestroyObjectsRaw();
            text.text = text.text.Replace(" Reverse | ", "");

        }

        public static IEnumerator PermaReverse()
        {
            yield return new WaitForSecondsRealtime(1f);
            float start = 2f + GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;

            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    note.SetProperty("colorType", note.colorType.Opposite());
                    return note;
                }
                return x;

            }, start);

            yield return new WaitForSeconds(2f);
            AdjustNjsOrOffset();
            GameModifiersController.reverseSound.Play();
            DestroyObjectsRaw();
        }

        public static void MirrorSection(float length)
        {
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 1.5f;
            float end = start + length;
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                
                if (x is BeatmapObjectData beatmapObject)
                {
                    beatmapObject.Mirror(4);
                    return beatmapObject;
                }
                return x;

            }, start, end);
       }

        /*
        public static void StaticLights()
        {

            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().Last();
            BeatmapData beatmapData = callbackController.GetField<IReadonlyBeatmapData, BeatmapObjectCallbackController>("_beatmapData") as BeatmapData;
            Plugin.Log("Grabbed BeatmapData");
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;

            BeatmapEventData[] newData = new BeatmapEventData[2];
            newData[0] = new BeatmapEventData(start + .01f, BeatmapEventType.Event0, 1);
            newData[1] = new BeatmapEventData(start + .01f, BeatmapEventType.Event4, 1);

            beatmapData.SetField("_beatmapEventsData", newData.ToList());
        }
        */



        public static IEnumerator Funky(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Funky | ";
            GMPUI.funky = true;
            yield return new WaitForSeconds(length);
            GMPUI.funky = false;
            text.text = text.text.Replace(" Funky | ", "");
        }

        public static IEnumerator TempMirror(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Mirror | ";
            MirrorSection(length);
            yield return new WaitForSeconds(length);
            text.text = text.text.Replace(" Mirror | ", "");

        }

        public static IEnumerator Rainbow(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Rainbow | ";
            GMPUI.rainbow = true;
            yield return new WaitForSeconds(length);
            GMPUI.rainbow = false;
            ColorController.ResetColors();
            text.text = text.text.Replace(" Rainbow | ", "");
        }
        /*
        public static void DestroyNotes()
        {
            foreach (GameNoteController gameNoteController in UnityEngine.Object.FindObjectsOfType<GameNoteController>())
            {
                try
                {
                    SaberSwingRatingCounter counter = Plugin.player.leftSaber.CreateSwingRatingCounter(gameNoteController.noteTransform);
                    counter.SetField("_beforeCutRating", 1f);
                    counter.SetField("_afterCutRating", 1f);
                    gameNoteController.InvokeMethod("SendNoteWasCutEvent", (new NoteCutInfo(true, true, true, false, 999f, Vector3.down,
                        (gameNoteController.noteData.noteType == NoteType.NoteA) ? SaberType.SaberA : SaberType.SaberB, 1f, 0f, Vector3.down, new Vector3(0.0001f, 0.00001f, 0.00001f), counter, 0f)));
                }
                catch (System.Exception ex)
                {
                    Plugin.Log("Error Trying to Destroy notes: " + ex);
                }
            }
        }
        */
            public static void DestroyObjectsRaw()
        {
            var notes = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<GameNoteController>, BasicBeatmapObjectManager>("_basicGameNotePoolContainer");
            var sliderHeads = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<GameNoteController>, BasicBeatmapObjectManager>("_burstSliderHeadGameNotePoolContainer");
            var sliders = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<BurstSliderGameNoteController>, BasicBeatmapObjectManager>("_burstSliderGameNotePoolContainer");
            var sliderfills = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<BurstSliderGameNoteController>, BasicBeatmapObjectManager>("_burstSliderFillPoolContainer");
            var bombs = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<BombNoteController>, BasicBeatmapObjectManager>("_bombNotePoolContainer");
            var walls = GameObjects.beatmapObjectManager.GetField<MemoryPoolContainer<ObstacleController>, BasicBeatmapObjectManager>("_obstaclePoolContainer");
            foreach (var note in notes.activeItems)
            {
                if (note == null) continue;
                note.Dissolve(0f);
                //    _beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", note as NoteController);
            }
            foreach (var sliderHead in sliderHeads.activeItems)
            {
                if (sliderHead == null) continue;
                sliderHead.Dissolve(0f);
                //    _beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", note as NoteController);
            }
            foreach (var slider in sliders.activeItems)
            {
                if (slider == null) continue;
                slider.Dissolve(0f);
                //    _beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", note as NoteController);
            }
            foreach (var slider in sliderfills.activeItems)
            {
                if (slider == null) continue;
                slider.Dissolve(0f);
                //    _beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", note as NoteController);
            }
            foreach (var bomb in bombs.activeItems)
            {
                if (bomb == null) continue;
                bomb.Dissolve(0f);
                //    _beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", bomb as NoteController);
            }
            foreach (var wall in walls.activeItems)
            {
                if (wall == null) continue;
                wall.Dissolve(0f);
                //_beatmapObjectManager.InvokeMethod<BeatmapObjectManager>("Despawn", wall);
            }


        }
        public static void ResetPowers(bool resetMessage)
        {
            GMPUI.rainbow = false;
            GMPUI.funky = false;
            GMPUI.njsRandom = false;
            GMPUI.offsetrandom = false;
            GMPUI.randomSize = false;
            GMPUI.reverse = false;
            GameModifiersController.altereddNoteScale = 1f;
            //       Time.timeScale = 1;
            //        Plugin.timeScale = 1;
            GameModifiersController.superRandom = false;
            if (resetMessage)
            {
                if (GameObjects.beatmapObjectManager != null)
                {
                    var initData = GameObjects.beatmapObjectManager.GetField<BasicBeatmapObjectManager.InitData, BasicBeatmapObjectManager>("_initData");
                    GameObjects.beatmapObjectManager.SetField("_initData", new BasicBeatmapObjectManager.InitData(false, false, initData.cutAngleTolerance, initData.notesUniformScale));
                    ResetGameNoteStates(NoteVisualModifierType.Normal);
                }
                ColorController.ResetColors();
                if (Plugin.isValidScene)
                    AdjustNjsOrOffset();
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                text.text = " ";
                var text2 = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
                text2.text = "";
                GameModifiersController.SetTimeScale(GameModifiersController.currentSongSpeed);

                resetMessage = false;
            }

        }


        public static IEnumerator GameTime(float duration)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;

            GameSaber.GameType game = UnityEngine.Random.Range(0, 2) == 0 ? GameSaber.GameType.TicTacToe : GameSaber.GameType.ConnectFour;
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + duration + 1f;
            var gameController = GameObject.Find("GameSaber Controller")?.GetComponent<GameSaber.GameController>();
            if(gameController != null)
            {
                text.text += " GameTime | ";
                GameSaber.GameController.mapParams = new GameSaber.GameParams.DiffGameParams { gameType = game, gameStart = start, gameEndTime = end, gameTurnInterval = 3.0f };
                gameController.Initialize();
            }
            yield return new WaitForSeconds(duration + 1f);
         //   GameSaber.GameController.mapParams = null;
            text.text = text.text.Replace(" GameTime | ", "");

        }
        public static IEnumerator MadScience(float length)
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            float start = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            Plugin.Log("Start: " + start + " End: " + end);
            GameObjects.callbacksController.ModifyBeatmap(x =>
            {
                if (x is NoteData note)
                {
                    float duration = (GameObjects.bpmController.currentBpm / 60f) * (1f / 32f);
                    return new ObstacleData(note.time, note.lineIndex, note.noteLineLayer, duration, 1, 1);
                }
                return x;

            }, start, end);
            yield return new WaitForSeconds(length + 2f);
        }

        public static IEnumerator Encasement(float duration)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Tunnel | ";
            float startTime = GameObjects.songAudio.time + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.2f;
            float durationTime = duration;
            MappingExtensions.Plugin.ForceActivateForSong();
            List<BeatmapObjectData> objects = new List<BeatmapObjectData>();
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)0, duration, 0, 5));
            objects.Add(new ObstacleData(startTime, 4, (NoteLineLayer)0, duration, 0, 5));
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)0, duration, 5, 0));
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)6, duration, 5, 0));
            GameObjects.callbacksController.AddObjectsToBeatmap(objects);
            yield return new WaitForSeconds(duration);
            text.text = text.text.Replace(" Tunnel | ", "");

        }
        
        public static IEnumerator PermaEncasement(float start)
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            float startTime = start + GameObjects.spawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float duration = GameObjects.songAudio.clip.length;
            List<BeatmapObjectData> objects = new List<BeatmapObjectData>();
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)0, duration, 1001, 5));
            objects.Add(new ObstacleData(startTime, 4, (NoteLineLayer)0, duration, 1001, 5));
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)0, duration, 5, 0));
            objects.Add(new ObstacleData(startTime, -1, (NoteLineLayer)4, duration, 5, 0));
            GameObjects.callbacksController.AddObjectsToBeatmap(objects);



        }



    }
}
