using IllusionPlugin;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using AsyncTwitch;
using IllusionInjector;
using TMPro;
using CustomUI.GameplaySettings;

namespace GamePlayModifiersPlus
{
    public class TwitchPowers : MonoBehaviour
    {
       
        public static IEnumerator TempDA(float length)
        {
            Plugin.Log("Starting");
            Plugin.spawnController.SetField("_disappearingArrows", true);
            yield return new WaitForSeconds(length);
            Plugin.spawnController.SetField("_disappearingArrows", false);
        }

        public static IEnumerator CoolDown(float waitTime, string cooldown, string message)
        {
            
            Plugin.cooldowns.SetCooldown(true, cooldown);
            if (Plugin.Config.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false)
            {
                SharedCoroutineStarter.instance.StartCoroutine(GlobalCoolDown());
                TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds." + "Global Command Cooldown Active for" + Plugin.Config.globalCommandCooldown + " seconds.");
            }
            else
            TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + " seconds");


            yield return new WaitForSeconds(waitTime);
            Plugin.cooldowns.SetCooldown(false, cooldown);
            //      TwitchConnection.Instance.SendChatMessage(cooldown + " Cooldown Deactivated, have fun!");
        }

        public static IEnumerator GlobalCoolDown()
        {
            
            Plugin.cooldowns.SetCooldown(true, "Global");
            yield return new WaitForSeconds(Plugin.Config.globalCommandCooldown);
            Plugin.cooldowns.SetCooldown(false, "Global");

        }

        public static IEnumerator TempInstaFail(float length)
        {
            Image energyBar = Plugin.energyPanel.GetField<Image>("_energyBar");
            energyBar.color = Color.red;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 1f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 1f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
        }

        public static IEnumerator TempInvincibility(float length)
        {
            Image energyBar = Plugin.energyPanel.GetField<Image>("_energyBar");
            energyBar.color = Color.yellow;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0f);
            yield return new WaitForSeconds(length);
            energyBar.color = Color.white;
            Plugin.energyCounter.SetField("_badNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_missNoteEnergyDrain", 0.1f);
            Plugin.energyCounter.SetField("_hitBombEnergyDrain", 0.15f);
            Plugin.energyCounter.SetField("_obstacleEnergyDrainPerSecond", 0.1f);
        }

        public static IEnumerator SpecialEvent()
        {
            Plugin.gnomeActive = true;
            yield return new WaitForSecondsRealtime(0.1f);
            Plugin.SetTimeScale(0f); ;
            Time.timeScale = 0f;
            Plugin.gnomeSound.Load();
            Plugin.gnomeSound.Play();
            Plugin.soundIsPlaying = true;
            Plugin.Log("Waiting");
            yield return new WaitForSecondsRealtime(16f);
            if (Plugin.isValidScene == true)
            {
                Plugin.soundIsPlaying = false;
                Plugin.SetTimeScale(0f); ;
                Time.timeScale = 1f;
                Plugin.Log("Unpaused");
                Plugin.gnomeActive = false;
            }
        }

        public static void AdjustNJS(float njs)
        {
            /*
            Log("_halfJumpDurationInBeats " + spawnController.GetField<float>("_halfJumpDurationInBeats").ToString());
            Log("_spawnAheadTime " + spawnController.GetField<float>("_spawnAheadTime").ToString());
            Log("_jumpDistance " + spawnController.GetField<float>("_jumpDistance").ToString());
            Log("_noteJumpMovementSpeed " + spawnController.GetField<float>("_noteJumpMovementSpeed").ToString());
            Log("_moveDistance " + spawnController.GetField<float>("_moveDistance").ToString());
            Log("_moveSpeed " + spawnController.GetField<float>("_moveSpeed").ToString());
            Log("_moveDurationInBeats " + spawnController.GetField<float>("_moveDurationInBeats").ToString());
            Log("_beatsPerMinut e" + spawnController.GetField<float>("_beatsPerMinute").ToString());
            Log("_moveDurationInBeats " + spawnController.GetField<float>("_moveDurationInBeats").ToString());
            */

            float halfJumpDur = 1f;
            float maxHalfJump = 18f;
            float minHalfJump = 8f;
            float moveSpeed = Plugin.spawnController.GetField<float>("_moveSpeed");
            float moveDir = Plugin.spawnController.GetField<float>("_moveDurationInBeats");
            float jumpDis;
            float spawnAheadTime;
            float moveDis;
            float bpm = Plugin.spawnController.GetField<float>("_beatsPerMinute");
            float num = 60f / bpm;
            moveDis = moveSpeed * num * moveDir;
            /*
                while (njs * num * halfJumpDur > maxHalfJump)
            {
                    halfJumpDur -= 1f;
            }

                while (njs * num * halfJumpDur < minHalfJump)
            {
                    halfJumpDur += 1f;
            }
         */
            halfJumpDur = Plugin.spawnController.GetField<float>("_halfJumpDurationInBeats");
            jumpDis = njs * num * halfJumpDur * 2f;
            spawnAheadTime = moveDis / moveSpeed + jumpDis * 0.5f / njs;
            Plugin.spawnController.SetField("_halfJumpDurationInBeats", halfJumpDur);
            Plugin.spawnController.SetField("_spawnAheadTime", spawnAheadTime);
            Plugin.spawnController.SetField("_jumpDistance", jumpDis);
            Plugin.spawnController.SetField("_noteJumpMovementSpeed", njs);
            Plugin.spawnController.SetField("_moveDistance", moveDis);



            /*
            Log("_halfJumpDurationInBeats " + spawnController.GetField<float>("_halfJumpDurationInBeats").ToString());
            Log("_spawnAheadTime " + spawnController.GetField<float>("_spawnAheadTime").ToString());
            Log("_jumpDistance " + spawnController.GetField<float>("_jumpDistance").ToString());
            Log("_noteJumpMovementSpeed " + spawnController.GetField<float>("_noteJumpMovementSpeed").ToString());
            Log("_moveDistance " + spawnController.GetField<float>("_moveDistance").ToString());
            Log("_moveSpeed " + spawnController.GetField<float>("_moveSpeed").ToString());
            Log("_moveDurationInBeats " + spawnController.GetField<float>("_moveDurationInBeats").ToString());
            Log("_beatsPerMinut e" + spawnController.GetField<float>("_beatsPerMinute").ToString());
            Log("_moveDurationInBeats " + spawnController.GetField<float>("_moveDurationInBeats").ToString());
            */

        }


        public static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

        }


        public static IEnumerator ScaleNotes(float scale, float length)
        {
            Plugin.altereddNoteScale = scale;
            yield return new WaitForSeconds(length);
            Plugin.altereddNoteScale = 1f;


        }

        public static IEnumerator RandomNotes(float length)
        {

            Plugin.randomSize = true;
            yield return new WaitForSeconds(length);
            Plugin.randomSize = false;
            

        }

        public static IEnumerator njsRandom(float length)
        {
            Plugin.randomNJS = true;
            yield return new WaitForSeconds(length);
            Plugin.randomNJS = false;
            AdjustNJS(Plugin.songNJS);
        }



        public static IEnumerator Pause(float waitTime)
        {
            Plugin.paused = true;
            Plugin.SetTimeScale(0f); ;
            Time.timeScale = 0f;
            Plugin.Log("Pausing");
            yield return new WaitForSecondsRealtime(waitTime);
            if (Plugin.isValidScene == true)
            {
                Plugin.SetTimeScale(1f); ;
                Time.timeScale = 1f;
                Plugin.Log("Unpaused");
                Plugin.paused = false;
            }
        }


        




        public static IEnumerator TempNoArrows(float length)
        {
            Plugin.Log("Starting");

            yield return new WaitForSeconds(0f);
            GameplayCoreSceneSetup gameplayCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            BeatmapDataModel dataModel = gameplayCoreSceneSetup.GetField<BeatmapDataModel>("_beatmapDataModel");
            Plugin.Log("Grabbed dataModel");
            Plugin.Log(dataModel.beatmapData.bombsCount.ToString());
            BeatmapData beatmapData = dataModel.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            float start = Plugin.songAudio.time;
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

                            note.SetNoteToAnyCutDirection();
                            note.TransformNoteAOrBToRandomType();

                        }

                }
            }
            dataModel.beatmapData = beatmapData;


        }

        public static IEnumerator Funky(float length)
        {
            Plugin.funky = true;
            yield return new WaitForSeconds(length);
            Plugin.funky = false;

        }
        public static IEnumerator Rainbow(float length)
        {
  
            Plugin.rainbow = true;
            yield return new WaitForSeconds(length);
            Plugin.rainbow = false;
            Plugin.colorA.SetColor(Plugin.oldColorA);
            Plugin.colorB.SetColor(Plugin.oldColorB);
            Plugin.Log("Done");

        }


        public static void ResetPowers()
        {
            Plugin.rainbow = false;
            Plugin.funky = false;
            Plugin.randomNJS = false;
            Plugin.altereddNoteScale = 1f;
            Time.timeScale = 1;
            Plugin.timeScale = 1;
            Plugin.superRandom = false;

        }

        /*
        public void TempBombs(float length)
        {
            float start = _songAudio.time;
            float end = start + length;
            BeatmapData mapData = levelData.difficultyBeatmap.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            foreach (BeatmapLineData line in mapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            note = beatmapObject as NoteData;
                            note.SetProperty("noteType", NoteType.Bomb);
                            //   note = new NoteData(note.id, note.time + .5f, note.lineIndex, note.noteLineLayer, note.startNoteLineLayer, NoteType.Bomb, NoteCutDirection.Up, note.timeToNextBasicNote, note.timeToPrevBasicNote);

                        }

                }
            }


        }
        */
        /*
        public void TempMirror(float length)
        {
            float start = _songAudio.time;
            float end = start + length;
            BeatmapData mapData = levelData.difficultyBeatmap.beatmapData;
            BeatmapObjectData[] objects;
            NoteData note;
            foreach (BeatmapLineData line in mapData.beatmapLinesData)
            {
                objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                        if (beatmapObject.time > start && beatmapObject.time < end)
                        {
                            note = beatmapObject as NoteData;
                            note.MirrorLineIndex(2);
                            note.SwitchNoteType();
                        }

                }
            }


        }

    */

    }
}
