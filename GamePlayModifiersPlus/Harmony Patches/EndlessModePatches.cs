using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(LevelSelectionFlowCoordinator))]
    [HarmonyPatch("StartLevelOrShow360Warning", MethodType.Normal)]
    class PlayPressedFetchLastLevelCollection
    {
        public static void Prefix(LevelSelectionFlowCoordinator __instance, ref LevelSelectionNavigationController ____levelSelectionNavigationController)
        {
            var lastLevelCollection =  ____levelSelectionNavigationController.GetField<LevelCollectionViewController>("_levelCollectionViewController")?
                .GetField<LevelCollectionTableView>("_levelCollectionTableView")?.GetField<IPreviewBeatmapLevel[]>("_previewBeatmapLevels");
            if (lastLevelCollection != null)
                EndlessBehavior.LastLevelCollection = lastLevelCollection;
        }
    }

    [HarmonyPatch(typeof(StandardLevelGameplayManager))]
    [HarmonyPatch("HandleGameEnergyDidReach0", MethodType.Normal)]
    class StandardLevelGameplayManagerEnergyReachedZero
    {
        public static bool Prefix(StandardLevelGameplayManager __instance, ref GameEnergyCounter ____gameEnergyCounter)
        {
            if (!(GMPUI.EndlessMode && Config.EndlessContinueOnFail)) return true;
            GameObject endlessObj = GameObject.Find("GMP Endless Behavior");
            if (endlessObj == null) return true;
            EndlessBehavior endlessBehavior = endlessObj.GetComponent<EndlessBehavior>();
            ____gameEnergyCounter.AddEnergy(0.5f);
            endlessBehavior.PlayerFailed();
            return false;
        }
    }
}
