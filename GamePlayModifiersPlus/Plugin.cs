namespace GamePlayModifiersPlus
{
    using AsyncTwitch;
    using CustomUI.GameplaySettings;
    using IllusionInjector;
    using IllusionPlugin;
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Media;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Harmony;
    using GamePlayModifiersPlus.TwitchStuff;
    using GamePlayModifiersPlus.Utilities;

    public class Plugin : IPlugin
    {
        internal static BS_Utils.Utilities.Config ChatConfigSettings = new BS_Utils.Utilities.Config("GameplayModifiersPlus");
        public string Name => "GameplayModifiersPlus";

        public string Version => "1.7.0beta2";
        public static string pluginVersion = "1.7.0beta2";

        public static float timeScale = 1;
        Multiplayer.MultiMain multi = null;
        public static bool multiInstalled = false;
        public TwitchCommands twitchCommands = new TwitchCommands();
        public static TwitchPowers twitchPowers = null;
        public static SoundPlayer gnomeSound = new SoundPlayer(Properties.Resources.gnome);
        public static SoundPlayer beepSound = new SoundPlayer(Properties.Resources.Beep);
        public static SoundPlayer reverseSound = new SoundPlayer(Properties.Resources.sectionpass);
        public static bool soundIsPlaying = false;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        public static AudioSource songAudio;
        public static bool isValidScene = false;
        public static bool gnomeActive = false;
        public static PlayerController player;
        public static Saber leftSaber;
        public static Saber rightSaber;
        public static bool playerInfo = false;
        public static float prevLeftPos;
        public static float prevRightPos;
        public static float prevHeadPos;
        public static bool calculating = false;
        public static bool paused = false;
        public static Cooldowns cooldowns;
        public static TMP_Text ppText;
        public static string rank;
        public static string pp;
        public static float currentpp;
        public static float oldpp = 0;
        public static int currentRank;
        public static int oldRank;
        public static float deltaPP;
        public static int deltaRank;
        public static bool firstLoad = true;
        public static VRController leftController;
        public static VRController rightController;
        public static StandardLevelSceneSetupDataSO levelData;
        private static bool invalidForScoring = false;
        private static bool _hasRegistered = false;
        public static BeatmapObjectSpawnController spawnController;
        public static GameEnergyCounter energyCounter;
        public static GameEnergyUIPanel energyPanel;
        public static GameObject chatIntegrationObj;
        public static int charges = 0;
        public static float altereddNoteScale = 1;
        public static bool trySuper = false;
        public static bool tempNoArrow;
        public static bool superRandom;
        public static bool healthActivated;
        public static bool sizeActivated;
        public static NoteData[] modifiedNotes;
        public static bool noArrow;
        public static float songNJS;
        public static bool haveSongNJS;
        public static SimpleColorSO colorA;
        public static SimpleColorSO colorB;
        public static SimpleColorSO oldColorA = ScriptableObject.CreateInstance<SimpleColorSO>();
        public static SimpleColorSO oldColorB = ScriptableObject.CreateInstance<SimpleColorSO>();
        public static SimpleColorSO defColorA = ScriptableObject.CreateInstance<SimpleColorSO>();
        public static SimpleColorSO defColorB = ScriptableObject.CreateInstance<SimpleColorSO>();
        public static int commandsLeftForMessage;
        public static bool test;
        public static float currentSongSpeed;
        public static StandardLevelGameplayManager pauseManager;
        public static NoteCutSoundEffectManager soundEffectManager;
        public static EnvironmentColorsSetter environmentColorsSetter;
        public static bool defaultRumble = true;
        static bool setDefaultRumble = false;
        static NoteCutEffectSpawner _noteCutEffectSpawner;
        static NoteCutHapticEffect _noteCutHapticEffect;
        static HapticFeedbackController _hapticFeedbackController;
        static MainSettingsModel _mainSettingsModel;
        static MainSettingsModel _mainSettingsModelOneC;
        static ColorManager ColorManager;
        public static bool activateDuringIsolated = false;
        public static HarmonyInstance harmony;

        internal static bool customColorsInstalled = false;
        internal static bool AsyncInstalled = false;
        GameObject chatPowers = null;

        public void OnApplicationStart()
        {
            //Asynchronous Twitch Library
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            {
                Log("Creating Harmony Instance");
                harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.GamePlayModifiersPlus");
                ApplyPatches();
            }
            SongLoaderPlugin.SongLoader.RegisterCapability("ChromaToggle");
            SongLoaderPlugin.SongLoader.RegisterCapability("Extra Lanes");
            CheckPlugins();
            ChatConfig.Load();
            ReadPrefs();
            //Delete old config if it exists
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\GamePlayModifiersPlusChatSettings.ini")))
                try
                {
                    File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData\\GamePlayModifiersPlusChatSettings.ini"));
                }
                catch (Exception ex)
                {
                    Log("Could not Delete Old Config: " + ex);
                }
            cooldowns = new Cooldowns();
            defColorA.SetColor(new Color(1f, 0, 0));
            defColorB.SetColor(new Color(0, .706f, 1));

            if (ModPrefs.GetInt("GameplayModifiersPlus", "GameRumbleSetting", -1, false) != -1)
            {
                Log("Rumble Key Exists");
                setDefaultRumble = true;
            }

        }

        private void TwitchConnection_OnMessageReceived(TwitchConnection arg1, TwitchMessage message)
        {

            if (charges < 0) charges = 0;
            twitchCommands.CheckChargeMessage(message);
            twitchCommands.CheckConfigMessage(message);
            twitchCommands.CheckStatusCommands(message);
            twitchCommands.CheckInfoCommands(message);

            if (multiInstalled)
                if (Multiplayer.MultiMain.multiActive) return;
            if (ChatConfig.allowEveryone || (ChatConfig.allowSubs && message.Author.IsSubscriber) || message.Author.IsMod)
            {
                if (GMPUI.chatIntegration && isValidScene && !cooldowns.GetCooldown("Global") && AsyncInstalled)
                {
                    commandsLeftForMessage = ChatConfig.commandsPerMessage;
                    twitchCommands.CheckPauseMessage(message);
                    twitchCommands.CheckGameplayCommands(message);
                    twitchCommands.CheckHealthCommands(message);
                    twitchCommands.CheckSizeCommands(message);
                    //     twitchCommands.CheckSpeedCommands(message);
                    twitchCommands.CheckGlobalCoolDown();
                }
            }
            trySuper = false;
            sizeActivated = false;
            healthActivated = false;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

            if (scene.name == "Menu")
            {

                ReadPrefs();
                try
                {
                    GMPUI.CreateUI();
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                }
                if (multiInstalled)
                    Multiplayer.MultiClientInterface.Init();

            }
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {

            if (scene.name == ("Menu"))
            {
                activateDuringIsolated = false;
                Log("Switched to Menu");
                SharedCoroutineStarter.instance.StartCoroutine(GrabPP());

                if (AsyncInstalled)
                    InitAsync();

                var controllers = Resources.FindObjectsOfTypeAll<VRController>();
                if (controllers != null)
                {
                    foreach (VRController controller in controllers)
                    {
                        if (controller != null)
                        {
                            if (controller.ToString() == "ControllerLeft (VRController)")
                                leftController = controller;
                            if (controller.ToString() == "ControllerRight (VRController)")
                                rightController = controller;
                        }
                        //        Log(controller.ToString());

                    }
                    //                 Log("Left:" + leftController.ToString());
                    //                   Log("Right: " + rightController.ToString());

                }


            }





            if (scene.name == "GameCore")
                RemovePatches();
            if (_mainSettingsModel == null)
            {
                var menu = Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault();
                if (menu != null)
                {
                    _mainSettingsModel = menu.GetField<MainSettingsModel>("_mainSettingsModel");
                    _mainSettingsModel.Load();
                    Log("RUMBLE: " + _mainSettingsModel.controllersRumbleEnabled.ToString());

                    if (!setDefaultRumble)
                    {
                        defaultRumble = _mainSettingsModel.controllersRumbleEnabled;
                        ModPrefs.SetInt("GameplayModifiersPlus", "GameRumbleSetting", _mainSettingsModel.controllersRumbleEnabled ? 1 : 0);
                        setDefaultRumble = true;
                        Log("Set Default Rumble Value");
                    }
                }

            }

            if (_mainSettingsModel != null)
            {
                defaultRumble = ModPrefs.GetInt("GameplayModifiersPlus", "GameRumbleSetting", -1, false) != 1 ? false : true;
                _mainSettingsModel.controllersRumbleEnabled = defaultRumble;
                _mainSettingsModel.Save();
            }


            paused = false;
            if (!customColorsInstalled)
            {
                if (colorA != null)
                    colorA.SetColor(defColorA);
                if (colorB != null)
                    colorB.SetColor(defColorB);
            }


            //        try
            //        {
            if (scene.name == "EmptyTransition")
            {
                Log("Resetting Chat Powers Object");
                if (chatPowers != null)
                    GameObject.Destroy(chatPowers);
            }
            if (chatPowers == null)
            {
                Log("Null Creation of Chat Powers Object");
                chatPowers = new GameObject("Chat Powers");
                GameObject.DontDestroyOnLoad(chatPowers);
                twitchPowers = chatPowers.AddComponent<TwitchPowers>();

            }

            //        }
            //        catch(Exception ex)
            //        {
            //           Log(ex.ToString());
            //        }

            GMPDisplay display = chatPowers.GetComponent<GMPDisplay>();
            if (display != null)
            {
                display.Destroy();
                GameObject.Destroy(display);
            }




            ReadPrefs();
            if (GMPUI.chatIntegration && AsyncInstalled)
            {
                if (twitchPowers != null)
                {
                    cooldowns.ResetCooldowns();
                    TwitchPowers.ResetPowers(false);
                    twitchPowers.StopAllCoroutines();
                }
                if (ChatConfig.resetChargesEachLevel)
                    charges = 0;


            }

            //    twitchCommands.StopAllCoroutines();
            haveSongNJS = false;
            if (soundIsPlaying == true)
                gnomeSound.Stop();
            soundIsPlaying = false;
            isValidScene = false;
            playerInfo = false;
            if (arg0.name == "EmpyTransition" && chatPowers != null)
                GameObject.Destroy(chatPowers);

            if (scene.name == "GameCore")
            {
                isValidScene = true;
                if (BS_Utils.Gameplay.Gamemode.IsIsolatedLevel && !activateDuringIsolated)
                {
                    Log("Isolated Level, not activating");
                    return;
                }

                foreach (string a in SongLoaderPlugin.SongLoader.currentRequirements)
                {
                    Plugin.Log(a);
                }


                GameObject.Destroy(GameObject.Find("Color Setter"));
                environmentColorsSetter = Resources.FindObjectsOfTypeAll<EnvironmentColorsSetter>().FirstOrDefault();
                soundEffectManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().First();
                levelData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().First();
                spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().First();
                energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().First();
                energyPanel = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().First();
                ColorManager = Resources.FindObjectsOfTypeAll<ColorManager>().First();
                spawnController.noteDidStartJumpEvent += SpawnController_ModifiedJump;
                spawnController.noteWasCutEvent += SpawnController_ScaleRemoveCut;
                spawnController.noteWasMissedEvent += SpawnController_ScaleRemoveMiss;

                currentSongSpeed = levelData.gameplayCoreSetupData.gameplayModifiers.songSpeedMul;

                levelData.didFinishEvent += LevelData_didFinishEvent;

                if (!Multiplayer.MultiMain.multiActive)
                {
                    if (GMPUI.chatIntegration && ChatConfig.maxCharges > 0 && AsyncInstalled)
                        chatPowers.AddComponent<GMPDisplay>();
                    if (GMPUI.chatIntegration && ChatConfig.timeForCharges > 0 && AsyncInstalled)
                        twitchPowers.StartCoroutine(TwitchPowers.ChargeOverTime());
                }



                pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().First();
                var colors = Resources.FindObjectsOfTypeAll<SimpleColorSO>();
                foreach (SimpleColorSO color in colors)
                {
                    //     Log(color.name);
                    if (color.name == "Color0")
                        colorA = color;
                    if (color.name == "Color1")
                        colorB = color;
                }
                oldColorA.SetColor(colorA);
                oldColorB.SetColor(colorB);


                //      Log(colorA.color.ToString());
                if (GMPUI.chatIntegration && charges <= ChatConfig.maxCharges && AsyncInstalled)
                {
                    charges += ChatConfig.chargesPerLevel;
                    if (charges > ChatConfig.maxCharges)
                        charges = ChatConfig.maxCharges;
                    //          TryAsyncMessage("Current Charges: " + charges);
                }



                //   ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                if (AudioTimeSync != null)
                {
                    songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                    if (songAudio == null)
                        Log("Audio null");
                    //              Log("Object Found");
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

                CheckGMPModifiers();

            }
        }




        private void SpawnController_ScaleRemoveMiss(BeatmapObjectSpawnController arg1, NoteController controller)
        {
            //         Log(songAudio.time.ToString());
            NoteData note = controller.noteData;
            Transform noteTransform = controller.GetField<Transform>("_noteTransform");
            //      Log("DESPAWN" + noteTransform.localScale.ToString());
            if (noteTransform.localScale.x != 1)
                noteTransform.localScale = new Vector3(1f, 1f, 1f);
            //      Log("DESPAWN" + noteTransform.localScale.ToString());
            //        if (modifiedNotes[note.id] != null)
            //           note = modifiedNotes[note.id];
            FloatBehavior behavior = noteTransform.gameObject.GetComponent<FloatBehavior>();
            if (behavior != null)
            {

                noteTransform.localPosition = new Vector3(behavior.originalX, behavior.originalY, noteTransform.localPosition.z);
                GameObject.Destroy(behavior);

            }
        }

        private void SpawnController_ScaleRemoveCut(BeatmapObjectSpawnController arg1, NoteController controller, NoteCutInfo arg3)
        {
            NoteData note = controller.noteData;
            Transform noteTransform = controller.GetField<Transform>("_noteTransform");
            //      Log("DESPAWN" + noteTransform.localScale.ToString());
            if (noteTransform.localScale.x != 1)
                noteTransform.localScale = new Vector3(1f, 1f, 1f);
            //          Log("DESPAWN" + noteTransform.localScale.ToString());



            //     if (modifiedNotes[note.id] != null)
            //          note = modifiedNotes[note.id];

            FloatBehavior behavior = noteTransform.gameObject.GetComponent<FloatBehavior>();
            if (behavior != null)
            {
                noteTransform.localPosition = new Vector3(behavior.originalX, behavior.originalY, noteTransform.localPosition.z);
                GameObject.Destroy(behavior);
            }


            if (GMPUI.oneColor)
            {
                if (!defaultRumble) return;
                _noteCutEffectSpawner = UnityEngine.Object.FindObjectOfType<NoteCutEffectSpawner>();
                if (_noteCutEffectSpawner != null)
                    _noteCutHapticEffect = ReflectionUtil.GetPrivateField<NoteCutHapticEffect>(_noteCutEffectSpawner, "_noteCutHapticEffect");
                if (_noteCutHapticEffect != null)
                    _hapticFeedbackController = ReflectionUtil.GetPrivateField<HapticFeedbackController>(_noteCutHapticEffect, "_hapticFeedbackController");
                if (_hapticFeedbackController != null)
                    _mainSettingsModelOneC = ReflectionUtil.GetPrivateField<MainSettingsModel>(_hapticFeedbackController, "_mainSettingsModel");

                if (_mainSettingsModelOneC == null) return;
                Vector3 notePos = controller.noteTransform.position;

                Vector3 leftPos = player.leftSaber.transform.position;
                leftPos += player.leftSaber.transform.forward * 0.5f;
                Vector3 rightPos = player.rightSaber.transform.position;
                rightPos += player.rightSaber.transform.forward * 0.5f;

                float leftDist = Vector3.Distance(leftPos, notePos);
                float rightDist = Vector3.Distance(rightPos, notePos);
                // Log(leftDist.ToString() + "   " + rightDist.ToString());
                _mainSettingsModelOneC.controllersRumbleEnabled = true;
                Saber.SaberType targetType = (leftDist > rightDist) ? Saber.SaberType.SaberB : Saber.SaberType.SaberA;
                if (!(Mathf.Abs(leftDist - rightDist) <= 0.2f))
                    _noteCutHapticEffect.RumbleController(targetType);
                else
                {
                    _noteCutHapticEffect.RumbleController(Saber.SaberType.SaberA);
                    _noteCutHapticEffect.RumbleController(Saber.SaberType.SaberB);
                }
                _mainSettingsModel.controllersRumbleEnabled = false;


            }



        }

        private void SpawnController_ModifiedJump(BeatmapObjectSpawnController arg1, NoteController controller)
        {
            if (GMPUI.rainbow)
            {
                Utilities.Rainbow.RandomizeColors();
                ColorManager?.RefreshColors();


            }

            Transform noteTransform = controller.GetField<Transform>("_noteTransform");
            //       noteTransform.Translate(0f, 4f, 0f);
            if (GMPUI.funky)
            {
                noteTransform.gameObject.AddComponent<FloatBehavior>();


            }


            //          Transform noteTransform = controller.GetField<Transform>("_noteTransform");
            //       Log("SPAWN" + noteTransform.localScale.ToString());
            if (GMPUI.chatIntegration || GMPUI.randomSize || Multiplayer.MultiMain.multiActive)
            {
                if (superRandom)
                {
                    noteTransform.localScale *= UnityEngine.Random.Range(ChatConfig.randomMin, ChatConfig.randomMax);

                }
                else
                {
                    if (!GMPUI.randomSize)
                    {
                        noteTransform.localScale *= altereddNoteScale;

                    }

                    if (GMPUI.randomSize)
                    {
                        noteTransform.localScale *= UnityEngine.Random.Range(ChatConfig.randomMin, ChatConfig.randomMax);

                    }
                }


            }

            //     Log("SPAWN" + noteTransform.localScale.ToString());

            if (GMPUI.fixedNoteScale != 1f)
            {

                //    Transform noteTransform = controller.GetField<Transform>("_noteTransform");
                //       Log("SPAWN" + noteTransform.localScale.ToString());
                noteTransform.localScale *= GMPUI.fixedNoteScale;
                //     Log("SPAWN" + noteTransform.localScale.ToString());
            }





        }

        private void LevelData_didFinishEvent(StandardLevelSceneSetupDataSO arg1, LevelCompletionResults arg2)
        {
            if (arg2.levelEndStateType == LevelCompletionResults.LevelEndStateType.Quit) return;
            if (arg2.levelEndStateType == LevelCompletionResults.LevelEndStateType.Restart) return;
            if (GMPUI.repeatSong)
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
        }

        public void OnUpdate()
        {

            if (multiInstalled)
                multi.Update();








            if (soundIsPlaying == true && songAudio != null && isValidScene == true)
            {
                SetTimeScale(0f); ;
                Time.timeScale = 0f;
                return;
            }

            if (GMPUI.bulletTime == true && isValidScene == true && soundIsPlaying == false)
            {
                SetTimeScale(1 - (leftController.triggerValue + rightController.triggerValue) / 2);
                Time.timeScale = timeScale;
                //    Time.fixedDeltaTime = timeScale;
                return;
            }

            /*
                        if (GMPUI.superHot == true && playerInfo == true && soundIsPlaying == false && isValidScene == true && startGMPUI.superHot == true)
                        {
                            speedPitch = (leftSaber.bladeSpeed / 15 + rightSaber.bladeSpeed / 15) / 1.5f;
                            if (speedPitch > 1)
                                speedPitch = 1;
                            ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", speedPitch);
                            Time.timeScale = speedPitch;
            */


            else
            {
                Time.timeScale = 1f;
            }
            if (playerInfo == true)
                if (player.disableSabers == true)
                    Time.timeScale = 1;
        }

        public void OnFixedUpdate()
        {
        }

        public static void ResetCustomColorsSabers(Color left, Color right)
        {
            CustomColors.Plugin.OverrideCustomSaberColors(left, right);
        }
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
            ChatConfig.Load();
            //  GMPUI.gnomeOnMiss = ModPrefs.GetBool("GameplayModifiersPlus", "GMPUI.gnomeOnMiss", false, true);
            //   GMPUI.superHot = ModPrefs.GetBool("GameplayModifiersPlus", "GMPUI.superHot", false, true);
            //   GMPUI.bulletTime = ModPrefs.GetBool("GameplayModifiersPlus", "GMPUI.bulletTime", false, true);
            //  GMPUI.chatIntegration = ModPrefs.GetBool("GameplayModifiersPlus", "GMPUI.chatIntegration", false, true);
            //    GMPUI.swapSabers = ModPrefs.GetBool("GameplayModifiersPlus", "swapSabers", false, true);
            GMPUI.chatDelta = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
            GMPUI.allowMulti = ModPrefs.GetBool("GameplayModifiersPlus", "allowMulti", false, true);
        }

        public IEnumerator GrabPP()
        {
            yield return new WaitForSecondsRealtime(1f);
            var texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            if (texts != null)
                foreach (TMP_Text text in texts)
                {
                    if (text != null)
                        if (text.ToString() == "PP (TMPro.TextMeshPro)")
                        {
                            ppText = text;
                            break;

                        }

                }
            yield return new WaitForSecondsRealtime(9f);
            if (ppText != null)
            {
                try
                {
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
                        //        if (firstLoad == true)
                        //           if (GMPUI.chatDelta)
                        //                 TryAsyncMessage("Loaded. PP: " + currentpp + " pp. Rank: " + currentRank);

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
                                        if (GMPUI.chatDelta)
                                            TryAsyncMessage("Gained " + deltaPP + " pp. Gained 1 Rank.");
                                        ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained 1 Rank";
                                    }

                                    else
                                    {
                                        if (GMPUI.chatDelta)
                                            TryAsyncMessage("Gained " + deltaPP + " pp. Gained " + Math.Abs(deltaRank) + " Ranks.");
                                        ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained " + Math.Abs(deltaRank) + " Ranks";
                                    }

                                }
                                else if (deltaRank == 0)
                                {
                                    if (GMPUI.chatDelta)
                                        TryAsyncMessage("Gained " + deltaPP + " pp. No change in Rank.");
                                    ppText.text += " Change: Gained " + deltaPP + " pp. " + "No change in Rank";
                                }

                                else if (deltaRank > 0)
                                {
                                    if (deltaRank == 1)
                                    {
                                        if (GMPUI.chatDelta)
                                            TryAsyncMessage("Gained " + deltaPP + " pp. Lost 1 Rank.");
                                        ppText.text += " Change: Gained " + deltaPP + " pp. " + "Lost 1 Rank";
                                    }

                                    else
                                    {
                                        if (GMPUI.chatDelta)
                                            TryAsyncMessage("Gained " + deltaPP + " pp. Lost " + Math.Abs(deltaRank) + " Ranks.");
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
                }
                catch
                {
                    Log("Exception Trying to Grab PP");
                }

            }
            firstLoad = false;
        }

        public static void SetTimeScale(float value)
        {
            timeScale = value;
            if ((timeScale != 1))
            {

                if (AudioTimeSync != null)
                {
                    AudioTimeSync.forcedAudioSync = true;
                }
            }
            else
            {
                if (AudioTimeSync != null)
                {
                    AudioTimeSync.forcedAudioSync = false;
                }
            }

            if (songAudio != null)
            {
                songAudio.pitch = timeScale;
            }
        }

        internal static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        public static bool IsCustomColorsDisabled()
        {
            return CustomColors.Plugin.disablePlugin;
        }
        public static bool DoesCustomColorsAllowEnvironmentColors()

        {
            return CustomColors.Plugin.allowEnvironmentColors;
        }
        public static void ApplyPatches()
        {
            Log("Apply Patch Function");
            try
            {

                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                Log("Applying Harmony Patches");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
       
        }

        public static void RemovePatches()
        {
            Log("Remove Patch Function: " + invalidForScoring);
            if (invalidForScoring)
            {
                harmony.UnpatchAll("com.kyle1413.BeatSaber.GamePlayModifiersPlus");
                invalidForScoring = false;
                Log("Unblocking Score Submission, Removing Harmony Patches");
            }
        }

        public static void CheckGMPModifiers()
        {
            if (GMPUI.bulletTime || GMPUI.swapSabers || GMPUI.fiveLanes || GMPUI.laneShift ||GMPUI.sixLanes || GMPUI.reverse || GMPUI.chatIntegration || GMPUI.funky || GMPUI.oneColor || GMPUI.gnomeOnMiss || GMPUI.njsRandom || GMPUI.noArrows || GMPUI.randomSize || GMPUI.fixedNoteScale != 1f || GMPUI.offsetrandom)
            {
                //     ApplyPatches();
                UnityEngine.Random.InitState(Plugin.levelData.difficultyBeatmap.beatmapData.notesCount);
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("Gameplay Modifiers Plus");

                if (GMPUI.njsRandom || GMPUI.offsetrandom)
                {
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.RandomNjsOrOffset());
                }


                if (GMPUI.sixLanes || GMPUI.fourLayers || GMPUI.fiveLanes || GMPUI.laneShift)
                {
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.ExtraLanes());
                }
                if (GMPUI.noArrows)
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.NoArrows());
                if (GMPUI.swapSabers)
                {
                    Log("Testing Ground Active");
                    try
                    {
                        SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.TestingGround(10f));
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                    }
                }
                if (GMPUI.oneColor)
                {
                    Log("One Color Activating");
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.OneColor());
                }

                if(GMPUI.reverse)
                {
                    Log("Map Reversal");
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.PermaReverse());
                }

                if (GMPUI.gnomeOnMiss == true)
                {

                    if (spawnController != null)
                    {
                        spawnController.noteWasMissedEvent += delegate (BeatmapObjectSpawnController beatmapObjectSpawnController2, NoteController noteController)
                        {
                            if (noteController.noteData.noteType != NoteType.Bomb)
                            {
                                try
                                {
                                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.SpecialEvent());
                                    Log("Gnoming");
                                }
                                catch (Exception ex)
                                {
                                    Log(ex.ToString());
                                }
                            }
                        };

                        spawnController.noteWasCutEvent += delegate (BeatmapObjectSpawnController beatmapObjectSpawnController2, NoteController noteController, NoteCutInfo noteCutInfo)
                        {
                            if (!noteCutInfo.allIsOK)
                            {
                                SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.SpecialEvent());
                                Log("Gnoming");
                            }

                        };

                    }
                }


            }
        }


        internal void CheckPlugins()
        {
            foreach (IPlugin plugin in PluginManager.Plugins)
            {
                switch (plugin.Name)
                {
                    case "Asynchronous Twitch Library":
                        AsyncInstalled = true;
                        break;

                    case "Beat Saber Multiplayer":
                        multi = new GamePlayModifiersPlus.Multiplayer.MultiMain();
                        multi.Initialize();
                        multiInstalled = true;
                        Log("Multiplayer Detected, enabling multiplayer functionality");
                        break;

                    case "CustomColorsEdit":
                        customColorsInstalled = true;
                        break;

                    case "BeatSaberChallenges":
                        ChallengeIntegration.AddListeners();
                        break;
                }

            }
        }

        internal void InitAsync()
        {
            if (_hasRegistered == false)
            {
                try
                {
                    TwitchConnection.Instance.StartConnection();
                    TwitchConnection.Instance.RegisterOnMessageReceived(TwitchConnection_OnMessageReceived);
                    if (multiInstalled)
                        TwitchConnection.Instance.RegisterOnMessageReceived(multi.TwitchConnectionMulti_OnMessageReceived);
                    _hasRegistered = true;
                }
                catch (Exception ex)
                {
                    Log("Failed To Connect with Async Twitch, Check Config and Internet Connection");
                    Log(ex.ToString());
                }

            }

        }


        internal static void TryAsyncMessage(string message)
        {
            if (!AsyncInstalled) return;
            SendAsyncMessage(message);
        }
        internal static void SendAsyncMessage(string message)
        {
            TwitchConnection.Instance.SendChatMessage(message);
        }
    }
}
