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

                if (Plugin.charges >= Plugin.Config.pauseChargeCost)
                {
                    Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Pause());
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.pauseGlobalCooldown, "Global", "Game Paused :)"));
                    Plugin.charges -= Plugin.Config.pauseChargeCost;
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

                    if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.instaFailChargeCost)
                    {
                        //      Plugin.beepSound.Play();

                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Insta Fail Active."));
                        Plugin.trySuper = false;
                        Plugin.healthActivated = true;
                        Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Plugin.Config.instaFailChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInstaFail(Plugin.Config.instaFailDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.instaFailCooldown, "Health", "Insta Fail Active."));
                        Plugin.healthActivated = true;
                        Plugin.charges -= Plugin.Config.instaFailChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }

                }

                if (message.ToLower().Contains("!gm invincible") && !Plugin.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {
                    if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.invincibleChargeCost)
                    {
                        //         Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.songAudio.clip.length));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Health", "Super Invincibility Active."));
                        Plugin.trySuper = false;
                        Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.invincibleChargeCost;
                        Plugin.commandsLeftForMessage -= 1;
                        globalActive = true;
                    }
                    else if (Plugin.charges >= Plugin.Config.invincibleChargeCost)
                    {
                        //          Plugin.beepSound.Play();
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempInvincibility(Plugin.Config.invincibleDuration));
                        Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.invincibleCooldown, "Health", "Invincibility Active."));
                        Plugin.charges -= Plugin.Config.invincibleChargeCost;
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
                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.smallerNoteChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(0.7f, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.smallerNoteChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.smallerNoteChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(0.7f, Plugin.Config.smallerNoteDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.smallerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.sizeActivated = true;
                    Plugin.charges -= Plugin.Config.smallerNoteChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.ToLower().Contains("!gm larger") && !Plugin.cooldowns.GetCooldown("NormalSize") && !Plugin.sizeActivated && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.largerNotesChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(1.3f, Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NormalSize", "Super Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.largerNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.largerNotesChargeCost)
                {
                    //      Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.ScaleNotes(1.3f, Plugin.Config.largerNotesDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.largerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                    Plugin.charges -= Plugin.Config.largerNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }

            if (message.ToLower().Contains("!gm random") && !Plugin.cooldowns.GetCooldown("Random") && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.randomNotesChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Random", "Super Random Note Scale Change Active."));
                    Plugin.trySuper = false;
                    Plugin.superRandom = true;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.randomNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.randomNotesChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomNotes(Plugin.Config.randomNotesDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.randomNotesCooldown, "Random", "Randomly Scaling Notes"));
                    Plugin.charges -= Plugin.Config.randomNotesChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }

            }
        }

        public void CheckGameplayCommands(String message)
        {

            if (message.ToLower().Contains("!gm da") && !Plugin.cooldowns.GetCooldown("Note") && !Plugin.levelData.gameplayCoreSetupData.gameplayModifiers.disappearingArrows && Plugin.commandsLeftForMessage > 0)
            {
                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.daChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "DA", "Super DA Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.daChargeCost)
                {
                    //      Plugin.beepSound.Play();

                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempDA(Plugin.Config.daDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.daCooldown, "DA", "DA Active."));
                    Plugin.charges -= Plugin.Config.daChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.ToLower().Contains("!gm njsrandom") && !Plugin.cooldowns.GetCooldown("RandomNJS") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.njsRandom(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NJSRandom", "Super Random Note Jump Speed Active."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.njsRandomChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.njsRandom(Plugin.Config.njsRandomDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                    Plugin.charges -= Plugin.Config.njsRandomChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm noarrows") && !Plugin.cooldowns.GetCooldown("NoArrows") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.noArrowsChargeCost)
                {
                    //        Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "NoArrows", "Super No Arrows Mode Activated."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.noArrowsChargeCost)
                {
                    //       Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.TempNoArrows(Plugin.Config.noArrowsDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                    Plugin.charges -= Plugin.Config.noArrowsChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm funky") && !Plugin.cooldowns.GetCooldown("Funky") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.funkyChargeCost)
                {
                    //         Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Funky", "Time to get Funky."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.funkyChargeCost)
                {
                    //           Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Funky(Plugin.Config.funkyDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.funkyCooldown, "Funky", "Funky Mode Activated"));
                    Plugin.charges -= Plugin.Config.funkyChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm rainbow") && !Plugin.cooldowns.GetCooldown("Rainbow") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Rainbow", "RAIIINBOWWS."));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.rainbowChargeCost)
                {
                    //          Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.Rainbow(Plugin.Config.rainbowDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.rainbowCooldown, "Rainbow", "Rainbow Activated"));
                    Plugin.charges -= Plugin.Config.rainbowChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

            if (message.ToLower().Contains("!gm bombs") && !Plugin.cooldowns.GetCooldown("Bombs") && Plugin.commandsLeftForMessage > 0 && Plugin.Config.bombChance > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.bombChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.songAudio.clip.length));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Bombs", "Bombs Away!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.bombChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.bombChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.RandomBombs(Plugin.Config.bombDuration));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.bombCooldown, "Bombs", "Sneaking Bombs into the map."));
                    Plugin.charges -= Plugin.Config.bombChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }

        }

        public void CheckSpeedCommands(String message)
        {
            if (message.ToLower().Contains("!gm faster") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.fasterChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Plugin.Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Fast Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.fasterChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.Config.fasterDuration, Plugin.Config.fasterMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                    Plugin.charges -= Plugin.Config.fasterChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
            if (message.ToLower().Contains("!gm slower") && !Plugin.cooldowns.GetCooldown("Speed") && Plugin.commandsLeftForMessage > 0)
            {

                if (Plugin.trySuper && Plugin.charges >= Plugin.Config.chargesForSuperCharge + Plugin.Config.slowerChargeCost)
                {
                    //               Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.songAudio.clip.length, Plugin.Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.songAudio.clip.length, "Speed", "Weakling Slower Song Time!"));
                    Plugin.trySuper = false;
                    Plugin.charges -= Plugin.Config.chargesForSuperCharge + Plugin.Config.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
                else if (Plugin.charges >= Plugin.Config.slowerChargeCost)
                {
                    //                Plugin.beepSound.Play();
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.SpeedChange(Plugin.Config.slowerDuration, Plugin.Config.slowerMultiplier));
                    Plugin.twitchPowers.StartCoroutine(TwitchPowers.CoolDown(Plugin.Config.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                    Plugin.charges -= Plugin.Config.slowerChargeCost;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;
                }
            }
        }
        public int CheckCommandScope()
        {

            if (Plugin.Config.allowEveryone) return 0;
            else if (Plugin.Config.allowSubs) return 1;
            else return 2;
        }

        public void CheckGlobalCoolDown()
        {
            if (Plugin.Config.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false && globalActive)
            {
                var text = GameObject.Find("Chat Powers").GetComponent<GamePlayModifiersPlus.TwitchStuff.GMPDisplay>().cooldownText;
                Plugin.twitchPowers.StartCoroutine(TwitchPowers.GlobalCoolDown());
                text.text += " " + "Global" + " | ";
            }
            globalActive = false;
        }
    }
}
