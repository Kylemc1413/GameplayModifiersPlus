namespace GamePlayModifiersPlus
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using HarmonyLib;
    using GamePlayModifiersPlus.TwitchStuff;
    using GamePlayModifiersPlus.Utilities;
    using IPA.Config;
    using IPA;
    using System.Net.Http;
    using System.Reflection;
    using SiraUtil.Zenject;
    using Zenject;

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static BS_Utils.Utilities.Config ConfigSettings = new BS_Utils.Utilities.Config("GameplayModifiersPlus");
        internal static IPA.Logging.Logger log;

        internal static HttpClient client;

        internal static bool mappingExtensionsPresent = false;
        internal static bool twitchPluginInstalled = false;
        internal static bool songRequestPluginInstalled = false;
        internal static bool gameSaberPluginInstalled = false;
        public static bool isValidScene = false;
        public static bool firstLoad = true;

        public static TwitchCommands twitchCommands = new TwitchCommands();
        public static TwitchPowers twitchPowers = null;
        public static GameObject chatIntegrationObj;
        public static Cooldowns cooldowns;
        internal static GameObject chatPowers = null;


        public static BS_Utils.Gameplay.LevelData levelData;
        public static bool activateDuringIsolated = false;
        public static Harmony harmony;


        [Init]
        public void Init(IPA.Logging.Logger logger, Zenjector zenjector)
        {
            log = logger;
            zenjector.Install<GmpInstaller>(Location.StandardPlayer);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent", $"GameplayModifiersPlus/{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            }

            Log("Creating Harmony Instance");
            harmony = new Harmony("com.kyle1413.BeatSaber.GamePlayModifiersPlus");
            ApplyPatches();
            CheckPlugins();
            Config.Load();
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
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("GameplayModifiersPlus", "GamePlayModifiersPlus.Utilities.GMPUI.bsml", GMPUI.instance);
            if (twitchPluginInstalled)
                InitChatCore();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }


        public void InitChatCore()
        {
            ChatMessageHandler.Load();
        }



        public IEnumerator LoadRealityCheckAudio()
        {
            AssetBundle realityAudioBundle = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GamePlayModifiersPlus.Resources.RealityCheck.rcttsaudio"));
            var asset = realityAudioBundle.LoadAssetAsync("RCTTS", typeof(AudioClip));
            yield return asset.isDone;
            TwitchPowers.RealityClip = (AudioClip)asset.asset;
            realityAudioBundle.Unload(false);
        }

        public IEnumerator LoadWorkoutAudio()
        {
            AssetBundle workoutAudio = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GamePlayModifiersPlus.Resources.Workout.workoutaudio"));
            var asset = workoutAudio.LoadAssetAsync("Workout", typeof(AudioClip));
            yield return asset.isDone;
            TwitchPowers.WorkoutClip = (AudioClip)asset.asset;
            workoutAudio.Unload(false);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {

            if (scene.name == BS_Utils.SceneNames.Menu)
            {
                if (TwitchPowers.RealityClip == null)
                    SharedCoroutineStarter.instance.StartCoroutine(LoadRealityCheckAudio());
                if (TwitchPowers.WorkoutClip == null)
                    SharedCoroutineStarter.instance.StartCoroutine(LoadWorkoutAudio());
                ReadPrefs();
            }
        }

        public void OnActiveSceneChanged(Scene arg0, Scene scene)
        {

            if (scene.name == BS_Utils.SceneNames.Menu)
            {
                activateDuringIsolated = false;
            }

            if (scene.name == "EmptyTransition")
            {
                Log("Resetting Chat Powers Object");
                if (chatPowers != null)
                    GameObject.Destroy(chatPowers);
            }
            if (chatPowers == null)
            {
                //     Log("Null Creation of Chat Powers Object");
                chatPowers = new GameObject("Chat Powers");
                GameObject.DontDestroyOnLoad(chatPowers);
                twitchPowers = chatPowers.AddComponent<TwitchPowers>();

            }

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
                if (Config.resetChargesEachLevel)
                    GameModifiersController.charges = 0;


            }

            isValidScene = false;
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
                if (!BS_Utils.Plugin.LevelData.IsSet || BS_Utils.Plugin.LevelData.Mode != BS_Utils.Gameplay.Mode.Standard) return;
                ColorController.oldColorScheme = null;
                levelData = BS_Utils.Plugin.LevelData;

                GameObjects.Load();

                GameModifiersController.currentSongSpeed = levelData.GameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;

                chatPowers.AddComponent<GMPDisplay>();
                if (GMPUI.chatIntegration && Config.timeForCharges > 0 && twitchPluginInstalled)
                    twitchPowers.StartCoroutine(TwitchPowers.ChargeOverTime());

                if (GMPUI.chatIntegration && GameModifiersController.charges <= Config.maxCharges && twitchPluginInstalled)
                {
                    GameModifiersController.charges += Config.chargesPerLevel;
                    if (GameModifiersController.charges > Config.maxCharges)
                        GameModifiersController.charges = Config.maxCharges;
                }

               SharedCoroutineStarter.instance.StartCoroutine(GameModifiersController.CheckGMPModifiers());
            }
        }


        public static void Log(string message)
        {
            log.Debug(message);
        }

        public void ReadPrefs()
        {
            Config.Load();
       //     GMPUI.disableFireworks = ModPrefs.GetBool("GameplayModifiersPlus", "DisableFireworks", false, false);
       //     GMPUI.chatDelta = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
        }

        /*
                public static TextMeshProUGUI ppText;
                public static string rank;
                public static string pp;
                public static float currentpp;
                public static float oldpp = 0;
                public static int currentRank;
                public static int oldRank;
                public static float deltaPP;
                public static int deltaRank;

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
                                        ChatMessageHandler.TryAsyncMessage("Loaded. PP: " + currentpp + " pp. Rank: " + currentRank);
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
                                                    ChatMessageHandler.TryAsyncMessage("Gained " + deltaPP + " pp. Gained 1 Rank.");
                                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained 1 Rank";
                                            }

                                            else
                                            {
                                                if (GMPUI.chatDelta)
                                                    ChatMessageHandler.TryAsyncMessage("Gained " + deltaPP + " pp. Gained " + Math.Abs(deltaRank) + " Ranks.");
                                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Gained " + Math.Abs(deltaRank) + " Ranks";
                                            }

                                        }
                                        else if (deltaRank == 0)
                                        {
                                            if (GMPUI.chatDelta)
                                                ChatMessageHandler.TryAsyncMessage("Gained " + deltaPP + " pp. No change in Rank.");
                                            ppText.text += " Change: Gained " + deltaPP + " pp. " + "No change in Rank";
                                        }

                                        else if (deltaRank > 0)
                                        {
                                            if (deltaRank == 1)
                                            {
                                                if (GMPUI.chatDelta)
                                                    ChatMessageHandler.TryAsyncMessage("Gained " + deltaPP + " pp. Lost 1 Rank.");
                                                ppText.text += " Change: Gained " + deltaPP + " pp. " + "Lost 1 Rank";
                                            }

                                            else
                                            {
                                                if (GMPUI.chatDelta)
                                                    ChatMessageHandler.TryAsyncMessage("Gained " + deltaPP + " pp. Lost " + Math.Abs(deltaRank) + " Ranks.");
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

            */


        public static void ApplyPatches()
        {
        
            try
            {
                Log("Applying Harmony Patches");
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

        }


        internal void CheckPlugins()
        {
            foreach (var plugin in IPA.Loader.PluginManager.EnabledPlugins)
            {
                switch (plugin.Id)
                {
                    case "ChatCore":
                              if(!(plugin.HVersion < new Hive.Versioning.Version("1.0.0-rc5")))
                                twitchPluginInstalled = File.Exists(Path.Combine(IPA.Utilities.UnityGame.LibraryPath, "ChatCore.dll"));
                        break;
                    case "GameSaber":
                     if (plugin.HVersion >= new Hive.Versioning.Version("1.3.0"))
                            gameSaberPluginInstalled = true;
                        break;
                    //     case "BeatSaberChallenges":
                    //         ChallengeIntegration.AddListeners();
                    //         break;
                    case "SongRequestManager":
                 //       songRequestPluginInstalled = true;
                        break;
                    case "MappingExtensions":
                        mappingExtensionsPresent = true;
                        break;

                }

            }
        }

    }
}
