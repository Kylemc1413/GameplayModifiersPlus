using ChatCore;
using ChatCore.Config;
using ChatCore.Interfaces;
using ChatCore.Models.Twitch;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
namespace GamePlayModifiersPlus.TwitchStuff
{
    public class ChatMessageHandler
    {
        public static ChatCoreInstance ChatCoreInstance;
        public static ChatCore.Services.ChatServiceMultiplexer streamingService;
        public static IChatChannel commandChannel;
        public static void Load()
        {
            ChatCoreInstance = ChatCore.ChatCoreInstance.Create();
            streamingService = ChatCoreInstance.RunAllServices();
            // Setup chat message callbacks
            streamingService.OnTextMessageReceived += (ChatCore.Interfaces.IChatService service, ChatCore.Interfaces.IChatMessage message) =>
            {

                if (message.Message.Contains("!gm"))
                    commandChannel = message.Channel;
                else
                    return;
                TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;


                if (GameModifiersController.charges < 0) GameModifiersController.charges = 0;
                Plugin.twitchCommands.CheckChargeMessage(message);
                Plugin.twitchCommands.CheckConfigMessage(message);
                Plugin.twitchCommands.CheckStatusCommands(message);
                Plugin.twitchCommands.CheckInfoCommands(message);

                if (Config.allowEveryone || (Config.allowSubs && (twitchMessage?.Sender as TwitchUser).IsSubscriber) || (twitchMessage?.Sender).IsModerator || (twitchMessage?.Sender).IsBroadcaster)
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
            streamingService.SendTextMessage(message, commandChannel);
            // ChatMessageHandler.streamingService.GetTwitchService().SendTextMessage(message, "kyle1413k");
        }


    }
}
