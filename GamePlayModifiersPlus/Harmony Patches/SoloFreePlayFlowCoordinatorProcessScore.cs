using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator),
          new Type[] {
            typeof(LevelCompletionResults),
            typeof(bool)})]
    [HarmonyPatch("ProcessScore", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorProcessScore
    {
        static void Prefix(LevelCompletionResults levelCompletionResults, ref bool practice)
        {
            practice = true;
        }
    }
}
