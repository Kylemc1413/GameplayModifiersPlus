using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayModifiersPlus
{
    public class Cooldowns
    {
        public static bool globalCoolDown;
        public static bool speedCoolDown;
        public static bool healthCoolDown;
        public static bool daCoolDown;
        public static bool normalSizeCoolDown;
        public static bool randomCoolDown;
        public static bool noArrowsCooldown;
        public static bool randomnjsCoolDown;
        public static bool funkyCoolDown;
        public static bool rainbowCoolDown;
        public void SetCooldown(bool state, string cooldown)
        {
            if (cooldown.ToLower() == "global")
                globalCoolDown = state;
            if (cooldown.ToLower() == "speed")
                speedCoolDown = state;
            if (cooldown.ToLower() == "health")
                healthCoolDown = state;
            if (cooldown.ToLower() == "da")
                daCoolDown = state;
            if (cooldown.ToLower() == "normalsize")
                normalSizeCoolDown = state;
            if (cooldown.ToLower() == "random")
                randomCoolDown = state;
            if (cooldown.ToLower() == "noarrows")
                noArrowsCooldown = state;
            if (cooldown.ToLower() == "randomnjs")
                randomnjsCoolDown = state;
            if (cooldown.ToLower() == "funky")
                funkyCoolDown = state;
            if (cooldown.ToLower() == "rainbow")
                rainbowCoolDown = state;

        }

        public bool GetCooldown(string cooldown)
        {
            if (cooldown.ToLower() == "global")
                return globalCoolDown;
            else if (cooldown.ToLower() == "speed")
                return globalCoolDown;
            else if (cooldown.ToLower() == "health")
                return healthCoolDown;
            else if (cooldown.ToLower() == "da")
                return daCoolDown;
            else if (cooldown.ToLower() == "normalsize")
                return normalSizeCoolDown;
            else if (cooldown.ToLower() == "random")
                return randomCoolDown;
            else if (cooldown.ToLower() == "noarrows")
                return noArrowsCooldown;
            else if (cooldown.ToLower() == "randomnjs")
                return randomnjsCoolDown;
            else if (cooldown.ToLower() == "funky")
                return funkyCoolDown;
            else if (cooldown.ToLower() == "rainbow")
                return rainbowCoolDown;
            else
                return false;
        }

        public void ResetCooldowns()
        {
            globalCoolDown = false;
            speedCoolDown = false;
            healthCoolDown = false;
            normalSizeCoolDown = false;
            daCoolDown = false;
            randomnjsCoolDown = false;
            rainbowCoolDown = false;
        }
    }
}
