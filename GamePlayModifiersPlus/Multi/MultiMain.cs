namespace GamePlayModifiersPlus.Multiplayer
{
    using AsyncTwitch;
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
    public class MultiMain
    {
        public static GameObject multiObject = null;
        public static MultiValues Config = new MultiValues();
        public static MultiPowers Powers;
        public static MultiCommands multiCommands = new MultiCommands();
        public static bool multiActive = false;
        public static bool activated = false;
        public static string currentPowerUp = "Charging...";
        public void TwitchConnectionMulti_OnMessageReceived(TwitchConnection arg1, TwitchMessage message)
        {
            if(multiActive)
            {
                Log("Checking message");
                string messageString = message.Content.ToLower();
                multiCommands.CheckHealthCommands(messageString);
                multiCommands.CheckSizeCommands(messageString);
                multiCommands.CheckGameplayCommands(messageString);
                multiCommands.CheckSpeedCommands(messageString);

            }
        }

        public void Initialize()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {

        }

        private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
        {
            Config.charges = 0;
            activated = false;
            MultiClientInterface.initialized = false;
            GameObject client = GameObject.Find("MultiplayerClient");
            if (client != null)
            {
                multiActive = true;
                Log("Found MultiplayerClient game object!");

            }
            else
            {
                multiActive = false;
                Log(" MultiplayerClient game object not found!");
            }

            if (multiActive)
            {
                MultiClientInterface.ResetName();
                if (newScene.name == "EmptyTransition")
                {
                    Log("Resetting Multi Powers Object");
                    if (multiObject != null)
                        GameObject.Destroy(multiObject);
                }
            }

            if (multiObject == null)
            {
                Log("Null Creation of Multi Powers Object");
                multiObject = new GameObject("Multi Powers");
                Powers = multiObject.AddComponent<MultiPowers>();
                GameObject.DontDestroyOnLoad(multiObject);
            }

            //        }
            //        catch(Exception ex)
            //        {
            //           Log(ex.ToString());
            //        }


            if (newScene.name == "GameCore")
            {
                if (multiActive)
                {
                        MultiClientInterface.Init();

                }

              
                if (multiActive)
                {

                    GamePlayModifiersPlus.TwitchStuff.GMPDisplay ChatDisplay = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>();
                    if (ChatDisplay != null)
                    {
                        ChatDisplay.Destroy();
                        GameObject.Destroy(ChatDisplay);
                    }
                }




            }




        }

        public void Update()
        {
            if (!activated) return;

            if(Config.charges >= Config.maxCharges)
            {
                if (currentPowerUp == "Charging...")
                    currentPowerUp = MultiPowers.GeneratePowerUp();

                if((GamePlayModifiersPlus.Plugin.leftController.triggerValue >= 0.8 || GamePlayModifiersPlus.Plugin.rightController.triggerValue >= 0.8))
                {
                    
                    MultiClientInterface.SendCommand("!gmm " + currentPowerUp.ToLower());
                    Config.charges = 0;
                    currentPowerUp = "Charging...";
                    var text = GameObject.Find("Multi Powers").GetComponent<MultiGMPDisplay>().chargeText;
                    text.text = currentPowerUp;
                }
            }



        }


        public static void Activate()
        {
            multiObject.AddComponent<MultiGMPDisplay>();
            Powers.StartCoroutine(MultiPowers.ChargeOverTime());
            activated = true;

        }


            public static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "GameplayModifiersPlus-Multi", message);
        }
    }
}
