using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace GamePlayModifiersPlus.Harmony_Patches
{

    [HarmonyPatch(typeof(NoteCutDirectionExtensions),
new Type[] {
            typeof(NoteCutDirection)
})]
    [HarmonyPatch("Rotation", MethodType.Normal)]
    class NoteCuDirectionExtensionsRotation
    {
        static bool Prefix(NoteCutDirection cutDirection, ref Quaternion __result)
        {
           if( (int)cutDirection >= 1000 && (int)cutDirection < 2000)
            {
                __result = default(Quaternion);
                __result.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                return false;
            }

            if ((int)cutDirection >= 2000)
            {
                __result = default(Quaternion);
                __result.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                return false;
            }

            return true;
        }





    }
}
