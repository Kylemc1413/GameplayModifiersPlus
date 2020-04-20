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

                if (Plugin.charges >= Config.pauseChargeCost)
                {
                    Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Pause());
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.pauseGlobalCooldown, "Global", "Game Paused 🙂"));
                    Plugin.charges -= Config.pauseChargeCost;
                    Plugin.commandsLeftForMessage = 0;
                }
            }
        }


        public void CheckChargeMessage(IChatMessage message)
        {
            TwitchMessage twitchMessage = message is TwitchMessage ? message as TwitchMessage : null;
            if (twitchMessage == null) return;

            if (twitchMessage.Bits >= Config.bitsPerCharge && Config.bitsPerCharge > 0)
            {

                Plugin.charges += (twitchMessage.Bits / Config.bitsPerCharge);
                Plugin.TryAsyncMessage("Current Charges: " + Plugin.charges);
            }
            if (twitchMessage.Sender.Name.ToLower() == "kyle1413k" && message.Message.ToLower().Contains("!gm admincharge"))
            {
                Plugin.charges += (Config.maxCharges / 2);
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
            if (!messageString.Contains("!gm configchange")) return;
            if (!(message.Sender.IsModerator && Config.allowModCommands) && !message.Sender.IsBroadcaster) return;
            string command = "";
            string property = "";
            bool isPropertyOnly = true;
            string value = value = messageString.Split('=')[1];
            string arg1 = messageString.Split(' ', ' ')[2];
            string arg2 = messageString.Split(' ', ' ', ' ', '=')[3];
            Plugin.Log(arg1 + " " + arg2 + " " + value);
            IniParser.Model.IniData data = Plugin.ConfigSettings.GetField<object>("_instance").GetField<IniParser.Model.IniData>("data");
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
                Plugin.TryAsyncMessage("Guides: For Regular Users - http://bit.ly/1413ChatUser | For Streamers - http://bit.ly/1413Readme | For moderators also view http://bit.ly/1413Config");

            }
            if (message.Message.ToLower().Contains("!gm currentsong"))
            {
                if (!Plugin.isValidScene)
                    Plugin.TryAsyncMessage("No song is currently being played.");
                else
                    Plugin.TryAsyncMessage("Current Song: " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songName
                        + " - " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.songSubName + " mapped by " + Plugin.levelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.levelAuthorName);
            }

            if (message.Message.ToLower().Contains("!gm chargehelp"))
            {
                if (Config.timeForCharges == 0 || Config.chargesOverTime == 0)
                    Plugin.TryAsyncMessage("Every " + Config.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + Config.chargesForSuperCharge + " Charges but will make the effect last much longer! " + Config.chargesPerLevel + " Charges are generated every song with chat mode on.");
                else
                    Plugin.TryAsyncMessage("Every " + Config.bitsPerCharge + " bits sent with a message adds a charge, which are used to activate commands! If you add super at the end of a command, it will cost " + Config.chargesForSuperCharge + " Charges but will make the effect last much longer! " + Config.chargesPerLevel + " Charges are generated every song with chat mode on. Every " + Config.timeForCharges + " seconds, " + Config.chargesOverTime + " are added.");

            }
            if (message.Message.ToLower().Contains("!gm commands"))
            {
                Plugin.TryAsyncMessage("Currently supported commands | status: Currrent Status of chat integration | charges: view current charges and costs | chargehelp: Explain charge system");
            }

            if (!Plugin.cooldowns.GetCooldown("chargescommand"))
                if (message.Message.ToLower().Contains("!gm charges"))
                {
                    Plugin.TryAsyncMessage("Charges: " + Plugin.charges + " | Commands Per Message: " + Config.commandsPerMessage + " | " + Config.GetChargeCostString());
                    SharedCoroutineStarter.instance.StartCoroutine(Cooldowns.CommandCoolDown("chargescommand", Config.chargesCommandCoolDown));
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
                    Plugin.charges = Config.chargesPerLevel;
                    Plugin.TryAsyncMessage("Resetting non Permanent Powers");
                }


            }

            /*
            if (message.Message.ToLower().Contains("!gm pp"))
            {
                if (Plugin.currentpp != 0)
                    Plugin.TryAsyncMessage("Streamer Rank: #" + Plugin.currentRank + ". Streamer pp: " + Plugin.currentpp + "pp");
                else
                    Plugin.TryAsyncMessage("Currently do not have streamer info");
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

                    if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.instaFailChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Insta Fail Active."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= Config.chargesForSuperCharge + Config.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Config.instaFailChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Config.instaFailDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.instaFailCooldown, "Health", "Insta Fail Active."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= Config.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }

                if (message.Message.ToLower().Contains("!gm invincible") && !Plugin.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {
                    if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.invincibleChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Invincibility Active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= Config.chargesForSuperCharge + Config.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Config.invincibleChargeCost)
                    {
                        //          Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Config.invincibleDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.invincibleCooldown, "Health", "Invincibility Active."));
                        Plugin.charges -= Config.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
                if (message.Message.ToLower().Contains("!gm poison"))
                {

                    if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.poisonChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Health Regen Super Disabled."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= Config.chargesForSuperCharge + Config.poisonChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Config.poisonChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempPoison(Config.poisonDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.poisonCooldown, "Health", "Health Regen Disabled."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= Config.poisonChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
            }
        }

        public void CheckRotationCommands(IChatMessage message)
        {
            if (Plugin.commandsLeftForMessage == 0) return;
            if (!Plugin.cooldowns.GetCooldown("Rotation"))
            {
                if (message.Message.ToLower().Contains("!gm left"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.charges >= Config.leftChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.LeftRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.leftCoolDownn, "Rotation", "Rotating level to the left."));
                        Plugin.charges -= Config.leftChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm right"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.charges >= Config.rightChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RightRotation());
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rightCoolDown, "Rotation", "Rotating level to the right."));
                        Plugin.charges -= Config.rightChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                }
                else if (message.Message.ToLower().Contains("!gm randomrotation"))
                {
                    if (!GMPUI.chatIntegration360)
                        Plugin.TryAsyncMessage("Rotation Based Commands currently disabled. Please turn on chatintegration360 if you would like to use these commands.");
                    else if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.randomRotationChargeCost)
                    {
                        //       Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rotation", "Super random level rotation active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= Config.chargesForSuperCharge + Config.randomRotationChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Config.randomRotationChargeCost)
                    {
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomRotation(Config.randomRotationDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.randomRotationCoolDown, "Rotation", "Random level rotation active."));
                        Plugin.charges -= Config.randomRotationChargeCost;
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
                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.daChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "DA", "Super DA Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.daChargeCost)
                {
                    //      Plugin.beepSound.Play();

                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Config.daDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.daCooldown, "DA", "DA Active."));
                    Plugin.charges -= Config.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm njsrandom") && !Plugin.cooldowns.GetCooldown("RandomNJS") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NJSRandom", "Super Random Note Jump Speed Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.NjsRandom(Config.njsRandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                    Plugin.charges -= Config.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm offsetrandom") && !Plugin.cooldowns.GetCooldown("OffsetRandom") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.offsetrandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "OffsetRandom", "Super Random Note Spawn Offset Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.offsetrandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.offsetrandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.OffsetRandom(Config.offsetrandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.offsetrandomCooldown, "OffsetRandom", "Random Note Spawn Offset Active."));
                    Plugin.charges -= Config.offsetrandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm noarrows") && !Plugin.cooldowns.GetCooldown("NoArrows") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.noArrowsChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NoArrows", "Super No Arrows Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.noArrowsChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Config.noArrowsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                    Plugin.charges -= Config.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm mirror") && !Plugin.cooldowns.GetCooldown("Mirror") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.mirrorChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Mirror", "Super Mirror Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.mirrorChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.mirrorChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempMirror(Config.mirrorDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.mirrorCooldown, "Mirror", "Temporary Mirror Mode Activated."));
                    Plugin.charges -= Config.mirrorChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm reverse") && !Plugin.cooldowns.GetCooldown("Map Swap") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.reverseChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Map Swap", "Reversing entire map :)"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.reverseChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.reverseChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Reverse(Config.reverseDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.reverseCooldown, "Map Swap", "Temporary Map Reversal."));
                    Plugin.charges -= Config.reverseChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm funky") && !Plugin.cooldowns.GetCooldown("Funky") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.funkyChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Funky", "Time to get Funky."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.funkyChargeCost)
                {
                    //           Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Config.funkyDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.funkyCooldown, "Funky", "Funky Mode Activated"));
                    Plugin.charges -= Config.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm rainbow") && !Plugin.cooldowns.GetCooldown("Rainbow") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rainbow", "RAIIINBOWWS."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Config.rainbowDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rainbowCooldown, "Rainbow", "Rainbow Activated"));
                    Plugin.charges -= Config.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.Message.ToLower().Contains("!gm bombs") && !Plugin.cooldowns.GetCooldown("Bombs") && Plugin.commandsLeftForMessage > 0 && Config.bombsChance > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.bombsChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Bombs", "Bombs Away!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.bombsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.bombsChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Config.bombsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.bombsCooldown, "Bombs", "Sneaking Bombs into the map."));
                    Plugin.charges -= Config.bombsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.Message.ToLower().Contains("!gm tunnel") && !Plugin.cooldowns.GetCooldown("Tunnel") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.tunnelChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Tunnel", "Entering Tunnel. Estimated time of exit: Unknown."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.tunnelChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.tunnelChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Encasement(Config.tunnelDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.tunnelCoolDown, "Tunnel", $"Entering Tunnel. Estimated time of exit: {Config.tunnelDuration} seconds."));
                    Plugin.charges -= Config.tunnelChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }

        public void CheckMapSwapCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm rctts") && !Plugin.cooldowns.GetCooldown("Map Swap") && Plugin.commandsLeftForMessage > 0)
            {
                //Supercharge on rctts is probably not necessary
                /*
                if (Plugin.trySuper && Plugin.charges >= ChatConfig.chargesForSuperCharge + ChatConfig.rcttsChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RealityCheck(TwitchPowers.RealityClip.length - 1f));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(TwitchPowers.RealityClip.length + Plugin.songAudio.clip.length, "RCTTS", "Strimmer play Reality Check... The whole thing :)"));
                    Plugin.trySuper = false;
                    Plugin.charges -= ChatConfig.chargesForSuperCharge + ChatConfig.rcttsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else 
                */
                if (Plugin.charges >= Config.rcttsChargeCost)
                {
                    Plugin.twitchPowers.StartCoroutine(Plugin.twitchPowers.RealityCheck(Mathf.Min(Config.rcttsDuration, TwitchPowers.RealityClip.length - 1f)));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.rcttsCooldown, "Map Swap", "Strimmer play Reality Check 🙂 "));
                    Plugin.charges -= Config.rcttsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }

        public void CheckSizeCommands(IChatMessage message)
        {
            if (message.Message.ToLower().Contains("!gm smaller") && !Plugin.cooldowns.GetCooldown("NormalSize") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.smallerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.smallerMultiplier, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.smallerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.smallerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.smallerMultiplier, Config.smallerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.smallerCoolDown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Config.smallerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm larger") && !Plugin.cooldowns.GetCooldown("NormalSize") && !Plugin.sizeActivated && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.largerChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.largerMultiplier, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.largerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.largerChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(Config.largerMultiplier, Config.largerDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.largerCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.charges -= Config.largerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.Message.ToLower().Contains("!gm random") && !message.Message.ToLower().Contains("!gm randomrotation") && !Plugin.cooldowns.GetCooldown("Random") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.randomChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Random", "Super Random Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.superRandom = true;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.randomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.randomChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Config.randomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.randomCooldown, "Random", "Randomly Scaling Notes"));
                    Plugin.charges -= Config.randomChargeCost;
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
                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.fasterChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Fast Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.fasterChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Config.fasterDuration, Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                    Plugin.charges -= Config.fasterChargeCost;
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


                if (Plugin.trySuper && Plugin.charges >= Config.chargesForSuperCharge + Config.slowerChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Weakling Slower Song Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Config.chargesForSuperCharge + Config.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Config.slowerChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Config.slowerDuration, Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Config.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                    Plugin.charges -= Config.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
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
