namespace GamePlayModifiersPlus
{
    using System;
    using UnityEngine;
    using CustomUI.GameplaySettings;
    using IllusionPlugin;
    public class GMPUI
    {
        private static Sprite _ChatDeltaIcon;
        private static Sprite _SwapSabersIcon;
        private static Sprite _RepeatIcon;
        private static Sprite _GnomeIcon;
        private static Sprite _BulletTimeIcon;
        private static Sprite _TwitchIcon;
        private static Sprite _BackButton;
        private static Sprite _GMPIcon;
        private static Sprite _RainbowIcon;
        private static Sprite _FunkyIcon;
        private static Sprite _RandomIcon;
        private static Sprite _NoArrowsIcon;


        public static bool chatDelta = false;
        public static bool chatIntegration = false;
        public static bool gnomeOnMiss = false;
        public static bool superHot = false;
        public static bool bulletTime = false;
        public static bool repeatSong = false;
        public static float fixedNoteScale = 1f;
        public static bool swapSabers;
        public static bool funky;
        public static bool rainbow;
        public static bool njsRandom;
        public static bool randomSize = false;
        public static bool noArrows = false;
        public static bool oneColor = false;
        public static bool AllowMulti = false;

        public static void CreateUI()
        {
            GetIcons();
            string disableScoreString = "<size=120%><color=#ff0000ff><b><u>Disables Score Submission</u> </b></color></size> \r\n<size=100%> ";

            var gmp1Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "GamePlayModifiersPlus", "MainMenu", "GMP1", "GameplayModifiersPlus Options", _GMPIcon);

            //GMP1 Options
      //      var backOption = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "Back", "GMP1", "MainMenu", "Return from GamePlayModifiersOptions", _BackButton);

            var twitchStuffOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Chat Integration", "GMP1", disableScoreString + "Allows Chat to mess with your game if connected. !gm help", _TwitchIcon);
            twitchStuffOption.GetValue = chatIntegration;
            twitchStuffOption.OnToggle += (value) => { chatIntegration = value; Plugin.Log("Changed value"); };

            var chatDeltaOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Chat Delta", "GMP1", "Display Change in Performance Points / Rank in Twitch Chat if Connected", _ChatDeltaIcon);
            chatDeltaOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
            chatDeltaOption.OnToggle += (chatDelta) => { ModPrefs.SetBool("GameplayModifiersPlus", "chatDelta", chatDelta); Plugin.Log("Changed value"); };

            //GMP2 Options
            var gmp2Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "Additional Modifiers", "GMP1", "GMP2", "Additional Modifiers", _GMPIcon);
            //Multiplayer Option toggle
            var multiOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Allow in Multiplayer", "GMP1", "Allow GMP activation in multiplayer room if players equip similar version of the plugin, refer to the readme for more information. Your Version: " + Plugin.pluginVersion, _GMPIcon);
            multiOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "allowMulti", false, true);
            multiOption.OnToggle += (value) => { ModPrefs.SetBool("GameplayModifiersPlus", "allowMulti", value); Plugin.Log("Changed value"); };


            var repeatOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Repeat", "GMP2", disableScoreString + "Restarts song on song end", _RepeatIcon);
            repeatOption.GetValue = repeatSong;
            repeatOption.OnToggle += (value) => { repeatSong = value; Plugin.Log("Changed value"); };

            var gnomeOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Gnome on miss", "GMP2", "Probably try not to miss.", _GnomeIcon);
            gnomeOption.GetValue = gnomeOnMiss;
            gnomeOption.OnToggle += (value) => { gnomeOnMiss = value; Plugin.Log("Changed value"); };
            gnomeOption.AddConflict("Chat Integration");
            gnomeOption.AddConflict("Faster Song");
            gnomeOption.AddConflict("Slower Song");

            
            var bulletTimeOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Bullet Time", "GMP2", disableScoreString + "Slow down time by pressing the triggers on your controllers.", _BulletTimeIcon);
            bulletTimeOption.GetValue = bulletTime;
            bulletTimeOption.OnToggle += (value) => { bulletTime = value; Plugin.Log("Changed value"); };
            bulletTimeOption.AddConflict("Faster Song");
            bulletTimeOption.AddConflict("Slower Song");
            bulletTimeOption.AddConflict("Chat Integration");
            
            
            var noteSizeOption = GameplaySettingsUI.CreateListOption(GameplaySettingsPanels.ModifiersRight, "Note Size", "GMP2", disableScoreString + "Change the size of the notes. Overwritten by Chat Integration and any other size changing options");
            for (float i = 10; i <= 200; i += 10)
                noteSizeOption.AddOption(i / 100);
            noteSizeOption.GetValue = (() =>
            {
                float num = fixedNoteScale;
                if (num % 0.1f != 0)
                    num = (float)Math.Round(num, 1);

                num = Mathf.Clamp(num, 0.1f, 2f);
                return num;
            });
            noteSizeOption.OnChange += (value) => { fixedNoteScale = value;  Plugin.Log("Changed Value"); };
            
            var swapSabersOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Testing Ground", "GMP2", disableScoreString + "Currently Used To test Random stuff", _SwapSabersIcon);
            swapSabersOption.GetValue = swapSabers;
            swapSabersOption.OnToggle += (value) => { swapSabers = value; Plugin.Log("Changed value"); };
            swapSabersOption.AddConflict("Chat Integration");

            var njsRandomOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Random NJS", "GMP2", disableScoreString + "Randomizes Note Jump Speed", _RandomIcon);
            njsRandomOption.GetValue = njsRandom;
            njsRandomOption.OnToggle += (value) => { njsRandom = value; Plugin.Log("Changed value"); };
            njsRandomOption.AddConflict("Chat Integration");

            var randomSizeOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Random Note Size", "GMP2", disableScoreString + "Randomizes Note Size", _RandomIcon);
            randomSizeOption.GetValue = njsRandom;
            randomSizeOption.OnToggle += (value) => { randomSize = value; Plugin.Log("Changed value"); };
            randomSizeOption.AddConflict("Chat Integration");

            var rainbowOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Rainbow", "GMP2", disableScoreString + "Rainbow Notes", _RainbowIcon);
            rainbowOption.GetValue = rainbow;
            rainbowOption.OnToggle += (value) => { rainbow = value; Plugin.Log("Changed value"); };
            rainbowOption.AddConflict("Chat Integration");

            var funkyOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Funky", "GMP2", disableScoreString + "Funky Notes", _FunkyIcon);
            funkyOption.GetValue = funky;
            funkyOption.OnToggle += (value) => { funky = value; Plugin.Log("Changed value"); };
            funkyOption.AddConflict("Chat Integration");

            var noArrowsOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "No Arrows", "GMP2", disableScoreString + "No arrows but without the color randomization", _NoArrowsIcon);
            noArrowsOption.GetValue = noArrows;
            noArrowsOption.OnToggle += (value) => { noArrows = value; Plugin.Log("Changed value"); };
            noArrowsOption.AddConflict("Chat Integration");

            var oneColorOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "One Color ", "GMP2", disableScoreString + "Changes Blocks to One Color, allows you to hit them with either saber. Haptics for saber clash and walls disabled with this modifier on", _GMPIcon);
            oneColorOption.GetValue = oneColor;
            oneColorOption.OnToggle += (value) => { oneColor = value; Plugin.Log("Changed value"); };
            oneColorOption.AddConflict("Chat Integration");
        }







        internal static void GetIcons()
        {
            if (_ChatDeltaIcon == null)
                _ChatDeltaIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.ChatDelta.png");
            if (_SwapSabersIcon == null)
                _SwapSabersIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.SwapSabers.png");
            if (_RepeatIcon == null)
                _RepeatIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.RepeatIcon.png");
            if (_GnomeIcon == null)
                _GnomeIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.gnomeIcon.png");
            if (_BulletTimeIcon == null)
                _BulletTimeIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.BulletIcon.png");
            if (_TwitchIcon == null)
                _TwitchIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.TwitchIcon.png");
            if (_BackButton == null)
                _BackButton = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.Back_Button.png");
            if (_GMPIcon == null)
                _GMPIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.GMPIcon.png");

            if (_FunkyIcon == null)
                _FunkyIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.FunkyIcon.png");
            if (_RainbowIcon == null)
                _RainbowIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.RainbowIcon.png");
            if (_RandomIcon == null)
                _RandomIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.RandomIcon.png");
            if (_NoArrowsIcon == null)
                _NoArrowsIcon = CustomUI.Utilities.UIUtilities.LoadSpriteFromResources("GamePlayModifiersPlus.Resources.NoArrowsIcon.png");
        }

    }
}
