namespace GamePlayModifiersPlus
{
    //using AsyncTwitch;
    //using IllusionInjector;
    //using IllusionPlugin;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Media;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using HarmonyLib;
    using GamePlayModifiersPlus.TwitchStuff;
    using GamePlayModifiersPlus.Utilities;
    using IPA.Old;
    using IPA.Config;
    using StreamCore;
    using StreamCore.Chat;
    using StreamCore.Config;
    using IPA.Loader;
    using IPA;
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static BS_Utils.Utilities.Config ChatConfigSettings = new BS_Utils.Utilities.Config("GameplayModifiersPlus");
        internal static IPA.Logging.Logger log;

        internal static bool mappingExtensionsPresent = false;
        public static float timeScale = 1;
        Multiplayer.MultiMain multi = null;
        public static bool multiInstalled = false;
        internal static bool practicePluginInstalled = true;
        internal static bool modifiersInit = false;
        public static TwitchCommands twitchCommands = new TwitchCommands();
        public static TwitchPowers twitchPowers = null;
        public static SoundPlayer gnomeSound = new SoundPlayer(Properties.Resources.gnome);
        public static SoundPlayer beepSound = new SoundPlayer(Properties.Resources.Beep);
        public static SoundPlayer reverseSound = new SoundPlayer(Properties.Resources.sectionpass);
        public static bool soundIsPlaying = false;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        public static AudioManagerSO Mixer { get; private set; }
        public static AudioSource songAudio;
        public static bool isValidScene = false;
        public static bool gnomeActive = false;
        public static PlayerController player;
        public static bool playerInfo = false;
        public static float prevLeftPos;
        public static float prevRightPos;
        public static float prevHeadPos;
        public static bool calculating = false;
        public static bool paused = false;
        public static Cooldowns cooldowns;
        public static TextMeshProUGUI ppText;
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
        public static BS_Utils.Gameplay.LevelData levelData;
        private static bool invalidForScoring = false;
        private static bool _hasRegistered = false;
        public static BeatmapObjectManager beatmapObjectManager;
        public static BeatmapObjectSpawnController spawnController;
        public static GameEnergyCounter energyCounter;
        private static GameEnergyUIPanel _energyPanel;
        public static GameEnergyUIPanel energyPanel
        {
            get
            {
                if (_energyPanel == null)
                    _energyPanel = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().First();
                return _energyPanel;
            }
            set
            {
                _energyPanel = value;
            }
        }

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
    //    public static SimpleColorSO colorA;
    //    public static SimpleColorSO colorB;
    //    public static SimpleColorSO oldColorA = ScriptableObject.CreateInstance<SimpleColorSO>();
    //    public static SimpleColorSO oldColorB = ScriptableObject.CreateInstance<SimpleColorSO>();
    //    public static SimpleColorSO defColorA = ScriptableObject.CreateInstance<SimpleColorSO>();
    //    public static SimpleColorSO defColorB = ScriptableObject.CreateInstance<SimpleColorSO>();
        public static int commandsLeftForMessage;
        public static bool test;
        public static float currentSongSpeed;
        public static StandardLevelGameplayManager pauseManager;
        public static NoteCutSoundEffectManager soundEffectManager;
        public static bool defaultRumble = true;
        static bool setDefaultRumble = false;
        static NoteCutEffectSpawner _noteCutEffectSpawner;
        static NoteCutHapticEffect _noteCutHapticEffect;
        static HapticFeedbackController _hapticFeedbackController;
        static MainSettingsModelSO _mainSettingsModel;
        static BoolSO _RumbleEnabledOneC;
        static ColorManager ColorManager;
        public static bool activateDuringIsolated = false;
        public static Harmony harmony;
        internal static bool twitchPluginInstalled = false;
        GameObject chatPowers = null;
        static ColorScheme GMPColorScheme = new ColorScheme("GMPColorScheme", "GMP Color Scheme", false, Color.white, Color.white, Color.white, Color.white, Color.white);
        static ColorScheme oldColorScheme = null;
        [OnStart]
        public void OnApplicationStart()
        {

            Log("Creating Harmony Instance");
            harmony = new Harmony("com.kyle1413.BeatSaber.GamePlayModifiersPlus");
            ApplyPatches();
            CheckPlugins();

            if (twitchPluginInstalled)
            {
                InitStreamCore();
            }
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
            if (ModPrefs.GetInt("GameplayModifiersPlus", "GameRumbleSetting", -1, false) != -1)
            {
                Log("Rumble Key Exists");
                setDefaultRumble = true;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("GameplayModifiersPlus", "GamePlayModifiersPlus.Utilities.GMPUI.bsml", GMPUI.instance);

        }

        [Init]
        public void Init(IPA.Logging.Logger logger)
        {
            log = logger;
        }

        public static void ResetColors()
        {
            if (!ColorManager) return;
            if (oldColorScheme == null)
                SetupColors();
            ColorManager.SetField("_colorScheme", oldColorScheme);
          //  ColorManager.GetField<SimpleColorSO>("_saberAColor").SetColor(ColorManager.GetField<ColorScheme>("_colorScheme").saberAColor);
          //  ColorManager.GetField<SimpleColorSO>("_saberBColor").SetColor(ColorManager.GetField<ColorScheme>("_colorScheme").saberBColor);
        }
        public static void SetupColors()
        {
            oldColorScheme = ColorManager.GetField<ColorScheme>("_colorScheme");
            GMPColorScheme.SetField("_saberAColor", oldColorScheme.saberAColor);
            GMPColorScheme.SetField("_saberBColor", oldColorScheme.saberBColor);
            GMPColorScheme.SetField("_environmentColor0", oldColorScheme.environmentColor0);
            GMPColorScheme.SetField("_environmentColor1", oldColorScheme.environmentColor1);
            GMPColorScheme.SetField("_obstaclesColor", oldColorScheme.obstaclesColor);
        }
        public static void SetColors(Color left, Color right)
        {
            if (!ColorManager) return;
            if (oldColorScheme == null)
                SetupColors();
            if(ColorManager.GetField<ColorScheme>("_colorScheme") != GMPColorScheme)
                ColorManager.SetField("_colorScheme", GMPColorScheme);

           GMPColorScheme.SetField("_saberAColor", left);
           GMPColorScheme.SetField("_saberBColor",right);

            //ColorManager.GetField<SimpleColorSO>("_saberAColor").SetColor(left);
            //ColorManager.GetField<SimpleColorSO>("_saberBColor").SetColor(right);
        }
        private void InitStreamCore()
        {
            //    TwitchStuff.ChatMessageHandler messageHandler = new GameObject("GMP Chat Message Handler").AddComponent<ChatMessageHandler>();
            //    GameObject.DontDestroyOnLoad(messageHandler.gameObject);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {

            if (scene.name == "MenuCore")
            {
                ReadPrefs();
                try
                {
                  //  GMPUI.CreateUI();
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                }
          //      if (multiInstalled)
               //     Multiplayer.MultiClientInterface.Init();

            }
        }

        public void OnActiveSceneChanged(Scene arg0, Scene scene)
        {

            if (scene.name == "MenuViewControllers")
            {
                activateDuringIsolated = false;
                Log("Switched to Menu");
                SharedCoroutineStarter.instance.StartCoroutine(GrabPP());

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

            if (_mainSettingsModel == null)
            {
                var menu = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().FirstOrDefault();
                if (menu != null)
                {
                    _mainSettingsModel = menu.GetField<MainSettingsModelSO>("_mainSettingsModel");
                    _mainSettingsModel.Load(true);
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
                _mainSettingsModel.controllersRumbleEnabled.value = defaultRumble;
                _mainSettingsModel.Save();
            }


            paused = false;


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
            if (GMPUI.chatIntegration && twitchPluginInstalled)
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
            modifiersInit = false;
            if (arg0.name == "EmpyTransition" && chatPowers != null)
                GameObject.Destroy(chatPowers);

            if (scene.name == "GameCore")
            {
                Log("Isolated: " + BS_Utils.Gameplay.Gamemode.IsIsolatedLevel);
                isValidScene = true;
                if (BS_Utils.Gameplay.Gamemode.IsIsolatedLevel && !activateDuringIsolated)
                {
                    Log("Isolated Level, not activating");
                    return;
                }
                //     Log("Pre GrabGrab");
                GameObject.Destroy(GameObject.Find("Color Setter"));
                soundEffectManager = Resources.FindObjectsOfTypeAll<NoteCutSoundEffectManager>().FirstOrDefault();
                beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectManager>().FirstOrDefault();
                spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
                energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().First();
                ColorManager = Resources.FindObjectsOfTypeAll<ColorManager>().Last();
                oldColorScheme = null;
                levelData = BS_Utils.Plugin.LevelData;
                //    Log("Post GrabGrab");
                if (beatmapObjectManager != null)
                {
                    beatmapObjectManager.noteDidStartJumpEvent += SpawnController_ModifiedJump;
                    beatmapObjectManager.noteWasCutEvent += SpawnController_ScaleRemoveCut;
                    beatmapObjectManager.noteWasMissedEvent += SpawnController_ScaleRemoveMiss;

                }
                else Log("Spawn Controller Null");
                //   Log("Post GrabGrab 2");
                currentSongSpeed = levelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;

                BS_Utils.Plugin.LevelDidFinishEvent += LevelData_didFinishEvent;
                //   Log("Post GrabGrab 3");
                if (!Multiplayer.MultiMain.multiActive.Value)
                {
                    if (twitchPluginInstalled)
                        chatPowers.AddComponent<GMPDisplay>();
                    if (GMPUI.chatIntegration && ChatConfig.timeForCharges > 0 && twitchPluginInstalled)
                        twitchPowers.StartCoroutine(TwitchPowers.ChargeOverTime());
                }
                //   Log("Post GrabGrab 4");


                pauseManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().FirstOrDefault();
                //      Log("Pre ChatInt");

                //      Log(colorA.color.ToString());
                if (GMPUI.chatIntegration && charges <= ChatConfig.maxCharges && twitchPluginInstalled)
                {
                    charges += ChatConfig.chargesPerLevel;
                    if (charges > ChatConfig.maxCharges)
                        charges = ChatConfig.maxCharges;
                    //          TryAsyncMessage("Current Charges: " + charges);
                }


                //  Log("Pre Audio/Player");
                //   ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                if (AudioTimeSync != null)
                {
                    songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                    if (songAudio == null)
                        Log("Audio null");
                    //              Log("Object Found");
                }
                var gameCoreSceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().FirstOrDefault();
                Mixer = gameCoreSceneSetup.GetPrivateField<AudioManagerSO>("_audioMixer");
                //Get Sabers
                player = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();
                if (player != null)
                {
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


        private void SpawnController_ScaleRemoveMiss(INoteController controller)
        {
            //         Log(songAudio.time.ToString());
            NoteData note = controller.noteData;
            Transform noteTransform = controller.noteTransform;
            //      Log("DESPAWN" + noteTransform.localScale.ToString());
            if (noteTransform?.localScale.x != 1)
                noteTransform.localScale = new Vector3(1f, 1f, 1f);
            //      Log("DESPAWN" + noteTransform.localScale.ToString());
            //        if (modifiedNotes[note.id] != null)
            //           note = modifiedNotes[note.id];
            FloatBehavior behavior = noteTransform?.gameObject.GetComponent<FloatBehavior>();
            if (behavior != null)
            {

                noteTransform.localPosition = new Vector3(behavior.originalX, behavior.originalY, noteTransform.localPosition.z);
                GameObject.Destroy(behavior);

            }
        }

        private void SpawnController_ScaleRemoveCut(INoteController controller, NoteCutInfo arg3)
        {
            NoteData note = controller.noteData;
            Transform noteTransform = controller.noteTransform;
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
                    _RumbleEnabledOneC = ReflectionUtil.GetPrivateField<BoolSO>(_hapticFeedbackController, "_controllersRumbleEnabled");

                if (_RumbleEnabledOneC == null) return;
                Vector3 notePos = controller.noteTransform.position;

                Vector3 leftPos = player.leftSaber.transform.position;
                leftPos += player.leftSaber.transform.forward * 0.5f;
                Vector3 rightPos = player.rightSaber.transform.position;
                rightPos += player.rightSaber.transform.forward * 0.5f;

                float leftDist = Vector3.Distance(leftPos, notePos);
                float rightDist = Vector3.Distance(rightPos, notePos);
                // Log(leftDist.ToString() + "   " + rightDist.ToString());
                _RumbleEnabledOneC.value = true;
                SaberType targetType = (leftDist > rightDist) ? SaberType.SaberB : SaberType.SaberA;
                if (!(Mathf.Abs(leftDist - rightDist) <= 0.2f))
                    _noteCutHapticEffect.HitNote(targetType);
                else
                {
                    _noteCutHapticEffect.HitNote(SaberType.SaberA);
                    _noteCutHapticEffect.HitNote(SaberType.SaberB);
                }
                _RumbleEnabledOneC.value = false;


            }



        }

        private void SpawnController_ModifiedJump(NoteController controller)
        {
            if (GMPUI.rainbow)
            {
                Utilities.Rainbow.RandomizeColors();


            }

            Transform noteTransform = controller.noteTransform;
            //       noteTransform.Translate(0f, 4f, 0f);
            if (GMPUI.funky)
            {
                noteTransform?.gameObject.AddComponent<FloatBehavior>();


            }


            //       Log("SPAWN" + noteTransform.localScale.ToString());
            if (GMPUI.chatIntegration || GMPUI.randomSize || Multiplayer.MultiMain.multiActive.Value)
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

        private void LevelData_didFinishEvent(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {
            if (arg2.levelEndAction == LevelCompletionResults.LevelEndAction.Quit) return;
            if (arg2.levelEndAction == LevelCompletionResults.LevelEndAction.Restart) return;
            if (GMPUI.repeatSong)
                ReflectionUtil.SetProperty(arg2, "levelEndAction", LevelCompletionResults.LevelEndAction.Restart);
        }
        [OnExit]
        public void OnApplicationQuit()
        {

        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }


        public static void ResetCustomColorsSabers(Color left, Color right)
        {
          //  CustomColors.Plugin.OverrideCustomSaberColors(left, right);
        }
        public static bool isModInstalled(string modName)
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
            log.Debug(message);
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
            GMPUI.disableFireworks = ModPrefs.GetBool("GameplayModifiersPlus", "DisableFireworks", false, false);
            GMPUI.chatDelta = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
            GMPUI.allowMulti = ModPrefs.GetBool("GameplayModifiersPlus", "allowMulti", false, true);
        }

        public IEnumerator GrabPP()
        {
            yield return new WaitForSecondsRealtime(0f);
            //
            //
            var texts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            if (texts != null)
                foreach (TextMeshProUGUI text in texts)
                {
                    if (text != null)
                        if (text.ToString().Contains("CustomUIText") && text.text.Contains("pp"))
                        {
                            ppText = text;
                            break;

                        }

                }
            yield return new WaitForSecondsRealtime(8f);
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
                        if (firstLoad == true)
                            if (GMPUI.chatDelta)
                            {
                                TryAsyncMessage("Loaded. PP: " + currentpp + " pp. Rank: " + currentRank);
                                firstLoad = false;
                            }


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
            //     firstLoad = false;
        }


        internal static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
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

        public static void SetTimeScale(float value)
        {
            if (AudioTimeSync == null) return;
            timeScale = value;
            AudioTimeSyncController.InitData initData = AudioTimeSync.GetPrivateField<AudioTimeSyncController.InitData>("_initData");
            AudioTimeSyncController.InitData newInitData = new AudioTimeSyncController.InitData(initData.audioClip,
                AudioTimeSync.songTime, initData.songTimeOffset, timeScale);
            AudioTimeSync.SetPrivateField("_initData", newInitData);
            //Chipmunk Removal as per base game

            if (timeScale == 1f)
                Mixer.musicPitch = 1;
            else
                Mixer.musicPitch = 1f / timeScale;

            ResetTimeSync(AudioTimeSync, timeScale, newInitData);
        }

        public static void ResetTimeSync(AudioTimeSyncController timeSync, float newTimeScale, AudioTimeSyncController.InitData newData)
        {
            //   timeSync.SetField("_timeScale", newData.timeScale);
            //   AudioSource audioSource = timeSync.GetField<AudioSource>("_audioSource");
            //   audioSource.pitch = newData.timeScale;
            //   timeSync.SetField("_startSongTime", newData.startSongTime);
            //   float songTimeOffset = newData.songTimeOffset + timeSync.GetField<FloatSO>("_audioLatency");
            //  timeSync.SetField("_songTimeOffset", songTimeOffset);
            //  timeSync.SetField("_songTime", newData.startSongTime);
            //    float num = newData.startSongTime + songTimeOffset;
            //  audioSource.time = num;
            //     timeSync.SetField("_audioStartTimeOffsetSinceStart", timeSync.GetProperty<float>("timeSinceStart") - num);
            //    timeSync.SetField("_fixingAudioSyncError", false);
            //  timeSync.SetField("_prevAudioSamplePos", (int)((float)audioSource.clip.frequency * num));
            // timeSync.SetField("_playbackLoopIndex", 0);
            //     timeSync.SetField("_dspTimeOffset", AudioSettings.dspTime - (double)num);
            //    timeSync.SetField("_songTime", newData.startSongTime);
            timeSync.SetPrivateField("_timeScale", newTimeScale);
            timeSync.SetPrivateField("_startSongTime", timeSync.songTime);
            timeSync.SetPrivateField("_audioStartTimeOffsetSinceStart", timeSync.GetProperty<float>("timeSinceStart") - (timeSync.songTime + newData.songTimeOffset));
            timeSync.SetPrivateField("_fixingAudioSyncError", false);
            timeSync.SetPrivateField("_playbackLoopIndex", 0);
            timeSync.audioSource.pitch = newTimeScale;
        }

        public static void CheckGMPModifiers()
        {
            //   Log($"badNote: {energyCounter.GetField("_badNoteEnergyDrain")}");
            //   Log($"missNote: {energyCounter.GetField("_missNoteEnergyDrain")}");
            //   Log($"goodNote: {energyCounter.GetField("_goodNoteEnergyCharge")}");
            //   Log($"obstaclePerSec: {energyCounter.GetField("_obstacleEnergyDrainPerSecond")}");
            //   Log($"obstaclePerSec: {energyCounter.GetField("_obstacleEnergyDrainPerSecond")}");
            //   Log($"hitBomb: {energyCounter.GetField("_hitBombEnergyDrain")}");


            if (GMPUI.removeCrouchWalls || GMPUI.swapSabers || GMPUI.fiveLanes || GMPUI.angleShift || GMPUI.laneShift || GMPUI.sixLanes || GMPUI.fourLayers || GMPUI.reverse || GMPUI.chatIntegration || GMPUI.funky || GMPUI.oneColor ||  GMPUI.njsRandom || GMPUI.noArrows || GMPUI.randomSize || GMPUI.fixedNoteScale != 1f || GMPUI.offsetrandom)
            {
                //     ApplyPatches();
                UnityEngine.Random.InitState(Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.beatmapData.notesCount);
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("GameplayModifiersPlus");

                if (GMPUI.njsRandom || GMPUI.offsetrandom)
                {
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.RandomNjsOrOffset());
                }

                if (GMPUI.removeCrouchWalls)
                {
                    if (levelData.GameplayCoreSceneSetupData.gameplayModifiers.enabledObstacleType != GameplayModifiers.EnabledObstacleType.NoObstacles)
                        levelData.GameplayCoreSceneSetupData.gameplayModifiers.enabledObstacleType = GameplayModifiers.EnabledObstacleType.FullHeightOnly;
                }
                if (GMPUI.sixLanes || GMPUI.fourLayers || GMPUI.fiveLanes || GMPUI.laneShift || GMPUI.angleShift)
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

                if (GMPUI.reverse)
                {
                    Log("Map Reversal");
                    SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.PermaReverse());
                }

                

                modifiersInit = true;
            }

            if (GMPUI.tunnel)
            {
                Log("Tunnel Activating");
                SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.PermaEncasement(0f));
            }
        }


        internal void CheckPlugins()
        {
            foreach (IPlugin plugin in IPA.Loader.PluginManager.Plugins)
            {
                switch (plugin.Name)
                {
                    case "StreamCore":
                    case "Stream Core":
                        twitchPluginInstalled = true;
                        break;

                    case "Beat Saber Multiplayer":
                        //        multi = new GamePlayModifiersPlus.Multiplayer.MultiMain();
                        //      multi.Initialize();
                        //    multiInstalled = true;
                        //  Log("Multiplayer Detected, enabling multiplayer functionality");
                        break;

             //       case "BeatSaberChallenges":
             //           ChallengeIntegration.AddListeners();
            //            break;
                    case "MappingExtensions":
                        mappingExtensionsPresent = true;
                        break;

                }

            }
            foreach (var plugin in IPA.Loader.PluginManager.AllPlugins)
            {
                switch (plugin.Id)
                {
                    case "Stream Core":
                        twitchPluginInstalled = true;
                        break;

                    case "Beat Saber Multiplayer":
                        //        multi = new GamePlayModifiersPlus.Multiplayer.MultiMain();
                        //      multi.Initialize();
                        //    multiInstalled = true;
                        //  Log("Multiplayer Detected, enabling multiplayer functionality");
                        break;

               //     case "BeatSaberChallenges":
               //         ChallengeIntegration.AddListeners();
               //         break;
                    case "MappingExtensions":
                        mappingExtensionsPresent = true;
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
                    //honestly too lazy to remove this cause i dont wanna break anything
                    _hasRegistered = true;
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                }

            }

        }


        internal static void TryAsyncMessage(string message)
        {
            if (!twitchPluginInstalled) return;
            SendAsyncMessage(message);
        }
        internal static void SendAsyncMessage(string message)
        {
            StreamCore.Twitch.TwitchWebSocketClient.SendMessage(message);
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
