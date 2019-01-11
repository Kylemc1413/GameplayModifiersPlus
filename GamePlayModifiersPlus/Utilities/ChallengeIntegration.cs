using BeatSaberDailyChallenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayModifiersPlus.Utilities
{
    class ChallengeIntegration
    {
        //These are where the options get saved to be returned to what they were before the challenge
        public static bool chatDelta;
        public static bool chatIntegration;
        public static bool gnomeOnMiss;
        public static bool superHot;
        public static bool bulletTime;
        public static bool repeatSong;
        public static float fixedNoteScale;
        public static bool swapSabers;
        public static bool funky;
        public static bool rainbow;
        public static bool njsRandom;
        public static bool randomSize;
        public static bool noArrows;
        public static bool oneColor;

        public static void AddListeners()
        {
            ChallengeExternalModifiers.onChallengeFailedToLoad += ReturnOptions;
            ChallengeExternalModifiers.onChallengeEnd += ReturnOptions;
            ChallengeExternalModifiers.RegisterHandler("GameplayModifiersPlus", delegate (string[] modifiers)
           {
               Plugin.activateDuringIsolated = true;
               SaveOptions();
               SetToDefaultOptions();
               foreach(string arg in modifiers)
               {
                   if (arg.StartsWith("fixedNoteScale"))
                   {
                       GMPUI.fixedNoteScale = float.Parse(arg.Split(':')[1]);
                       continue;
                   }
                   switch (arg)
                   {
                       case "gnomeOnMiss":
                           GMPUI.gnomeOnMiss = true;
                           break;
                       case "bulletTime":
                           GMPUI.bulletTime = true;
                           break;
                       case "swapSabers":
                           GMPUI.swapSabers = true;
                           break;
                       case "funky":
                           GMPUI.funky = true;
                           break;
                       case "rainbow":
                           GMPUI.rainbow = true;
                           break;
                       case "njsRandom":
                           GMPUI.njsRandom = true;
                           break;
                       case "randomSize":
                           GMPUI.randomSize = true;
                           break;
                       case "noArrows":
                           GMPUI.noArrows = true;
                           break;
                       case "oneColor":
                           GMPUI.oneColor = true;
                           break;
                       default:
                           return false;
                   }
               }
               return true;
           });
        }
        private static void SaveOptions()
        {
            chatDelta = GMPUI.chatDelta;
            chatIntegration = GMPUI.chatIntegration;
            gnomeOnMiss = GMPUI.gnomeOnMiss;
            superHot = GMPUI.superHot;
            bulletTime = GMPUI.bulletTime;
            repeatSong = GMPUI.repeatSong;
            fixedNoteScale = GMPUI.fixedNoteScale;
            swapSabers = GMPUI.swapSabers;
            funky = GMPUI.funky;
            rainbow = GMPUI.rainbow;
            njsRandom = GMPUI.njsRandom;
            randomSize = GMPUI.randomSize;
            noArrows = GMPUI.noArrows;
            oneColor = GMPUI.oneColor;
        }
        private static void SetToDefaultOptions()
        {
            GMPUI.chatDelta = false;
            GMPUI.chatIntegration = false;
            GMPUI.gnomeOnMiss = false;
            GMPUI.superHot = false;
            GMPUI.bulletTime = false;
            GMPUI.repeatSong = false;
            GMPUI. fixedNoteScale = 1f;
            GMPUI.swapSabers = false;
            GMPUI.funky = false;
            GMPUI.rainbow = false;
            GMPUI.njsRandom = false;
            GMPUI.randomSize = false;
            GMPUI.noArrows = false;
            GMPUI.oneColor = false;
        }
        private static void ReturnOptions()
        {
            Plugin.activateDuringIsolated = false;
            GMPUI.chatDelta = chatDelta;
            GMPUI.chatIntegration = chatIntegration;
            GMPUI.gnomeOnMiss = gnomeOnMiss;
            GMPUI.superHot = superHot;
            GMPUI.bulletTime = bulletTime;
            GMPUI.repeatSong = repeatSong;
            GMPUI.fixedNoteScale = fixedNoteScale;
            GMPUI.swapSabers = swapSabers;
            GMPUI.funky = funky;
            GMPUI.rainbow = rainbow;
            GMPUI.njsRandom = njsRandom;
            GMPUI.randomSize = randomSize;
            GMPUI.noArrows = noArrows;
            GMPUI.oneColor = oneColor;
        }
    }
}
