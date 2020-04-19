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

        public static int rcttsChargeCost = 15;
        public static float rcttsDuration = 30f;
        public static float rcttsCooldown = 45f;
        public static bool rcttsRandomizeStart = false;
        public static float chargesCommandCoolDown = 0f;

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
            invincibleDuration = Plugin.ChatConfigSettings.GetFloat("Invincible", "Duration", 15f, true);
            invincibleCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Invincible", "CoolDown", 15f, true), invincibleDuration);

            //Poison
            poisonChargeCost = Plugin.ChatConfigSettings.GetInt("Poison", "ChargeCost", 5, true);
            poisonDuration = Plugin.ChatConfigSettings.GetFloat("Poison", "Duration", 15f, true);
            poisonCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Poison", "CoolDown", 30f, true), poisonDuration);

            //Instafail
            instaFailChargeCost = Plugin.ChatConfigSettings.GetInt("Instafail", "ChargeCost", 40, true);
            instaFailDuration = Plugin.ChatConfigSettings.GetFloat("Instafail", "Duration", 8f, true);
            instaFailCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Instafail", "CoolDown", 45f, true), instaFailDuration);

            //NjsRandom
            njsRandomChargeCost = Plugin.ChatConfigSettings.GetInt("NjsRandom", "ChargeCost", 7, true);
            njsRandomDuration = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Duration", 10f, true);
            njsRandomCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("NjsRandom", "CoolDown", 30f, true), njsRandomDuration);

            njsRandomMin = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Min", 8f, true);
            njsRandomMax = Plugin.ChatConfigSettings.GetFloat("NjsRandom", "Max", 16f, true);
            //OffsetRandom
            offsetrandomChargeCost = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "ChargeCost", 5, true);
            offsetrandomDuration = Plugin.ChatConfigSettings.GetFloat("OffsetRandom", "Duration", 10f, true);
            offsetrandomCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("OffsetRandom", "CoolDown", 30f, true), offsetrandomDuration);

            offsetrandomMin = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "Min", 0, true);
            offsetrandomMax = Plugin.ChatConfigSettings.GetInt("OffsetRandom", "Max", 4, true);
            //Smaller
            smallerChargeCost = Plugin.ChatConfigSettings.GetInt("Smaller", "ChargeCost", 1, true);
            smallerDuration = Plugin.ChatConfigSettings.GetFloat("Smaller", "Duration", 10f, true);
            smallerCoolDown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Smaller", "CoolDown", 15f, true), smallerDuration);

            smallerMultiplier = Plugin.ChatConfigSettings.GetFloat("Smaller", "Multiplier", 0.65f, true);

            //Larger
            largerChargeCost = Plugin.ChatConfigSettings.GetInt("Larger", "ChargeCost", 1, true);
            largerDuration = Plugin.ChatConfigSettings.GetFloat("Larger", "Duration", 10f, true);
            largerCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Larger", "CoolDown", 15f, true), largerDuration);

            largerMultiplier = Plugin.ChatConfigSettings.GetFloat("Larger", "Multiplier", 1.45f, true);
            //Random
            randomChargeCost = Plugin.ChatConfigSettings.GetInt("Random", "ChargeCost", 4, true);
            randomDuration = Plugin.ChatConfigSettings.GetFloat("Random", "Duration", 10f, true);
            randomCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Random", "CoolDown", 15f, true), randomDuration);

            randomMin = Plugin.ChatConfigSettings.GetFloat("Random", "Min", 0.75f, true);
            randomMax = Plugin.ChatConfigSettings.GetFloat("Random", "Max", 1.4f, true);

            //DA
            daChargeCost = Plugin.ChatConfigSettings.GetInt("DA", "ChargeCost", 0, true);
            daDuration = Plugin.ChatConfigSettings.GetFloat("DA", "Duration", 15f, true);
            daCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("DA", "CoolDown", 20f, true), daDuration);


            //Faster
            fasterChargeCost = Plugin.ChatConfigSettings.GetInt("Faster", "ChargeCost", 5, true);
            fasterDuration = Plugin.ChatConfigSettings.GetFloat("Faster", "Duration", 15f, true);
            fasterCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Faster", "CoolDown", 30f, true), fasterDuration);

            fasterMultiplier = Plugin.ChatConfigSettings.GetFloat("Faster", "Multiplier", 1.2f, true);
            //Slower
            slowerChargeCost = Plugin.ChatConfigSettings.GetInt("Slower", "ChargeCost", 5, true);
            slowerDuration = Plugin.ChatConfigSettings.GetFloat("Slower", "Duration", 15f, true);
            slowerCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Slower", "CoolDown", 30f, true), slowerDuration);

            slowerMultiplier = Plugin.ChatConfigSettings.GetFloat("Slower", "Multiplier", 0.8f, true);

            //NoArrows
            noArrowsChargeCost = Plugin.ChatConfigSettings.GetInt("NoArrows", "ChargeCost", 10, true);
            noArrowsDuration = Plugin.ChatConfigSettings.GetFloat("NoArrows", "Duration", 10f, true);
            noArrowsCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("NoArrows", "CoolDown", 30f, true), noArrowsDuration);

            //Funky
            funkyChargeCost = Plugin.ChatConfigSettings.GetInt("Funky", "ChargeCost", 5, true);
            funkyDuration = Plugin.ChatConfigSettings.GetFloat("Funky", "Duration", 10f, true);
            funkyCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Funky", "CoolDown", 20f, true), funkyDuration);

            //Rainbow
            rainbowChargeCost = Plugin.ChatConfigSettings.GetInt("Rainbow", "ChargeCost", 3, true);
            rainbowDuration = Plugin.ChatConfigSettings.GetFloat("Rainbow", "Duration", 10f, true);
            rainbowCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Rainbow", "CoolDown", 15f, true), rainbowDuration);

            //Mirror
            mirrorChargeCost = Plugin.ChatConfigSettings.GetInt("Mirror", "ChargeCost", 3, true);
            mirrorDuration = Plugin.ChatConfigSettings.GetFloat("Mirror", "Duration", 20f, true);
            mirrorCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Mirror", "CoolDown", 20f, true), mirrorDuration);

            //Reverse
            reverseChargeCost = Plugin.ChatConfigSettings.GetInt("Reverse", "ChargeCost", 10, true);
            reverseDuration = Plugin.ChatConfigSettings.GetFloat("Reverse", "Duration", 15f, true);
            reverseCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Reverse", "CoolDown", 60f, true), reverseDuration);

            //Pause
            pauseChargeCost = Plugin.ChatConfigSettings.GetInt("Pause", "ChargeCost", 30, true);
            pauseGlobalCooldown = Plugin.ChatConfigSettings.GetFloat("Pause", "GlobalCoolDown", 60f, true);
            //Bombs
            bombsChargeCost = Plugin.ChatConfigSettings.GetInt("Bombs", "ChargeCost", 7, true);

            bombsDuration = Plugin.ChatConfigSettings.GetFloat("Bombs", "Duration", 15f, true);
            bombsCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Bombs", "CoolDown", 45f, true), bombsDuration);
            bombsChance = Plugin.ChatConfigSettings.GetFloat("Bombs", "Chance", 0.1f, true);
            //Tunnel
            tunnelChargeCost = Plugin.ChatConfigSettings.GetInt("Tunnel", "ChargeCost", 5, true);
            tunnelDuration = Plugin.ChatConfigSettings.GetFloat("Tunnel", "Duration", 15f, true);
            tunnelCoolDown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("Tunnel", "CoolDown", 30f, true), tunnelDuration);
            //Left
            leftChargeCost = Plugin.ChatConfigSettings.GetInt("Left", "ChargeCost", 5, true);
            leftCoolDownn = Plugin.ChatConfigSettings.GetFloat("Left", "CoolDown", 15f, true);
            //Right
            rightChargeCost = Plugin.ChatConfigSettings.GetInt("Right", "ChargeCost", 5, true);
            rightCoolDown = Plugin.ChatConfigSettings.GetFloat("Right", "CoolDown", 15f, true);
            //Random Rotation
            randomRotationChargeCost = Plugin.ChatConfigSettings.GetInt("RandomRotation", "ChargeCost", 5, true);
            randomRotationDuration = Plugin.ChatConfigSettings.GetFloat("RandomRotation", "Duration", 5f, true);
            randomRotationCoolDown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("RandomRotation", "CoolDown", 30f, true), randomRotationDuration);

            //RCTTS
            rcttsChargeCost = Plugin.ChatConfigSettings.GetInt("RCTTS", "ChargeCost", 15, true);
            rcttsDuration = Plugin.ChatConfigSettings.GetFloat("RCTTS", "Duration", 30f, true);
            rcttsCooldown = UnityEngine.Mathf.Max(Plugin.ChatConfigSettings.GetFloat("RCTTS", "CoolDown", 45f, true), rcttsDuration);
            rcttsRandomizeStart = Plugin.ChatConfigSettings.GetBool("RCTTS", "RandomizeStart", false, true);
            // Command Cooldowns
            chargesCommandCoolDown = Plugin.ChatConfigSettings.GetFloat("Command Cooldowns", "Charges", 0f, true);

            CompileChargeCostString();
        }

        static void CompileChargeCostString()
        {
            var iniData = Plugin.ChatConfigSettings.GetField<object>("_instance").GetField<IniParser.Model.IniData>("data");
            System.Text.StringBuilder result = new System.Text.StringBuilder("Current Costs | ");
            int totalCosts = 0;
            foreach (var section in iniData.Sections)
            {
                string name = section.SectionName;
                if (iniData.Sections[name].ContainsKey("ChargeCost"))
                {
                    int cost = 0;
                    int.TryParse(iniData[name]["ChargeCost"], out cost);
                    totalCosts += cost;
                    result.Append($"{name}: {cost} | ");
                }
            }
            if (totalCosts == 0)
            {
                result.Clear();
                result.Append("Current Costs: None");
            }

            chargeCostString = result.ToString();
            return;

        }

        public static string GetChargeCostString()
        {
            // CompileChargeCostString();
            return chargeCostString;
        }

        public static void ChangeConfigValue(string property, string value)
        {
            bool success = true;
            string propertyLower = property.ToLower();
            Plugin.Log("Config Change Attempt: " + property + " " + value);

            object inifile = Plugin.ChatConfigSettings.GetField<object>("_instance");
            IniParser.Model.IniData data = inifile.GetField<IniParser.Model.IniData>("data");
            foreach(var section in data.Sections)
            {
                foreach (var key in section.Keys)
                {
                    if (key.KeyName.ToLower() == propertyLower)
                    {
                        inifile.InvokeMethod("IniWriteValue", section.SectionName, property, value);
                        break;
                    }
                }
            }

                Plugin.TryAsyncMessage("Changed Value");
                ChatConfig.Load();
            
        }

        public static void ChangeConfigValue(string command, string property, string value)
        {
            Plugin.Log("Config Change Attempt: " + command + " " + property + " " + value);
            bool success = true;
            object inifile = Plugin.ChatConfigSettings.GetField<object>("_instance");
            IniParser.Model.IniData data = inifile.GetField<IniParser.Model.IniData>("data");
            if (data.Sections.ContainsSection(command))
            {
                if (data.Sections[command].ContainsKey(property))
                {
                    inifile.InvokeMethod("IniWriteValue", command, property, value);
                    Plugin.TryAsyncMessage("Changed Value");
                    ChatConfig.Load();            
                }
                
            }

        }





    }
}
