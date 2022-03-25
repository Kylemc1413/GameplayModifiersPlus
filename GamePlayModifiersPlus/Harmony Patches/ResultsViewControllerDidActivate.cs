using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using TMPro;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    /*
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("DidActivate", MethodType.Normal)]

    class ResultsViewControllerDidActivate
    {
        internal static bool initialNewHighScore = false;
        static void Prefix(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling, ref ResultsViewController __instance, ref bool ____newHighScore)
        {
            if (addedToHierarchy)
            {
                if (GMPUI.disableFireworks)
                {
                    Plugin.Log("Disabling fireworks");
                    initialNewHighScore = ____newHighScore;
                    ____newHighScore = false;
                }

            }
        }
        static void Postfix(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling, ref ResultsViewController __instance, ref bool ____newHighScore, ref GameObject ____newHighScoreText)
        {
            if (GMPUI.disableFireworks)
            {
                ____newHighScoreText.SetActive(initialNewHighScore);
            }




        }

    }
    */
}