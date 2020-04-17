using StreamCore;
using StreamCore.Config;
using StreamCore.Interfaces;
using StreamCore.Models.Twitch;
using UnityEngine;

namespace GamePlayModifiersPlus.TwitchStuff
{
    public class ChatMessageHandler
    {
        public static StreamCoreInstance streamCoreInstance;
        public static StreamCore.Services.StreamingService streamingService;
        public static IChatChannel commandChannel;
        public static void Load()
        {
            streamCoreInstance = StreamCore.StreamCoreInstance.Create();
            streamingService = streamCoreInstance.RunAllServices();
            // Setup chat message callbacks
            streamingService.OnTextMessageReceived += (StreamCore.Interfaces.IStreamingService service, StreamCore.Interfaces.IChatMessage message) =>
            {

                if (message.Message.Contains("!gm"))
                    commandChannel = message.Channel;
                else
                    return;
                TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;


                if (Plugin.charges < 0) Plugin.charges = 0;
                Plugin.twitchCommands.CheckChargeMessage(message);
                Plugin.twitchCommands.CheckConfigMessage(message);
                Plugin.twitchCommands.CheckStatusCommands(message);
                Plugin.twitchCommands.CheckInfoCommands(message);

                if (ChatConfig.allowEveryone || (ChatConfig.allowSubs && (twitchMessage?.Sender as TwitchUser).IsSubscriber) || (twitchMessage?.Sender).IsModerator)
                {
                    if (GMPUI.chatIntegration && Plugin.isValidScene && !Plugin.cooldowns.GetCooldown("Global") && Plugin.twitchPluginInstalled)
                    {
                        Plugin.commandsLeftForMessage = ChatConfig.commandsPerMessage;

                        Plugin.twitchCommands.CheckRotationCommands(message);
                        Plugin.twitchCommands.CheckSpeedCommands(message);
                        Plugin.twitchCommands.CheckPauseMessage(message);
                        Plugin.twitchCommands.CheckGameplayCommands(message);
                        Plugin.twitchCommands.CheckHealthCommands(message);
                        Plugin.twitchCommands.CheckSizeCommands(message);
                        Plugin.twitchCommands.CheckGlobalCoolDown();
                    }
                }
                Plugin.trySuper = false;
                Plugin.sizeActivated = false;
                Plugin.healthActivated = false;

            };

        }
    }
}
