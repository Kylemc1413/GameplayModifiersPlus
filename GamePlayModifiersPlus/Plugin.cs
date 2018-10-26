using IllusionPlugin;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using System.Media;
using System.Linq;
namespace GamePlayModifiersPlus
{
    public class Plugin : IPlugin
    {
        public string Name => "GameplayModifiersPlus";
        public string Version => "0.0.1";

        public static bool gnomeOnMiss = false;
        SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.gnome);
        bool soundIsPlaying = false;
        bool songIsPaused = false;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        private static AudioSource _songAudio;
        public static bool isValidScene = false;
        public static bool gnomeActive = false;
        private IEnumerator coroutine;

        public static bool superHot = false;
        public static PlayerController player;
        public static Saber leftSaber;
        public static Saber rightSaber;
        public static bool playerInfo = false;
        public static float prevLeftPos;
        public static float prevRightPos;
        public static float prevHeadPos;
        public static float speedPitch = 1;
        public static bool calculating = false;
        public static float deltaLeft;
        public static float deltaRight;
        public static float deltaHead;
        public static float deltaSpeedL;
        public static float deltaSpeedR;
        public static float deltaRotHead;
        public static float prevSpeedL;
        public static float prevSpeedR;
        public static float prevRotHead;
        public static bool startSuperHot;


        public static bool bulletTime = false;

        VRController leftController;
        VRController rightController;

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            gnomeOnMiss = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
            superHot = ModPrefs.GetBool("GameplayModifiersPlus", "superHot", false, true);
            bulletTime = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
            coroutine = SpecialEvent();
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            Time.timeScale = 1;
            if (soundIsPlaying == true)
                simpleSound.Stop();
            soundIsPlaying = false;
            SharedCoroutineStarter.instance.StopAllCoroutines();
            isValidScene = false;
            playerInfo = false;
            if (scene.name == "Menu")
            {
                var controllers = Resources.FindObjectsOfTypeAll<VRController>();
                foreach (VRController controller in controllers)
                {
                    //        Log(controller.ToString());
                    if (controller.ToString() == "ControllerLeft (VRController)")
                        leftController = controller;
                    if (controller.ToString() == "ControllerRight (VRController)")
                        rightController = controller;
                }
                Log("Left:" + leftController.ToString());
                Log("Right: " + rightController.ToString());

                var gnomeOption = GameOptionsUI.CreateToggleOption("Gnome on miss");
                gnomeOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
                gnomeOption.OnToggle += (gnomeOnMiss) => { ModPrefs.SetBool("GameplayModifiersPlus", "gnomeOnMiss", gnomeOnMiss); Log("Changed Modprefs value"); };

                var superHotOption = GameOptionsUI.CreateToggleOption("SuperHot");
                superHotOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "superHot", false, true);
                superHotOption.OnToggle += (superHot) => { ModPrefs.SetBool("GameplayModifiersPlus", "superHot", superHot); Log("Changed Modprefs value"); };

                var bulletTimeOption = GameOptionsUI.CreateToggleOption("Bullet Time");
                bulletTimeOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
                bulletTimeOption.OnToggle += (bulletTime) => { ModPrefs.SetBool("GameplayModifiersPlus", "bulletTime", bulletTime); Log("Changed Modprefs value"); };
            }
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            gnomeOnMiss = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
            superHot = ModPrefs.GetBool("GameplayModifiersPlus", "superHot", false, true);
            bulletTime = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
            if (bulletTime == true)
                superHot = false;
            if (scene.name == "GameCore")
            {
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                isValidScene = true;
                AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                if (AudioTimeSync != null)
                {
                    _songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                    if (_songAudio != null)
                        Log("Audio not null");
                    Log("Object Found");
                }
                if (gnomeOnMiss == true)
                {

                    BeatmapObjectSpawnController[] beatmapObjectSpawnControllers = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>();
                    BeatmapObjectSpawnController beatmapObjectSpawnController = beatmapObjectSpawnControllers.Length > 0 ? beatmapObjectSpawnControllers?[0] : null;
                    if (beatmapObjectSpawnController != null)
                    {
                        beatmapObjectSpawnController.noteWasMissedEvent += delegate (BeatmapObjectSpawnController beatmapObjectSpawnController2, NoteController noteController)
                        {
                            if (noteController.noteData.noteType != NoteType.Bomb)
                            {
                                try
                                {
                                    SharedCoroutineStarter.instance.StopAllCoroutines();
                                    SharedCoroutineStarter.instance.StartCoroutine(SpecialEvent());
                                    Log("Gnoming");
                                }
                                catch (Exception ex)
                                {
                                    Log(ex.ToString());
                                }
                            }
                        };

                        beatmapObjectSpawnController.noteWasCutEvent += delegate (BeatmapObjectSpawnController beatmapObjectSpawnController2, NoteController noteController, NoteCutInfo noteCutInfo)
                        {
                            if (!noteCutInfo.allIsOK)
                            {
                                SharedCoroutineStarter.instance.StopAllCoroutines();
                                SharedCoroutineStarter.instance.StartCoroutine(SpecialEvent());
                                Log("Gnoming");
                            }

                        };

                    }
                }
                if(superHot == true)
                {
                    startSuperHot = false;
                    player = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();
                    if(player != null)
                    {
                    leftSaber = player.leftSaber;
                    rightSaber = player.rightSaber;
                    playerInfo = true;
                    }
                    else
                    {
                        playerInfo = false;
                        Log("Player is null");
                    }
                    SharedCoroutineStarter.instance.StartCoroutine(Wait(1f));

                }






            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        { 
        }

        public void OnUpdate()
        {
            if (soundIsPlaying == true && _songAudio != null && isValidScene == true)
            {
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 0f);
                Time.timeScale = 0f;
                return;
            }

            if (bulletTime == true && isValidScene == true && soundIsPlaying == false)
            {
                speedPitch = 1 - (leftController.triggerValue + rightController.triggerValue) / 2;
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", speedPitch);
                Time.timeScale = speedPitch;
                return;
            }
        

            if (superHot == true && playerInfo == true && soundIsPlaying == false && isValidScene == true && startSuperHot == true)
            {
                speedPitch = (leftSaber.bladeSpeed / 15 + rightSaber.bladeSpeed / 15) / 1.5f;
                if (speedPitch > 1)
                    speedPitch = 1;
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", speedPitch);
                Time.timeScale = speedPitch;
       /*     if (calculating == false)
                {
                prevLeftPos = leftSaber.handlePos.magnitude;
                prevRightPos = rightSaber.handlePos.magnitude;
                prevHeadPos = player.headPos.magnitude;
                prevRotHead = player.GetField<Transform>("_headTransform").rotation.eulerAngles.magnitude;
                prevSpeedL = leftSaber.bladeSpeed;
                prevSpeedR = rightSaber.bladeSpeed;
                SharedCoroutineStarter.instance.StartCoroutine(Delta());
                    ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", speedPitch);                }
*/      
            }
            else
            {
                Time.timeScale = 1f;
            }
            if (playerInfo == true)
                if(player.disableSabers == true)
                    Time.timeScale = 1;
        }
    
        public void OnFixedUpdate()
        {
        }


        private IEnumerator SpecialEvent()
        {
            gnomeActive = true;
            yield return new WaitForSecondsRealtime(0.1f);
            songIsPaused = true;
            ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 0f);
            Time.timeScale = 0f;
            simpleSound.Load();
            simpleSound.Play();
            soundIsPlaying = true;
            Log("Waiting");
            yield return new WaitForSecondsRealtime(16f);
            if(isValidScene == true)
            {
            soundIsPlaying = false;
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                Time.timeScale = 1f;
                songIsPaused = false;
            Log("Unpaused");
            gnomeActive = false;
            }        
        }
        private static IEnumerator Delta()
        {
            calculating = true;
       //     Log("Previous Left: " + prevLeftPos);
       //     Log("Previous Right: " + prevRightPos);
       //     Log("Previous Head: " + prevRightPos);
            yield return new WaitForSecondsRealtime(0.1f);
       //         Log("Current Left: " + leftSaber.handlePos.magnitude);
       //         Log("Current Right: " + rightSaber.handlePos.magnitude);
       //         Log("Current Head: " + player.headPos.magnitude);

            deltaLeft = leftSaber.handlePos.magnitude - prevLeftPos;
            deltaRight = rightSaber.handlePos.magnitude - prevLeftPos;
            deltaHead = player.headPos.magnitude - prevHeadPos;
            deltaSpeedL = leftSaber.bladeSpeed - prevSpeedL;
            deltaSpeedR = rightSaber.bladeSpeed - prevSpeedR;
            deltaRotHead = player.GetField<Transform>("_headTransform").rotation.eulerAngles.magnitude - prevRotHead;
         //   Log("Left: " + Mathf.Abs(deltaLeft));
         //   Log("Right: " + Mathf.Abs(deltaRight));
         //   Log("Head: " + Mathf.Abs(deltaHead));
         //   Log("Speed Left: " + Mathf.Abs(deltaSpeedL));
          //  Log("Speed Left: " + Mathf.Abs(deltaSpeedR));
          //  Log("Head Rotation: " + Mathf.Abs(Mathf.Cos(deltaRotHead)));
            //Correct Values as per logs
            deltaLeft = Mathf.Abs(deltaLeft);
            deltaRight = Mathf.Abs(deltaRight);
            deltaHead = Mathf.Abs(deltaHead);
            deltaSpeedL = Mathf.Abs(deltaSpeedL);
            deltaSpeedR = Mathf.Abs(deltaSpeedR);
            deltaRotHead = Mathf.Abs(Mathf.Cos(deltaRotHead));


            float avgBladeSpeedDelta = (deltaSpeedL /15 + deltaSpeedR /15) / 2;
            float avgHandPositionDelta = (deltaLeft + deltaRight) / 2;
            speedPitch = (avgBladeSpeedDelta / 1.5f)  + (avgHandPositionDelta * 2f) + (deltaHead * 10f);
          //  Log("Head Rotation Delta: " + (deltaRotHead / 3));
            Log("Blade Delta: " + avgBladeSpeedDelta / 1.5f);
            Log("Hand Delta: " + avgHandPositionDelta * 2f);
            Log("Head Delta: " + deltaHead * 10f);
            Log("Speed Pitch: " + speedPitch);

            calculating = false;
            
        }
        private static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            startSuperHot = true;
        }

        public static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "GameplayModifiersPlus", message);
        }
    }
}
