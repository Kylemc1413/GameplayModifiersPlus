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


        public void CheckHealthCommands(String message)
        {
            if (!Plugin.cooldowns.GetCooldown("Health"))
            {
                if (message.Contains("!gmm instafail") && message.Contains(MultiClientInterface.version))
                {

                    //         Plugin.beepSound.Play();
                    MultiMain.Powers.StartCoroutine(MultiPowers.TempInstaFail(MultiMain.Config.instaFailDuration));
                    MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.instaFailCooldown, "Health", "Insta Fail Active."));
                    Plugin.healthActivated = true;
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;


                }

                if (message.Contains("!gmm invincible") && message.Contains(MultiClientInterface.version) && !Plugin.healthActivated && !Plugin.cooldowns.GetCooldown("Health"))
                {

                    //          Plugin.beepSound.Play();
                    MultiMain.Powers.StartCoroutine(MultiPowers.TempInvincibility(MultiMain.Config.invincibleDuration));
                    MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.invincibleCooldown, "Health", "Invincibility Active."));
                    Plugin.commandsLeftForMessage -= 1;
                    globalActive = true;


                }
            }
        }

        public void CheckSizeCommands(String message)
        {
            if (message.Contains("!gmm smaller") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("NormalSize"))
            {

                //      Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.ScaleNotes(0.7f, MultiMain.Config.smallerNoteDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.smallerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                Plugin.sizeActivated = true;
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;


            }

            if (message.Contains("!gmm larger") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("NormalSize") && !Plugin.sizeActivated)
            {

                //      Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.ScaleNotes(1.3f, MultiMain.Config.largerNotesDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.largerNotesCooldown, "NormalSize", "Temporarily Scaling Notes"));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;


            }

            if (message.Contains("!gmm random") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Random"))
            {

                //       Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.RandomNotes(MultiMain.Config.randomNotesDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.randomNotesCooldown, "Random", "Randomly Scaling Notes"));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;
            }


        }

        public void CheckGameplayCommands(String message)
        {
            if (message.Contains("!gmm da") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("DA"))
            {
                MultiMain.Log("Trying DA");
                //      Plugin.beepSound.Play();

                MultiMain.Powers.StartCoroutine(MultiPowers.TempDA(MultiMain.Config.daDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.daCooldown, "DA", "DA Active."));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }

            if (message.Contains("!gmm njsrandom") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("RandomNJS"))
            {

                //         Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.NjsRandom(MultiMain.Config.njsRandomDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.njsRandomCooldown, "NJSRandom", "Random Note Jump Speed Active."));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }
            if (message.Contains("!gmm noarrows") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("NoArrows"))
            {


                //       Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.TempNoArrows(MultiMain.Config.noArrowsDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.noArrowsCooldown, "NoArrows", "Temporary No Arrows Activated"));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }
            if (message.Contains("!gmm funky") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Funky"))
            {


                //           Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.Funky(MultiMain.Config.funkyDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.funkyCooldown, "Funky", "Funky Mode Activated"));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }
            if (message.Contains("!gmm rainbow") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Rainbow"))
            {


                //          Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.Rainbow(MultiMain.Config.rainbowDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.rainbowCooldown, "Rainbow", "Rainbow Activated"));
 
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }

            if (message.Contains("!gmm bombs") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Bombs") && MultiMain.Config.bombChance > 0)
            {


                //                Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.RandomBombs(MultiMain.Config.bombDuration));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.bombCooldown, "Bombs", "Sneaking Bombs into the map."));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }

        }

        public void CheckSpeedCommands(String message)
        {
            if (message.Contains("!gmm faster") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Speed"))
            {

                //                Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.SpeedChange(MultiMain.Config.fasterDuration, MultiMain.Config.fasterMultiplier));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.fasterCooldown, "Speed", "Temporary faster song speed Active."));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }
            if (message.Contains("!gmm slower") && message.Contains(MultiClientInterface.version) && !Plugin.cooldowns.GetCooldown("Speed"))
            {
                //                Plugin.beepSound.Play();
                MultiMain.Powers.StartCoroutine(MultiPowers.SpeedChange(MultiMain.Config.slowerDuration, MultiMain.Config.slowerMultiplier));
                MultiMain.Powers.StartCoroutine(MultiPowers.CoolDown(MultiMain.Config.slowerCooldown, "Speed", "Temporary slower song speed Active."));
                Plugin.commandsLeftForMessage -= 1;
                globalActive = true;

            }
        }


        public void CheckGlobalCoolDown()
        {
            if (MultiMain.Config.globalCommandCooldown > 0 && Plugin.cooldowns.GetCooldown("Global") == false && globalActive)
            {
                var text = GameObject.Find("Chat Powers").GetComponent<MultiGMPDisplay>().cooldownText;
                MultiMain.Powers.StartCoroutine(MultiPowers.GlobalCoolDown());
                text.text += " " + "Global" + " | ";
            }
            globalActive = false;
        }
    }
}
