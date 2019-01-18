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
            yield return new WaitForSeconds(Plugin.ChatConfig.timeForCharges);
            Plugin.charges += Plugin.ChatConfig.chargesOverTime;
            if (Plugin.charges > Plugin.ChatConfig.maxCharges) Plugin.charges = Plugin.ChatConfig.maxCharges;
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
            if (Plugin.ChatConfig.showCooldownOnMessage)
            {
                if (Plugin.ChatConfig.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false)
                {
                    TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds." + "Global Command Cooldown Active for " + Plugin.ChatConfig.globalCommandCooldown + " seconds.");
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
            yield return new WaitForSeconds(Plugin.ChatConfig.globalCommandCooldown);
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
            yield return new WaitForSecondsRealtime(10f);
   //         SharedCoroutineStarter.instance.StartCoroutine(Reverse(10f));
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

        public static IEnumerator offsetrandom(float length)
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
                njs = UnityEngine.Random.Range(Plugin.ChatConfig.njsRandomMin, Plugin.ChatConfig.njsRandomMax);
            if (GMPUI.reverse)
                njs *= -1;
            int noteJumpStartBeatOffset = Plugin.levelData.difficultyBeatmap.noteJumpStartBeatOffset;
            if (GMPUI.offsetrandom)
                noteJumpStartBeatOffset += UnityEngine.Random.Range(Plugin.ChatConfig.offsetrandomMin, Plugin.ChatConfig.offsetrandomMax);
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

                                int randMax = (int)((1 / Plugin.ChatConfig.bombChance) * 100);
                                int randMin = 100;
                                int random = Random.Range(1, randMax);

                                //                Plugin.Log("Min: " + randMin + " Max: " + randMax + " Number: " + random);

                                if (random <= randMin || Plugin.ChatConfig.bombChance == 1)
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
            float beatAlignOffset = Plugin.soundEffectManager.GetField<float>("_beatAlignOffset");
            GameplayCoreSceneSetup sceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            float songspeedmul = Plugin.levelData.gameplayCoreSetupData.gameplayModifiers.songSpeedMul;
            Plugin.songAudio.pitch = pitch;
            Plugin.currentSongSpeed = pitch;
            AudioMixerSO mixer = sceneSetup.GetField<AudioMixerSO>("_audioMixer");
            mixer.musicPitch = 1f / pitch;

            if (pitch != 1f)
                Plugin.AudioTimeSync.forcedAudioSync = true;
            else
                Plugin.AudioTimeSync.forcedAudioSync = false;

            Plugin.soundEffectManager.SetField("_beatAlignOffset", beatAlignOffset * (1.5f * pitch));


            yield return new WaitForSeconds(length);

            Plugin.songAudio.pitch = songspeedmul;
            Plugin.currentSongSpeed = songspeedmul;
            mixer.musicPitch = 1 / songspeedmul;

            if (songspeedmul == 1f)
            {
                mixer.musicPitch = 1;
                Plugin.AudioTimeSync.forcedAudioSync = false;
            }
            Plugin.soundEffectManager.SetField("_beatAlignOffset", beatAlignOffset);
            text.text = text.text.Replace(" Speed | ", "");
        }



        public static IEnumerator NoArrows()
        {
            Plugin.Log("Starting");

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
            Plugin.Log("Starting");

            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            ObstacleData obstacle;
            float start = Plugin.songAudio.time + 2;
            float end = start + length + 2f + 300f;
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
            Plugin.Log("Starting");

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
                if(!GMPUI.oneColor)
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
                        (gameNoteController.noteData.noteType == NoteType.NoteA) ? Saber.SaberType.SaberA : Saber.SaberType.SaberB, 1f, 0f, 0f, Vector3.down, Vector3.zero, counter, 0f));
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
            Plugin.altereddNoteScale = 1f;
     //       Time.timeScale = 1;
    //        Plugin.timeScale = 1;
            Plugin.superRandom = false;
            if (resetMessage)
            {
                Plugin.spawnController.SetField("_disappearingArrows", false);
                Plugin.colorA.SetColor(Plugin.oldColorA);
                Plugin.colorB.SetColor(Plugin.oldColorB);
                if(Plugin.isValidScene)
                    AdjustNjsOrOffset();
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                text.text = " ";
                var text2 = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().activeCommandText;
                text2.text = "";
                //           GameplayCoreSceneSetup sceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
                //            AudioMixerSO mixer = sceneSetup.GetField<AudioMixerSO>("_audioMixer");
                //           float songspeedmul = Plugin.levelData.gameplayCoreSetupData.gameplayModifiers.songSpeedMul;
                //            Plugin.AudioTimeSync.SetField("_timeScale", songspeedmul);
                //            Plugin.songAudio.pitch = songspeedmul;
                //            Plugin.currentSongSpeed = songspeedmul;
                //            mixer.musicPitch = 1 / songspeedmul;
                //            if (songspeedmul == 1f)
                //            {
                //                mixer.musicPitch = 1;
                //                Plugin.AudioTimeSync.forcedAudioSync = false;
                //            }
                //            if (Plugin.pauseManager.gameState == StandardLevelGameplayManager.GameState.Paused)
                //                 Plugin.AudioTimeSync.Pause();

                resetMessage = false;
            }

        }
    }
}
