namespace GamePlayModifiersPlus
{
    using System;
    using System.IO;

    public class ChatConfig
    {
        public string FilePath { get; }
        public int commandsPerMessage = 2;
        public float globalCommandCooldown = 10f;
        public bool allowSubs = true;
        public bool allowEveryone = true;
        public int chargesForSuperCharge = 10;
        public int chargesPerLevel = 10;
        public int maxCharges = 30;
        public int bitsPerCharge = 10;

        public int chargesOverTime = 1;
        public float timeForCharges = 20f;

        public int daChargeCost = 0;
        public int smallerNoteChargeCost = 1;
        public int largerNotesChargeCost = 1;
        public int randomNotesChargeCost = 4;
        public int instaFailChargeCost = 20;
        public int invincibleChargeCost = 0;
        public int njsRandomChargeCost = 5;
        public int noArrowsChargeCost = 0;
        public int funkyChargeCost = 5;
        public int rainbowChargeCost = 3;
        //     public int nMirrorChargeCost = 0;

        public float daDuration = 15f;
        public float smallerNoteDuration = 10f;
        public float largerNotesDuration = 10f;
        public float randomNotesDuration = 10f;
        public float instaFailDuration = 10f;
        public float invincibleDuration = 15f;
        public float njsRandomDuration = 10f;
        public float noArrowsDuration = 10f;
        public float funkyDuration = 10f;
        public float rainbowDuration = 10f;
        //   public float nMirrorDuration = 15f;

        public float daCooldown = 20f;
        public float smallerNotesCooldown = 15f;
        public float largerNotesCooldown = 15f;
        public float randomNotesCooldown = 15f;
        public float instaFailCooldown = 45f;
        public float invincibleCooldown = 20f;
        public float njsRandomCooldown = 30f;
        public float noArrowsCooldown = 20f;
        public float funkyCooldown = 30f;
        public float rainbowCooldown = 20f;
        //   public float nMirrorCooldown= 20f;
        public float njsRandomMin = 8f;
        public float njsRandomMax = 16f;
        public float randomMin = 0.6f;
        public float randomMax = 1.5f;
        private readonly FileSystemWatcher _configWatcher;
        public event Action<ChatConfig> ConfigChangedEvent;
        private bool _saving;
        private string chargeCostString;


        public ChatConfig(String filePath)
        {
            FilePath = filePath;

            if (File.Exists(FilePath))
            {
                Load();
            }
            else
            {
                Save();
            }



            _configWatcher = new FileSystemWatcher($"{Environment.CurrentDirectory}\\UserData")
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "GameplayModifiersPlusChatSettings.ini",
                EnableRaisingEvents = true
            };
            _configWatcher.Changed += ConfigWatcherOnChanged;
        }




        ~ChatConfig()
        {
            _configWatcher.Changed -= ConfigWatcherOnChanged;
        }
        public void Save()
        {
            _saving = true;
            ConfigSerializer.SaveConfig(this, FilePath);
        }

        public void Load()
        {
            ConfigSerializer.LoadConfig(this, FilePath);
            CompileChargeCostString();
        }

        private void ConfigWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if (_saving)
            {
                _saving = false;
                return;
            }

            Load();

            if (ConfigChangedEvent != null)
            {
                ConfigChangedEvent(this);
            }

        }

        void CompileChargeCostString()
        {
            chargeCostString = "Current Costs: ";
            chargeCostString += "DA: " + daChargeCost;
            chargeCostString += " | Smaller: " + smallerNoteChargeCost;
            chargeCostString += " | Larger: " + largerNotesChargeCost;
            chargeCostString += " | Random: " + randomNotesChargeCost;
            chargeCostString += " | Instafail: " + instaFailChargeCost;
            chargeCostString += " | Invincible: " + invincibleChargeCost;
            chargeCostString += " | NjsRandom: " + njsRandomChargeCost;
            chargeCostString += " | NoArrows: " + noArrowsChargeCost;
            chargeCostString += " | Funky: " + funkyChargeCost;
            chargeCostString += " | Rainbow: " + rainbowChargeCost;
            if (chargeCostString == "Current Costs: DA: 0 | Smaller: 0 | Larger: 0 | Random: 0 | Instafail: 0 | Invincible: 0 | NjsRandom: 0 | NoArrows: 0 | Funky: 0 | Rainbow: 0")
                chargeCostString = "Current Costs: None!";
        }

        public string GetChargeCostString()
        {
            CompileChargeCostString();
            return chargeCostString;
        }

        public void ChangeConfigValue(string property, string value)
        {
            bool success = true;
            Plugin.Log("Config Change Attempt: " + property + " " + value);
            switch (property)
            {
                case "bitspercharge":
                    bitsPerCharge = int.Parse(value);
                    break;
                case "chargesforsupercharge":
                    chargesForSuperCharge = int.Parse(value);
                    break;
                case "maxcharges":
                    maxCharges = int.Parse(value);
                    break;
                case "chargesperlevel":
                    chargesPerLevel = int.Parse(value);
                    break;
                case "allowsubs":
                    allowSubs = Convert.ToBoolean(value);
                    break;
                case "alloweveryone":
                    allowSubs = Convert.ToBoolean(value);
                    break;
                case "commandspermessage":
                    commandsPerMessage = int.Parse(value);
                    break;
                case "globalcommandcooldown":
                    globalCommandCooldown = float.Parse(value);
                    break;
                case "timeforcharges":
                    timeForCharges = float.Parse(value);
                    break;
                case "chargesovertime":
                    chargesOverTime = int.Parse(value);
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                Save();
                AsyncTwitch.TwitchConnection.Instance.SendChatMessage("Changed Value");
            }
        }
        public void ChangeConfigValue(string command, string property, string value)
        {
            Plugin.Log("Config Change Attempt: " + command + " " + property + " " + value);
            bool success = true;
            switch (command)
            {

                case "da":
                    if (property == "chargecost")
                        daChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        daCooldown = float.Parse(value);
                    else if (property == "duration")
                        daDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "smaller":
                    if (property == "chargecost")
                        smallerNoteChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        smallerNotesCooldown = float.Parse(value);
                    else if (property == "duration")
                        smallerNoteDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "larger":
                    if (property == "chargecost")
                        largerNotesChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        largerNotesCooldown = float.Parse(value);
                    else if (property == "duration")
                        largerNotesDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "random":
                    if (property == "chargecost")
                        randomNotesChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        randomNotesCooldown = float.Parse(value);
                    else if (property == "duration")
                        randomNotesDuration = float.Parse(value);
                    else if (property == "min")
                        randomMin = float.Parse(value);
                    else if (property == "max")
                        randomMax = float.Parse(value);
                    else
                        success = false;
                    break;
                case "instafail":
                    if (property == "chargecost")
                        instaFailChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        instaFailCooldown = float.Parse(value);
                    else if (property == "duration")
                        instaFailDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "invincible":
                    if (property == "chargecost")
                        invincibleChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        invincibleCooldown = float.Parse(value);
                    else if (property == "duration")
                        invincibleDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "njsrandom":
                    if (property == "chargecost")
                        njsRandomChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        njsRandomCooldown = float.Parse(value);
                    else if (property == "duration")
                        njsRandomDuration = float.Parse(value);
                    else if (property == "min")
                        njsRandomMin = float.Parse(value);
                    else if (property == "max")
                        njsRandomMax = float.Parse(value);
                    else
                        success = false;
                    break;
                case "noarrows":
                    if (property == "chargecost")
                        noArrowsChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        noArrowsCooldown = float.Parse(value);
                    else if (property == "duration")
                        noArrowsDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "funky":
                    if (property == "chargecost")
                        funkyChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        funkyCooldown = float.Parse(value);
                    else if (property == "duration")
                        funkyDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                case "rainbow":
                    if (property == "chargecost")
                        rainbowChargeCost = int.Parse(value);
                    else if (property == "cooldown")
                        rainbowCooldown = float.Parse(value);
                    else if (property == "duration")
                        rainbowDuration = float.Parse(value);
                    else
                        success = false;
                    break;
                default:
                    return;


            }

            //    Plugin.Log(allowEveryone.ToString());
            if (success)
            {
                Save();
                AsyncTwitch.TwitchConnection.Instance.SendChatMessage("Changed Value");
            }

        }

         



    }
}
