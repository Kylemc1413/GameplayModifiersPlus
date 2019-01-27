using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace GamePlayModifiersPlus.Harmony_Patches
{

    [HarmonyPatch(typeof(ObstacleData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("MirrorLineIndex", MethodType.Normal)]
    class ObstacleDataMirrorLineIndex
    {
        static bool Prefix(int lineCount, ref ObstacleData __instance)
        {

            if (__instance.lineIndex > 3 || __instance.lineIndex < 0)
            {
                if (__instance.lineIndex >= 1000 || __instance.lineIndex <= -1000)
                {
                    int newIndex = __instance.lineIndex;

                    if (newIndex <= -1000)
                        newIndex += 2000;

                    newIndex = 5001 - __instance.width - newIndex;
                    if (newIndex <= 1000)
                        newIndex -= 2000;

                    __instance.SetProperty("lineIndex", newIndex);
                    return false;
                }
                if (__instance.lineIndex == 4)
                    __instance.SetProperty("lineIndex", -1);
                if (__instance.lineIndex == -1)
                    __instance.SetProperty("lineIndex", 4);
                return false;

            }
            return true;
        }

    }
}
