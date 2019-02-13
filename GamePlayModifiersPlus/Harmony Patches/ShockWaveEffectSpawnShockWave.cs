using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using TMPro;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(ShockwaveEffect),
           new Type[] {
            typeof(Vector3),
           })]
    [HarmonyPatch("SpawnShockwave", MethodType.Normal)]

    class ShockwaveEffectSpawnShockWave
    {
        static bool Prefix(Vector3 pos)
        {
            if (Plugin.isValidScene && GMPUI.disableRipple)
                return false;
            else
                return true;
        }
 

    }
}