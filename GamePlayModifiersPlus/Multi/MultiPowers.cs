namespace GamePlayModifiersPlus.Multiplayer
{
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using GamePlayModifiersPlus;
    public class MultiPowers : MonoBehaviour
    {
        public static IEnumerator ChargeOverTime()
        {
            yield return new WaitForSeconds(MultiMain.Config.timeForCharges);
            float random = UnityEngine.Random.Range(0f, 10f);
            if (random >= 4f)
                MultiMain.Config.charges += MultiMain.Config.chargesOverTime;
            if (MultiMain.Config.charges > MultiMain.Config.maxCharges) MultiMain.Config.charges = MultiMain.Config.maxCharges;
            MultiMain.Powers.StartCoroutine(ChargeOverTime());
        }

        public static string GeneratePowerUp()
        {
            int power = (int)UnityEngine.Random.Range(0f, 8f);
            string powerup;
            switch (power)
            {
                case 0:
                    powerup = "DA";
                    break;
                case 1:
                    powerup = "Smaller";
                    break;
                case 2:
                    powerup = "Larger";
                    break;
                case 3:
                    powerup = "Random";
                    break;
                case 4:
                    powerup = "NjsRandom";
                    break;
                case 5:
                    powerup = "Funky";
                    break;
                case 6:
                    powerup = "Rainbow";
                    break;
                case 7:
                    powerup = "Bombs";
                    break;
                //               case 8:
                //                   powerup = "Faster";
                //                   break;
                case 8:
                    powerup = "NoArrows";
                    break;
                //                case 10:
                //                   powerup = "Slower";
                //                   break;
                //case 11:
                //     powerup = "InstaFail";
                //       break;
                default:
                    powerup = "DA";
                    break;
            }
            return powerup;
        }
        public static IEnumerator TempDA(float length)
        {

            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " DA | ";
            Plugin.spawnController.SetField("_disappearingArrows", true);
            yield return new WaitForSeconds(length);
            Plugin.spawnController.SetField("_disappearingArrows", false);
            text.text = text.text.Replace(" DA | ", "");
        }

        public static IEnumerator CoolDown(float waitTime, string cooldown, string message)
        {

            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().cooldownText;
            text.text += " " + cooldown + " | ";
            Plugin.cooldowns.SetCooldown(true, cooldown);

            yield return new WaitForSeconds(waitTime);
            Plugin.cooldowns.SetCooldown(false, cooldown);
            text.text = text.text.Replace(" " + cooldown + " | ", "");
        }

        public static IEnumerator GlobalCoolDown()
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().cooldownText;

            Plugin.cooldowns.SetCooldown(true, "Global");
            yield return new WaitForSeconds(MultiMain.Config.globalCommandCooldown);
            Plugin.cooldowns.SetCooldown(false, "Global");
            text.text = text.text.Replace(" " + "Global" + " | ", "");
        }

        public static IEnumerator TempInstaFail(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
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
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
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

        public static void AdjustNJS(float njs)
        {
            /*
     Plugin.Log("NJS " + Plugin.spawnController.GetField<float>("_noteJumpMovementSpeed"));
     Plugin.Log("_MaxHalfJump " + Plugin.spawnController.GetField<float>("_maxHalfJumpDistance"));
     Plugin.Log("_halfJumpDurationInBeats " + Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats").ToString());
     Plugin.Log("_spawnAheadTime " + Plugin.spawnController.GetField<float>("_spawnAheadTime").ToString());
     Plugin.Log("_jumpDistance " + Plugin.spawnController.GetField<float>("_jumpDistance").ToString());
     Plugin.Log("_noteJumpMovementSpeed " + Plugin.spawnController.GetField<float>("_noteJumpMovementSpeed").ToString());
     Plugin.Log("_moveDistance " + Plugin.spawnController.GetField<float>("_moveDistance").ToString());
     Plugin.Log("_moveSpeed " + Plugin.spawnController.GetField<float>("_moveSpeed").ToString());
     Plugin.Log("_moveDurationInBeats " + Plugin.spawnController.GetField<float>("_moveDurationInBeats").ToString());
     Plugin.Log("_beatsPerMinute " + Plugin.spawnController.GetField<float>("_beatsPerMinute").ToString());
     Plugin.Log("_moveDurationInBeats " + Plugin.spawnController.GetField<float>("_moveDurationInBeats").ToString());
     */
            Plugin.Log("JumpOffset " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset);
            float halfJumpDur = 4f;
            float maxHalfJump = Plugin.spawnController.GetField<float>("_maxHalfJumpDistance");
            float noteJumpStartBeatOffset = Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset;
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
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " Smaller/Larger | ";
            Plugin.altereddNoteScale = scale;
            yield return new WaitForSeconds(length);
            Plugin.altereddNoteScale = 1f;
            text.text = text.text.Replace(" Smaller/Larger | ", "");
        }

        public static IEnumerator RandomNotes(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " Random | ";
            GMPUI.randomSize = true;
            yield return new WaitForSeconds(length);
            GMPUI.randomSize = false;
            text.text = text.text.Replace(" Random | ", "");
        }

        public static IEnumerator NjsRandom(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " NJSRandom | ";
            GMPUI.njsRandom = true;
            yield return new WaitForSeconds(length);
            GMPUI.njsRandom = false;
            AdjustNJS(Plugin.songNJS);
            text.text = text.text.Replace(" NJSRandom | ", "");
        }

        public static IEnumerator RandomNJS()
        {
            AdjustNJS(UnityEngine.Random.Range(MultiMain.Config.njsRandomMin, MultiMain.Config.njsRandomMax));
            yield return new WaitForSeconds(0.33f);
            if (GMPUI.njsRandom)
                MultiMain.Powers.StartCoroutine(RandomNJS());

        }


        public static IEnumerator Pause()
        {
            yield return new WaitForSeconds(0f);
            StandardLevelGameplayManager pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().First();
            pauseManager.HandlePauseTriggered();
        }

        public static IEnumerator TempNoArrows(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
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
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
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

                                int randMax = (int)((1 / MultiMain.Config.bombChance) * 100);
                                int randMin = 100;
                                int random = Random.Range(1, randMax);

                                //                Plugin.Log("Min: " + randMin + " Max: " + randMax + " Number: " + random);

                                if (random <= randMin || MultiMain.Config.bombChance == 1)
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
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " Speed | ";
            float beatAlignOffset = Plugin.soundEffectManager.GetField<float>("_beatAlignOffset");
            GameplayCoreSceneSetup sceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            float songspeedmul = Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;
            Plugin.songAudio.pitch = pitch;
            Plugin.currentSongSpeed = pitch;
            AudioManagerSO mixer = sceneSetup.GetField<AudioManagerSO>("_audioMixer");
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


        public static IEnumerator Funky(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " Funky | ";
            GMPUI.funky = true;
            yield return new WaitForSeconds(length);
            GMPUI.funky = false;
            text.text = text.text.Replace(" Funky | ", "");
        }

        public static IEnumerator Rainbow(float length)
        {
            var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().activeCommandText;
            text.text += " Rainbow | ";
            GMPUI.rainbow = true;
            yield return new WaitForSeconds(length);
            GMPUI.rainbow = false;
            Plugin.colorA.SetColor(Plugin.oldColorA);
            Plugin.colorB.SetColor(Plugin.oldColorB);
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
        
    }
}
