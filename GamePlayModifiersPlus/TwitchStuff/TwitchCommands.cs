namespace GamePlayModifiersPlus
{
    //using AsyncTwitch;
    using StreamCore;
    using StreamCore.Interfaces;
    using StreamCore.Models.Twitch;
    using UnityEngine;
    public class TwitchCommands
    {
        public static bool globalActive = false;

        public void CheckPauseMessage(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm pause") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.charges >= ChatConfig.pauseChargeCost)
                {
                    Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Pause());
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.pauseGlobalCooldown, "Global", "Game Paused :)"));
                    Plugin.charges -= ChatConfig.pauseChargeCost;
                    Plugin.commandsLeftForMessage = 0;
                }
            }
        }


        public void CheckChargeMessage(IChatMessage message)
        {
            TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;
            if (twitchMessage == null) return;

            if (twitchMessage.Bits >= ChatConfig.bitsPerCharge && ChatConfig.bitsPerCharge > 0)
            {

                Plugin.charges += (twitchMessage.Bits / ChatConfig.bitsPerCharge);
                Plugin.TryAsyncMessage("Current Charges: " + Plugin.charges);
            }
            if (twitchMessage.Sender.Name.ToLower() == "kyle1413k" && message.Message.ToLower().Contains("!charge"))
            {
                Plugin.charges += (ChatConfig.maxCharges / 2);
                Plugin.TryAsyncMessage("Current Charges: " + Plugin.charges);
            }
            if (message.Message.ToLower().Contains("!gm") && message.Message.ToLower().Contains("super"))
            {
                Plugin.trySuper = true;
            }
        }

        public void CheckConfigMessage(IChatMessage message)
        {
            string messageString = message.Message.ToLower();
            if (!messageString.Contains("!configchange")) return;
            if (!(message.Sender.IsModerator && ChatConfig.allowModCommands) && !message.Sender.IsBroadcaster) return;
            string command = "";
            string property = "";
            bool isPropertyOnly = false;
            string value = value = messageString.Split('=')[1];
            string arg1 = messageString.Split(' ', ' ')[1];
            string arg2 = messageString.Split(' ', ' ', '=')[2];
            Plugin.Log(arg1 + " " + arg2 + " " + value);
            switch (arg1)
            {
                case "da":
                    command = "DA";
                    break;
                case "smaller":
                    command = "Smaller";
                    break;
                case "larger":
                    command = "Larger";
                    break;
                case "random":
                    command = "Random";
                    break;
                case "instafail":
                    command = "Instafail";
                    break;
                case "invincible":
                    command = "Invincible";
                    break;
                case "njsrandom":
                    command = "NjsRandom";
                    break;
                case "noarrows":
                    command = "NoArrows";
                    break;
                case "funky":
                    command = "Funky";
                    break;
                case "rainbow":
                    command = "Rainbow";
                    break;
                case "pause":
                    command = "Pause";
                    break;
                case "bombs":
                    command = "Bombs";
                    break;
                case "faster":
                    command = "Faster";
                    break;
                case "slower":
                    command = "Slower";
                    break;
                case "poison":
                    command = "Poison";
                    break;
                case "offsetrandom":
                    command = "OffsetRandom";
                    break;
                case "reverse":
                    command = "Reverse";
                    break;
                case "mirror":
                    command = "Mirror";
                    break;
                case "tunnel":
                    command = "Tunnel";
                    break;
                case "left":
                    command = "Left";
                    break;
                case "right":
                    command = "Right";
                    break;
                case "randomrotation":
                    command = "RandomRotation";
                    break;
                default:
                    isPropertyOnly = true;
                    break;

            }
            if (isPropertyOnly)
            {
                switch (arg1.Split('=')[0])
                {
                    case "bitspercharge":
                        property = "bitsPerCharge";
                        break;
                    case "chargesforsupercharge":
                        property = "chargesForSuperCharge";
                        break;
                    case "maxcharges":
                        property = "maxCharges";
                        break;
                    case "chargesperlevel":
                        property = "chargesPerLevel";
                        break;
                    case "allowsubs":
                        property = "allowSubs";
                        break;
                    case "alloweveryone":
                        property = "allowEveryone";
                        break;
                    case "commandspermessage":
                        property = "commandsPerMessage";
                        break;
                    case "globalcommandcooldown":
                        property = "globalCommandCooldown";
                        break;
                    case "timeforcharges":
                        property = "timeForCharges";
                        break;
                    case "chargesovertime":
                        property = "chargesOverTime";
                        break;
                    case "showcooldownonmessage":
                        property = "showCooldownOnMessage";
                        break;
                    case "uiontop":
                        property = "uiOnTop";
                        break;
                    case "resetchargeseachlevel":
                        property = "resetChargesEachLevel";
                        break;
                    case "allowmodcommands":
                        property = "allowModCommands";
                        break;
                    case "chatintegration360":
                        property = "chatintegration360";
                        break;
                    default:
                        return;
                }
                ChatConfig.ChangeConfigValue(property, value);
            }
            else
            {
                switch (arg2)
                {
                    case "chargecost":
                        property = "ChargeCost";
                        break;
                    case "cooldown":
                        property = "CoolDown";
                        break;
                    case "globalcooldown":
                        property = "CoolDown";
                        break;
                    case "duration":
                        property = "Duration";
                        break;
                    case "min":
                        property = "Min";
                        break;
                    case "max":
                        property = "Max";
                        break;
                    case "chance":
                        property = "Chance";
                        break;
                    case "multiplier":
                        property = "Multiplier";
                        break;
                    default:
                        return;
                }

                ChatConfig.ChangeConfigValue(command, property, value);
            }
        }

        public void CheckInfoCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm help"))
            {
                Plugin.TryAsyncMessage("Guides: For Regular Users - http://bit.ly/1413ChatUser | For Streamers - http://bit.ly/1413Readme | For moderators also view http://bit.ly/1413Config");

            }
            if (message.Message.ToLower().Contains("!currentsong"))
            {
                if (!Plugin.isValidScene)
                    Plugin.TryAsyncMessage("No song is currently being played.");
                else
                    Plugin.TryAsyncMessage("Current Song: " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songName
                        + " - " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songSubName + " mapped by " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.levelAuthorName);
            }

            if (message.Message.ToLower().Contains("!gm chargehelp"))
            {
                if (ChatConfig.timeForCharges == 0 || ChatConfig.chargesOverTime == 0)
                    Plugin.TryAsyncMessage("Every " + ChatConfig.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + ChatConfig.chargesForSuperCharge + " Charges but will make the effect last much longer! " + ChatConfig.chargesPerLevel + " Charges are generated every song with chat mode on.");
                else
                    Plugin.TryAsyncMessage("Every " + ChatConfig.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + ChatConfig.chargesForSuperCharge + " Charges but will make the effect last much longer! " + ChatConfig.chargesPerLevel + " Charges are generated every song with chat mode on. Every " + ChatConfig.timeForCharges + " seconds, " + ChatConfig.chargesOverTime + " are added.");

            }
            if (message.Message.ToLower().Contains("!gm commands"))
            {
                Plugin.TryAsyncMessage("Currently supported commands | status: Currrent Status of chat integration | charges: view current charges and costs | chargehelp: Explain charge system");
            }

            if (!Plugin.cooldowns.GetCooldown("chargescommand"))
                if (message.Message.ToLower().Contains("!gm charges"))
                {
                    Plugin.TryAsyncMessage("Charges: " + Plugin.charges + " | Commands Per Message: " + ChatConfig.commandsPerMessage + " | " + ChatConfig.GetChargeCostString());
                    SharedCoroutineStarter.instance.StartCoroutine(Cooldowns.CommandCoolDown("chargescommand", ChatConfig.chargesCommandCoolDown));
                }
        }

        public void CheckStatusCommands(IChatMessage message)
        {
            if (message.Sender.IsBroadcaster || message.Sender.IsModerator)
            {
                if (message.Message.ToLower().Contains("!gm reset"))
                {
                    Plugin.cooldowns.ResetCooldowns();
                    TwitchPowers.ResetPowers(true);
                    Plugin.twitchPowers.StopAllCoroutines();
                    Plugin.charges = ChatConfig.chargesPerLevel;
                    Plugin.TryAsyncMessage("Resetting non Permanent Powers");
                }


            }


            if (message.Message.ToLower().Contains("!gm pp"))
            {
                if (Plugin.currentpp != 0)
                    Plugin.TryAsyncMessage("Streamer Rank: #" + Plugin.currentRank + ". Streamer pp: " + Plugin.currentpp + "pp");
                else
                    Plugin.TryAsyncMessage("Currently do not have streamer info");
            }
            if (message.Message.ToLower().Contains("!gm status"))
            {
                string scopeMessage = "";
                int scope = CheckCommandScope();
                switch (scope)
                {
                    case 0:
                        scopeMessage = "Everyone has access to commands";
                        break;
                    case 1:
                        scopeMessage = "Subscribers have access to commands";
                        break;
                    case 2:
                        scopeMessage = "Moderators have access to commands";
                        break;

                }

                Plugin.beepSound.Play();
                if (GMPUI.chatIntegration)
                    Plugin.TryAsyncMessage("Chat Integration Enabled. " + scopeMessage);
                else
                    Plugin.TryAsyncMessage("Chat Integration Not Enabled. " + scopeMessage);
            }
        }

        public void CheckHealthCommands(IChatMessage message)
        {
            if (!Plugin.cooldowns.GetCooldown("Health"))
            {
                if (message.Message.ToLower().Contains("!gm instafail"))
                {

                    if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.instaFailChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Insta Fail Active."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= ChatConfig.instaFailChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(ChatConfig.instaFailDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.instaFailCooldown, "Health", "Insta Fail Active."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= ChatConfig.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }

                if (message.Message.ToLower().Contains("!gm invincible") && !Plugin.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {
                    if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.invincibleChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Invincibility Active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= ChatConfig.invincibleChargeCost)
                    {
                        //          Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(ChatConfig.invincibleDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.invincibleCooldown, "Health", "Invincibility Active."));
                        Plugin.charges -= ChatConfig.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
                if (message.Message.ToLower().Contains("!gm poison"))
                {

                    if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.poisonChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Health Regen Super Disabled."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.poisonChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= ChatConfig.poisonChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(ChatConfig.poisonDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.poisonCooldown, "Health", "Health Regen Disabled."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= ChatConfig.poisonChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
            }
        }

        public void CheckRotationCommands(IChatMessage message)
        {
            if (!Plugin.cooldowns.GetCooldown("Rotation"))
            {
                if (message.Message.ToLower().Contains("!gm left"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.charges >= ChatConfig.leftChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.LeftRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.leftCoolDownn, "Rotation", "Rotating level to the left."));
                        Plugin.charges -= ChatConfig.leftChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm right"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.charges >= ChatConfig.rightChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RightRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.rightCoolDown, "Rotation", "Rotating level to the right."));
                        Plugin.charges -= ChatConfig.rightChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm randomrotation"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.randomRotationChargeCost)
                    {
                        //       Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rotation", "Super random level rotation active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.randomRotationChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= ChatConfig.randomRotationChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(ChatConfig.randomRotationDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.randomRotationCoolDown, "Rotation", "Random level rotation active."));
                        Plugin.charges -= ChatConfig.randomRotationChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
            }
        }
        public void CheckGameplayCommands(IChatMessage message)
        {

            if (message.Message.ToLower().Contains("!gm da") && !Plugin.cooldowns.GetCooldown("Note") && !Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.disappearingArrows && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.daChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "DA", "Super DA Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.daChargeCost)
                {
                    //      Plugin.beepSound.Play();

                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(ChatConfig.daDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.daCooldown, "DA", "DA Active."));
                    Plugin.charges -= ChatConfig.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm njsrandom") && !Plugin.cooldowns.GetCooldown("RandomNJS") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NJSRandom", "Super Random Note Jump Speed Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(ChatConfig.njsRandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                    Plugin.charges -= ChatConfig.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm offsetrandom") && !Plugin.cooldowns.GetCooldown("OffsetRandom") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.offsetrandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "OffsetRandom", "Super Random Note Spawn Offset Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.offsetrandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.offsetrandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(ChatConfig.offsetrandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.offsetrandomCooldown, "OffsetRandom", "Random Note Spawn Offset Active."));
                    Plugin.charges -= ChatConfig.offsetrandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm noarrows") && !Plugin.cooldowns.GetCooldown("NoArrows") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.noArrowsChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NoArrows", "Super No Arrows Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.noArrowsChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(ChatConfig.noArrowsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                    Plugin.charges -= ChatConfig.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm mirror") && !Plugin.cooldowns.GetCooldown("Mirror") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.mirrorChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Mirror", "Super Mirror Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.mirrorChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.mirrorChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(ChatConfig.mirrorDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.mirrorCooldown, "Mirror", "Temporary Mirror Mode Activated."));
                    Plugin.charges -= ChatConfig.mirrorChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm reverse") && !Plugin.cooldowns.GetCooldown("Reverse") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.reverseChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Reverse", "Reversing entire map :)"));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.reverseChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.reverseChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(ChatConfig.reverseDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.reverseCooldown, "Reverse", "Temporary Map Reversal."));
                    Plugin.charges -= ChatConfig.reverseChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm funky") && !Plugin.cooldowns.GetCooldown("Funky") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.funkyChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Funky", "Time to get Funky."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.funkyChargeCost)
                {
                    //           Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(ChatConfig.funkyDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.funkyCooldown, "Funky", "Funky Mode Activated"));
                    Plugin.charges -= ChatConfig.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm rainbow") && !Plugin.cooldowns.GetCooldown("Rainbow") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rainbow", "RAIIINBOWWS."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(ChatConfig.rainbowDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.rainbowCooldown, "Rainbow", "Rainbow Activated"));
                    Plugin.charges -= ChatConfig.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm bombs") && !Plugin.cooldowns.GetCooldown("Bombs") && Plugin.commandsLeftForMessage > 0 && ChatConfig.bombsChance > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.bombsChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Bombs", "Bombs Away!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.bombsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.bombsChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(ChatConfig.bombsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.bombsCooldown, "Bombs", "Sneaking Bombs into the map."));
                    Plugin.charges -= ChatConfig.bombsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm tunnel") && !Plugin.cooldowns.GetCooldown("Tunnel") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.tunnelChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Tunnel", "Entering Tunnel. Estimated time of exit: Unknown."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.tunnelChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.tunnelChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(ChatConfig.tunnelDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.tunnelCoolDown, "Tunnel", $"Entering Tunnel. Estimated time of exit: {ChatConfig.tunnelDuration} seconds."));
                    Plugin.charges -= ChatConfig.tunnelChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }



        public void CheckSizeCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm smaller") && !Plugin.cooldowns.GetCooldown("NormalSize") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.smallerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(ChatConfig.smallerMultiplier, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.sizeActivated = true;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.smallerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.smallerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(ChatConfig.smallerMultiplier, ChatConfig.smallerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.smallerCoolDown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.sizeActivated = true;
                    Plugin.charges -= ChatConfig.smallerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm larger") && !Plugin.cooldowns.GetCooldown("NormalSize") && !Plugin.sizeActivated && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.largerChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(ChatConfig.largerMultiplier, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.largerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.largerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(ChatConfig.largerMultiplier, ChatConfig.largerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.largerCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.charges -= ChatConfig.largerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm random") && !message.Message.ToLower().Contains("!gm randomrotation") && !Plugin.cooldowns.GetCooldown("Random") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.randomChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Random", "Super Random Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.superRandom = true;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.randomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.randomChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(ChatConfig.randomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.randomCooldown, "Random", "Randomly Scaling Notes"));
                    Plugin.charges -= ChatConfig.randomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }
        }

        public void CheckSpeedCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm faster") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {
                if (!Plugin.practicePluginInstalled)
                {
                    Plugin.TryAsyncMessage("Speed altering commands currently require Practice Plugin to be installed");
                    return;
                }
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.fasterChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, ChatConfig.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Fast Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.fasterChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(ChatConfig.fasterDuration, ChatConfig.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                    Plugin.charges -= ChatConfig.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm slower") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {
                if (!Plugin.practicePluginInstalled)
                {
                    Plugin.TryAsyncMessage("Speed altering commands currently require Practice Plugin to be installed");
                    return;
                }


                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.slowerChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, ChatConfig.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Weakling Slower Song Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= ChatConfig.slowerChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(ChatConfig.slowerDuration, ChatConfig.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(ChatConfig.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                    Plugin.charges -= ChatConfig.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
        }
        public int CheckCommandScope()
        {

            if (ChatConfig.allowEveryone) return 0;
            else if (ChatConfig.allowSubs) return 1;
            else return 2;
        }

        public void CheckGlobalCoolDown()
        {
            if (ChatConfig.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false && globalActive)
            {
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                Plugin.twitchPowers.StartCoroutine(TwitchPowers.GlobalCoolDown());
                text.text += " " + "Global" + " | ";
            }
            globalActive = false;
        }
    }
}
