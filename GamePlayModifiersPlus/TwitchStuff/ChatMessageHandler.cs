using CatCore;
using CatCore.Services.Multiplexer;
using CatCore.Models.Twitch.IRC;
using CatCore.Services.Twitch;
using CatCore.Services.Twitch.Interfaces;
using CatCore.Models.Twitch;
using CatCore.Models.Shared;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
namespace GamePlayModifiersPlus.TwitchStuff
{
    public class ChatMessageHandler
    {
        public static CatCoreInstance ChatCoreInstance;
        public static ITwitchService streamingService;
        public static void Load()
        {
            ChatCoreInstance = CatCore.CatCoreInstance.Create();
            streamingService = ChatCoreInstance.RunTwitchServices();
            // Setup chat message callbacks
            streamingService.OnTextMessageReceived += (ITwitchService service, TwitchMessage message) =>
            {

            //    if (message.Message.Contains("!gm"))
            //        commandChannel = message.Channel;
            //    else
            //        return;
                //TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;


                if (GameModifiersController.charges < 0) GameModifiersController.charges = 0;
                Plugin.twitchCommands.CheckChargeMessage(message);
                Plugin.twitchCommands.CheckConfigMessage(message);
                Plugin.twitchCommands.CheckStatusCommands(message);
                Plugin.twitchCommands.CheckInfoCommands(message);

                if (Config.allowEveryone || (Config.allowSubs && (message?.Sender as TwitchUser).IsSubscriber) || (message?.Sender).IsModerator || (message?.Sender).IsBroadcaster)
                {
                    if (GMPUI.chatIntegration && Plugin.isValidScene && !Plugin.cooldowns.GetCooldown("Global") && Plugin.twitchPluginInstalled)
                    {
                        GameModifiersController.commandsLeftForMessage = Config.commandsPerMessage;
                        Plugin.twitchCommands.CheckMapSwapCommands(message);
                        Plugin.twitchCommands.CheckRotationCommands(message);
                        Plugin.twitchCommands.CheckSpeedCommands(message);
                        Plugin.twitchCommands.CheckPauseMessage(message);
                        Plugin.twitchCommands.CheckGameplayCommands(message);
                        Plugin.twitchCommands.CheckHealthCommands(message);
                        Plugin.twitchCommands.CheckSizeCommands(message);
                        Plugin.twitchCommands.CheckGlobalCoolDown();
                    }
                }
                GameModifiersController.trySuper = false;
                GameModifiersController.sizeActivated = false;
                GameModifiersController.healthActivated = false;

            };

        }

        internal static void TryAsyncMessage(string message)
        {
            if (!Plugin.twitchPluginInstalled) return;
            //  IPA.Utilities.Async.UnityMainThreadTaskScheduler.Factory.StartNew( () => { SendAsyncMessage(message); });
            SendAsyncMessage(message);
        }
        internal static void SendAsyncMessage(string message)
        {
            streamingService.DefaultChannel.SendMessage(message);
            // ChatMessageHandler.streamingService.GetTwitchService().SendTextMessage(message, "kyle1413k");
        }


    }
}
