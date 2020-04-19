namespace GamePlayModifiersPlus
{
    using StreamCore;
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine.Networking;
    public class TwitchPowers : MonoBehaviour
    {
        public static IEnumerator ChargeOverTime()
        {
            yield return new WaitForSeconds(ChatConfig.timeForCharges);
            Plugin.charges += ChatConfig.chargesOverTime;
            if (Plugin.charges > ChatConfig.maxCharges) Plugin.charges = ChatConfig.maxCharges;
            Plugin.twitchPowers.StartCoroutine(ChargeOverTime());
        }


        public static IEnumerator TempDA(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " DA | ";
            Plugin.spawnController.SetField("_disappearingArrows", true);
            yield return new WaitForSeconds(length);
            Plugin.spawnController.SetField("_disappearingArrows", false);
            text.text = text.text.Replace(" DA | ", "");
        }

        public static IEnumerator CoolDown(float waitTime, string cooldown, string message)
        {

            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
            text.text += " " + cooldown + " | ";
            Plugin.cooldowns.SetCooldown(true, cooldown);
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (ChatConfig.showCooldownOnMessage)
                {
                    if (ChatConfig.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false)
                    {
                        Plugin.TryAsyncMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds." + "Global Command Cooldown Active for " + ChatConfig.globalCommandCooldown + " seconds.");
                    }
                    else
                        Plugin.TryAsyncMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds");
                }
                else
                    Plugin.TryAsyncMessage(message);
            }



            yield return new WaitForSeconds(waitTime);
            Plugin.cooldowns.SetCooldown(false, cooldown);
            text.text = text.text.Replace(" " + cooldown + " | ", "");
        }

        public static IEnumerator GlobalCoolDown()
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;

            Plugin.cooldowns.SetCooldown(true, "Global");
            yield return new WaitForSeconds(ChatConfig.globalCommandCooldown);
            Plugin.cooldowns.SetCooldown(false, "Global");
            text.text = text.text.Replace(" " + "Global" + " | ", "");
        }

        public static IEnumerator TempInstaFail(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " InstaFail | ";
            Image energyBar = Plugin.energyPanel.GetField<Image>("_energyBar");
            energyBar.color = Color.red;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 1f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
            text.text = text.text.Replace(" InstaFail | ", "");
        }

        public static IEnumerator TempInvincibility(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Invincible | ";
            Image energyBar = Plugin.energyPanel.GetField<Image>("_energyBar");
            energyBar.color = Color.yellow;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
            text.text = text.text.Replace(" Invincible | ", "");
        }

        public static IEnumerator TempPoison(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Poison | ";
            Image energyBar = Plugin.energyPanel.GetField<Image>("_energyBar");
            energyBar.color = Color.magenta;
            Plugin.energyCounter.SetField("_goodNoteEnergyCharge", 0f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            Plugin.energyCounter.SetField("_goodNoteEnergyCharge", 0.01f);

            text.text = text.text.Replace(" Poison | ", "");
        }




        public static IEnumerator TestingGround(float length)
        {
            yield return new WaitForSecondsRealtime(10f);
            SharedCoroutineStarter.instance.StartCoroutine(Plugin.twitchPowers.Workout());
        }

        public static IEnumerator LeftRotation()
        {
            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapCallbackItemDataList callBackDataList = Plugin.spawnController.GetField<BeatmapCallbackItemDataList>("_beatmapCallbackItemDataList");

            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            float eventTime = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            //    List<BeatmapEventData> data = beatmapData.beatmapEventData.ToList();
            //14 Early Rotation 15 Late Rotation
            /*
                Rotation Values
	        	-60f,
	        	-45f,
	        	-30f,
	        	-15f,
	        	15f,
	        	30f,
	        	45f,
	            60f
            */
            BeatmapEventData newEvent = new BeatmapEventData(eventTime, BeatmapEventType.Event14, 2);

            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < beatmapData.beatmapLinesData.Length; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(beatmapData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time < eventTime);
                beatmapData.beatmapLinesData[i].beatmapObjectsData = data2[i].ToArray();
            }
            beatmapData.SetProperty<BeatmapData>("beatmapEventData", beatmapData.beatmapEventData.Append(newEvent).OrderBy(o => o.time).ToArray());
            callbackController.SetNewBeatmapData(beatmapData);
        }

        public static IEnumerator RandomRotation(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " RandomRotation | ";
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().FirstOrDefault();
            BeatmapCallbackItemDataList callBackDataList = Plugin.spawnController.GetField<BeatmapCallbackItemDataList>("_beatmapCallbackItemDataList");
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            float startTime = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
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
                        BeatmapEventData leftEvent = new BeatmapEventData(marker, BeatmapEventType.Event14, 2);
                        data.Add(leftEvent);
                        break;
                    case 1:
                        Plugin.Log($"right insertion at {marker}");
                        BeatmapEventData rightEvent = new BeatmapEventData(marker, BeatmapEventType.Event14, 5);
                        data.Add(rightEvent);
                        break;
                    case 2:
                        Plugin.Log($"Skip at {marker}");
                        break;
                    default:
                        break;

                }
            }
            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < beatmapData.beatmapLinesData.Length; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(beatmapData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time < startTime);
                beatmapData.beatmapLinesData[i].beatmapObjectsData = data2[i].ToArray();
            }

            beatmapData.SetProperty<BeatmapData>("beatmapEventData", beatmapData.beatmapEventData.Concat(data).OrderBy(o => o.time).ToArray());
            callbackController.SetNewBeatmapData(beatmapData);
            yield return new WaitForSeconds(length);
            //    List<BeatmapEventData> data = beatmapData.beatmapEventData.ToList();
            //14 Early Rotation 15 Late Rotation
            /*
                Rotation Values
	        	-60f,
	        	-45f,
	        	-30f,
	        	-15f,
	        	15f,
	        	30f,
	        	45f,
	            60f

            */
            text.text = text.text.Replace(" RandomRotation | ", "");
        }


        public static IEnumerator RightRotation()
        {
            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapCallbackItemDataList callBackDataList = Plugin.spawnController.GetField<BeatmapCallbackItemDataList>("_beatmapCallbackItemDataList");

            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            float startTime = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            BeatmapEventData rightEvent = new BeatmapEventData(startTime, BeatmapEventType.Event14, 5);

            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < beatmapData.beatmapLinesData.Length; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(beatmapData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time < startTime);
                beatmapData.beatmapLinesData[i].beatmapObjectsData = data2[i].ToArray();
            }
            beatmapData.SetProperty<BeatmapData>("beatmapEventData", beatmapData.beatmapEventData.Append(rightEvent).OrderBy(o => o.time).ToArray());
            callbackController.SetNewBeatmapData(beatmapData);
        }

        public static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }

        public static IEnumerator ScaleNotes(float scale, float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Smaller/Larger | ";
            Plugin.altereddNoteScale = scale;
            yield return new WaitForSeconds(length);
            Plugin.altereddNoteScale = 1f;
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
            if (Plugin.spawnController == null) return;
            BeatmapObjectSpawnMovementData spawnMovementData =
            Plugin.spawnController.GetPrivateField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData");
            if (Plugin.levelData.GameplayCoreSceneSetupData == null) return;
            if (spawnMovementData == null) return;
            if (!Plugin.haveSongNJS)
            {
                Plugin.songNJS = spawnMovementData.GetField<float>("_startNoteJumpMovementSpeed");
                Plugin.haveSongNJS = true;
            }
            float njs = Plugin.songNJS;
            if (GMPUI.njsRandom)
                njs = UnityEngine.Random.Range(ChatConfig.njsRandomMin, ChatConfig.njsRandomMax);
            if (GMPUI.reverse)
                njs *= -1;
            float noteJumpStartBeatOffset = Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset;
            if (GMPUI.offsetrandom)
                noteJumpStartBeatOffset += UnityEngine.Random.Range((float)ChatConfig.offsetrandomMin, (float)ChatConfig.offsetrandomMax);


            float bpm = Plugin.spawnController.GetPrivateField<VariableBPMProcessor>("_variableBPMProcessor").currentBPM;



            spawnMovementData.SetPrivateField("_startNoteJumpMovementSpeed", njs);
            spawnMovementData.SetPrivateField("_noteJumpStartBeatOffset", noteJumpStartBeatOffset);

            spawnMovementData.Update(bpm,
                Plugin.spawnController.GetPrivateField<float>("_jumpOffsetY"));
            //       Plugin.Log(njs.ToString());
            //       Plugin.Log(noteJumpStartBeatOffset.ToString());
        }

        public static IEnumerator Pause()
        {
            yield return new WaitForSeconds(0f);
            PauseController pauseManager = Resources.FindObjectsOfTypeAll<PauseController>().First();
            pauseManager.HandlePauseTriggered();
        }

        public static IEnumerator TempNoArrows(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " NoArrows | ";
            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            note = beatmapObject as NoteData;

                            note.SetNoteToAnyCutDirection();
                            note.TransformNoteAOrBToRandomType();

                        }

                }
            }
            yield return new WaitForSeconds(length + 2f);
            text.text = text.text.Replace(" NoArrows | ", "");
            //    dataModel.beatmapData = beatmapData;
        }

        public static IEnumerator RandomBombs(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Bombs | ";


            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            try
                            {
                                //                        Plugin.Log("Attempting to Convert to Bomb");
                                note = beatmapObject as NoteData;

                                int randMax = (int)((1 / ChatConfig.bombsChance) * 100);
                                int randMin = 100;
                                int random = Random.Range(1, randMax);

                                //                Plugin.Log("Min: " + randMin + " Max: " + randMax + " Number: " + random);

                                if (random <= randMin || ChatConfig.bombsChance == 1)
                                    note.SetProperty<NoteData>("noteType", NoteType.Bomb);
                            }
                            catch (System.Exception ex)
                            {
                                Plugin.Log(ex.ToString());
                            }


                        }

                }
            }
            yield return new WaitForSeconds(length + 2f);

            text.text = text.text.Replace(" Bombs | ", "");
            callbackController.SetField("_beatmapData", beatmapData);



        }

        public static IEnumerator SpeedChange(float length, float pitch)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Speed | ";

            float songspeedmul = Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;

            Plugin.SetTimeScale(pitch);


            yield return new WaitForSeconds(length);


            Plugin.SetTimeScale(songspeedmul);
            text.text = text.text.Replace(" Speed | ", "");
        }



        public static IEnumerator NoArrows()
        {

            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        note = beatmapObject as NoteData;

                        note.SetNoteToAnyCutDirection();
                    }





                }
            }
            //    dataModel.beatmapData = beatmapData;
        }
        public static AudioClip RealityClip = null;
        public static BeatmapData realityCheckData = null;
        public IEnumerator RealityCheck(float duration = 10f)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " RCTTS | ";
            if (realityCheckData == null)
            {
                BeatmapDataLoader dataLoader = new BeatmapDataLoader();
                string json = new System.IO.StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GamePlayModifiersPlus.Resources.RealityCheck.ExpertPlus.dat")).ReadToEnd();
                realityCheckData = dataLoader.GetBeatmapDataFromJson(json, 260f, 0f, 0.5f);
            }


            StartCoroutine(SwitchMap(realityCheckData, RealityClip, 260f, 0f, 17f, 0f, duration));
            yield return new WaitForSeconds(duration);
            text.text = text.text.Replace(" RCTTS | ", "");
        }

        public static AudioClip WorkoutClip = null;
        public static BeatmapData workoutData = null;
        public IEnumerator Workout(float duration = 30f)
        {
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
        }
        public IEnumerator SwitchMap(BeatmapData newDataBase, AudioClip newAudio, float newBpm, float newTimeOffset, float newNjs, float newSpawnOffset, float duration)
        {
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapObjectSpawnMovementData originalSpawnMovementData = Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData");
            BeatmapCallbackItemDataList callBackDataList = Plugin.spawnController.GetField<BeatmapCallbackItemDataList>("_beatmapCallbackItemDataList");

            float startOffset = originalSpawnMovementData.spawnAheadTime + 0.1f;
            //Get initial Map data
            float originalTime = Plugin.songAudio.time;
            float originalBPM = Plugin.spawnController.currentBPM;
            float originalTimeOffset = Plugin.AudioTimeSync.GetField<float>("_songTimeOffset");
            float originalNJS = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed;
            float originalSpawnOffset = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset;
            AudioClip originalClip = Plugin.songAudio.clip;
            BeatmapData originalData = callbackController.GetField<BeatmapData>("_beatmapData");

            //Switch To Reality Check

            BeatmapData newData = newDataBase.GetCopy();
            if (BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings.staticLights)
                newData.SetProperty<BeatmapData>("beatmapEventData", new BeatmapEventData[0]);

            ResetTimeSync(newAudio, 0f, newTimeOffset, 1f);
            ManuallySetNJSOffset(Plugin.spawnController, newNjs, newSpawnOffset, newBpm);
            ClearCallbackItemDataList(callBackDataList);
            // DestroyNotes();
            DestroyObjectsRaw();
            callbackController.SetField("_spawningStartTime", 0f);
            callbackController.SetNewBeatmapData(newData);
            yield return new WaitForSeconds(duration - 0.2f);
            //Restore Original Map
            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < originalData.beatmapLinesData.Length; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(originalData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time <= originalTime + startOffset);
                originalData.beatmapLinesData[i].beatmapObjectsData = data2[i].ToArray();
            }
            ResetTimeSync(originalClip, originalTime, originalTimeOffset, BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul);
            ManuallySetNJSOffset(Plugin.spawnController, originalNJS, originalSpawnOffset, originalBPM);
            ClearCallbackItemDataList(callBackDataList);
            //  DestroyNotes();
            DestroyObjectsRaw();
            callbackController.SetNewBeatmapData(originalData);
        }
        public static void ClearCallbackItemDataList(BeatmapCallbackItemDataList list)
        {
            list.GetField<List<BeatmapObjectData>>("_beatmapObjectDataList").Clear();
            list.GetField<List<BeatmapEventData>>("_beatmapEventDataList").Clear();
            var noteLists = list.GetField<Dictionary<NoteType, List<NoteData>>>("_notesByType").Values;
            foreach (var noteList in noteLists)
                noteList.Clear();
            list.GetField<List<ObstacleData>>("_obstacles").Clear();
            list.GetField<List<BeatmapEventData>>("_beatmapEarlyEvents").Clear();
            list.GetField<List<BeatmapEventData>>("_beatmapLateEvents").Clear();

        }
        public static void ResetTimeSync(AudioClip clip, float time, float timeOffset, float timeScale)
        {
            AudioTimeSyncController.InitData initData = Plugin.AudioTimeSync.GetPrivateField<AudioTimeSyncController.InitData>("_initData");
            AudioTimeSyncController.InitData newInitData = new AudioTimeSyncController.InitData(clip,
                            time, timeOffset, 1f);
            Plugin.AudioTimeSync.SetPrivateField("_initData", newInitData);
            Plugin.AudioTimeSync.StartSong();
        }

        public static void ManuallySetNJSOffset(BeatmapObjectSpawnController _spawnController, float njs, float offset, float bpm)
        {
            BeatmapObjectSpawnMovementData spawnMovementData =
  _spawnController.GetPrivateField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData");

            spawnMovementData.SetPrivateField("_startNoteJumpMovementSpeed", njs);
            spawnMovementData.SetPrivateField("_noteJumpStartBeatOffset", offset);

            spawnMovementData.Update(bpm,
                _spawnController.GetPrivateField<float>("_jumpOffsetY"));
        }

        public static IEnumerator ExtraLanes()
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            float beatTime = 60f / Plugin.spawnController.currentBPM;
            float sliderTime = beatTime / 6f;
            float centerDistTime = beatTime / 2f;
            float claimedCenterTime = 0f;
            Plugin.Log("Grabbed BeatmapData");
            //        List<System.Tuple<NoteType, float>> noteTimes = new List<System.Tuple<NoteType, float>>();
            //        List<float> doubleTimes = new List<float>();
            Dictionary<System.Tuple<int, float, NoteType>, int> laneShifts = new Dictionary<System.Tuple<int, float, NoteType>, int>();

            List<BeatmapObjectData> objects = new List<BeatmapObjectData>();
            NoteData note;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects.AddRange(line.beatmapObjectsData);

            }
            objects.RemoveAll(x => x.time < start);
            objects = objects.OrderBy(x => x.time).ToList();
            foreach (BeatmapObjectData beatmapObject in objects)
            {
                if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                {
                    int newIndex = -1413;
                    note = beatmapObject as NoteData;
                    System.Tuple<int, float, NoteType> noteTuple = new System.Tuple<int, float, NoteType>(note.lineIndex, note.time, note.noteType);
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
                                        newIndex = -1500;
                                    }
                                    if (note.lineIndex > 3)
                                    {
                                        newIndex = 4500;
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
                            note.SetProperty<NoteData>("noteLineLayer", (NoteLineLayer)3);
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
                            note.SetProperty<NoteData>("cutDirection", angle);
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
                        note.SetProperty<NoteData>("lineIndex", newIndex);
                        note.SetProperty<NoteData>("flipLineIndex", newIndex);
                    }


                }
            }
        }

        public static bool IsUpward(NoteCutDirection direction)
        {
            return direction == NoteCutDirection.Up || direction == NoteCutDirection.UpLeft || direction == NoteCutDirection.UpRight;
        }

        public static IEnumerator Reverse(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Reverse | ";
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            float wait = Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float start = Plugin.songAudio.time + wait * 2f;
            float end = start + length;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            note = beatmapObject as NoteData;

                            note.SwitchNoteType();
                            //                           note.MirrorTransformCutDirection();
                            //                           note.MirrorLineIndex(4);
                        }
                }
            }
            List<BeatmapObjectData>[] data2 = new List<BeatmapObjectData>[4];
            for (int i = 0; i < beatmapData.beatmapLinesData.Length; i++)
            {
                data2[i] = new List<BeatmapObjectData>();
                data2[i].AddRange(beatmapData.beatmapLinesData[i].beatmapObjectsData);
                data2[i].RemoveAll(x => x.time < start || (x.time > end && x.time < end + wait));
                beatmapData.beatmapLinesData[i].beatmapObjectsData = data2[i].ToArray();
            }
            callbackController.SetNewBeatmapData(beatmapData);
            GMPUI.reverse = true;
            AdjustNjsOrOffset();
            Plugin.reverseSound.Play();
            DestroyObjectsRaw();
            //   DestroyNotes();
            //Code to show some kinda prompt here maybe
            yield return new WaitForSeconds(length);
            GMPUI.reverse = false;
            AdjustNjsOrOffset();
            //Code to destroy notes here
            Plugin.reverseSound.Play();
            //   DestroyNotes();
            DestroyObjectsRaw();
            text.text = text.text.Replace(" Reverse | ", "");

        }

        public static IEnumerator PermaReverse()
        {
            yield return new WaitForSecondsRealtime(1f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;

            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start)
                        {
                            note = beatmapObject as NoteData;

                            note.SwitchNoteType();
                            //                           note.MirrorTransformCutDirection();
                            //                           note.MirrorLineIndex(4);
                        }
                }
            }

            yield return new WaitForSeconds(2f);
            AdjustNjsOrOffset();
            Plugin.reverseSound.Play();
            DestroyNotes();


        }

        public static void MirrorSection(float length)
        {

            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            ObstacleData obstacle;
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 1.5f;
            float end = start + length;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            note = beatmapObject as NoteData;
                            note.SwitchNoteType();
                            if (note.noteType != NoteType.Bomb)
                                note.MirrorTransformCutDirection();
                            note.MirrorLineIndex(4);
                        }
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Obstacle)
                    {
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            obstacle = beatmapObject as ObstacleData;
                            obstacle.MirrorLineIndex(4);
                        }

                    }
                }
            }
        }


        public static void StaticLights()
        {

            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            float start = Plugin.songAudio.time;
            int? nextIndex = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>()?.FirstOrDefault()?.GetField<int>("_nextEventIndex");
            BeatmapEventData[] newData = new BeatmapEventData[beatmapData.beatmapEventData.Length];
            if (nextIndex.HasValue)
            {
                newData[nextIndex.Value] = new BeatmapEventData(start + .01f, BeatmapEventType.Event0, 1);
                newData[nextIndex.Value + 1] = new BeatmapEventData(start + .01f, BeatmapEventType.Event4, 1);
            }
            beatmapData.SetProperty("beatmapEventData", newData);
        }




        public static IEnumerator OneColor()
        {
            yield return new WaitForSeconds(0f);
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            BeatmapObjectData[] objects;
            NoteData note;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        note = beatmapObject as NoteData;
                        note.SetProperty<NoteData>("noteType", NoteType.NoteB);
                    }
                }
            }
            //Adjust Sabers for one color
            yield return new WaitForSeconds(0.2f);
            var leftSaberType = Plugin.player.leftSaber.GetField<SaberTypeObject>("_saberType");

            try
            {
                leftSaberType.SetField("_saberType", SaberType.SaberB);
            }
            catch (System.Exception ex)
            {
                Plugin.Log(ex.ToString());
            }

            //     Plugin.Log("2 " + Plugin.player.leftSaber.saberType.ToString());
            //      if (Plugin.customColorsInstalled)
            //          Plugin.ResetCustomColorsSabers(Plugin.oldColorB, Plugin.oldColorB);
            /*
            var playerController = Resources.FindObjectsOfTypeAll<PlayerController>().First();
            Saber targetSaber = playerController.rightSaber;
            Saber otherSaber = playerController.leftSaber;
            var targetCopy = Instantiate(targetSaber.gameObject);
            Saber newSaber = targetCopy.GetComponent<Saber>();
            targetCopy.transform.parent = targetSaber.transform.parent;
            targetCopy.transform.localPosition = Vector3.zero;
            targetCopy.transform.localRotation = Quaternion.identity;
            targetSaber.transform.parent = otherSaber.transform.parent;
            targetSaber.transform.localPosition = Vector3.zero;
            targetSaber.transform.localRotation = Quaternion.identity;
            otherSaber.gameObject.SetActive(false);

            ReflectionUtil.SetPrivateField(playerController, "_leftSaber", targetSaber);
            ReflectionUtil.SetPrivateField(playerController, "_rightSaber", newSaber);

            playerController.leftSaber.gameObject.SetActive(true);
            */
            //    dataModel.beatmapData = beatmapData;
        }
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
            Plugin.ResetColors();
            text.text = text.text.Replace(" Rainbow | ", "");
        }

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

        public static void DestroyObjectsRaw()
        {
            Plugin.beatmapObjectManager.DissolveAllObjects();
            /*
            foreach (NoteController noteController in UnityEngine.Object.FindObjectsOfType<NoteController>())
            {
                try
                {
                    GameObject.Destroy(noteController.gameObject);

                }
                catch (System.Exception ex)
                {
                    Plugin.Log("Error Trying to Destroy objects: " + ex);
                }
            }
            // Not good enough, exceptions form on anything referencing obstacles
            
            foreach (ObstacleController obstacleController in UnityEngine.Object.FindObjectsOfType<ObstacleController>())
            {
                try
                {
                    obstacleController.StartCoroutine(obstacleController.DissolveCoroutine());

                }
                catch (System.Exception ex)
                {
                    Plugin.Log("Error Trying to Destroy objects: " + ex);
                }
            }
            */

        }
        public static void ResetPowers(bool resetMessage)
        {
            GMPUI.rainbow = false;
            GMPUI.funky = false;
            GMPUI.njsRandom = false;
            GMPUI.offsetrandom = false;
            GMPUI.randomSize = false;
            GMPUI.reverse = false;
            Plugin.altereddNoteScale = 1f;
            //       Time.timeScale = 1;
            //        Plugin.timeScale = 1;
            Plugin.superRandom = false;
            if (resetMessage)
            {
                Plugin.spawnController.SetField("_disappearingArrows", false);
                Plugin.ResetColors();
                if (Plugin.isValidScene)
                    AdjustNjsOrOffset();
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                text.text = " ";
                var text2 = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
                text2.text = "";
                Plugin.SetTimeScale(Plugin.currentSongSpeed);

                resetMessage = false;
            }

        }


        public static IEnumerator MadScience(float length)
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            List<BeatmapObjectData> objects;
            float start = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float end = start + length + 2f;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData.ToList();
                for (int i = 0; i < objects.Count; ++i)
                {
                    BeatmapObjectData beatmapObject = objects[i];
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        objects.Remove(beatmapObject);
                        objects.Add(NoteToWall(beatmapObject, Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.beatsPerMinute));

                    }
                }
                objects = objects.OrderBy(o => o.time).ToList();
                line.beatmapObjectsData = objects.ToArray();
                objects.Clear();
            }
            yield return new WaitForSeconds(length + 2f);
            callbackController.SetField("_beatmapData", beatmapData);



        }

        public static IEnumerator Encasement(float duration)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Tunnel | ";
            float startTime = Plugin.songAudio.time + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.2f;
            float durationTime = duration;
            MappingExtensions.Plugin.ForceActivateForSong();

            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            List<BeatmapObjectData> objects;
            objects = beatmapData.beatmapLinesData[0].beatmapObjectsData.ToList();
            objects.Add(new ObstacleData(14131, startTime, -1, (ObstacleType)4000, durationTime, 1001));
            objects.Add(new ObstacleData(14132, startTime, 4, (ObstacleType)4000, durationTime, 1001));
            objects.Add(new ObstacleData(14133, startTime, -1, (ObstacleType)1001, durationTime, 6500));
            objects.Add(new ObstacleData(14134, startTime, -1, (ObstacleType)5000, durationTime, 6500));
            objects = objects.OrderBy(o => o.time).ToList();
            beatmapData.beatmapLinesData[0].beatmapObjectsData = objects.ToArray();
            objects.Clear();
            yield return new WaitForSeconds(duration);
            text.text = text.text.Replace(" Tunnel | ", "");

        }

        public static IEnumerator PermaEncasement(float start)
        {
            yield return new WaitForSeconds(0f);
            MappingExtensions.Plugin.ForceActivateForSong();
            float startTime = start + Plugin.spawnController.GetField<BeatmapObjectSpawnMovementData>("_beatmapObjectSpawnMovementData").spawnAheadTime + 0.1f;
            float durationTime = Plugin.songAudio.clip.length;

            BeatmapObjectCallbackController callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            BeatmapData beatmapData = callbackController.GetField<BeatmapData>("_beatmapData");
            Plugin.Log("Grabbed BeatmapData");
            List<BeatmapObjectData> objects;
            objects = beatmapData.beatmapLinesData[0].beatmapObjectsData.ToList();
            objects.Add(new ObstacleData(14131, startTime, -1, (ObstacleType)4000, durationTime, 1001));
            objects.Add(new ObstacleData(14132, startTime, 4, (ObstacleType)4000, durationTime, 1001));
            objects.Add(new ObstacleData(14133, startTime, -1, (ObstacleType)1001, durationTime, 6500));
            objects.Add(new ObstacleData(14134, startTime, -1, (ObstacleType)5000, durationTime, 6500));
            objects = objects.OrderBy(o => o.time).ToList();
            beatmapData.beatmapLinesData[0].beatmapObjectsData = objects.ToArray();
            objects.Clear();



        }
        public static BeatmapObjectData NoteToWall(BeatmapObjectData original, float bpm)
        {
            NoteData note = original as NoteData;
            int startHeight = 120 + (((int)note.noteLineLayer) * 120);
            int height = 150;
            int width = 1800;
            int type = height * 1000 + startHeight + 4001;
            float duration = (bpm / 60f) * (1f / 32f);

            BeatmapObjectData newWall = new ObstacleData(original.id * 14130, original.time, original.lineIndex, (ObstacleType)type, duration, width);
            return newWall;
        }



    }
}
