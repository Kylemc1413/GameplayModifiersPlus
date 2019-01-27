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
            })]
    [HarmonyPatch("MirrorTransformCutDirection", MethodType.Normal)]
    class NoteDataMirrorTransformCutDirection
    {
        static bool Prefix(ref NoteData __instance)
        {
            if ((int)__instance.cutDirection >= 1000)
            {
                int cutdir = (int)__instance.cutDirection;
                int angle = cutdir >= 2000 ? 2000 - cutdir : 1000 - cutdir;
                angle = angle > 180 ? 360 - angle : 180 - angle;

                int newdir = cutdir >= 2000 ? angle + 2000 : angle + 1000;

                __instance.SetProperty("cutDirection", (NoteCutDirection)newdir);
                return false;
            }




            return true;
        }

    }
}
