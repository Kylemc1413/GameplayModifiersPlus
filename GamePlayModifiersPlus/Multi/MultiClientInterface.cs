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
    using BeatSaberMultiplayer;
    public class MultiClientInterface
    {
        public static BeatSaberMultiplayer.Data.PlayerInfo playerInfo;
        public static Client playerClient;
        public static bool otherGmpPlayer = false;
        public static bool initialized = false;
        public static string playerName = "";
        public static string version = GamePlayModifiersPlus.Plugin.pluginVersion;
        public static void Init()
        {

                MultiMain.Log("initializing");
                playerClient = GameObject.Find("MultiplayerClient").GetComponent<Client>();
            Client.EventMessageReceived += Client_EventMessageReceived;
            Client.ClientCreated += Client_ClientCreated;
            Client.ClientDestroyed += Client_ClientDestroyed;
            SharedCoroutineStarter.instance.StartCoroutine(DelayedSendPluginCheck());


            Client_EventMessageReceived("GMP", "HasPlugin" + version);
        }

        private static void Client_ClientDestroyed()
        {

        }

        private static void Client_ClientCreated()
        {

        }

        public static IEnumerator DelayedSendPluginCheck()
        {
            yield return new WaitForSeconds(0.1f);
                SendCommand("HavePlugin?");

        }
        private static void Client_EventMessageReceived(string header, string data)
        {
            MultiMain.Log("Message Recieved: " + header + " : " + data);
            if (header != "GMP") return;
            if(otherGmpPlayer)
            {
                if (data.Contains("!gmm"))
                {
                    string command = data.ToLower();
                    MultiMain.multiCommands.CheckHealthCommands(command);
                    MultiMain.multiCommands.CheckSizeCommands(command);
                    MultiMain.multiCommands.CheckGameplayCommands(command);
                    MultiMain.multiCommands.CheckSpeedCommands(command);
                }
            }
            else
            {
                if (data == "HasPlugin" + version)
                {
                    otherGmpPlayer = true;
                    if (playerName == "")
                        playerName = Client.instance.playerInfo.playerName;
                    Client.instance.playerInfo.playerName += " (GMP)";
                }

                else if (data.Contains("HavePlugin?"))
                {
                    Client.instance.SendEventMessage("GMP", "HasPlugin" + version);
                }
            }

            if (otherGmpPlayer && !initialized)
            {
                MultiMain.Activate();
                initialized = true;
            }

        }


        public static void SendCommand(string command)
        {
                MultiMain.Log("Sending Command: " + command + version);
               Client.instance.SendEventMessage("GMP", command + version);
        }
        public static void ResetName()
        {
            if (playerName != "")
                Client.instance.playerInfo.playerName = playerName;
        }
    }
}
