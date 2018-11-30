using IllusionPlugin;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using AsyncTwitch;
using IllusionInjector;
using TMPro;
using CustomUI.GameplaySettings;
namespace GamePlayModifiersPlus
{
    public class Plugin : IPlugin
    {
        public string Name => "GameplayModifiersPlus";
        public string Version => "0.0.1";

        public static bool gnomeOnMiss = false;
        SoundPlayer gnomeSound = new SoundPlayer(Properties.Resources.gnome);
        SoundPlayer beepSound = new SoundPlayer(Properties.Resources.Beep);
        bool soundIsPlaying = false;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        private static AudioSource _songAudio;
        public static bool isValidScene = false;
        public static bool gnomeActive = false;
        public static bool twitchStuff = false;
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
        public static bool startSuperHot;
        public static bool swapSabers;
        public static bool bulletTime = false;
        public static bool paused = false;
        private static Cooldowns _cooldowns;
        public static TMP_Text ppText;
        public static string rank;
        public static string pp;
        public static float currentpp;
        public static float oldpp = 0;
        public static int currentRank;
        public static int oldRank;
        public static float deltaPP;
        public static int deltaRank;
        public static bool chatDelta;
        public static bool firstLoad = true;
        VRController leftController;
        VRController rightController;
        private Sprite _ChatDeltaIcon;
        private Sprite _SwapSabersIcon;
        private Sprite _RepeatIcon;
        StandardLevelSceneSetupDataSO levelData;
        bool invalidForScoring = false;
        bool repeatSong;
        private static bool _hasRegistered = false;
        BeatmapObjectSpawnController spawnController;
        GameEnergyCounter energyCounter;
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            ReadPrefs();
            //        _cooldowns = new Cooldowns();
        }


        private void TwitchConnection_OnMessageReceived(TwitchConnection arg1, TwitchMessage message)
        {
            Log("Message Recieved, AsyncTwitch currently working");
            //Status check message
            if (message.Content.ToLower().Contains("!gm status"))
            {
                beepSound.Play();
                TwitchConnection.Instance.SendChatMessage("Currently Not Borked");
            }
            if (message.Content.ToLower().Contains("!gm help"))
            {
                TwitchConnection.Instance.SendChatMessage("Include !gm followed by a command in your message while the streamer has twitch mode on to mess with their game." +
                    " Currently supported commands: status - check the plugin is still working, pp - Show streamer current rank and pp");
            }

            if (message.Content.ToLower().Contains("!gm pp"))
            {
                if (currentpp != 0)
                    TwitchConnection.Instance.SendChatMessage("Streamer Rank: #" + currentRank + ". Streamer pp: " + currentpp + "pp");
                else
                    TwitchConnection.Instance.SendChatMessage("Currently do not have streamer info");
            }

 

        }
    
        

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

            if (scene.name == "Menu")
            {

                ReadPrefs();
                if(_ChatDeltaIcon == null)
                    _ChatDeltaIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.ChatDelta.png");
                if (_SwapSabersIcon == null)
                    _SwapSabersIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.SwapSabers.png");
                if (_RepeatIcon == null)
                    _RepeatIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.RepeatIcon.png");

                var swapSabersOption = GameplaySettingsUI.CreateToggleOption("Swap Sabers", "Swaps your sabers. Warning: Haptics are not swapped (CURRENTLY NOT WORKING)", _SwapSabersIcon);
                swapSabersOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "swapSabers", false, true);
                swapSabersOption.OnToggle += (swapSabers) => { ModPrefs.SetBool("GameplayModifiersPlus", "swapSabers", swapSabers); Log("Changed Modprefs value"); };

                var chatDeltaOption = GameplaySettingsUI.CreateToggleOption("Chat Delta", "Display Change in Performance Points / Rank in Twitch Chat if Connected", _ChatDeltaIcon);
                chatDeltaOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
                chatDeltaOption.OnToggle += (chatDelta) => { ModPrefs.SetBool("GameplayModifiersPlus", "chatDelta", chatDelta); Log("Changed Modprefs value"); };

                var repeatOption = GameplaySettingsUI.CreateToggleOption("Repeat", "Restarts song on song end, does not submit scores.", _RepeatIcon);
                repeatOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "repeatSong", false, true);
                repeatOption.OnToggle += (repeatSong) => { ModPrefs.SetBool("GameplayModifiersPlus", "repeatSong", repeatSong); Log("Changed Modprefs value"); };


            }
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            Time.timeScale = 1;
            speedPitch = 1;
            if (soundIsPlaying == true)
                gnomeSound.Stop();
            soundIsPlaying = false;
            isValidScene = false;
            playerInfo = false;
            if (scene.name == "Menu")
            {
                if(_hasRegistered == false)
                {
                TwitchConnection.Instance.StartConnection();
                TwitchConnection.Instance.RegisterOnMessageReceived(TwitchConnection_OnMessageReceived);
                _hasRegistered = true;
                }

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
   
            }

            ReadPrefs();
            if (scene.name == "Menu")
            {
                SharedCoroutineStarter.instance.StartCoroutine(GrabPP());
              


            }
            if (bulletTime == true)
                superHot = false;
            if (twitchStuff == true)
            {
                superHot = false;
                bulletTime = false;
                gnomeOnMiss = false;
            }


            if (scene.name == "GameCore")
            {
                ReadPrefs();
                levelData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().First();
                spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().First();
                energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().First();
                levelData.didFinishEvent += LevelData_didFinishEvent;
                //   ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                isValidScene = true;
                AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                if (AudioTimeSync != null)
                {
                    _songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                    if (_songAudio != null)
                        Log("Audio not null");
                    Log("Object Found");
                }
                //Get Sabers
                player = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();
                if (player != null)
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
                Log(leftSaber.handlePos.ToString());
                Log(leftSaber.saberBladeTopPos.ToString());
                if (swapSabers)
                {
                    

                }
              //  SharedCoroutineStarter.instance.StartCoroutine(SwapSabers(leftSaber, rightSaber));
                /*
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
                */
                /*
                if(superHot == true)
                {
                    startSuperHot = false;
                    SharedCoroutineStarter.instance.StartCoroutine(Wait(1f));

                }

            */




            }
        }
        private void LevelData_didFinishEvent(StandardLevelSceneSetupDataSO arg1, LevelCompletionResults arg2)
        {
            if (arg2.levelEndStateType == LevelCompletionResults.LevelEndStateType.Quit) return;

            if (invalidForScoring)
                ReflectionUtil.SetProperty(arg2, "levelEndStateType", LevelCompletionResults.LevelEndStateType.None);
            if (repeatSong)
                ReflectionUtil.SetProperty(arg2, "levelEndStateType", LevelCompletionResults.LevelEndStateType.Restart);

        }


        public void OnApplicationQuit()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;

        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
            //test
        }
        
        public void OnUpdate()
        {
                /*
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


                     }
                     else
                     {
                         Time.timeScale = 1f;
                     }
                     if (playerInfo == true)
                         if(player.disableSabers == true)
                             Time.timeScale = 1;
                             */
            }
    
        public void OnFixedUpdate()
        {
        }

        /*
        private IEnumerator SpecialEvent()
        {
            gnomeActive = true;
            yield return new WaitForSecondsRealtime(0.1f);
            ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 0f);
            Time.timeScale = 0f;
            gnomeSound.Load();
            gnomeSound.Play();
            soundIsPlaying = true;
            Log("Waiting");
            yield return new WaitForSecondsRealtime(16f);
            if(isValidScene == true)
            {
            soundIsPlaying = false;
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                Time.timeScale = 1f;
            Log("Unpaused");
            gnomeActive = false;
            }        
        }
      */
        private static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            startSuperHot = true;
        }

        private static IEnumerator CoolDown(float waitTime, string cooldown, string message)
        {
            _cooldowns.SetCooldown(true, cooldown);
            TwitchConnection.Instance.SendChatMessage(message + " " + cooldown + " Cooldown Active for " + waitTime.ToString() + "seconds");
            yield return new WaitForSeconds(waitTime);
            _cooldowns.SetCooldown(false, cooldown);
      //      TwitchConnection.Instance.SendChatMessage(cooldown + " Cooldown Deactivated, have fun!");
        }
        /*
        private IEnumerator Pause(float waitTime)
        {
            paused = true;
            ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 0f);
            Time.timeScale = 0f;
            Log("Pausing");
            yield return new WaitForSecondsRealtime(waitTime);
            if (isValidScene == true)
            {
                ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                Time.timeScale = 1f;
                Log("Unpaused");
                paused = false;
            }
        }
        */
        public static bool IsModInstalled(string modName)
        {
            foreach (IPlugin p in PluginManager.Plugins)
            {
                if (p.Name == modName)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "GameplayModifiersPlus", message);
        }
        public IEnumerator SwapSabers(Saber saber1, Saber saber2)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            beepSound.Play();
            Log("Swapping sabers");
            Transform transform1 = saber1.transform.parent.transform;

            Transform transform2 = saber2.transform.parent.transform;

            saber2.transform.parent = transform1;
            saber1.transform.parent = transform2;
            saber2.transform.SetPositionAndRotation(transform1.transform.position, player.rightSaber.transform.parent.rotation);
            saber1.transform.SetPositionAndRotation(transform2.transform.position, player.leftSaber.transform.parent.rotation);
        }

        public void ReadPrefs()
        {
         //   gnomeOnMiss = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
         //   superHot = ModPrefs.GetBool("GameplayModifiersPlus", "superHot", false, true);
         //   bulletTime = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
         //   twitchStuff = ModPrefs.GetBool("GameplayModifiersPlus", "twitchStuff", false, true);
            swapSabers = ModPrefs.GetBool("GameplayModifiersPlus", "swapSabers", false, true);
            chatDelta = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
            repeatSong = ModPrefs.GetBool("GameplayModifiersPlus", "repeatSong", false, true);
        }
        public IEnumerator GrabPP()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            var texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                if (text.ToString() == "PP (TMPro.TextMeshPro)")
                {
                    ppText = text;
                    break;

                }

            }
                yield return new WaitForSecondsRealtime(10);
            if (!ppText.text.Contains("html"))
            Log(ppText.text);
            if (!(ppText.text.Contains("Refresh") || ppText.text.Contains("html")))
            {
                rank = ppText.text.Split('#', '<')[1];
                pp = ppText.text.Split('(', 'p')[1];
                currentpp = float.Parse(pp, System.Globalization.CultureInfo.InvariantCulture);
                currentRank = int.Parse(rank, System.Globalization.CultureInfo.InvariantCulture);
                Log("Rank: " + currentRank);
                Log("PP: " + currentpp);
                if (firstLoad == true)
                    if (chatDelta)
                        TwitchConnection.Instance.SendChatMessage("Loaded. PP: " + currentpp + " pp. Rank: " + currentRank);

                if (oldpp != 0)
                {
                    deltaPP = 0;
                    deltaRank = 0;
                    deltaPP = currentpp - oldpp;
                    deltaRank = currentRank - oldRank;
                    
                    if (deltaPP != 0 || deltaRank != 0)
                    {
                        ppText.enableWordWrapping = false;
                        if (deltaRank < 0)
                        {
                            if (deltaRank == -1)
                            {
                                if (chatDelta)
                                    TwitchConnection.Instance.SendChatMessage("Gained " + deltaPP + " pp. Gained 1 Rank.");
                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained 1 Rank";
                            }

                            else
                            {
                                if (chatDelta)
                                    TwitchConnection.Instance.SendChatMessage("Gained " + deltaPP + " pp. Gained " + Math.Abs(deltaRank) + " Ranks.");
                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained " + Math.Abs(deltaRank) + " Ranks";
                            }

                        }
                        else if (deltaRank == 0)
                        {
                            if (chatDelta)
                                TwitchConnection.Instance.SendChatMessage("Gained " + deltaPP + " pp. No change in Rank.");
                            ppText.text += " Change: Gained " + deltaPP + " pp. " + "No change in Rank";
                        }

                        else if (deltaRank > 0)
                        {
                            if (deltaRank == 1)
                            {
                                if (chatDelta)
                                    TwitchConnection.Instance.SendChatMessage("Gained " + deltaPP + " pp. Lost 1 Rank.");
                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Lost 1 Rank";
                            }

                            else
                            {
                                if (chatDelta)
                                    TwitchConnection.Instance.SendChatMessage("Gained " + deltaPP + " pp. Lost " + Math.Abs(deltaRank) + " Ranks.");
                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Lost " + Math.Abs(deltaRank) + " Ranks";
                            }

                        }

                        oldRank = currentRank;
                        oldpp = currentpp;
                    }
                }
                else
                {
                    oldRank = currentRank;
                    oldpp = currentpp;
                    deltaPP = 0;
                    deltaRank = 0;
                }

            }
            firstLoad = false;



        }

        public IEnumerator TempDA(float length)
        {
            yield return new WaitForSeconds(length);
            spawnController.SetField("_disappearingArrows", true);
            yield return new WaitForSeconds(length);
            spawnController.SetField("_disappearingArrows", false);
        }

        public IEnumerator TempInstaFail(float length)
        {
            yield return new WaitForSeconds(length);
            energyCounter.Init(GameplayModifiers.EnergyType.Battery, false, true, false);

            yield return new WaitForSeconds(length);

            energyCounter.Init(GameplayModifiers.EnergyType.Bar, false, false, false);


        }
    }
}
