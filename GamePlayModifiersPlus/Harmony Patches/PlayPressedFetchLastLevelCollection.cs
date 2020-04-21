using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
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
}
