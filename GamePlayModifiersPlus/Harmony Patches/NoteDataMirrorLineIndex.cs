using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace GamePlayModifiersPlus.Harmony_Patches
{

    [HarmonyPatch(typeof(NoteData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("MirrorLineIndex", MethodType.Normal)]
    class NoteDataMirrorLineIndex
    {
        static bool Prefix(int lineCount, ref NoteData __instance)
        {
            if (__instance.lineIndex > 3 || __instance.lineIndex < 0 || __instance.flipLineIndex > 3 || __instance.flipLineIndex < 0)
            {
                if (__instance.lineIndex >= 1000 || __instance.lineIndex <= -1000)
                {
                    int newIndex = __instance.lineIndex;

                    if (newIndex <= -1000)
                        newIndex += 2000;

                    newIndex = 5001 - 1 - newIndex;
                    if (newIndex <= 1000)
                        newIndex -= 2000;

                    __instance.SetProperty("lineIndex", newIndex);
                }
                if (__instance.lineIndex == 4)
                    __instance.SetProperty("lineIndex", -1);
                if (__instance.lineIndex == -1)
                    __instance.SetProperty("lineIndex", 4);

                if (__instance.flipLineIndex >= 1000 || __instance.flipLineIndex <= -1000)
                {
                    int newIndex = __instance.flipLineIndex;

                    if (newIndex <= -1000)
                        newIndex += 2000;

                    newIndex = 5001 - 1 - newIndex;
                    if (newIndex <= 1000)
                        newIndex -= 2000;

                    __instance.SetProperty("flipLineIndex", newIndex);
                }
                if (__instance.flipLineIndex == 4)
                    __instance.SetProperty("flipLineIndex", -1);
                if (__instance.flipLineIndex == -1)
                    __instance.SetProperty("flipLineIndex", 4);
                return false;
            }

            return true;

        }

    }
}
