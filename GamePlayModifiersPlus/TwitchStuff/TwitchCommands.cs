namespace GamePlayModifiersPlus.TwitchStuff
{
    //using AsyncTwitch;
    using ChatCore;
    using ChatCore.Interfaces;
    using ChatCore.Models.Twitch;
    using UnityEngine;
    using GamePlayModifiersPlus.Utilities;
    using System.CodeDom;
    using IPA.Utilities;
    public class TwitchCommands
    {
        public static bool globalActive = false;

        public void CheckPauseMessage(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm pause") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.charges >= Config.pauseChargeCost)
                {
                    GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Pause());
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.pauseGlobalCooldown, "Global", "Game Paused 🙂"));
                    GameModifiersController.charges -= Config.pauseChargeCost;
                    GameModifiersController.commandsLeftForMessage = 0;
                }
            }
        }


        public void CheckChargeMessage(IChatMessage message)
        {
            TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;
            if (twitchMessage == null) return;

            if (twitchMessage.Bits >= Config.bitsPerCharge && Config.bitsPerCharge > 0)
            {

                GameModifiersController.charges += (twitchMessage.Bits / Config.bitsPerCharge);
                ChatMessageHandler.TryAsyncMessage("Current Charges: " + GameModifiersController.charges);
            }
            if (twitchMessage.Sender.UserName.ToLower() == "kyle1413k" && message.Message.ToLower().Contains("!gm admincharge"))
            {
                GameModifiersController.charges += (Config.maxCharges / 2);
                ChatMessageHandler.TryAsyncMessage("Current Charges: " + GameModifiersController.charges);
            }
            if (message.Message.ToLower().Contains("!gm") && message.Message.ToLower().Contains("super"))
            {
                GameModifiersController.trySuper = true;
            }
        }

        public void CheckConfigMessage(IChatMessage message)
        {
            string messageString = message.Message.ToLower();
            if (!messageString.Contains("!gm configchange")) return;
            if (!(message.Sender.IsModerator && Config.allowModCommands) && !message.Sender.IsBroadcaster) return;
            string command = "";
            string property = "";
            bool isPropertyOnly = true;
            string value = value = messageString.Split('=')[1];
            string arg1 = messageString.Split(' ', ' ')[2];
            string arg2 = messageString.Split(' ', ' ', ' ', '=')[3];
            Plugin.Log(arg1 + " " + arg2 + " " + value);
            var data = Plugin.ConfigSettings.GetField<BS_Utils.Utilities.IniFile, BS_Utils.Utilities.Config>("_instance").GetField<IniParser.Model.IniData, BS_Utils.Utilities.IniFile>("data");
            foreach (var section in data.Sections)
            {
                string sectionName = section.SectionName.ToLower();
                if (sectionName == arg1)
                {
                    isPropertyOnly = false;
                    command = section.SectionName;
                }

            }

            if (isPropertyOnly)
            {
                var arg = arg1.Split('=')[0];
                bool found = false;
                foreach (var section in data.Sections)
                {
                    foreach (var key in section.Keys)
                    {
                        if (arg == key.KeyName.ToLower())
                        {
                            found = true;
                            property = key.KeyName;
                        }
                    }
                }
                if (!found) return;
                Config.ChangeConfigValue(property, value);
            }
            else
            {
                bool found = false;
                foreach (var section in data.Sections)
                {
                    if (section.SectionName != command) continue;
                    foreach (var key in section.Keys)
                    {
                        if (arg2 == key.KeyName.ToLower())
                        {
                            found = true;
                            property = key.KeyName;
                        }
                    }
                }
                if (!found) return;
                Config.ChangeConfigValue(command, property, value);
            }
        }

        public void CheckInfoCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm help"))
            {
                ChatMessageHandler.TryAsyncMessage("Guides: For Regular Users - http://bit.ly/1413ChatUser | For Streamers - http://bit.ly/1413Readme | For moderators also view http://bit.ly/1413Config");

            }
            if (message.Message.ToLower().Contains("!gm currentsong"))
            {
                if (!Plugin.isValidScene)
                    ChatMessageHandler.TryAsyncMessage("No song is currently being played.");
                else
                    ChatMessageHandler.TryAsyncMessage("Current Song: " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songName
                        + " - " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songSubName + " mapped by " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.levelAuthorName);
            }

            if (message.Message.ToLower().Contains("!gm chargehelp"))
            {
                if (Config.timeForCharges == 0 || Config.chargesOverTime == 0)
                    ChatMessageHandler.TryAsyncMessage("Every " + Config.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + Config.chargesForSuperCharge + " Charges but will make the effect last much longer! " + Config.chargesPerLevel + " Charges are generated every song with chat mode on.");
                else
                    ChatMessageHandler.TryAsyncMessage("Every " + Config.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + Config.chargesForSuperCharge + " Charges but will make the effect last much longer! " + Config.chargesPerLevel + " Charges are generated every song with chat mode on. Every " + Config.timeForCharges + " seconds, " + Config.chargesOverTime + " are added.");

            }
            if (message.Message.ToLower().Contains("!gm commands"))
            {
                ChatMessageHandler.TryAsyncMessage("Currently supported commands | status: Currrent Status of chat integration | charges: view current charges and costs | chargehelp: Explain charge system");
            }

            if (!Plugin.cooldowns.GetCooldown("chargescommand"))
                if (message.Message.ToLower().Contains("!gm charges"))
                {
                    string output = "Charges: " + GameModifiersController.charges + " | Commands Per Message: " + Config.commandsPerMessage + " | " + Config.GetChargeCostString();
                    string pasteLink = ""; //await PasteRequests.GetHastebin(output, "GamePlayModifiersPlus Response", $"Charge Info for {ChatMessageHandler.commandChannel.Id}");
                    if (string.IsNullOrEmpty(pasteLink))
                    {
                        output = output.Replace("\n", "");
                        ChatMessageHandler.TryAsyncMessage(output);
                    }

                    else
                        ChatMessageHandler.TryAsyncMessage($"View Charge Information Here: {pasteLink}");
                    SharedCoroutineStarter.instance.StartCoroutine(Cooldowns.CommandCoolDown("chargescommand", Config.chargesCommandCoolDown));
                }
        }

        public void CheckStatusCommands(IChatMessage message)
        {
            if (message.Sender.IsBroadcaster || message.Sender.IsModerator)
            {

                if (message.Message.ToLower().Contains("!gm reset") && GMPUI.chatIntegration == true)
                {
                    try
                    {
                        Plugin.cooldowns.ResetCooldowns();
                        TwitchPowers.ResetPowers(true);
                        Plugin.twitchPowers.StopAllCoroutines();
                        GameModifiersController.charges = Config.chargesPerLevel;
                        ChatMessageHandler.TryAsyncMessage("Resetting non Permanent Powers");
                    }
                    catch (System.Exception ex)
                    {
                        Plugin.log.Error("Reset Command Failed: " + ex);
                    }
                }


            }

            /*
            if (message.Message.ToLower().Contains("!gm pp"))
            {
                if (Plugin.currentpp != 0)
                    ChatMessageHandler.TryAsyncMessage("Streamer Rank: #" + Plugin.currentRank + ". Streamer pp: " + Plugin.currentpp + "pp");
                else
                    ChatMessageHandler.TryAsyncMessage("Currently do not have streamer info");
            }
            */
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

                GameModifiersController.beepSound.Play();
                if (GMPUI.chatIntegration)
                    ChatMessageHandler.TryAsyncMessage("Chat Integration Enabled. " + scopeMessage);
                else
                    ChatMessageHandler.TryAsyncMessage("Chat Integration Not Enabled. " + scopeMessage);
            }
        }

        public void CheckHealthCommands(IChatMessage message)
        {
            if (!Plugin.cooldowns.GetCooldown("Health"))
            {
                if (message.Message.ToLower().Contains("!gm instafail"))
                {

                    if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.instaFailChargeCost)
                    {
                        //      GameModifiersController.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(GameObjects.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Health", "Super Insta Fail Active."));
                        GameModifiersController.trySuper = false;
                        GameModifiersController.healthActivated = true;
                        GameModifiersController.charges -= Config.chargesForSuperCharge + Config.instaFailChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (GameModifiersController.charges >= Config.instaFailChargeCost)
                    {
                        //         GameModifiersController.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Config.instaFailDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.instaFailCooldown, "Health", "Insta Fail Active."));
                        GameModifiersController.healthActivated = true;
                        GameModifiersController.charges -= Config.instaFailChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }

                if (message.Message.ToLower().Contains("!gm invincible") && !GameModifiersController.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {
                    if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.invincibleChargeCost)
                    {
                        //         GameModifiersController.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(GameObjects.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Health", "Super Invincibility Active."));
                        GameModifiersController.trySuper = false;
                        GameModifiersController.charges -= Config.chargesForSuperCharge + Config.invincibleChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (GameModifiersController.charges >= Config.invincibleChargeCost)
                    {
                        //          GameModifiersController.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Config.invincibleDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.invincibleCooldown, "Health", "Invincibility Active."));
                        GameModifiersController.charges -= Config.invincibleChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
                if (message.Message.ToLower().Contains("!gm poison"))
                {

                    if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.poisonChargeCost)
                    {
                        //      GameModifiersController.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(GameObjects.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Health", "Health Regen Super Disabled."));
                        GameModifiersController.trySuper = false;
                        GameModifiersController.healthActivated = true;
                        GameModifiersController.charges -= Config.chargesForSuperCharge + Config.poisonChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (GameModifiersController.charges >= Config.poisonChargeCost)
                    {
                        //         GameModifiersController.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(Config.poisonDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.poisonCooldown, "Health", "Health Regen Disabled."));
                        GameModifiersController.healthActivated = true;
                        GameModifiersController.charges -= Config.poisonChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
            }
        }

        public void CheckRotationCommands(IChatMessage message)
        {
            if (GameModifiersController.commandsLeftForMessage == 0) return;
            if (!Plugin.cooldowns.GetCooldown("Rotation"))
            {
                if (message.Message.ToLower().Contains("!gm left"))
                {
                    if (!GMPUI.chatIntegration360)
                        ChatMessageHandler.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (GameModifiersController.charges >= Config.leftChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.LeftRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.leftCoolDownn, "Rotation", "Rotating level to the left."));
                        GameModifiersController.charges -= Config.leftChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm right"))
                {
                    if (!GMPUI.chatIntegration360)
                        ChatMessageHandler.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (GameModifiersController.charges >= Config.rightChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RightRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rightCoolDown, "Rotation", "Rotating level to the right."));
                        GameModifiersController.charges -= Config.rightChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm randomrotation"))
                {
                    if (!GMPUI.chatIntegration360)
                        ChatMessageHandler.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.randomRotationChargeCost)
                    {
                        //       GameModifiersController.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(GameObjects.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Rotation", "Super random level rotation active."));
                        GameModifiersController.trySuper = false;
                        GameModifiersController.charges -= Config.chargesForSuperCharge + Config.randomRotationChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (GameModifiersController.charges >= Config.randomRotationChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(Config.randomRotationDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.randomRotationCoolDown, "Rotation", "Random level rotation active."));
                        GameModifiersController.charges -= Config.randomRotationChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
            }
        }
        public void CheckGameplayCommands(IChatMessage message)
        {

            if (message.Message.ToLower().Contains("!gm da") && !Plugin.cooldowns.GetCooldown("Note") && !Plugin.levelData.GameplayCoreSceneSetupData.gameplayModifiers.disappearingArrows && GameModifiersController.commandsLeftForMessage > 0)
            {
                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.daChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "DA", "Super DA Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.daChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.daChargeCost)
                {
                    //      GameModifiersController.beepSound.Play();

                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Config.daDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.daCooldown, "DA", "DA Active."));
                    GameModifiersController.charges -= Config.daChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm njsrandom") && !Plugin.cooldowns.GetCooldown("RandomNJS") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.njsRandomChargeCost)
                {
                    //         GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "NJSRandom", "Super Random Note Jump Speed Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.njsRandomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.njsRandomChargeCost)
                {
                    //         GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(Config.njsRandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                    GameModifiersController.charges -= Config.njsRandomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm offsetrandom") && !Plugin.cooldowns.GetCooldown("OffsetRandom") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.offsetrandomChargeCost)
                {
                    //         GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "OffsetRandom", "Super Random Note Spawn Offset Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.offsetrandomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.offsetrandomChargeCost)
                {
                    //         GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(Config.offsetrandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.offsetrandomCooldown, "OffsetRandom", "Random Note Spawn Offset Active."));
                    GameModifiersController.charges -= Config.offsetrandomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm noarrows") && !Plugin.cooldowns.GetCooldown("NoArrows") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.noArrowsChargeCost)
                {
                    //        GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "NoArrows", "Super No Arrows Mode Activated."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.noArrowsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.noArrowsChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Config.noArrowsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                    GameModifiersController.charges -= Config.noArrowsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm mirror") && !Plugin.cooldowns.GetCooldown("Mirror") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.mirrorChargeCost)
                {
                    //        GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Mirror", "Super Mirror Mode Activated."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.mirrorChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.mirrorChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(Config.mirrorDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.mirrorCooldown, "Mirror", "Temporary Mirror Mode Activated."));
                    GameModifiersController.charges -= Config.mirrorChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm reverse") && !Plugin.cooldowns.GetCooldown("Map Swap") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.reverseChargeCost)
                {
                    //        GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Map Swap", "Reversing entire map :)"));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.reverseChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.reverseChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(Config.reverseDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.reverseCooldown, "Map Swap", "Temporary Map Reversal."));
                    GameModifiersController.charges -= Config.reverseChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm funky") && !Plugin.cooldowns.GetCooldown("Funky") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.funkyChargeCost)
                {
                    //         GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Funky", "Time to get Funky."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.funkyChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.funkyChargeCost)
                {
                    //           GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Config.funkyDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.funkyCooldown, "Funky", "Funky Mode Activated"));
                    GameModifiersController.charges -= Config.funkyChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm rainbow") && !Plugin.cooldowns.GetCooldown("Rainbow") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.rainbowChargeCost)
                {
                    //          GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Rainbow", "RAIIINBOWWS."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.rainbowChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.rainbowChargeCost)
                {
                    //          GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Config.rainbowDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rainbowCooldown, "Rainbow", "Rainbow Activated"));
                    GameModifiersController.charges -= Config.rainbowChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm bombs") && !Plugin.cooldowns.GetCooldown("Bombs") && GameModifiersController.commandsLeftForMessage > 0 && Config.bombsChance > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.bombsChargeCost)
                {
                    //               GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Bombs", "Bombs Away!"));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.bombsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.bombsChargeCost)
                {
                    //                GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Config.bombsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.bombsCooldown, "Bombs", "Sneaking Bombs into the map."));
                    GameModifiersController.charges -= Config.bombsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm tunnel") && !Plugin.cooldowns.GetCooldown("Tunnel") && GameModifiersController.commandsLeftForMessage > 0)
            {

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.tunnelChargeCost)
                {
                    //               GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Tunnel", "Entering Tunnel. Estimated time of exit: Unknown."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.tunnelChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.tunnelChargeCost)
                {
                    //                GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(Config.tunnelDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.tunnelCoolDown, "Tunnel", $"Entering Tunnel. Estimated time of exit: {Config.tunnelDuration} seconds."));
                    GameModifiersController.charges -= Config.tunnelChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm gametime") && !Plugin.cooldowns.GetCooldown("Map Swap") && GameModifiersController.commandsLeftForMessage > 0)
            {
                if (GameModifiersController.charges >= Config.gameTimeChargeCost)
                {
                    if (!Plugin.gameSaberPluginInstalled)
                        ChatMessageHandler.TryAsyncMessage("A compatible version of the GameSaber Plugin is required to use this command.");
                    else
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.GameTime(Config.gameTimeDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.gameTimeCoolDown, "Map Swap", $"Game Starting, game will last {Config.gameTimeDuration} seconds. Better not Lose."));
                        GameModifiersController.charges -= Config.gameTimeChargeCost;
                        GameModifiersController.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    //                GameModifiersController.beepSound.Play();

                }
            }

        }

        public void CheckMapSwapCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm rctts") && !Plugin.cooldowns.GetCooldown("Map Swap") && GameModifiersController.commandsLeftForMessage > 0)
            {
                //Supercharge on rctts is probably not necessary
                /*
                if (GameModifiersController.trySuper && GameModifiersController.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.rcttsChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RealityCheck(TwitchPowers.RealityClip.length - 1f));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(TwitchPowers.RealityClip.length + GameObjects.songAudio.clip.length, "RCTTS", "Strimmer play Reality Check... The whole thing :)"));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.rcttsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else 
                */
                if (GameModifiersController.charges >= Config.rcttsChargeCost)
                {
                    Plugin.twitchPowers.StartCoroutine(Plugin.twitchPowers.RealityCheck(Mathf.Min(Config.rcttsDuration, TwitchPowers.RealityClip.length - 1f)));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rcttsCooldown, "Map Swap", "Strimmer play Reality Check 🙂 "));
                    GameModifiersController.charges -= Config.rcttsChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }

        public void CheckSizeCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm smaller") && !Plugin.cooldowns.GetCooldown("NormalSize") && GameModifiersController.commandsLeftForMessage > 0)
            {
                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.smallerChargeCost)
                {
                    //      GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.smallerMultiplier, GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.sizeActivated = true;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.smallerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.smallerChargeCost)
                {
                    //      GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.smallerMultiplier, Config.smallerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.smallerCoolDown, "NormalSize", "Temporarily Scaling Notes"));
                    GameModifiersController.sizeActivated = true;
                    GameModifiersController.charges -= Config.smallerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm larger") && !Plugin.cooldowns.GetCooldown("NormalSize") && !GameModifiersController.sizeActivated && GameModifiersController.commandsLeftForMessage > 0)
            {
                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.largerChargeCost)
                {
                    //        GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.largerMultiplier, GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.largerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.largerChargeCost)
                {
                    //      GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.largerMultiplier, Config.largerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.largerCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    GameModifiersController.charges -= Config.largerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm random") && !message.Message.ToLower().Contains("!gm randomrotation") && !Plugin.cooldowns.GetCooldown("Random") && GameModifiersController.commandsLeftForMessage > 0)
            {
                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.randomChargeCost)
                {
                    //        GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(GameObjects.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Random", "Super Random Note Scale Change Active."));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.superRandom = true;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.randomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.randomChargeCost)
                {
                    //       GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Config.randomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.randomCooldown, "Random", "Randomly Scaling Notes"));
                    GameModifiersController.charges -= Config.randomChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }
        }

        public void CheckSpeedCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm faster") && !Plugin.cooldowns.GetCooldown("Speed") && GameModifiersController.commandsLeftForMessage > 0)
            {
                /*
                if (!Plugin.practicePluginInstalled)
                {
                    ChatMessageHandler.TryAsyncMessage("Speed altering commands currently require Practice Plugin to be installed");
                    return;
                }
                */
                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.fasterChargeCost)
                {
                    //               GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(GameObjects.songAudio.clip.length, Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Speed", "Fast Time!"));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.fasterChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.fasterChargeCost)
                {
                    //                GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Config.fasterDuration, Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                    GameModifiersController.charges -= Config.fasterChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm slower") && !Plugin.cooldowns.GetCooldown("Speed") && GameModifiersController.commandsLeftForMessage > 0)
            {
                /*
                if (!Plugin.practicePluginInstalled)
                {
                    ChatMessageHandler.TryAsyncMessage("Speed altering commands currently require Practice Plugin to be installed");
                    return;
                }
                */

                if (GameModifiersController.trySuper && GameModifiersController.charges >= Config.chargesForSuperCharge + Config.slowerChargeCost)
                {
                    //               GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(GameObjects.songAudio.clip.length, Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(GameObjects.songAudio.clip.length, "Speed", "Weakling Slower Song Time!"));
                    GameModifiersController.trySuper = false;
                    GameModifiersController.charges -= Config.chargesForSuperCharge + Config.slowerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (GameModifiersController.charges >= Config.slowerChargeCost)
                {
                    //                GameModifiersController.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Config.slowerDuration, Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                    GameModifiersController.charges -= Config.slowerChargeCost;
                    GameModifiersController.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
        }
        public int CheckCommandScope()
        {

            if (Config.allowEveryone) return 0;
            else if (Config.allowSubs) return 1;
            else return 2;
        }

        public void CheckGlobalCoolDown()
        {
            if (Config.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false && globalActive)
            {
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                Plugin.twitchPowers.StartCoroutine(TwitchPowers.GlobalCoolDown());
                text.text += " " + "Global" + " | ";
            }
            globalActive = false;
        }
    }
}
