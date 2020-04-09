namespace GamePlayModifiersPlus.Multiplayer
{
    
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
        public static bool otherGmpPlayer = false;
        public static bool initialized = false;
        //   public static string playerName = "";
        public static string version = "0.0.1";
        public static bool spectating;
        public static void Init()
        {


            Client.EventMessageReceived += Client_EventMessageReceived;
     //       Client.ClientLevelStarted += Client_ClientLevelStarted;
            Client.ClientJoinedRoom += Client_ClientJoinedRoom;
            MultiMain.Log("initializing");
        }



        private static void Client_ClientJoinedRoom()
        {
            MultiMain.Log("Joined Room, Logging spectator setting");
            spectating = BeatSaberMultiplayer.Config.Instance.SpectatorMode;
        }

        public static void Client_ClientLevelStarted()
        {
            initialized = false;
            otherGmpPlayer = false;
            MultiMain.Log("Multiplayer Level Started");
            Client.disableScoreSubmission = false;
            if (!GMPUI.allowMulti) return;
            if (spectating) return; 
            SharedCoroutineStarter.instance.StartCoroutine(DelayedSendPluginCheck());
    //        Client_EventMessageReceived("GMP", "HasPlugin" + version);
        }

        public static IEnumerator DelayedSendPluginCheck()
        {
            yield return new WaitForSeconds(1f);
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
                    if (!GMPUI.allowMulti) return;
                    if (spectating) return;
                        otherGmpPlayer = true;
            //        if (playerName == "")
             //           playerName = Client.Instance.playerInfo.playerName;
                 //   Client.Instance.playerInfo.playerName += " (GMP)";
                    Client.disableScoreSubmission = true;
                }

                if (data.Contains("HavePlugin?"))
                {
                    if (!GMPUI.allowMulti) return;
                    if (spectating) return;
                    SendCommand("HasPlugin");
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
               Client.Instance.SendEventMessage("GMP", command + version);
        }
        public static void ResetName()
        {
        //    if (playerName != "")
        //        Client.Instance.playerInfo.playerName = playerName;
        }
    }
}
