using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("SetDataToUI", MethodType.Normal)]
    class ResultsViewControllerSetDataToUI
    {
        static void Postfix(ref TextMeshProUGUI ____clearedDifficultyText)
        {
            ____clearedDifficultyText.overflowMode = TextOverflowModes.Overflow;
            ____clearedDifficultyText.richText = true;
            ____clearedDifficultyText.text += "  \r\n<color=#ff0000ff><size=35%><b>Score Submission Disabled by Gameplay Modifiers Plus</b></size>";
        }
    }
}
