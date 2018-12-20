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
        public static void Init()
        {

                MultiMain.Log("initializing");
                playerClient = GameObject.Find("MultiplayerClient").GetComponent<Client>();
            Client.EventMessageReceived += Client_EventMessageReceived;
            Client.ClientCreated += Client_ClientCreated;
            Client.ClientDestroyed += Client_ClientDestroyed;
            SharedCoroutineStarter.instance.StartCoroutine(DelayedSendPluginCheck());


        //    Client_EventMessageReceived("GMP", "HasPlugin");
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
                    MultiMain.multiCommands.CheckHealthCommands(data);
                    MultiMain.multiCommands.CheckSizeCommands(data);
                    MultiMain.multiCommands.CheckGameplayCommands(data);
                    MultiMain.multiCommands.CheckSpeedCommands(data);
                }
            }
            else
            {
                if (data == "HasPlugin")
                {
                    otherGmpPlayer = true;
                    if (playerName == "")
                        playerName = Client.instance.playerInfo.playerName;
                    Client.instance.playerInfo.playerName += " (GMP)";
                }

                else if (data.Contains("HavePlugin?"))
                {
                    Client.instance.SendEventMessage("GMP", "HasPlugin");
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
                MultiMain.Log("Sending Command: " + command);
               Client.instance.SendEventMessage("GMP", command);
        }
        public static void ResetName()
        {
            if (playerName != "")
                Client.instance.playerInfo.playerName = playerName;
        }
    }
}
