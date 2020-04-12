using StreamCore;
using StreamCore.Chat;
using StreamCore.Config;
using StreamCore.YouTube;
using StreamCore.Twitch;
using UnityEngine;
namespace GamePlayModifiersPlus.TwitchStuff
{
    public class ChatMessageHandler : MonoBehaviour, ITwitchIntegration, IYouTubeIntegration
    {
        public bool IsPluginReady { get; set; } = false;

        public void Awake()
        {
            // Setup chat message callbacks
            TwitchMessageHandlers.PRIVMSG += (TwitchMessage message) => {
                if (message.channelName != TwitchLoginConfig.Instance.TwitchChannelName)
                    return;

                if (Plugin.charges < 0) Plugin.charges = 0;
                Plugin.twitchCommands.CheckChargeMessage(message);
                Plugin.twitchCommands.CheckConfigMessage(message);
                Plugin.twitchCommands.CheckStatusCommands(message);
                Plugin.twitchCommands.CheckInfoCommands(message);
                //       twitchCommands.CheckSpeedCommands(message);

                if (Plugin.multiInstalled)
                    if (Multiplayer.MultiMain.multiActive.Value) return;
                if (ChatConfig.allowEveryone || (ChatConfig.allowSubs && message.user.Twitch.isSub) || message.user.Twitch.isMod)
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


                if (Multiplayer.MultiMain.multiActive.Value)
                {
                    string messageString = message.message.ToLower();
                    Multiplayer.MultiMain.multiCommands.CheckHealthCommands(messageString);
                    Multiplayer.MultiMain.multiCommands.CheckSizeCommands(messageString);
                    Multiplayer.MultiMain.multiCommands.CheckGameplayCommands(messageString);
                    Multiplayer.MultiMain.multiCommands.CheckSpeedCommands(messageString);
                }

            };


            // Signal to StreamCore that this class is ready to receive chat callbacks
            IsPluginReady = true;
        }
    }
}
