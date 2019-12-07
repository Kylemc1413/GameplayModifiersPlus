namespace GamePlayModifiersPlus.Multiplayer
{
    using StreamCore.Chat;
    using StreamCore.Config;
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
        public static MultiGMPDisplay multiGMPDisplay;
        public static bool? multiActive = false;
        public static bool activated = false;
        public static string currentPowerUp = "Charging...";

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
            if (multiGMPDisplay != null)
            {
                multiGMPDisplay.DestroyDis();
                GameObject.Destroy(multiGMPDisplay);
            }


            Config.charges = 0;
            activated = false;
            MultiClientInterface.initialized = false;
            multiActive = BeatSaberMultiplayer.Client.Instance?.Connected;

            if (multiActive.Value)
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

            }

            //        }
            //        catch(Exception ex)
            //        {
            //           Log(ex.ToString());
            //        }


            if (newScene.name == "GameCore")
            {
                Log("GameCore");
                if (!GMPUI.allowMulti)
                {
                    Log("Multi Not Allowed, Returning");
                    return;
                }
                if (multiActive.Value)
                {

                    GamePlayModifiersPlus.TwitchStuff.GMPDisplay ChatDisplay = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>();
                    if (ChatDisplay != null)
                    {
                        ChatDisplay.Destroy();
                        GameObject.Destroy(ChatDisplay);
                    }
                    //    Log("MultiMain - Multi Level Started");
                    MultiClientInterface.Client_ClientLevelStarted();
                }
                else
                    Log("Multi Not Active, Returning");




            }




        }

        public void Update()
        {
            if (!activated) return;

            if (Config.charges >= Config.maxCharges)
            {
                if (multiGMPDisplay.chargeText.text.Contains("Charging"))
                {
                    currentPowerUp = MultiPowers.GeneratePowerUp();
                    multiGMPDisplay.chargeText.text = currentPowerUp;
                }


                if ((GamePlayModifiersPlus.Plugin.leftController.triggerValue >= 0.8 || GamePlayModifiersPlus.Plugin.rightController.triggerValue >= 0.8))
                {

                    MultiClientInterface.SendCommand("!gmm " + currentPowerUp.ToLower());
                    Config.charges = 0;
                    currentPowerUp = "Charging...";
                    multiGMPDisplay.chargeText.text = currentPowerUp;
                }
            }



        }


        public static void Activate()
        {
            Log("Activating Multi UI");
            if(multiObject != null)
            multiGMPDisplay = multiObject.AddComponent<MultiGMPDisplay>();
            else
            {
                Log("Multi Object null, creating it");
                multiObject = new GameObject("Multi Powers");
                Powers = multiObject.AddComponent<MultiPowers>();
                multiGMPDisplay = multiObject.AddComponent<MultiGMPDisplay>();
            }
            Powers.StartCoroutine(MultiPowers.ChargeOverTime());
            activated = true;

        }


        public static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "GameplayModifiersPlus-Multi", message);
        }
    }
}
