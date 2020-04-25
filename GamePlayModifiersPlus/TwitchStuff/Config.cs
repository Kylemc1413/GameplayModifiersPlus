namespace GamePlayModifiersPlus
{
    using System;
    using System.IO;
    using BS_Utils.Utilities;
    using GamePlayModifiersPlus.TwitchStuff;
    internal static class Config
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

        public static bool EndlessAllow360 = false;
        public static BeatmapDifficulty EndlessPrefDifficulty = BeatmapDifficulty.ExpertPlus;
        public static BeatmapDifficulty EndlessMinDifficulty = BeatmapDifficulty.Hard;
        public static BeatmapDifficulty EndlessMaxDifficulty = BeatmapDifficulty.ExpertPlus;
        public static bool EndlessUseCurrentLevelCollection = true;
        public static bool EndlessContinueOnFail = true;


        public static void Load()
        {
            //Endless Config
            EndlessAllow360 = Plugin.ConfigSettings.GetBool("Endless", "Allow360", false, true);
            EndlessContinueOnFail = Plugin.ConfigSettings.GetBool("Endless", "ContinueOnFail", true, true);
            EndlessUseCurrentLevelCollection = Plugin.ConfigSettings.GetBool("Endless", "UseCurrentLevelCollection", true, true);
            if (Enum.TryParse<BeatmapDifficulty>(Plugin.ConfigSettings.GetString("Endless", "PreferredDifficulty", "ExpertPlus", true), out var PrefDiff))
            {
                EndlessPrefDifficulty = PrefDiff;
            }
            else
            {
                Plugin.ConfigSettings.SetString("Endless", "PreferredDifficulty", "ExpertPlus");
            }

            if (Enum.TryParse<BeatmapDifficulty>(Plugin.ConfigSettings.GetString("Endless", "MinimumDifficulty", "Hard", true), out var minDiff))
            {
                EndlessMinDifficulty = minDiff;
            }
            else
            {
                Plugin.ConfigSettings.SetString("Endless", "MinimumDifficulty", "Expert");
            }
            if (Enum.TryParse<BeatmapDifficulty>(Plugin.ConfigSettings.GetString("Endless", "MaximumDifficulty", "ExpertPlus", true), out var maxDiff))
            {
                EndlessMaxDifficulty = maxDiff;
            }
            else
            {
                Plugin.ConfigSettings.SetString("Endless", "MinimumDifficulty", "Expert");
            }

            //Basic Setup
            uiOnTop = Plugin.ConfigSettings.GetBool("Basic Setup", "uiOnTop", true, true);
            allowEveryone = Plugin.ConfigSettings.GetBool("Basic Setup", "allowEveryone", true, true);
            allowSubs = Plugin.ConfigSettings.GetBool("Basic Setup", "allowSubs", true, true);
            allowModCommands = Plugin.ConfigSettings.GetBool("Basic Setup", "allowModCommands", true, true);
            commandsPerMessage = Plugin.ConfigSettings.GetInt("Basic Setup", "commandsPerMessage", 2, true);
            globalCommandCooldown = Plugin.ConfigSettings.GetFloat("Basic Setup", "globalCommandCooldown", 10f, true);
            showCooldownOnMessage = Plugin.ConfigSettings.GetBool("Basic Setup", "showCooldownOnMessage", false, true);
            GMPUI.chatIntegration360 = Plugin.ConfigSettings.GetBool("Basic Setup", "chatintegration360", false, true);

            //Charges
            resetChargesEachLevel = Plugin.ConfigSettings.GetBool("Charges", "resetChargesEachLevel", false, true);
            maxCharges = Plugin.ConfigSettings.GetInt("Charges", "maxCharges", 50, true);
            chargesForSuperCharge = Plugin.ConfigSettings.GetInt("Charges", "chargesForSuperCharge", 10, true);
            chargesPerLevel = Plugin.ConfigSettings.GetInt("Charges", "chargesPerLevel", 10, true);
            chargesOverTime = Plugin.ConfigSettings.GetInt("Charges", "chargesOverTime", 1, true);
            timeForCharges = Plugin.ConfigSettings.GetFloat("Charges", "timeForCharges", 15f, true);
            bitsPerCharge = Plugin.ConfigSettings.GetInt("Charges", "bitsPerCharge", 10, true);

            // COMMANDS

            //Invincible
            invincibleChargeCost = Plugin.ConfigSettings.GetInt("Invincible", "ChargeCost", 0, true);
            invincibleDuration = Plugin.ConfigSettings.GetFloat("Invincible", "Duration", 15f, true);
            invincibleCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Invincible", "CoolDown", 15f, true), invincibleDuration);

            //Poison
            poisonChargeCost = Plugin.ConfigSettings.GetInt("Poison", "ChargeCost", 5, true);
            poisonDuration = Plugin.ConfigSettings.GetFloat("Poison", "Duration", 15f, true);
            poisonCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Poison", "CoolDown", 30f, true), poisonDuration);

            //Instafail
            instaFailChargeCost = Plugin.ConfigSettings.GetInt("Instafail", "ChargeCost", 40, true);
            instaFailDuration = Plugin.ConfigSettings.GetFloat("Instafail", "Duration", 8f, true);
            instaFailCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Instafail", "CoolDown", 45f, true), instaFailDuration);

            //NjsRandom
            njsRandomChargeCost = Plugin.ConfigSettings.GetInt("NjsRandom", "ChargeCost", 7, true);
            njsRandomDuration = Plugin.ConfigSettings.GetFloat("NjsRandom", "Duration", 10f, true);
            njsRandomCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("NjsRandom", "CoolDown", 30f, true), njsRandomDuration);
            njsRandomMin = Plugin.ConfigSettings.GetFloat("NjsRandom", "Min", 8f, true);
            njsRandomMax = Plugin.ConfigSettings.GetFloat("NjsRandom", "Max", 16f, true);

            //OffsetRandom
            offsetrandomChargeCost = Plugin.ConfigSettings.GetInt("OffsetRandom", "ChargeCost", 5, true);
            offsetrandomDuration = Plugin.ConfigSettings.GetFloat("OffsetRandom", "Duration", 10f, true);
            offsetrandomCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("OffsetRandom", "CoolDown", 30f, true), offsetrandomDuration);
            offsetrandomMin = Plugin.ConfigSettings.GetInt("OffsetRandom", "Min", 0, true);
            offsetrandomMax = Plugin.ConfigSettings.GetInt("OffsetRandom", "Max", 4, true);

            //Smaller
            smallerChargeCost = Plugin.ConfigSettings.GetInt("Smaller", "ChargeCost", 1, true);
            smallerDuration = Plugin.ConfigSettings.GetFloat("Smaller", "Duration", 10f, true);
            smallerCoolDown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Smaller", "CoolDown", 15f, true), smallerDuration);
            smallerMultiplier = Plugin.ConfigSettings.GetFloat("Smaller", "Multiplier", 0.65f, true);

            //Larger
            largerChargeCost = Plugin.ConfigSettings.GetInt("Larger", "ChargeCost", 1, true);
            largerDuration = Plugin.ConfigSettings.GetFloat("Larger", "Duration", 10f, true);
            largerCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Larger", "CoolDown", 15f, true), largerDuration);
            largerMultiplier = Plugin.ConfigSettings.GetFloat("Larger", "Multiplier", 1.45f, true);

            //Random
            randomChargeCost = Plugin.ConfigSettings.GetInt("Random", "ChargeCost", 4, true);
            randomDuration = Plugin.ConfigSettings.GetFloat("Random", "Duration", 10f, true);
            randomCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Random", "CoolDown", 15f, true), randomDuration);
            randomMin = Plugin.ConfigSettings.GetFloat("Random", "Min", 0.75f, true);
            randomMax = Plugin.ConfigSettings.GetFloat("Random", "Max", 1.4f, true);

            //DA
            daChargeCost = Plugin.ConfigSettings.GetInt("DA", "ChargeCost", 0, true);
            daDuration = Plugin.ConfigSettings.GetFloat("DA", "Duration", 15f, true);
            daCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("DA", "CoolDown", 20f, true), daDuration);

            //Faster
            fasterChargeCost = Plugin.ConfigSettings.GetInt("Faster", "ChargeCost", 5, true);
            fasterDuration = Plugin.ConfigSettings.GetFloat("Faster", "Duration", 15f, true);
            fasterCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Faster", "CoolDown", 30f, true), fasterDuration);
            fasterMultiplier = Plugin.ConfigSettings.GetFloat("Faster", "Multiplier", 1.2f, true);

            //Slower
            slowerChargeCost = Plugin.ConfigSettings.GetInt("Slower", "ChargeCost", 5, true);
            slowerDuration = Plugin.ConfigSettings.GetFloat("Slower", "Duration", 15f, true);
            slowerCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Slower", "CoolDown", 30f, true), slowerDuration);
            slowerMultiplier = Plugin.ConfigSettings.GetFloat("Slower", "Multiplier", 0.8f, true);

            //NoArrows
            noArrowsChargeCost = Plugin.ConfigSettings.GetInt("NoArrows", "ChargeCost", 10, true);
            noArrowsDuration = Plugin.ConfigSettings.GetFloat("NoArrows", "Duration", 10f, true);
            noArrowsCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("NoArrows", "CoolDown", 30f, true), noArrowsDuration);

            //Funky
            funkyChargeCost = Plugin.ConfigSettings.GetInt("Funky", "ChargeCost", 5, true);
            funkyDuration = Plugin.ConfigSettings.GetFloat("Funky", "Duration", 10f, true);
            funkyCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Funky", "CoolDown", 20f, true), funkyDuration);

            //Rainbow
            rainbowChargeCost = Plugin.ConfigSettings.GetInt("Rainbow", "ChargeCost", 3, true);
            rainbowDuration = Plugin.ConfigSettings.GetFloat("Rainbow", "Duration", 10f, true);
            rainbowCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Rainbow", "CoolDown", 15f, true), rainbowDuration);

            //Mirror
            mirrorChargeCost = Plugin.ConfigSettings.GetInt("Mirror", "ChargeCost", 3, true);
            mirrorDuration = Plugin.ConfigSettings.GetFloat("Mirror", "Duration", 20f, true);
            mirrorCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Mirror", "CoolDown", 20f, true), mirrorDuration);

            //Reverse
            reverseChargeCost = Plugin.ConfigSettings.GetInt("Reverse", "ChargeCost", 10, true);
            reverseDuration = Plugin.ConfigSettings.GetFloat("Reverse", "Duration", 15f, true);
            reverseCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Reverse", "CoolDown", 60f, true), reverseDuration);

            //Pause
            pauseChargeCost = Plugin.ConfigSettings.GetInt("Pause", "ChargeCost", 30, true);
            pauseGlobalCooldown = Plugin.ConfigSettings.GetFloat("Pause", "GlobalCoolDown", 60f, true);

            //Bombs
            bombsChargeCost = Plugin.ConfigSettings.GetInt("Bombs", "ChargeCost", 7, true);
            bombsDuration = Plugin.ConfigSettings.GetFloat("Bombs", "Duration", 15f, true);
            bombsCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Bombs", "CoolDown", 45f, true), bombsDuration);
            bombsChance = Plugin.ConfigSettings.GetFloat("Bombs", "Chance", 0.1f, true);

            //Tunnel
            tunnelChargeCost = Plugin.ConfigSettings.GetInt("Tunnel", "ChargeCost", 5, true);
            tunnelDuration = Plugin.ConfigSettings.GetFloat("Tunnel", "Duration", 15f, true);
            tunnelCoolDown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("Tunnel", "CoolDown", 30f, true), tunnelDuration);

            //Left
            leftChargeCost = Plugin.ConfigSettings.GetInt("Left", "ChargeCost", 5, true);
            leftCoolDownn = Plugin.ConfigSettings.GetFloat("Left", "CoolDown", 15f, true);

            //Right
            rightChargeCost = Plugin.ConfigSettings.GetInt("Right", "ChargeCost", 5, true);
            rightCoolDown = Plugin.ConfigSettings.GetFloat("Right", "CoolDown", 15f, true);

            //Random Rotation
            randomRotationChargeCost = Plugin.ConfigSettings.GetInt("RandomRotation", "ChargeCost", 5, true);
            randomRotationDuration = Plugin.ConfigSettings.GetFloat("RandomRotation", "Duration", 5f, true);
            randomRotationCoolDown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("RandomRotation", "CoolDown", 30f, true), randomRotationDuration);

            //RCTTS
            rcttsChargeCost = Plugin.ConfigSettings.GetInt("RCTTS", "ChargeCost", 15, true);
            rcttsDuration = Plugin.ConfigSettings.GetFloat("RCTTS", "Duration", 30f, true);
            rcttsCooldown = UnityEngine.Mathf.Max(Plugin.ConfigSettings.GetFloat("RCTTS", "CoolDown", 45f, true), rcttsDuration);
            rcttsRandomizeStart = Plugin.ConfigSettings.GetBool("RCTTS", "RandomizeStart", false, true);

            // Command Cooldowns
            chargesCommandCoolDown = Plugin.ConfigSettings.GetFloat("Command Cooldowns", "Charges", 0f, true);

            CompileChargeCostString();
        }

        static void CompileChargeCostString()
        {
            var iniData = Plugin.ConfigSettings.GetField<object>("_instance").GetField<IniParser.Model.IniData>("data");
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
            bool success = false;
            string propertyLower = property.ToLower();
            Plugin.Log("Config Change Attempt: " + property + " " + value);

            object inifile = Plugin.ConfigSettings.GetField<object>("_instance");
            IniParser.Model.IniData data = inifile.GetField<IniParser.Model.IniData>("data");

            foreach (var section in data.Sections)
            {
                if (success) break;
                foreach (var key in section.Keys)
                {
                    if (key.KeyName.ToLower() == propertyLower && !data.Sections[section.SectionName].ContainsKey("ChargeCost"))
                    {
                        inifile.InvokeMethod("IniWriteValue", section.SectionName, property, value);
                        ChatMessageHandler.TryAsyncMessage("Changed Value");
                        Config.Load();
                        success = true;
                        break;
                    }
                }
            }



        }

        public static void ChangeConfigValue(string command, string property, string value)
        {
            Plugin.Log("Config Change Attempt: " + command + " " + property + " " + value);
            object inifile = Plugin.ConfigSettings.GetField<object>("_instance");
            IniParser.Model.IniData data = inifile.GetField<IniParser.Model.IniData>("data");
            if (data.Sections.ContainsSection(command))
            {
                if (data.Sections[command].ContainsKey(property))
                {
                    inifile.InvokeMethod("IniWriteValue", command, property, value);
                    ChatMessageHandler.TryAsyncMessage("Changed Value");
                    Config.Load();
                }

            }

        }





    }
}
