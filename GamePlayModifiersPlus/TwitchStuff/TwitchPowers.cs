namespace GamePlayModifiersPlus
{
    using AsyncTwitch;
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

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
            if (ChatConfig.showCooldownOnMessage)
            {
                if (ChatConfig.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false)
                {
                    TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds." + "Global Command Cooldown Active for " + ChatConfig.globalCommandCooldown + " seconds.");
                }
                else
                    TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds");
            }
            else
                TwitchConnection.Instance.SendChatMessage(message);


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


        public static IEnumerator SpecialEvent()
        {
            Plugin.gnomeActive = true;
            yield return new WaitForSecondsRealtime(0.1f);
            Plugin.SetTimeScale(0f);
            Time.timeScale = 0f;
            Plugin.gnomeSound.Load();
            Plugin.gnomeSound.Play();
            Plugin.soundIsPlaying = true;
            Plugin.Log("Waiting");
            yield return new WaitForSecondsRealtime(16f);
            if (Plugin.isValidScene == true)
            {
                Plugin.soundIsPlaying = false;
                Plugin.SetTimeScale(1f); ;
                Time.timeScale = 1f;
                Plugin.songAudio.pitch = 1f;
                Plugin.Log("Unpaused");
                Plugin.gnomeActive = false;
            }
        }

        public static IEnumerator TestingGround(float length)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            //    SharedCoroutineStarter.instance.StartCoroutine(ExtraLanes());
        }

        public static void AdjustNJS(float njs)
        {

            float halfJumpDur = 4f;
            float maxHalfJump = Plugin.spawnController.GetField<float>("_maxHalfJumpDistance");
            float noteJumpStartBeatOffset = Plugin.levelData.difficultyBeatmap.noteJumpStartBeatOffset;
            float moveSpeed = Plugin.spawnController.GetField<float>("_moveSpeed");
            float moveDir = Plugin.spawnController.GetField<float>("_moveDurationInBeats");
            float jumpDis;
            float spawnAheadTime;
            float moveDis;
            float bpm = Plugin.spawnController.GetField<float>("_beatsPerMinute");
            float num = 60f / bpm;
            moveDis = moveSpeed * num * moveDir;
            while (njs * num * halfJumpDur > maxHalfJump)
            {
                halfJumpDur /= 2f;
            }
            halfJumpDur += noteJumpStartBeatOffset;
            if (halfJumpDur < 1f) halfJumpDur = 1f;
            //        halfJumpDur = Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats");
            jumpDis = njs * num * halfJumpDur * 2f;
            spawnAheadTime = moveDis / moveSpeed + jumpDis * 0.5f / njs;
            Plugin.spawnController.SetField("_halfJumpDurationInBeats", halfJumpDur);
            Plugin.spawnController.SetField("_spawnAheadTime", spawnAheadTime);
            Plugin.spawnController.SetField("_jumpDistance", jumpDis);
            Plugin.spawnController.SetField("_noteJumpMovementSpeed", njs);
            Plugin.spawnController.SetField("_moveDistance", moveDis);

        }

        public static void ResetNjsAndOffset()
        {

            float njs = Plugin.songNJS;
            float halfJumpDur = 4f;
            float maxHalfJump = Plugin.spawnController.GetField<float>("_maxHalfJumpDistance");
            float noteJumpStartBeatOffset = Plugin.levelData.difficultyBeatmap.noteJumpStartBeatOffset;
            float moveSpeed = Plugin.spawnController.GetField<float>("_moveSpeed");
            float moveDir = Plugin.spawnController.GetField<float>("_moveDurationInBeats");
            float jumpDis;
            float spawnAheadTime;
            float moveDis;
            float bpm = Plugin.spawnController.GetField<float>("_beatsPerMinute");
            float num = 60f / bpm;
            moveDis = moveSpeed * num * moveDir;
            while (njs * num * halfJumpDur > maxHalfJump)
            {
                halfJumpDur /= 2f;
            }
            halfJumpDur += noteJumpStartBeatOffset;
            if (halfJumpDur < 1f) halfJumpDur = 1f;
            //        halfJumpDur = Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats");
            jumpDis = njs * num * halfJumpDur * 2f;
            spawnAheadTime = moveDis / moveSpeed + jumpDis * 0.5f / njs;
            Plugin.spawnController.SetField("_halfJumpDurationInBeats", halfJumpDur);
            Plugin.spawnController.SetField("_spawnAheadTime", spawnAheadTime);
            Plugin.spawnController.SetField("_jumpDistance", jumpDis);
            Plugin.spawnController.SetField("_noteJumpMovementSpeed", njs);
            Plugin.spawnController.SetField("_moveDistance", moveDis);

        }
        public static void AdjustOffset(int offset)
        {

            float njs = Plugin.spawnController.GetField<float>("_noteJumpMovementSpeed");
            float halfJumpDur = 4f;
            float maxHalfJump = Plugin.spawnController.GetField<float>("_maxHalfJumpDistance");
            float noteJumpStartBeatOffset = Plugin.levelData.difficultyBeatmap.noteJumpStartBeatOffset + offset;
            float moveSpeed = Plugin.spawnController.GetField<float>("_moveSpeed");
            float moveDir = Plugin.spawnController.GetField<float>("_moveDurationInBeats");
            float jumpDis;
            float spawnAheadTime;
            float moveDis;
            float bpm = Plugin.spawnController.GetField<float>("_beatsPerMinute");
            float num = 60f / bpm;
            moveDis = moveSpeed * num * moveDir;
            while (njs * num * halfJumpDur > maxHalfJump)
            {
                halfJumpDur /= 2f;
            }
            halfJumpDur += noteJumpStartBeatOffset;
            if (halfJumpDur < 1f) halfJumpDur = 1f;
            //        halfJumpDur = Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats");
            jumpDis = njs * num * halfJumpDur * 2f;
            spawnAheadTime = moveDis / moveSpeed + jumpDis * 0.5f / njs;
            Plugin.spawnController.SetField("_halfJumpDurationInBeats", halfJumpDur);
            Plugin.spawnController.SetField("_spawnAheadTime", spawnAheadTime);
            Plugin.spawnController.SetField("_jumpDistance", jumpDis);
            Plugin.spawnController.SetField("_noteJumpMovementSpeed", njs);
            Plugin.spawnController.SetField("_moveDistance", moveDis);

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
            if (!Plugin.haveSongNJS)
            {
                Plugin.songNJS = Plugin.spawnController.GetField<float>("_noteJumpMovementSpeed");
                Plugin.haveSongNJS = true;
            }
            float njs = Plugin.songNJS;
            if (GMPUI.njsRandom)
                njs = UnityEngine.Random.Range(ChatConfig.njsRandomMin, ChatConfig.njsRandomMax);
            if (GMPUI.reverse)
                njs *= -1;
            int noteJumpStartBeatOffset = Plugin.levelData.difficultyBeatmap.noteJumpStartBeatOffset;
            if (GMPUI.offsetrandom)
                noteJumpStartBeatOffset += UnityEngine.Random.Range(ChatConfig.offsetrandomMin, ChatConfig.offsetrandomMax);
            float halfJumpDur = 4f;
            float maxHalfJump = Plugin.spawnController.GetField<float>("_maxHalfJumpDistance");
            float moveSpeed = Plugin.spawnController.GetField<float>("_moveSpeed");
            float moveDir = Plugin.spawnController.GetField<float>("_moveDurationInBeats");
            float jumpDis;
            float spawnAheadTime;
            float moveDis;
            float bpm = Plugin.spawnController.GetField<float>("_beatsPerMinute");
            float num = 60f / bpm;
            moveDis = moveSpeed * num * moveDir;
            while (njs * num * halfJumpDur > maxHalfJump)
            {
                halfJumpDur /= 2f;
            }
            halfJumpDur += noteJumpStartBeatOffset;
            if (halfJumpDur < 1f) halfJumpDur = 1f;
            //        halfJumpDur = Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats");
            jumpDis = njs * num * halfJumpDur * 2f;
            spawnAheadTime = moveDis / moveSpeed + jumpDis * 0.5f / njs;
            Plugin.spawnController.SetField("_halfJumpDurationInBeats", halfJumpDur);
            Plugin.spawnController.SetField("_spawnAheadTime", spawnAheadTime);
            Plugin.spawnController.SetField("_jumpDistance", jumpDis);
            Plugin.spawnController.SetField("_noteJumpMovementSpeed", njs);
            Plugin.spawnController.SetField("_moveDistance", moveDis);

            //       Plugin.Log(njs.ToString());
            //       Plugin.Log(noteJumpStartBeatOffset.ToString());
        }

        public static IEnumerator Pause()
        {
            yield return new WaitForSeconds(0f);
            StandardLevelGameplayManager pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().First();
            pauseManager.HandlePauseTriggered();
        }

        public static IEnumerator TempNoArrows(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " NoArrows | ";
            yield return new WaitForSeconds(0f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + 2;
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
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + 2;
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
                                    note.SetProperty("noteType", NoteType.Bomb);
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
            dataModel.beatmapData = beatmapData;

                
            
        }

        public static IEnumerator SpeedChange(float length, float pitch)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Speed | ";

            float songspeedmul = Plugin.levelData.gameplayCoreSetupData.gameplayModifiers.songSpeedMul;

            Plugin.SetTimeScale(pitch);


            yield return new WaitForSeconds(length);


            Plugin.SetTimeScale(songspeedmul);
            text.text = text.text.Replace(" Speed | ", "");
        }



        public static IEnumerator NoArrows()
        {

            yield return new WaitForSeconds(0f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log(dataModel.beatmapData.bombsCount.ToString());
            BeatmapData beatmapData = dataModel.beatmapData;
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
        public static IEnumerator ExtraLanes()
        {

            yield return new WaitForSeconds(0f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log(dataModel.beatmapData.bombsCount.ToString());
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            System.Collections.Generic.List<float> claimedCenterTimes = new System.Collections.Generic.List<float>();
            System.Collections.Generic.List<float> noteTimes = new System.Collections.Generic.List<float>();
            System.Collections.Generic.List<float> doubleTimes = new System.Collections.Generic.List<float>();
            //Iterate through once to log double times
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        note = beatmapObject as NoteData;

                        if (noteTimes.Contains(note.time))
                            doubleTimes.Add(note.time);
                        else
                            noteTimes.Add(note.time);
                    }

                }
            }
            noteTimes.Clear();

            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        note = beatmapObject as NoteData;
                        if (GMPUI.sixLanes || GMPUI.fiveLanes)
                        {
                            if (!doubleTimes.Contains(note.time))// || GMPUI.laneShift)
                            {
                                if (note.lineIndex == 0 && Random.Range(1, 4) >= 2)
                                    note.MirrorLineIndex(0);
                                // line index 3
                                if (note.lineIndex == 3 && Random.Range(1, 4) >= 2)
                                    note.MirrorLineIndex(8);
                            }
                        }
                        if (GMPUI.fourLayers)
                        {
                            if (note.noteLineLayer == NoteLineLayer.Top && Random.Range(1, 4) > 2)
                                note.SetProperty("noteLineLayer", (NoteLineLayer)3);
                        }
                        int newIndex = 0;
                        if (GMPUI.fiveLanes)
                        {
                            switch (note.lineIndex)
                            {
                                case 0:
                                    newIndex = 1500;
                                    break;

                                case 1:
                                    newIndex = UnityEngine.Random.Range(0, 10) > 3 || claimedCenterTimes.Contains(note.time) ? 1500 : 2500;
                                    if (Random.Range(0, 8) > 6) newIndex = -1500;
                                    break;

                                case 2:
                                    newIndex = UnityEngine.Random.Range(0, 10) > 3 || claimedCenterTimes.Contains(note.time) ? 3500 : 2500;
                                    if (Random.Range(0, 8) > 6) newIndex = 4500;
                                    break;

                                case 3:
                                    newIndex = 3500;
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
                            note.SetProperty("lineIndex", newIndex);
                            note.SetProperty("flipLineIndex", newIndex);
                            if (newIndex == 2500)
                            {
                                claimedCenterTimes.Add(note.time);
                            }


                        }

                        if(!doubleTimes.Contains(note.time))
                        if (GMPUI.laneShift)
                        {
                            if (!(note.lineIndex >= 1000 || note.lineIndex <= -1000))
                            {
                                int shiftedIndex = (note.lineIndex * 1000) + (UnityEngine.Random.Range(1, 8) * 100);
                                    if(note.lineIndex < 0)
                                        shiftedIndex = (note.lineIndex * 1000) - (UnityEngine.Random.Range(1, 8) * 100);
                                    if (note.lineIndex == 0)
                                    shiftedIndex = 1000 + (UnityEngine.Random.Range(1, 8) * 100);
                                note.SetProperty("lineIndex", shiftedIndex);
                                note.SetProperty("flipLineIndex", shiftedIndex);
                            }
                            else if (note.lineIndex >= 1000 && note.lineIndex <= 4500)
                            {

                                int shiftedIndex = note.lineIndex + (UnityEngine.Random.Range(1, 7) * 100);
                                note.SetProperty("lineIndex", shiftedIndex);
                                note.SetProperty("flipLineIndex", shiftedIndex);
                            }

                        }

                        if(GMPUI.angleShift && !((int)note.cutDirection >= 1000))
                        {
                            int angle = 1000;
                            switch(note.cutDirection)
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
                            if(angle == 2000)
                            {
                               //Do Nothing for now
                            }
                            else if(angle >= 1000)
                            {
                                angle += Random.Range(-30, 30);
                                angle = Mathf.Clamp(angle, 1000, 1360);
                                note.SetProperty("cutDirection", angle);
                            }
                        }

                        noteTimes.Add(note.time);
                    }





                }
            }
            //    dataModel.beatmapData = beatmapData;
        }
        public static IEnumerator Reverse(float length)
        {
            var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
            text.text += " Reverse | ";
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + 2;
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

                            note.SwitchNoteType();
                            //                           note.MirrorTransformCutDirection();
                            //                           note.MirrorLineIndex(4);
                        }
                }
            }

            yield return new WaitForSeconds(2f);
            GMPUI.reverse = true;
            AdjustNjsOrOffset();
            Plugin.reverseSound.Play();
            DestroyNotes();
            //Code to show some kinda prompt here maybe
            yield return new WaitForSeconds(length);
            GMPUI.reverse = false;
            AdjustNjsOrOffset();
            //Code to destroy notes here
            Plugin.reverseSound.Play();
            DestroyNotes();

            text.text = text.text.Replace(" Reverse | ", "");

        }

        public static IEnumerator PermaReverse()
        {
            yield return new WaitForSecondsRealtime(1f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time + 2;

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

            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            ObstacleData obstacle;
            float start = Plugin.songAudio.time + 4f;
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





        public static IEnumerator OneColor()
        {
            yield return new WaitForSeconds(0f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log(dataModel.beatmapData.bombsCount.ToString());
            BeatmapData beatmapData = dataModel.beatmapData;
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
                        note.SetProperty("noteType", NoteType.NoteB);
                    }
                }
            }
            //Adjust Sabers for one color
            var leftSaberType = Plugin.player.leftSaber.GetField<SaberTypeObject>("_saberType");

            try
            {
                leftSaberType.SetField("_saberType", Saber.SaberType.SaberB);
            }
            catch (System.Exception ex)
            {
                Plugin.Log(ex.ToString());
            }

            Plugin.Log("2 " + Plugin.player.leftSaber.saberType.ToString());
            yield return new WaitForSeconds(0.1f);
            if (Plugin.customColorsInstalled)
                Plugin.ResetCustomColorsSabers(Plugin.oldColorB, Plugin.oldColorB);
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
            Plugin.colorA.SetColor(Plugin.oldColorA);
            Plugin.colorB.SetColor(Plugin.oldColorB);
            if (Plugin.customColorsInstalled)
                if (!GMPUI.oneColor)
                    Plugin.ResetCustomColorsSabers(Plugin.oldColorA, Plugin.oldColorB);
                else
                    Plugin.ResetCustomColorsSabers(Plugin.oldColorB, Plugin.oldColorB);
            if (Plugin.environmentColorsSetter != null)
            {
                Color overrrideA = Plugin.environmentColorsSetter.GetField<Color>("_overrideColorA");
                Color overrrideB = Plugin.environmentColorsSetter.GetField<Color>("_overrideColorB");
                if (Plugin.customColorsInstalled)
                {
                    if (Plugin.IsCustomColorsDisabled() || Plugin.DoesCustomColorsAllowEnvironmentColors())
                    {
                        Plugin.colorB.SetColor(overrrideA);
                        Plugin.colorA.SetColor(overrrideB);
                        if (!GMPUI.oneColor)
                            Plugin.ResetCustomColorsSabers(overrrideB, overrrideA);
                        else
                            Plugin.ResetCustomColorsSabers(overrrideA, overrrideA);
                    }
                }
                else
                {
                    Plugin.colorB.SetColor(overrrideA);
                    Plugin.colorA.SetColor(overrrideB);
                }
            }
            text.text = text.text.Replace(" Rainbow | ", "");
        }

        public static void DestroyNotes()
        {
            foreach (GameNoteController gameNoteController in UnityEngine.Object.FindObjectsOfType<GameNoteController>())
            {
                try
                {
                    SaberAfterCutSwingRatingCounter counter = Plugin.player.leftSaber.CreateAfterCutSwingRatingCounter();
                    counter.SetField("_rating", 1f);
                    gameNoteController.SendNoteWasCutEvent(new NoteCutInfo(true, true, true, false, 999f, Vector3.down,
                        (gameNoteController.noteData.noteType == NoteType.NoteA) ? Saber.SaberType.SaberA : Saber.SaberType.SaberB, 1f, 0f, 0f, Vector3.down, new Vector3(0.0001f, 0.00001f, 0.00001f), counter, 0f));
                }
                catch (System.Exception ex)
                {
                    Plugin.Log("Error Trying to Destroy notes: " + ex);
                }
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
            Plugin.altereddNoteScale = 1f;
            //       Time.timeScale = 1;
            //        Plugin.timeScale = 1;
            Plugin.superRandom = false;
            if (resetMessage)
            {
                Plugin.spawnController.SetField("_disappearingArrows", false);
                Plugin.colorA.SetColor(Plugin.oldColorA);
                Plugin.colorB.SetColor(Plugin.oldColorB);
                if (Plugin.isValidScene)
                    AdjustNjsOrOffset();
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                text.text = " ";
                var text2 = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
                text2.text = "";
                if (Plugin.practicePluginInstalled)
                    Plugin.SetTimeScale(Plugin.currentSongSpeed);

                resetMessage = false;
            }

        }
    }
}
