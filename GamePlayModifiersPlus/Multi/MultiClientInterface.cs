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
        public static void Init()
        {
            try
            {
                MultiMain.Log("initializing");
                playerClient = GameObject.Find("MultiplayerClient").GetComponent<Client>();
            Client.EventMessageReceived += Client_EventMessageReceived;
            Client.ClientCreated += Client_ClientCreated;
            Client.ClientDestroyed += Client_ClientDestroyed;
            SharedCoroutineStarter.instance.StartCoroutine(DelayedSendPluginCheck());
                MultiMain.Log("Init checkpoint 2");
            }
            catch(Exception ex)
            {
                MultiMain.Log(ex.ToString());
            }

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
            try
            {
                SendCommand("HavePlugin?");
            }
            catch(Exception ex)
            {
                MultiMain.Log(ex.ToString());
            }

        }
        private static void Client_EventMessageReceived(string header, string data)
        {
            MultiMain.Log("Message Recieved: " + header + " : " + data);
            if (header != "GMP") return;

            if (!otherGmpPlayer)
            {
                if (data == "HasPlugin") otherGmpPlayer = true;
                else if (data == "HavePlugin?")
                {
                    playerClient.SendEventMessage("GMP", "HasPlugin");
                    Client.instance.SendEventMessage("GMP", "HasPlugin");
                }
            }

            if (!otherGmpPlayer) return;
            if (otherGmpPlayer && !initialized)
            {
                MultiMain.Activate();
                initialized = true;
            }

            if(data.StartsWith("!gmm"))
            {
            MultiMain.multiCommands.CheckHealthCommands(data);
            MultiMain.multiCommands.CheckSizeCommands(data);
            MultiMain.multiCommands.CheckGameplayCommands(data);
            MultiMain.multiCommands.CheckSpeedCommands(data);
            }

        }


        public static void SendCommand(string command)
        {

            try
            {
                MultiMain.Log("Sending Command: " + command);
                playerClient.SendEventMessage("GMP", command);
               Client.instance.SendEventMessage("GMP", command);
            }
            catch(Exception ex)
            {
                MultiMain.Log(ex.ToString());
            }
        }
    }
}
