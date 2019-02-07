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
    [HarmonyPatch(typeof(ResultsViewController),
           new Type[] {
            typeof(bool),
            typeof(VRUI.VRUIViewController.ActivationType),
           })]
    [HarmonyPatch("DidActivate", MethodType.Normal)]

    class ResultsViewControllerDidActivate
    {
        internal static bool initialNewHighScore = false;
        static void Prefix(bool firstActivation, VRUI.VRUIViewController.ActivationType activationType, ref ResultsViewController __instance, ref bool ____newHighScore)
        {
            if (activationType == VRUI.VRUIViewController.ActivationType.AddedToHierarchy)
            {
                if (GMPUI.disableFireworks)
                {
                    Plugin.Log("Disabling fireworks");
                    initialNewHighScore = ____newHighScore;
                    ____newHighScore = false;
                }

            }
        }
        static void Postfix(bool firstActivation, VRUI.VRUIViewController.ActivationType activationType, ref ResultsViewController __instance, ref bool ____newHighScore, ref GameObject ____newHighScoreText)
        {
            if (GMPUI.disableFireworks)
            {
                ____newHighScoreText.SetActive(initialNewHighScore);
            }




        }

    }
}