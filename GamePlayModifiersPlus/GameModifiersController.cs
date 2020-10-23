using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GamePlayModifiersPlus.Utilities;
using System.Media;
using GamePlayModifiersPlus.TwitchStuff;
using BS_Utils.Utilities;

namespace GamePlayModifiersPlus
{
    public class GameModifiersController
    {
        public static int charges = 0;
        public static int commandsLeftForMessage;
        public static float timeScale = 1;
        public static float altereddNoteScale = 1;
        public static bool trySuper = false;
        public static bool tempNoArrow;
        public static bool superRandom;
        public static bool healthActivated;
        public static bool sizeActivated;
        public static bool noArrow;
        public static float currentSongSpeed;

        public static SoundPlayer beepSound = new SoundPlayer(Properties.Resources.Beep);
        public static SoundPlayer reverseSound = new SoundPlayer(Properties.Resources.sectionpass);

        public static void SetupSpawnCallbacks()
        {
            if (GameObjects.beatmapObjectManager != null)
            {
                GameObjects.beatmapObjectManager.noteDidStartJumpEvent += SpawnController_ModifiedJump;
                GameObjects.beatmapObjectManager.noteWasCutEvent += SpawnController_ScaleRemoveCut;
                GameObjects.beatmapObjectManager.noteWasMissedEvent += SpawnController_ScaleRemoveMiss;

            }
        }

        public static void SpawnController_ScaleRemoveMiss(NoteController controller)
        {
            NoteData note = controller.noteData;
            Transform noteTransform = controller.noteTransform;
            if (noteTransform?.localScale.x != 1)
                noteTransform.localScale = new Vector3(1f, 1f, 1f);
            FloatBehavior behavior = noteTransform?.gameObject.GetComponent<FloatBehavior>();
            if (behavior != null)
            {

                noteTransform.localPosition = new Vector3(behavior.originalX, behavior.originalY, noteTransform.localPosition.z);
                GameObject.Destroy(behavior);

            }
        }

        public static void SpawnController_ScaleRemoveCut(NoteController controller, NoteCutInfo arg3)
        {
            NoteData note = controller.noteData;
            Transform noteTransform = controller.noteTransform;
            if (noteTransform.localScale.x != 1)
                noteTransform.localScale = new Vector3(1f, 1f, 1f);

            FloatBehavior behavior = noteTransform.gameObject.GetComponent<FloatBehavior>();
            if (behavior != null)
            {
                noteTransform.localPosition = new Vector3(behavior.originalX, behavior.originalY, noteTransform.localPosition.z);
                GameObject.Destroy(behavior);
            }






        }

        public static void SpawnController_ModifiedJump(NoteController controller)
        {
            if (GMPUI.rainbow)
            {
                Utilities.Rainbow.RandomizeColors();
            }
            Transform noteTransform = controller.noteTransform;
            if (GMPUI.funky)
            {
                noteTransform?.gameObject.AddComponent<FloatBehavior>();


            }
            if (GMPUI.chatIntegration || GMPUI.randomSize)
            {
                if (superRandom)
                {
                    noteTransform.localScale *= UnityEngine.Random.Range(Config.randomMin, Config.randomMax);

                }
                else
                {
                    if (!GMPUI.randomSize)
                    {
                        noteTransform.localScale *= altereddNoteScale;

                    }

                    if (GMPUI.randomSize)
                    {
                        noteTransform.localScale *= UnityEngine.Random.Range(Config.randomMin, Config.randomMax);

                    }
                }
            }
            if (GMPUI.fixedNoteScale != 1f)
            {
                noteTransform.localScale *= GMPUI.fixedNoteScale;
            }
        }


        public static void SetTimeScale(float value)
        {
            if (GameObjects.AudioTimeSync == null) return;
            timeScale = value;
            AudioTimeSyncController.InitData initData = GameObjects.AudioTimeSync.GetPrivateField<AudioTimeSyncController.InitData>("_initData");
            AudioTimeSyncController.InitData newInitData = new AudioTimeSyncController.InitData(initData.audioClip,
                GameObjects.AudioTimeSync.songTime, initData.songTimeOffset, timeScale);
            GameObjects.AudioTimeSync.SetPrivateField("_initData", newInitData);
            //Chipmunk Removal as per base game

            if (timeScale == 1f)
                GameObjects.Mixer.musicPitch = 1;
            else
                GameObjects.Mixer.musicPitch = 1f / timeScale;

            ResetTimeSync(GameObjects.AudioTimeSync, timeScale, newInitData);
        }

        public static void ResetTimeSync(AudioTimeSyncController timeSync, float newTimeScale, AudioTimeSyncController.InitData newData)
        {
            timeSync.SetField("_timeScale", newTimeScale);
            timeSync.SetField("_startSongTime", timeSync.songTime);
            timeSync.SetField("_audioStartTimeOffsetSinceStart", timeSync.GetProperty<float>("timeSinceStart") - (timeSync.songTime + newData.songTimeOffset));
            timeSync.SetField("_fixingAudioSyncError", false);
            timeSync.SetField("_playbackLoopIndex", 0);
            timeSync.audioSource.pitch = newTimeScale;
        }

        public static IEnumerator CheckGMPModifiers()
        {
            yield return new WaitForSeconds(0.1f);
            if (GMPUI.EndlessMode || GMPUI.removeCrouchWalls || GMPUI.swapSabers || GMPUI.fiveLanes || GMPUI.angleShift || GMPUI.laneShift || GMPUI.sixLanes || GMPUI.fourLayers || GMPUI.reverse || GMPUI.chatIntegration || GMPUI.funky || GMPUI.njsRandom || GMPUI.noArrows || GMPUI.randomSize || GMPUI.fixedNoteScale != 1f || GMPUI.offsetrandom)
            {
                //     ApplyPatches();
                UnityEngine.Random.InitState(Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.beatmapData.beatmapObjectsData.Count());
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("GameplayModifiersPlus");

                if (GMPUI.njsRandom || GMPUI.offsetrandom)
                {
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.RandomNjsOrOffset());
                }

                if (GMPUI.removeCrouchWalls)
                {
                    if (Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.enabledObstacleType != GameplayModifiers.EnabledObstacleType.NoObstacles)
                        Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.SetProperty("enabledObstacleType", GameplayModifiers.EnabledObstacleType.FullHeightOnly);
                }
                if (GMPUI.sixLanes || GMPUI.fourLayers || GMPUI.fiveLanes || GMPUI.laneShift || GMPUI.angleShift)
                {
                    bool notSafe = false;
                    var songdata = SongCore.Collections.RetrieveDifficultyData(BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap);
                    if (songdata == null) notSafe = false;
                    else
                    {
                        if (songdata.additionalDifficultyData._requirements.Count() > 0)
                            notSafe = true;
                    }
                    if (!notSafe)
                        SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.ExtraLanes());
                    else
                        Plugin.log.Warn("Not activating Mapping Extensions Modifiers for Map with Requirements");
                }
                if (GMPUI.noArrows)
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.NoArrows());
                if (GMPUI.swapSabers)
                {
                    Plugin.Log("Testing Ground Active");
                    try
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TestingGround(10f));
                    }
                    catch (Exception ex)
                    {
                        Plugin.Log(ex.ToString());
                    }
                }

                if (GMPUI.reverse)
                {
                    Plugin.Log("Map Reversal");
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.PermaReverse());
                }



            }

            if (GMPUI.tunnel)
            {
                Plugin.Log("Tunnel Activating");
                SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.PermaEncasement(0f));
            }

            if (GMPUI.EndlessMode)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("GameplayModifiersPlus");
                new GameObject("GMP Endless Behavior").AddComponent<EndlessBehavior>();
            }

        }


    }
}
