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
        public void SetCooldown(bool state, string cooldown)
        {
            if (cooldown.ToLower() == "global")
                globalCoolDown = state;
            if (cooldown.ToLower() == "speed")
                speedCoolDown = state;
        }

        public bool GetCooldown(string cooldown)
        {
            if (cooldown.ToLower() == "global")
                return globalCoolDown;
             if (cooldown.ToLower() == "speed")
                return globalCoolDown;
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
