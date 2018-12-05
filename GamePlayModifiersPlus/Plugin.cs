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
    public class Plugin : IPlugin
    {
        public static readonly Config Config = new Config(Path.Combine(Environment.CurrentDirectory, "UserData\\GamePlayModifiersPlusChatSettings.ini"));


        public string Name => "GameplayModifiersPlus";
        public string Version => "0.9.5";
      
        public static float timeScale = 1;
        public static TwitchPowers twitchPowers = new TwitchPowers();
        public static bool gnomeOnMiss = false;
        public static SoundPlayer gnomeSound = new SoundPlayer(Properties.Resources.gnome);
        public static SoundPlayer beepSound = new SoundPlayer(Properties.Resources.Beep);
        public static bool soundIsPlaying = false;
        public static AudioTimeSyncController AudioTimeSync { get; private set; }
        public static AudioSource songAudio;
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
        public static bool calculating = false;
        public static bool startSuperHot;
        public static bool swapSabers;
        public static bool bulletTime = false;
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
        public static bool chatDelta;
        public static bool firstLoad = true;
        VRController leftController;
        VRController rightController;
        private Sprite _ChatDeltaIcon;
        private Sprite _SwapSabersIcon;
        private Sprite _RepeatIcon;
        private Sprite _GnomeIcon;
        private Sprite _BulletTimeIcon;
        private Sprite _TwitchIcon;
        public static StandardLevelSceneSetupDataSO levelData;
        bool invalidForScoring = false;
        bool repeatSong;
        private static bool _hasRegistered = false;
        public static BeatmapObjectSpawnController spawnController;
        public static GameEnergyCounter energyCounter;
        public static GameEnergyUIPanel energyPanel;
        public static NoteController.Pool poolA;
        public static NoteController.Pool poolB;
        public static int charges = 0;
        public static float altereddNoteScale = 1;
        public static float fixedNoteScale = 1f;
        public static bool randomSize = false;
        public static bool nextIsSuper = false;
        public static bool tempNoArrow;
        public static bool superRandom;
        public static bool healthActivated;
        public static bool sizeActivated;
        public static NoteData[] modifiedNotes;
        public static bool noArrow;
        public static bool randomNJS;
        public static float songNJS;
        public static bool haveSongNJS;
        public static bool funky;
        public static bool rainbow;
        public static SimpleColorSO colorA;
        public static SimpleColorSO colorB;
        public static SimpleColorSO oldColorA = new SimpleColorSO();
        public static SimpleColorSO oldColorB = new SimpleColorSO();
        public static int commandsLeftForMessage;
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            ReadPrefs();
           cooldowns = new Cooldowns();
        }


        private void TwitchConnection_OnMessageReceived(TwitchConnection arg1, TwitchMessage message)
        {
            Log("Message Recieved, AsyncTwitch currently working");
            //Status check message

            TwitchCommands.CheckChargeMessage(message);
            TwitchCommands.CheckConfigMessage(message);
            TwitchCommands.CheckStatusCommands(message);
            TwitchCommands.CheckInfoCommands(message);

            if (Config.allowEveryone || (Config.allowSubs && message.Author.IsSubscriber) || message.Author.IsMod)
            {
                if (twitchStuff && isValidScene && !cooldowns.GetCooldown("Global"))
            {
                    commandsLeftForMessage = Config.commandsPerMessage;
                TwitchCommands.CheckGameplayCommands(message);
                TwitchCommands.CheckHealthCommands(message);
                TwitchCommands.CheckSizeCommands(message);
            }
            }

            sizeActivated = false;
            healthActivated = false;

        }



        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

            if (scene.name == "Menu")
            {
                
                ReadPrefs();
                GetIcons();
                var swapSabersOption = GameplaySettingsUI.CreateToggleOption("Testing Ground", "Currently Used To test Random stuff", _SwapSabersIcon);
                swapSabersOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "swapSabers", false, true);
                swapSabersOption.OnToggle += (swapSabers) => { ModPrefs.SetBool("GameplayModifiersPlus", "swapSabers", swapSabers); Log("Changed Modprefs value"); };

                var chatDeltaOption = GameplaySettingsUI.CreateToggleOption("Chat Delta", "Display Change in Performance Points / Rank in Twitch Chat if Connected", _ChatDeltaIcon);
                chatDeltaOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
                chatDeltaOption.OnToggle += (chatDelta) => { ModPrefs.SetBool("GameplayModifiersPlus", "chatDelta", chatDelta); Log("Changed Modprefs value"); };

                var repeatOption = GameplaySettingsUI.CreateToggleOption("Repeat", "Restarts song on song end, does not submit scores.", _RepeatIcon);
                repeatOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "repeatSong", false, true);
                repeatOption.OnToggle += (repeatSong) => { ModPrefs.SetBool("GameplayModifiersPlus", "repeatSong", repeatSong); Log("Changed Modprefs value"); };

                var gnomeOption = GameplaySettingsUI.CreateToggleOption("Gnome on miss", "Probably try not to miss. (Disables Score Submission)", _GnomeIcon);
                gnomeOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
                gnomeOption.OnToggle += (gnomeOnMiss) => { ModPrefs.SetBool("GameplayModifiersPlus", "gnomeOnMiss", gnomeOnMiss); Log("Changed Modprefs value"); };
                gnomeOption.AddConflict("Faster Song");
                gnomeOption.AddConflict("Slower Song");


                var bulletTimeOption = GameplaySettingsUI.CreateToggleOption("Bullet Time", "Slow down time by pressing the triggers on your controllers. (Disables Score Submission)", _BulletTimeIcon);
                bulletTimeOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
                bulletTimeOption.OnToggle += (bulletTime) => { ModPrefs.SetBool("GameplayModifiersPlus", "bulletTime", bulletTime); Log("Changed Modprefs value"); };
                bulletTimeOption.AddConflict("Faster Song");
                bulletTimeOption.AddConflict("Slower Song");

                var twitchStuffOption = GameplaySettingsUI.CreateToggleOption("Chat Integration", "Allows Chat to mess with your game if connected. !gm help (Disables Score Submission)", _TwitchIcon);
                twitchStuffOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "twitchStuff", false, true);
                twitchStuffOption.OnToggle += (twitchStuff) => { ModPrefs.SetBool("GameplayModifiersPlus", "twitchStuff", twitchStuff); Log("Changed Modprefs value"); };
                twitchStuffOption.AddConflict("Bullet Time");



                var noteSizeOption = GameplaySettingsUI.CreateListOption("Note Size", "Change the size of the notes (Disables Score Submission");
                for (float i = 10; i <= 200; i += 10)
                    noteSizeOption.AddOption(i / 100);
                noteSizeOption.GetValue = (() =>
                {
                    float num = ModPrefs.GetFloat("GameplayModifiersPlus", "noteScale", 1f, true);
                    if (num % 0.1f != 0)
                        num = (float)Math.Round(num, 1);

                    num = Mathf.Clamp(num, 0.1f, 2f);
                    return num;
                });
                noteSizeOption.OnChange += (fixedNoteScale) => { ModPrefs.SetFloat("GameplayModifiersPlus", "noteScale", fixedNoteScale); };
                

            }
            
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            cooldowns.ResetCooldowns();
            ReadPrefs();
            TwitchPowers.ResetPowers();
            haveSongNJS = false;
            
            invalidForScoring = false;
            if (soundIsPlaying == true)
                gnomeSound.Stop();
            soundIsPlaying = false;
            isValidScene = false;
            playerInfo = false;
            if (scene.name == "Menu")
            {
                Log(Config.daCooldown.ToString());
                SharedCoroutineStarter.instance.StartCoroutine(GrabPP());
                SharedCoroutineStarter.instance.StopAllCoroutines();

                if (_hasRegistered == false)
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


            if (bulletTime == true)
                superHot = false;
            if (twitchStuff == true)
            {
                superHot = false;
                bulletTime = false;
                gnomeOnMiss = false;
                fixedNoteScale = 1f;
            }


            if (scene.name == "GameCore")
            {
                var colors = Resources.FindObjectsOfTypeAll<SimpleColorSO>();
                foreach(SimpleColorSO color in colors)
                {
                    Log(color.name);
                    if (color.name == "Color0")
                        colorA = color;
                    if (color.name == "Color1")
                        colorB = color;
                }
                    oldColorA.SetColor(colorA);
                    oldColorB.SetColor(colorB);


                Log(colorA.color.ToString());
                if (twitchStuff &&  charges <= Config.maxCharges)
                {
                    charges += Config.chargesPerLevel;
                    TwitchConnection.Instance.SendChatMessage("Current Charges: " + charges);
                }


                levelData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().First();
                spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().First();
                energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().First();
                energyPanel = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().First();
                poolA = spawnController.GetField<NoteController.Pool>("_noteAPool");
                poolB = spawnController.GetField<NoteController.Pool>("_noteBPool");
                if(twitchStuff || fixedNoteScale != 1f || swapSabers)
                {



                spawnController.noteDidStartJumpEvent += SpawnController_ModifiedJump;
                spawnController.noteWasCutEvent += SpawnController_ScaleRemoveCut;
                spawnController.noteWasMissedEvent += SpawnController_ScaleRemoveMiss;
                }



                levelData.didFinishEvent += LevelData_didFinishEvent;
                //   ReflectionUtil.SetProperty(typeof(PracticePlugin.Plugin), "TimeScale", 1f);
                isValidScene = true;
                AudioTimeSync = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                if (AudioTimeSync != null)
                {
                    songAudio = AudioTimeSync.GetField<AudioSource>("_audioSource");
                    if (songAudio != null)
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
                    funky = true;
                    rainbow = true;
                }
                //  SharedCoroutineStarter.instance.StartCoroutine(SwapSabers(leftSaber, rightSaber));

                if (gnomeOnMiss == true)
                {
                    invalidForScoring = true;

                    if (spawnController != null)
                    {
                        spawnController.noteWasMissedEvent += delegate (BeatmapObjectSpawnController beatmapObjectSpawnController2, NoteController noteController)
                        {
                            if (noteController.noteData.noteType != NoteType.Bomb)
                            {
                                try
                                {
                                    SharedCoroutineStarter.instance.StopAllCoroutines();
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
                                SharedCoroutineStarter.instance.StopAllCoroutines();
                                SharedCoroutineStarter.instance.StartCoroutine(TwitchPowers.SpecialEvent());
                                Log("Gnoming");
                            }

                        };

                    }
                }
                if (bulletTime || twitchStuff || fixedNoteScale != 1f)
                    invalidForScoring = true;

                /*
                if(superHot == true)
                {
                    startSuperHot = false;
                    SharedCoroutineStarter.instance.StartCoroutine(Wait(1f));

                }

            */




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
        }

        private void SpawnController_ModifiedJump(BeatmapObjectSpawnController arg1, NoteController controller)
        {
            if(rainbow)
            {
                   colorA.SetColor(new Color(UnityEngine.Random.Range(0.2f, 1.7f) , UnityEngine.Random.Range(0.2f, 1.7f), UnityEngine.Random.Range(0.2f, 1.7f)));
                   colorB.SetColor(new Color(UnityEngine.Random.Range(0.2f, 1.7f), UnityEngine.Random.Range(0.2f, 1.7f), UnityEngine.Random.Range(0.2f, 1.7f)));

            }





            Transform noteTransform = controller.GetField<Transform>("_noteTransform");
            //       noteTransform.Translate(0f, 4f, 0f);
            if(funky)
            noteTransform.gameObject.AddComponent<FloatBehavior>();

            if (randomNJS)
            {

                TwitchPowers.AdjustNJS(UnityEngine.Random.Range(Config.njsRandomMin, Config.njsRandomMax));
            }
            if (!haveSongNJS)
            {
            songNJS = spawnController.GetField<float>("_noteJumpMovementSpeed");
                haveSongNJS = true;
            }

            if (twitchStuff)
            {
                if (altereddNoteScale == 1 && !randomSize) return;
                invalidForScoring = true;
                //          Transform noteTransform = controller.GetField<Transform>("_noteTransform");
                //       Log("SPAWN" + noteTransform.localScale.ToString());
                if(superRandom)
                {
                    noteTransform.localScale *= UnityEngine.Random.Range(Config.randomMin, Config.randomMax);
                }
                else
                {
                    if (!randomSize)
                        noteTransform.localScale *= altereddNoteScale;
                    if (randomSize)
                        noteTransform.localScale *= UnityEngine.Random.Range(Config.randomMin, Config.randomMax);
                }

                //     Log("SPAWN" + noteTransform.localScale.ToString());
            }
            if (fixedNoteScale != 1f)
            {
                invalidForScoring = true;
       //         Transform noteTransform = controller.GetField<Transform>("_noteTransform");
                //       Log("SPAWN" + noteTransform.localScale.ToString());
                noteTransform.localScale *= fixedNoteScale;
                //     Log("SPAWN" + noteTransform.localScale.ToString());
            }
            NoteData note = controller.noteData;




        }

        private void LevelData_didFinishEvent(StandardLevelSceneSetupDataSO arg1, LevelCompletionResults arg2)
        {
            if (arg2.levelEndStateType == LevelCompletionResults.LevelEndStateType.Quit) return;
            if (arg2.levelEndStateType == LevelCompletionResults.LevelEndStateType.Restart) return;
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
            


            if (soundIsPlaying == true && songAudio != null && isValidScene == true)
            {
                SetTimeScale(0f);;
                Time.timeScale = 0f;
                return;
            }

            if (bulletTime == true && isValidScene == true && soundIsPlaying == false)
            {
                SetTimeScale(1 - (leftController.triggerValue + rightController.triggerValue) / 2);
                Time.timeScale = timeScale;
            //    Time.fixedDeltaTime = timeScale;
                return;
            }

            /*
                        if (superHot == true && playerInfo == true && soundIsPlaying == false && isValidScene == true && startSuperHot == true)
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

        void GetIcons()
        {
            if (_ChatDeltaIcon == null)
                _ChatDeltaIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.ChatDelta.png");
            if (_SwapSabersIcon == null)
                _SwapSabersIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.SwapSabers.png");
            if (_RepeatIcon == null)
                _RepeatIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.RepeatIcon.png");
            if (_GnomeIcon == null)
                _GnomeIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.gnomeIcon.png");
            if (_BulletTimeIcon == null)
                _BulletTimeIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.BulletIcon.png");
            if (_TwitchIcon == null)
                _TwitchIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.TwitchIcon.png");

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
            Config.Load();
            gnomeOnMiss = ModPrefs.GetBool("GameplayModifiersPlus", "gnomeOnMiss", false, true);
         //   superHot = ModPrefs.GetBool("GameplayModifiersPlus", "superHot", false, true);
            bulletTime = ModPrefs.GetBool("GameplayModifiersPlus", "bulletTime", false, true);
            twitchStuff = ModPrefs.GetBool("GameplayModifiersPlus", "twitchStuff", false, true);
            swapSabers = ModPrefs.GetBool("GameplayModifiersPlus", "swapSabers", false, true);
            chatDelta = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
            repeatSong = ModPrefs.GetBool("GameplayModifiersPlus", "repeatSong", false, true);
            fixedNoteScale = ModPrefs.GetFloat("GameplayModifiersPlus", "noteScale", 1f, true);
            Config.Save();
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
        //        if (firstLoad == true)
         //           if (chatDelta)
       //                 TwitchConnection.Instance.SendChatMessage("Loaded. PP: " + currentpp + " pp. Rank: " + currentRank);

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


        static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

    }
}
