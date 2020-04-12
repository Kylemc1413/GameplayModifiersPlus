namespace GamePlayModifiersPlus
{
    using System;
    using System.IO;
    using BS_Utils.Utilities;
    internal static class ChatConfig
    {
        public static bool uiOnTop = true;
        public static bool showCooldownOnMessage = false;
        public static bool allowModCommands = true;
        public static int commandsPerMessage = 2;
        public static float globalCommandCooldown = 10f;
        public static bool allowSubs = true;
        public static bool allowEveryone = true;
        public static int chargesForSuperCharge = 10;
        public static int chargesPerLevel = 10;
        public static int maxCharges = 50;
        public static int bitsPerCharge = 10;
        public static bool resetChargesEachLevel = false;

        public static int chargesOverTime = 1;
        public static float timeForCharges = 15f;

        public static int daChargeCost = 0;
        public static int smallerChargeCost = 1;
        public static int largerChargeCost = 1;
        public static int randomChargeCost = 4;
        public static int instaFailChargeCost = 20;
        public static int invincibleChargeCost = 0;
        public static int njsRandomChargeCost = 5;
        public static int noArrowsChargeCost = 10;
        public static int funkyChargeCost = 5;
        public static int rainbowChargeCost = 3;
        public static int pauseChargeCost = 20;
        public static int bombsChargeCost = 7;
        public static int fasterChargeCost = 4;
        public static int slowerChargeCost = 4;
        public static int poisonChargeCost = 5;
        public static int mirrorChargeCost = 3;
        public static int offsetrandomChargeCost = 4;
        public static int reverseChargeCost = 7;
        //     public static int nMirrorChargeCost = 0;
        public static float daDuration = 15f;
        public static float smallerDuration = 10f;
        public static float largerDuration = 10f;
        public static float randomDuration = 10f;
        public static float instaFailDuration = 10f;
        public static float invincibleDuration = 15f;
        public static float njsRandomDuration = 10f;
        public static float noArrowsDuration = 10f;
        public static float funkyDuration = 10f;
        public static float rainbowDuration = 10f;
        public static float bombsDuration = 15f;
        public static float fasterDuration = 15f;
        public static float slowerDuration = 15f;
        public static float poisonDuration = 15f;
        public static float mirrorDuration = 20f;
        public static float offsetrandomDuration = 10f;
        public static float reverseDuration = 15f;
        //   public static float nMirrorDuration = 15f;

        public static float daCooldown = 20f;
        public static float smallerCoolDown = 15f;
        public static float largerCooldown = 15f;
        public static float randomCooldown = 15f;
        public static float instaFailCooldown = 45f;
        public static float invincibleCooldown = 20f;
        public static float njsRandomCooldown = 30f;
        public static float noArrowsCooldown = 20f;
        public static float funkyCooldown = 30f;
        public static float rainbowCooldown = 20f;
        public static float pauseGlobalCooldown = 60f;
        public static float bombsCooldown = 45f;
        public static float fasterCooldown = 30f;
        public static float slowerCooldown = 30f;
        public static float poisonCooldown = 30f;
        public static float mirrorCooldown = 20f;
        public static float offsetrandomCooldown = 30f;
        public static float reverseCooldown = 60f;
        //   public static float nMirrorCooldown= 20f;
        public static float njsRandomMin = 8f;
        public static float njsRandomMax = 16f;

        public static int offsetrandomMin = 0;
        public static int offsetrandomMax = 4;

        public static float randomMin = 0.6f;
        public static float randomMax = 1.5f;

        public static float bombsChance = 0.33f;

        public static float smallerMultiplier = .75f;
        public static float largerMultiplier = 1.35f;
        public static float fasterMultiplier = 1.2f;
        public static float slowerMultiplier = .85f;

        public static int leftChargeCost = 5;
        public static int rightChargeCost = 5;
        public static float leftCoolDownn = 15f;
        public static float rightCoolDown = 15f;

        public static int randomRotationChargeCost = 10;
        public static float randomRotationDuration = 5f;
        public static float randomRotationCoolDown = 30f;
        public static int tunnelChargeCost = 5;
        public static float tunnelDuration = 15f;
        public static float tunnelCoolDown = 30f;
        private static string chargeCostString;








        public static void Load()
        {
            //Basic Setup
            uiOnTop = Plugin.ChatConfigSettings.GetBool("Basic Setup", "uiOnTop", true, true);
            allowEveryone = Plugin.ChatConfigSettings.GetBool("Basic Setup", "allowEveryone", true, true);
            allowSubs = Plugin.ChatConfigSettings.GetBool("Basic Setup", "allowSubs", true, true);
            allowModCommands = Plugin.ChatConfigSettings.GetBool("Basic Setup", "allowModCommands", true, true);
            commandsPerMessage = Plugin.ChatConfigSettings.GetInt("Basic Setup", "commandsPerMessage", 2, true);
            globalCommandCooldown = Plugin.ChatConfigSettings.GetFloat("Basic Setup", "globalCommandCooldown", 10f, true);
            showCooldownOnMessage = Plugin.ChatConfigSettings.GetBool("Basic Setup", "showCooldownOnMessage", false, true);

            GMPUI.chatIntegration360 = Plugin.ChatConfigSettings.GetBool("Basic Setup", "chatintegration360", false, true);
            //Charges
            resetChargesEachLevel = Plugin.ChatConfigSettings.GetBool("Charges", "resetChargesEachLevel", false, true);
            maxCharges = Plugin.ChatConfigSettings.GetInt("Charges", "maxCharges", 50, true);
            chargesForSuperCharge = Plugin.ChatConfigSettings.GetInt("Charges", "chargesForSuperCharge", 10, true);
            chargesPerLevel = Plugin.ChatConfigSettings.GetInt("Charges", "chargesPerLevel", 10, true);
            chargesOverTime = Plugin.ChatConfigSettings.GetInt("Charges", "chargesOverTime", 1, true);
            timeForCharges = Plugin.ChatConfigSettings.GetFloat("Charges", "timeForCharges", 15f, true);
            bitsPerCharge = Plugin.ChatConfigSettings.GetInt("Charges", "bitsPerCharge", 10, true);

            // COMMANDS

            //Invincible
            invincibleChargeCost = Plugin.ChatConfigSettings.GetInt("Invincible", "ChargeCost", 0, true);
            invincibleCooldown = Plugin.ChatConfigSettings.GetFloat("Invincible", "CoolDown", 15f, true);
            invincibleDuration = Plugin.ChatConfigSettings.GetFloat("Invincible", "Duration", 15f, true);
            //Poison
            poisonChargeCost = Plugin.ChatConfigSettings.GetInt("Poison", "ChargeCost", 5, true);
            poisonCooldown = Plugin.ChatConfigSettings.GetFloat("Poison", "CoolDown", 30f, true);
            poisonDuration = Plugin.ChatConfigSettings.GetFloat("Poison", "Duration", 15f, true);
            //Instafail
            instaFailChargeCost = Plugin.ChatConfigSettings.GetInt("Instafail", "ChargeCost", 40, true);
            instaFailCooldown = Plugin.ChatConfigSettings.GetFloat("Instafail", "CoolDown", 45f, true);
            instaFailDuration = Plugin.ChatConfigSettings.GetFloat("Instafail", "Duration", 8f, true);
            //NjsRandom
            njsRandomChargeCost = Plugin.ChatConfigSettings.GetInt("NjsRandom", "ChargeCost", 7, true);
            njsRandomCooldown = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "CoolDown", 30f, true);
            njsRandomDuration = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Duration", 10f, true);
            njsRandomMin = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Min", 8f, true);
            njsRandomMax = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Max", 16f, true);
            //OffsetRandom
            offsetrandomChargeCost = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "ChargeCost", 5, true);
            offsetrandomCooldown = Plugin.ChatConfigSettings.GetFloat("OffsetRandom", "CoolDown", 30f, true);
            offsetrandomDuration = Plugin.ChatConfigSettings.GetFloat("OffsetRandom", "Duration", 10f, true);
            offsetrandomMin = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "Min", 0, true);
            offsetrandomMax = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "Max", 4, true);
            //Smaller
            smallerChargeCost = Plugin.ChatConfigSettings.GetInt("Smaller", "ChargeCost", 1, true);
            smallerCoolDown = Plugin.ChatConfigSettings.GetFloat("Smaller", "CoolDown", 15f, true);
            smallerDuration = Plugin.ChatConfigSettings.GetFloat("Smaller", "Duration", 10f, true);
            smallerMultiplier = Plugin.ChatConfigSettings.GetFloat("Smaller", "Multiplier", 0.65f, true);

            //Larger
            largerChargeCost = Plugin.ChatConfigSettings.GetInt("Larger", "ChargeCost", 1, true);
            largerCooldown = Plugin.ChatConfigSettings.GetFloat("Larger", "CoolDown", 15f, true);
            largerDuration = Plugin.ChatConfigSettings.GetFloat("Larger", "Duration", 10f, true);
            largerMultiplier = Plugin.ChatConfigSettings.GetFloat("Larger", "Multiplier", 1.45f, true);
            //Random
            randomChargeCost = Plugin.ChatConfigSettings.GetInt("Random", "ChargeCost", 4, true);
            randomCooldown = Plugin.ChatConfigSettings.GetFloat("Random", "CoolDown", 15f, true);
            randomDuration = Plugin.ChatConfigSettings.GetFloat("Random", "Duration", 10f, true);
            randomMin = Plugin.ChatConfigSettings.GetFloat("Random", "Min", 0.75f, true);
            randomMax = Plugin.ChatConfigSettings.GetFloat("Random", "Max", 1.4f, true);

            //DA
            daChargeCost = Plugin.ChatConfigSettings.GetInt("DA", "ChargeCost", 0, true);
            daCooldown = Plugin.ChatConfigSettings.GetFloat("DA", "CoolDown", 20f, true);
            daDuration = Plugin.ChatConfigSettings.GetFloat("DA", "Duration", 15f, true);

            //Faster
            fasterChargeCost = Plugin.ChatConfigSettings.GetInt("Faster", "ChargeCost", 5, true);
            fasterCooldown = Plugin.ChatConfigSettings.GetFloat("Faster", "CoolDown", 30f, true);
            fasterDuration = Plugin.ChatConfigSettings.GetFloat("Faster", "Duration", 15f, true);
            fasterMultiplier = Plugin.ChatConfigSettings.GetFloat("Faster", "Multiplier", 1.2f, true);
            //Slower
            slowerChargeCost = Plugin.ChatConfigSettings.GetInt("Slower", "ChargeCost", 5, true);
            slowerCooldown = Plugin.ChatConfigSettings.GetFloat("Slower", "CoolDown", 30f, true);
            slowerDuration = Plugin.ChatConfigSettings.GetFloat("Slower", "Duration", 15f, true);
            slowerMultiplier = Plugin.ChatConfigSettings.GetFloat("Slower", "Multiplier", 0.8f, true);

            //NoArrows
            noArrowsChargeCost = Plugin.ChatConfigSettings.GetInt("NoArrows", "ChargeCost", 10, true);
            noArrowsCooldown = Plugin.ChatConfigSettings.GetFloat("NoArrows", "CoolDown", 30f, true);
            noArrowsDuration = Plugin.ChatConfigSettings.GetFloat("NoArrows", "Duration", 10f, true);
            //Funky
            funkyChargeCost = Plugin.ChatConfigSettings.GetInt("Funky", "ChargeCost", 5, true);
            funkyCooldown = Plugin.ChatConfigSettings.GetFloat("Funky", "CoolDown", 20f, true);
            funkyDuration = Plugin.ChatConfigSettings.GetFloat("Funky", "Duration", 10f, true);
            //Rainbow
            rainbowChargeCost = Plugin.ChatConfigSettings.GetInt("Rainbow", "ChargeCost", 3, true);
            rainbowCooldown = Plugin.ChatConfigSettings.GetFloat("Rainbow", "CoolDown", 15f, true);
            rainbowDuration = Plugin.ChatConfigSettings.GetFloat("Rainbow", "Duration", 10f, true);
            //Mirror
            mirrorChargeCost = Plugin.ChatConfigSettings.GetInt("Mirror", "ChargeCost", 3, true);
            mirrorCooldown = Plugin.ChatConfigSettings.GetFloat("Mirror", "CoolDown", 20f, true);
            mirrorDuration = Plugin.ChatConfigSettings.GetFloat("Mirror", "Duration", 20f, true);
            //Reverse
            reverseChargeCost = Plugin.ChatConfigSettings.GetInt("Reverse", "ChargeCost", 10, true);
            reverseCooldown = Plugin.ChatConfigSettings.GetFloat("Reverse", "CoolDown", 60f, true);
            reverseDuration = Plugin.ChatConfigSettings.GetFloat("Reverse", "Duration", 15f, true);
            //Pause
            pauseChargeCost = Plugin.ChatConfigSettings.GetInt("Pause", "ChargeCost", 30, true);
            pauseGlobalCooldown = Plugin.ChatConfigSettings.GetFloat("Pause", "GlobalCoolDown", 60f, true);
            //Bombs
            bombsChargeCost = Plugin.ChatConfigSettings.GetInt("Bombs", "ChargeCost", 7, true);
            bombsCooldown = Plugin.ChatConfigSettings.GetFloat("Bombs", "CoolDown", 45f, true);
            bombsDuration = Plugin.ChatConfigSettings.GetFloat("Bombs", "Duration", 15f, true);
            bombsChance = Plugin.ChatConfigSettings.GetFloat("Bombs", "Chance", 0.1f, true);
            //Tunnel
            tunnelChargeCost = Plugin.ChatConfigSettings.GetInt("Tunnel", "ChargeCost", 5, true);
            tunnelDuration = Plugin.ChatConfigSettings.GetFloat("Tunnel", "Duration", 15f, true);
            tunnelCoolDown = Plugin.ChatConfigSettings.GetFloat("Tunnel", "CoolDown", 30f, true);
            //Left
            leftChargeCost = Plugin.ChatConfigSettings.GetInt("Left", "ChargeCost", 5, true);
            leftCoolDownn = Plugin.ChatConfigSettings.GetFloat("Left", "CoolDown", 15f, true);
            //Right
            rightChargeCost = Plugin.ChatConfigSettings.GetInt("Right", "ChargeCost", 5, true);
            rightCoolDown = Plugin.ChatConfigSettings.GetFloat("Right", "CoolDown", 15f, true);
            //Random Rotation
            randomRotationChargeCost = Plugin.ChatConfigSettings.GetInt("RandomRotation", "ChargeCost", 5, true);
            randomRotationCoolDown = Plugin.ChatConfigSettings.GetFloat("RandomRotation", "CoolDown", 30f, true);
            randomRotationDuration = Plugin.ChatConfigSettings.GetFloat("RandomRotation", "Duration", 5f, true);
            CompileChargeCostString();
        }

        static void CompileChargeCostString()
        {

            chargeCostString = "Current Costs: ";
            chargeCostString += "DA: " + daChargeCost;
            chargeCostString += " | Smaller: " + smallerChargeCost;
            chargeCostString += " | Larger: " + largerChargeCost;
            chargeCostString += " | Random: " + randomChargeCost;
            chargeCostString += " | Instafail: " + instaFailChargeCost;
            chargeCostString += " | Invincible: " + invincibleChargeCost;
            chargeCostString += " | NjsRandom: " + njsRandomChargeCost;
            chargeCostString += " | NoArrows: " + noArrowsChargeCost;
            chargeCostString += " | Funky: " + funkyChargeCost;
            chargeCostString += " | Rainbow: " + rainbowChargeCost;
            chargeCostString += " | Pause: " + pauseChargeCost;
            chargeCostString += " | Bombs: " + bombsChargeCost;
            chargeCostString += " | Poison: " + poisonChargeCost;
            chargeCostString += " | offsetrandom: " + offsetrandomChargeCost;
            chargeCostString += " | Mirror: " + mirrorChargeCost;
            chargeCostString += " | Reverse: " + reverseChargeCost;
            chargeCostString += " | Faster: " + fasterChargeCost;
            chargeCostString += " | Slower: " + slowerChargeCost;
            chargeCostString += " | Tunnel: " + tunnelChargeCost;
            chargeCostString += " | Left: " + leftChargeCost;
            chargeCostString += " | Right: " + rightChargeCost;
            chargeCostString += " | RandomRotation: " + randomRotationChargeCost;
            if (chargeCostString == "Current Costs: DA: 0 | Smaller: 0 | Larger: 0 | Random: 0 | Instafail: 0 | Invincible: 0 | NjsRandom: 0 " +
                "| NoArrows: 0 | Funky: 0 | Rainbow: 0 | Pause: 0 | Bombs: 0 | Poison: 0 | offsetrandom: 0 | Mirror: 0 | Reverse: 0 | Faster: 0 | Slower: 0 | Tunnel: 0 | Left: 0 | Right: 0 | RandomRotation: 0")
                chargeCostString = "Current Costs: None!";
        }

        public static string GetChargeCostString()
        {
            CompileChargeCostString();
            return chargeCostString;
        }

        public static void ChangeConfigValue(string property, string value)
        {
            bool success = true;
            property = property.ToLower();
            Plugin.Log("Config Change Attempt: " + property + " " + value);
            switch (property)
            {
                case "bitspercharge":
                    bitsPerCharge = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Charges", property, bitsPerCharge);
                    break;
                case "chargesforsupercharge":
                    chargesForSuperCharge = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Charges", property, chargesForSuperCharge);
                    break;
                case "maxcharges":
                    maxCharges = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Charges", property, maxCharges);
                    break;
                case "chargesperlevel":
                    chargesPerLevel = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Charges", property, chargesPerLevel);
                    break;
                case "allowsubs":
                    allowSubs = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, allowSubs);
                    break;
                case "alloweveryone":
                    allowSubs = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, allowEveryone);
                    break;
                case "commandspermessage":
                    commandsPerMessage = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Basic Setup", property, commandsPerMessage);
                    break;
                case "globalcommandcooldown":
                    globalCommandCooldown = float.Parse(value);
                    Plugin.ChatConfigSettings.SetFloat("Basic Setup", property, globalCommandCooldown);
                    break;
                case "timeforcharges":
                    timeForCharges = float.Parse(value);
                    Plugin.ChatConfigSettings.SetFloat("Charges", property, timeForCharges);
                    break;
                case "chargesovertime":
                    chargesOverTime = int.Parse(value);
                    Plugin.ChatConfigSettings.SetInt("Charges", property, chargesOverTime);
                    break;
                case "showcooldownonmessage":
                    showCooldownOnMessage = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, showCooldownOnMessage);
                    break;
                case "uiontop":
                    uiOnTop = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, uiOnTop);
                    break;
                case "resetchargesperlevel":
                    resetChargesEachLevel = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Charges", property, resetChargesEachLevel);
                    break;
                case "allowmodcommands":
                    allowModCommands = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, allowModCommands);
                    break;
                case "chatintegration360":
                    GMPUI.chatIntegration360 = Convert.ToBoolean(value);
                    Plugin.ChatConfigSettings.SetBool("Basic Setup", property, GMPUI.chatIntegration360);
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {

                Plugin.TryAsyncMessage("Changed Value");
            }
        }
        public static void ChangeConfigValue(string command, string property, string value)
        {
            Plugin.Log("Config Change Attempt: " + command + " " + property + " " + value);
            bool success = true;
            switch (command)
            {

                case "DA":
                    switch (property)
                    {
                        case "ChargeCost":
                            daChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, daChargeCost);
                            break;
                        case "CoolDown":
                            daCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, daCooldown);
                            break;
                        case "Duration":
                            daDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, daDuration);
                            break;
                    }
                    break;
                case "Smaller":
                    switch (property)
                    {
                        case "ChargeCost":
                            smallerChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, smallerChargeCost);
                            break;
                        case "CoolDown":
                            smallerCoolDown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, smallerCoolDown);
                            break;
                        case "Duration":
                            smallerDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, smallerDuration);
                            break;
                        case "Multiplier":
                            smallerMultiplier = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, smallerMultiplier);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Larger":
                    switch (property)
                    {
                        case "ChargeCost":
                            largerChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, largerChargeCost);
                            break;
                        case "CoolDown":
                            largerCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, largerCooldown);
                            break;
                        case "Duration":
                            largerDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, largerDuration);
                            break;
                        case "Multiplier":
                            largerMultiplier = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, largerMultiplier);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Random":
                    switch (property)
                    {
                        case "ChargeCost":
                            randomChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, randomChargeCost);
                            break;
                        case "CoolDown":
                            randomCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomCooldown);
                            break;
                        case "Duration":
                            randomDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomDuration);
                            break;
                        case "Min":
                            randomMin = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomMin);
                            break;
                        case "Max":
                            randomMax = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomMax);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Instafail":
                    switch (property)
                    {
                        case "ChargeCost":
                            instaFailChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, instaFailChargeCost);
                            break;
                        case "CoolDown":
                            instaFailCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, instaFailCooldown);
                            break;
                        case "Duration":
                            instaFailDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, instaFailDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Invincible":
                    switch (property)
                    {
                        case "ChargeCost":
                            invincibleChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, invincibleChargeCost);
                            break;
                        case "CoolDown":
                            invincibleCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, invincibleCooldown);
                            break;
                        case "Duration":
                            invincibleDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, invincibleDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "NjsRandom":
                    switch (property)
                    {
                        case "ChargeCost":
                            njsRandomChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, njsRandomChargeCost);
                            break;
                        case "CoolDown":
                            njsRandomCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, njsRandomCooldown);
                            break;
                        case "Duration":
                            njsRandomDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, njsRandomDuration);
                            break;
                        case "Min":
                            njsRandomMin = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, njsRandomMin);
                            break;
                        case "Max":
                            njsRandomMax = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, njsRandomMax);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "NoArrows":
                    switch (property)
                    {
                        case "ChargeCost":
                            noArrowsChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, noArrowsChargeCost);
                            break;
                        case "CoolDown":
                            noArrowsCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, noArrowsCooldown);
                            break;
                        case "Duration":
                            noArrowsDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, noArrowsDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Funky":
                    switch (property)
                    {
                        case "ChargeCost":
                            funkyChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, funkyChargeCost);
                            break;
                        case "CoolDown":
                            funkyCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, funkyCooldown);
                            break;
                        case "Duration":
                            funkyDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, funkyDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Rainbow":
                    switch (property)
                    {
                        case "ChargeCost":
                            rainbowChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, rainbowChargeCost);
                            break;
                        case "CoolDown":
                            rainbowCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, rainbowCooldown);
                            break;
                        case "Duration":
                            rainbowDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, rainbowDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Pause":
                    switch (property)
                    {
                        case "ChargeCost":
                            pauseChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, pauseChargeCost);
                            break;
                        case "CoolDown":
                            pauseGlobalCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, "GlobalCoolDown", pauseGlobalCooldown);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Bombs":
                    switch (property)
                    {
                        case "ChargeCost":
                            bombsChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, bombsChargeCost);
                            break;
                        case "CoolDown":
                            bombsCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, bombsCooldown);
                            break;
                        case "Duration":
                            bombsDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, bombsDuration);
                            break;
                        case "Chance":
                            bombsChance = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, bombsChance);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Faster":
                    switch (property)
                    {
                        case "ChargeCost":
                            fasterChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, fasterChargeCost);
                            break;
                        case "CoolDown":
                            fasterCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, fasterCooldown);
                            break;
                        case "Duration":
                            fasterDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, fasterDuration);
                            break;
                        case "Multiplier":
                            fasterMultiplier = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, fasterMultiplier);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Slower":
                    switch (property)
                    {
                        case "ChargeCost":
                            slowerChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, slowerChargeCost);
                            break;
                        case "CoolDown":
                            slowerCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, slowerCooldown);
                            break;
                        case "Duration":
                            slowerDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, slowerDuration);
                            break;
                        case "Multiplier":
                            slowerMultiplier = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, slowerMultiplier);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Poison":
                    switch (property)
                    {
                        case "ChargeCost":
                            poisonChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, poisonChargeCost);
                            break;
                        case "CoolDown":
                            poisonCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, poisonCooldown);
                            break;
                        case "Duration":
                            poisonDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, poisonDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Mirror":
                    switch (property)
                    {
                        case "ChargeCost":
                            mirrorChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, mirrorChargeCost);
                            break;
                        case "CoolDown":
                            mirrorCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, mirrorCooldown);
                            break;
                        case "Duration":
                            mirrorDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, mirrorDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Reverse":
                    switch (property)
                    {
                        case "ChargeCost":
                            reverseChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, reverseChargeCost);
                            break;
                        case "CoolDown":
                            reverseCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, reverseCooldown);
                            break;
                        case "Duration":
                            reverseDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, reverseDuration);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "OffsetRandom":
                    switch (property)
                    {
                        case "ChargeCost":
                            offsetrandomChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, offsetrandomChargeCost);
                            break;
                        case "CoolDown":
                            offsetrandomCooldown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, offsetrandomCooldown);
                            break;
                        case "Duration":
                            offsetrandomDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, offsetrandomDuration);
                            break;
                        case "Min":
                            offsetrandomMin = int.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, offsetrandomMin);
                            break;
                        case "Max":
                            offsetrandomMax = int.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, offsetrandomMax);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Tunnel":
                    switch (property)
                    {
                        case "ChargeCost":
                            tunnelChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, tunnelChargeCost);
                            break;
                        case "CoolDown":
                            tunnelCoolDown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, tunnelCoolDown);
                            break;
                        case "Duration":
                            tunnelDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, tunnelDuration);
                            break;
                            success = false;
                            break;

                    }
                    break;
                case "Left":
                    switch (property)
                    {
                        case "ChargeCost":
                            leftChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, leftChargeCost);
                            break;
                        case "CoolDown":
                            leftCoolDownn = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, leftCoolDownn);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "Right":
                    switch (property)
                    {
                        case "ChargeCost":
                            rightChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, rightChargeCost);
                            break;
                        case "CoolDown":
                            rightCoolDown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, rightCoolDown);
                            break;
                        default:
                            success = false;
                            break;

                    }
                    break;
                case "RandomRotation":
                    switch (property)
                    {
                        case "ChargeCost":
                            randomRotationChargeCost = int.Parse(value);
                            Plugin.ChatConfigSettings.SetInt(command, property, randomRotationChargeCost);
                            break;
                        case "CoolDown":
                            randomRotationCoolDown = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomRotationCoolDown);
                            break;
                        case "Duration":
                            randomRotationDuration = float.Parse(value);
                            Plugin.ChatConfigSettings.SetFloat(command, property, randomRotationDuration);
                            break;
                    }
                    break;
                default:
                    break;


            }

            //    Plugin.Log(allowEveryone.ToString());
            if (success)
            {
                Plugin.TryAsyncMessage("Changed Value");
            }

        }





    }
}
