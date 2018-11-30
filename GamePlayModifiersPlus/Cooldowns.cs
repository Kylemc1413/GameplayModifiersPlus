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
        public static bool noteCoolDown;
        public void SetCooldown(bool state, string cooldown)
        {
            if (cooldown.ToLower() == "global")
                globalCoolDown = state;
            if (cooldown.ToLower() == "speed")
                speedCoolDown = state;
            if (cooldown.ToLower() == "health")
                healthCoolDown = state;
            if (cooldown.ToLower() == "note")
                noteCoolDown = state;
        }

        public bool GetCooldown(string cooldown)
        {
            if (cooldown.ToLower() == "global")
                return globalCoolDown;
            else if (cooldown.ToLower() == "speed")
                return globalCoolDown;
            else if (cooldown.ToLower() == "health")
                return healthCoolDown;
            else if (cooldown.ToLower() == "note")
                return noteCoolDown;
            else
                return false;
        }

        public void ResetCooldowns()
        {
            globalCoolDown = false;
            speedCoolDown = false;
        }
    }
}
