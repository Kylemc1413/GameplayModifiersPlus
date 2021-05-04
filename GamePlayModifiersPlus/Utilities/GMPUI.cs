﻿namespace GamePlayModifiersPlus
{
    using System;
    using UnityEngine;
    using IPA.Config;
    using BeatSaberMarkupLanguage.Attributes;
    using BeatSaberMarkupLanguage;
    using BeatSaberMarkupLanguage.Components;
    using TMPro;
    public class GMPUI : NotifiableSingleton<GMPUI>
    {

        public static bool chatDelta = false;
        public static float fixedNoteScale = 1f;
        public static bool noArrows = false;
        public static bool disableFireworks = false;
        public static bool disableSubmission = false;

        public static bool EndlessMode = false;
        [UIValue("Endless")]
        public bool Endless
        {
            get => EndlessMode;
            set
            {
                EndlessMode = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetEndless")]
        public void SetEndless(bool value)
        {
            Endless = value;
        }
        //   public static bool disableRipple = false;


        [UIComponent("modifiers")]
        RectTransform modifiers;

        [UIParams]
        BeatSaberMarkupLanguage.Parser.BSMLParserParams parserParams;
        public static bool chatIntegration = false;
        [UIValue("ChatIntegration")]
        public bool ChatIntegration
        {
            get => chatIntegration;
            set
            {
                chatIntegration = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetChatIntegration")]
        public void SetChatIntegration(bool value)
        {
            ChatIntegration = value;
        }
        public static bool chatIntegration360 = false;
        public static bool repeatSong = false;
        [UIValue("Repeat")]
        public bool Repeat
        {
            get => repeatSong;
            set
            {

                repeatSong = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetRepeat")]
        public void SetRepeat(bool value)
        {
            Repeat = value;
        }

        public static bool swapSabers = false;
        [UIValue("SwapSabers")]
        public bool SwapSabers
        {
            get => swapSabers;
            set
            {

                swapSabers = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetSwapSabers")]
        public void SetSwapSabers(bool value)
        {
            SwapSabers = value;
        }
        public static bool funky = false;
        [UIValue("Funky")]
        public bool Funky
        {
            get => funky;
            set
            {

                funky = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetFunky")]
        public void SetFunky(bool value)
        {
            Funky = value;
        }
        public static bool rainbow = false;
        [UIValue("Rainbow")]
        public bool Rainbow
        {
            get => rainbow;
            set
            {

                rainbow = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetRainbow")]
        public void SetRainbow(bool value)
        {
            Rainbow = value;
        }
        public static bool njsRandom = false;
        [UIValue("NjsRandom")]
        public bool NjsRandom
        {
            get => njsRandom;
            set
            {

                njsRandom = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetNjsRandom")]
        public void SetNjsRandom(bool value)
        {
            NjsRandom = value;
        }
        public static bool randomSize = false;
        [UIValue("RandomSize")]
        public bool RandomSize
        {
            get => randomSize;
            set
            {

                randomSize = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetRandomSize")]
        public void SetRandomSize(bool value)
        {
            RandomSize = value;
        }

        public static bool reverse = false;
        [UIValue("Reverse")]
        public bool Reverse
        {
            get => reverse;
            set
            {

                reverse = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetReverse")]
        public void SetReverse(bool value)
        {
            Reverse = value;
        }
        public static bool offsetrandom = false;
        [UIValue("OffsetRandom")]
        public bool OffsetRandom
        {
            get => offsetrandom;
            set
            {

                offsetrandom = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetOffsetRandom")]
        public void SetOffsetRandom(bool value)
        {
            OffsetRandom = value;
        }

        public static bool sixLanes = false;
        [UIValue("SixLane")]
        public bool SixLane
        {
            get => sixLanes;
            set
            {

                sixLanes = value;
                if (value)
                {
                    SetFiveLane(false);
                    parserParams.EmitEvent("getModifiers");
                }

                NotifyPropertyChanged();
            }
        }
        [UIAction("SetSixLane")]
        public void SetSixLane(bool value)
        {
            SixLane = value;
        }
        public static bool fourLayers = false;
        [UIValue("FourLayer")]
        public bool FourLayer
        {
            get => fourLayers;
            set
            {

                fourLayers = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetFourLayer")]
        public void SetFourLayer(bool value)
        {
            FourLayer = value;
        }
        public static bool fiveLanes = false;
        [UIValue("FiveLane")]
        public bool FiveLane
        {
            get => fiveLanes;
            set
            {

                fiveLanes = value;
                if (value)
                {
                    SetSixLane(false);
                    parserParams.EmitEvent("getModifiers");
                }
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetFiveLane")]
        public void SetFiveLane(bool value)
        {
            FiveLane = value;
        }
        public static bool laneShift = false;
        [UIValue("LaneShift")]
        public bool LaneShift
        {
            get => laneShift;
            set
            {

                laneShift = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetLaneShift")]
        public void SetLaneShift(bool value)
        {
            LaneShift = value;
        }
        public static bool angleShift = false;
        [UIValue("AngleShift")]
        public bool AngleShift
        {
            get => angleShift;
            set
            {

                angleShift = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetAngleShift")]
        public void SetAngleShift(bool value)
        {
            AngleShift = value;
        }
        public static bool removeCrouchWalls = false;
        [UIValue("RemoveCrouch")]
        public bool RemoveCrouch
        {
            get => removeCrouchWalls;
            set
            {

                removeCrouchWalls = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetRemoveCrouch")]
        public void SetRemoveCrouch(bool value)
        {
            RemoveCrouch = value;
        }

        public static bool tunnel = false;
        [UIValue("Tunnel")]
        public bool Tunnel
        {
            get => tunnel;
            set
            {

                tunnel = value;
                NotifyPropertyChanged();
            }
        }
        [UIAction("SetTunnel")]
        public void SetTunnel(bool value)
        {
            Tunnel = value;
        }
        [UIValue("mappingExtensions")]
        public bool MappingExtensions { get => Plugin.mappingExtensionsPresent; }
        [UIValue("twitchPlugin")]
        public bool twitchPlugin { get => Plugin.twitchPluginInstalled; }
        [UIAction("#post-parse")]
        public void PostSetup()
        {
            modifiers.localScale = new Vector3(0.9f, 0.9f, 1);

        }
        /*
        public static void CreateUI()
        {
            GetIcons();

            if (BS_Utils.Gameplay.GetUserInfo.GetUserID() == 76561198047644920)
            {

                CustomUI.MenuButton.MenuButtonUI.AddButton("AYAYA", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("papatutuwawa", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("chikin dayo", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("cececlown", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("desu", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("Smilew", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("hamnomanim", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("catgirls", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("mousegirl", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("miku", null, _GMPIcon);
                CustomUI.MenuButton.MenuButtonUI.AddButton("ohisee", null, _GMPIcon);

            }
            string disableScoreString = "<size=120%><color=#ff0000ff><b><u>Disables Score Submission</u> </b></color></size> \r\n<size=100%> ";
            //Rumble Option
            var gmpTweaksMenu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "GMP Tweaks", "MainMenu", "gmptweaksMenu", "GameplayModifiersPlus Tweaks", _GMPIcon);
            var rumbleOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Controller Rumble", "MainMenu", "Toggle Controller Vibration in songs", _GMPIcon);
            rumbleOption.GetValue = ModPrefs.GetInt("GameplayModifiersPlus", "GameRumbleSetting", -1, false) != 1 ? false : true; ;
            rumbleOption.OnToggle += (value) => { ModPrefs.SetInt("GameplayModifiersPlus", "GameRumbleSetting", value == true ? 1 : 0); Plugin.Log("Changed value"); };

            var disableFireWorksOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Disable Fireworks", "gmptweaksMenu", "Disable fireworks after a song", _GMPIcon);
            disableFireWorksOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "DisableFireworks", false, false);
            disableFireWorksOption.OnToggle += (value) => { disableFireworks = value; ModPrefs.SetBool("GameplayModifiersPlus", "DisableFireworks", value); Plugin.Log("Changed value"); };

            var disableRippleOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Disable Note Ripple", "gmptweaksMenu", "Disable ripple effect after cutting a note", _GMPIcon);
            disableRippleOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "DisableRipple", false, false);
            disableRippleOption.OnToggle += (value) => { disableRipple = value; ModPrefs.SetBool("GameplayModifiersPlus", "DisableRipple", value); Plugin.Log("Changed value"); };


            var gmp1Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "GamePlayModifiersPlus", "MainMenu", "GMP1", "GameplayModifiersPlus Options", _GMPIcon);

            var disableScoresOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsLeft, "Disable Score Submission", "MainMenu", "<size=120%><color=#ff0000ff><b><u>Disables Score Submission</u></b></color></size>", _GMPIcon);
            disableScoresOption.GetValue = disableSubmission;
            disableScoresOption.OnToggle += (value) =>
            {
                disableSubmission = value;
                if (value)
                    BS_Utils.Gameplay.ScoreSubmission.ProlongedDisableSubmission("Gameplay Modifiers Plus");
                else
                    BS_Utils.Gameplay.ScoreSubmission.RemoveProlongedDisable("Gameplay Modifiers Plus");
            };



            //GMP1 Options
            //      var backOption = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "Back", "GMP1", "MainMenu", "Return from GamePlayModifiersOptions", _BackButton);
            if (Plugin.twitchPluginInstalled)
            {
                var twitchStuffOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Chat Integration", "GMP1", disableScoreString + "Allows Chat to mess with your game if connected. !gm help", _TwitchIcon);
                twitchStuffOption.GetValue = chatIntegration;
                twitchStuffOption.OnToggle += (value) => { chatIntegration = value; Plugin.Log("Changed value"); };



                var chatDeltaOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Chat Delta", "GMP1", "Display Change in Performance Points / Rank in Twitch Chat if Connected", _ChatDeltaIcon);
                chatDeltaOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "chatDelta", false, true);
                chatDeltaOption.OnToggle += (chatDelta) => { ModPrefs.SetBool("GameplayModifiersPlus", "chatDelta", chatDelta); Plugin.Log("Changed value"); };

            }

            //GMP2 Options
            var gmp2Menu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersRight, "Additional Modifiers", "GMP1", "GMP2", "Additional Modifiers", _GMPIcon);
            //Multiplayer Option toggle
            var multiOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Allow in Multiplayer", "GMP1", "Allow GMP activation in multiplayer room if players equip similar version of the plugin, refer to the readme for more information. Your Version: " + Plugin.Version, _GMPIcon);
            multiOption.GetValue = ModPrefs.GetBool("GameplayModifiersPlus", "allowMulti", false, true);
            multiOption.OnToggle += (value) => { allowMulti = value; ModPrefs.SetBool("GameplayModifiersPlus", "allowMulti", value); Plugin.Log("Changed value"); };


            var rainbowOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Rainbow", "GMP2", "Rainbow Notes", _RainbowIcon);
            rainbowOption.GetValue = rainbow;
            rainbowOption.OnToggle += (value) => { rainbow = value; Plugin.Log("Changed value"); };
            rainbowOption.AddConflict("Chat Integration");

            var reverseOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Reversal ", "GMP2", disableScoreString + "Reverses the direction the notes come from", _SwapSabersIcon);
            reverseOption.GetValue = reverse;
            reverseOption.OnToggle += (value) => { reverse = value; Plugin.Log("Changed value"); };
            reverseOption.AddConflict("Chat Integration");


            var repeatOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Repeat", "GMP2", disableScoreString + "Restarts song on song end", _RepeatIcon);
            repeatOption.GetValue = repeatSong;
            repeatOption.OnToggle += (value) => { repeatSong = value; Plugin.Log("Changed value"); };
            if (Plugin.mappingExtensionsPresent)
            {
                var sixlanesOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Six Lanes", "GMP2", disableScoreString + "Extends the map to be in 6 lanes", _GMPIcon);
                sixlanesOption.GetValue = sixLanes;
                sixlanesOption.OnToggle += (value) => { sixLanes = value; Plugin.Log("Changed value"); };

                var fourlayersOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Four Layers", "GMP2", disableScoreString + "Extends the map to be in 4 vertical layers rather than 3", _GMPIcon);
                fourlayersOption.GetValue = fourLayers;
                fourlayersOption.OnToggle += (value) => { fourLayers = value; Plugin.Log("Changed value"); };

                var fiveLanesOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Five Lanes", "GMP2", disableScoreString + "Extends the map to be in 5 lanes", _GMPIcon);
                fiveLanesOption.GetValue = fiveLanes;
                fiveLanesOption.OnToggle += (value) => { fiveLanes = value; Plugin.Log("Changed value"); };
                fiveLanesOption.AddConflict("Six Lanes");

                var laneshiftOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Lane Shift", "GMP2", disableScoreString + "Randomly shifts notes off of their normal lane", _GMPIcon);
                laneshiftOption.GetValue = laneShift;
                laneshiftOption.OnToggle += (value) => { laneShift = value; Plugin.Log("Changed value"); };

                var angleShiftOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Angle Shift", "GMP2", disableScoreString + "Randomly shifts notes off of their normal Angle", _GMPIcon);
                angleShiftOption.GetValue = angleShift;
                angleShiftOption.OnToggle += (value) => { angleShift = value; Plugin.Log("Changed value"); };

                var tunnelOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Tunnel", "GMP2", "Play the song in a tunnel", _GMPIcon);
                tunnelOption.GetValue = tunnel;
                tunnelOption.OnToggle += (value) => { tunnel = value; Plugin.Log("Changed value"); };


            }


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
            noteSizeOption.OnChange += (value) => { fixedNoteScale = value; Plugin.Log("Changed Value"); };

            var swapSabersOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Testing Ground", "GMP2", disableScoreString + "Currently Used To test Random stuff");
            swapSabersOption.GetValue = swapSabers;
            swapSabersOption.OnToggle += (value) => { swapSabers = value; Plugin.Log("Changed value"); };
            swapSabersOption.AddConflict("Chat Integration");

            var njsRandomOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Random NJS", "GMP2", disableScoreString + "Randomizes Note Jump Speed", _RandomIcon);
            njsRandomOption.GetValue = njsRandom;
            njsRandomOption.OnToggle += (value) => { njsRandom = value; Plugin.Log("Changed value"); };
            njsRandomOption.AddConflict("Chat Integration");

            var offsetrandomOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Random Spawn Offset", "GMP2", disableScoreString + "Randomizes Note Spawn offset", _RandomIcon);
            offsetrandomOption.GetValue = offsetrandom;
            offsetrandomOption.OnToggle += (value) => { offsetrandom = value; Plugin.Log("Changed value"); };
            offsetrandomOption.AddConflict("Chat Integration");

            var randomSizeOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Random Note Size", "GMP2", disableScoreString + "Randomizes Note Size", _RandomIcon);
            randomSizeOption.GetValue = njsRandom;
            randomSizeOption.OnToggle += (value) => { randomSize = value; Plugin.Log("Changed value"); };
            randomSizeOption.AddConflict("Chat Integration");

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

            var removeCrouchOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersRight, "Remove Crouch Walls ", "GMP2", disableScoreString + "Removes walls that require you to duck, leaving other walls in tact", _GMPIcon);
            removeCrouchOption.GetValue = removeCrouchWalls;
            removeCrouchOption.OnToggle += (value) => { removeCrouchWalls = value; Plugin.Log("Changed value"); };

        }

        */


        /*
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
    */
    }
}
