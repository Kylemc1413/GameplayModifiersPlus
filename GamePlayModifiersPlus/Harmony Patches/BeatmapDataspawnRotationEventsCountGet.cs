using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(BeatmapData))]
    [HarmonyPatch("spawnRotationEventsCount", MethodType.Getter)]
    public class BeatmapDataspawnRotationEventsCountGet
    {
        public static void Postfix(ref int __result)
        {
            if (__result == 0 && ((GMPUI.chatIntegration360 && GMPUI.chatIntegration) || (GMPUI.EndlessMode && Config.EndlessAllow360)) )
                __result = 1;
        }
    }
}
