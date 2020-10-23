using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using IPA.Utilities;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(SinglePlayerLevelSelectionFlowCoordinator))]
    [HarmonyPatch("StartLevelOrShow360Prompt", MethodType.Normal)]
    class PlayPressedFetchLastLevelCollection
    {
        public static void Prefix(SinglePlayerLevelSelectionFlowCoordinator __instance)
        {
            var lastLevelCollection = __instance.GetField<LevelSelectionNavigationController, LevelSelectionFlowCoordinator>("levelSelectionNavigationController").GetField<LevelCollectionNavigationController, LevelSelectionNavigationController>("_levelCollectionNavigationController").GetField<LevelCollectionViewController, LevelCollectionNavigationController>("_levelCollectionViewController")?
                .GetField<LevelCollectionTableView, LevelCollectionViewController>("_levelCollectionTableView")?.GetField<IPreviewBeatmapLevel[], LevelCollectionTableView>("_previewBeatmapLevels");
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
            if (endlessBehavior.nextSong != null)
            {
                ____gameEnergyCounter.AddEnergy(0.5f);
                endlessBehavior.SongEnd();
                return false;
            }
            return true;
        }
    }

}
