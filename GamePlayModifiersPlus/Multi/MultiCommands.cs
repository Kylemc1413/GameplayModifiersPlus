namespace GamePlayModifiersPlus.Multiplayer
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using UnityEngine;
    using GamePlayModifiersPlus;
    public class MultiCommands
    {
        public static bool globalActive = false;

        public void CheckPauseMessage(String message)
        {
            if (message.ToLower().Contains("!gm pause") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.charges >= Plugin.ChatConfig.pauseChargeCost)
                {
                    Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Pause());
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.pauseGlobalCooldown, "Global", "Game Paused :)"));
                    Plugin.charges -= Plugin.ChatConfig.pauseChargeCost;
                    Plugin.commandsLeftForMessage = 0;
                }
            }
        }


        public void CheckHealthCommands(String message)
        {
            if (!Plugin.cooldowns.GetCooldown("Health"))
            {
                if (message.ToLower().Contains("!gm instafail"))
                {

                    if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.instaFailChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Insta Fail Active."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Plugin.ChatConfig.instaFailChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.ChatConfig.instaFailDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.instaFailCooldown, "Health", "Insta Fail Active."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= Plugin.ChatConfig.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }

                if (message.ToLower().Contains("!gm invincible") && !Plugin.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {
                    if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.invincibleChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Invincibility Active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Plugin.ChatConfig.invincibleChargeCost)
                    {
                        //          Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.ChatConfig.invincibleDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.invincibleCooldown, "Health", "Invincibility Active."));
                        Plugin.charges -= Plugin.ChatConfig.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }
            }
        }

        public void CheckSizeCommands(String message)
        {
            if (message.ToLower().Contains("!gm smaller") && !Plugin.cooldowns.GetCooldown("NormalSize") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.smallerNoteChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(0.7f, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.smallerNoteChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.smallerNoteChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(0.7f, Plugin.ChatConfig.smallerNoteDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.smallerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Plugin.ChatConfig.smallerNoteChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.ToLower().Contains("!gm larger") && !Plugin.cooldowns.GetCooldown("NormalSize") && !Plugin.sizeActivated && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.largerNotesChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(1.3f, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.largerNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.largerNotesChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(1.3f, Plugin.ChatConfig.largerNotesDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.largerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.charges -= Plugin.ChatConfig.largerNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.ToLower().Contains("!gm random") && !Plugin.cooldowns.GetCooldown("Random") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.randomNotesChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Random", "Super Random Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.superRandom = true;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.randomNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.randomNotesChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.ChatConfig.randomNotesDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.randomNotesCooldown, "Random", "Randomly Scaling Notes"));
                    Plugin.charges -= Plugin.ChatConfig.randomNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }
        }

        public void CheckGameplayCommands(String message)
        {

            if (message.ToLower().Contains("!gm da") && !Plugin.cooldowns.GetCooldown("Note") && !Plugin.levelData.gameplayCoreSetupData.gameplayModifiers.disappearingArrows && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.daChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "DA", "Super DA Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.daChargeCost)
                {
                    //      Plugin.beepSound.Play();

                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.ChatConfig.daDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.daCooldown, "DA", "DA Active."));
                    Plugin.charges -= Plugin.ChatConfig.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.ToLower().Contains("!gm njsrandom") && !Plugin.cooldowns.GetCooldown("RandomNJS") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.njsRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NJSRandom", "Super Random Note Jump Speed Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.njsRandom(Plugin.ChatConfig.njsRandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                    Plugin.charges -= Plugin.ChatConfig.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm noarrows") && !Plugin.cooldowns.GetCooldown("NoArrows") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.noArrowsChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NoArrows", "Super No Arrows Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.noArrowsChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.ChatConfig.noArrowsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                    Plugin.charges -= Plugin.ChatConfig.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm funky") && !Plugin.cooldowns.GetCooldown("Funky") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.funkyChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Funky", "Time to get Funky."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.funkyChargeCost)
                {
                    //           Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.ChatConfig.funkyDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.funkyCooldown, "Funky", "Funky Mode Activated"));
                    Plugin.charges -= Plugin.ChatConfig.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm rainbow") && !Plugin.cooldowns.GetCooldown("Rainbow") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rainbow", "RAIIINBOWWS."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.ChatConfig.rainbowDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.rainbowCooldown, "Rainbow", "Rainbow Activated"));
                    Plugin.charges -= Plugin.ChatConfig.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.ToLower().Contains("!gm bombs") && !Plugin.cooldowns.GetCooldown("Bombs") && Plugin.commandsLeftForMessage > 0 && Plugin.ChatConfig.bombChance > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.bombChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Bombs", "Bombs Away!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.bombChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.bombChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.ChatConfig.bombDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.bombCooldown, "Bombs", "Sneaking Bombs into the map."));
                    Plugin.charges -= Plugin.ChatConfig.bombChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }

        public void CheckSpeedCommands(String message)
        {
            if (message.ToLower().Contains("!gm faster") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.fasterChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Plugin.ChatConfig.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Fast Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.fasterChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.ChatConfig.fasterDuration, Plugin.ChatConfig.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                    Plugin.charges -= Plugin.ChatConfig.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm slower") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.slowerChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Plugin.ChatConfig.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Weakling Slower Song Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.ChatConfig.chargesForSuperCharge + Plugin.ChatConfig.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.ChatConfig.slowerChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.ChatConfig.slowerDuration, Plugin.ChatConfig.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.ChatConfig.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                    Plugin.charges -= Plugin.ChatConfig.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
        }
        public int CheckCommandScope()
        {

            if (Plugin.ChatConfig.allowEveryone) return 0;
            else if (Plugin.ChatConfig.allowSubs) return 1;
            else return 2;
        }

        public void CheckGlobalCoolDown()
        {
            if (Plugin.ChatConfig.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false && globalActive)
            {
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                Plugin.twitchPowers.StartCoroutine(TwitchPowers.GlobalCoolDown());
                text.text += " " + "Global" + " | ";
            }
            globalActive = false;
        }
    }
}
