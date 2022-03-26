using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using IPA.Utilities;
using GamePlayModifiersPlus.Utilities;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    /*
    [HarmonyPatch(typeof(SinglePlayerLevelSelectionFlowCoordinator))]
    [HarmonyPatch("StartLevelOrShow360Prompt", MethodType.Normal)]
    class PlayPressedFetchLastLevelCollection
    {
        public static void Prefix(SinglePlayerLevelSelectionFlowCoordinator __instance)
        {
            var lastLevelCollection = __instance.GetField<LevelSelectionNavigationController, LevelSelectionFlowCoordinator>("levelSelectionNavigationController")
                .GetField<LevelCollectionNavigationController, LevelSelectionNavigationController>("_levelCollectionNavigationController").
                GetField<LevelCollectionViewController, LevelCollectionNavigationController>("_levelCollectionViewController")?
                .GetField<LevelCollectionTableView, LevelCollectionViewController>("_levelCollectionTableView")?
                .GetField<IPreviewBeatmapLevel[], LevelCollectionTableView>("_previewBeatmapLevels");
            if (lastLevelCollection != null)
                EndlessBehavior.LastLevelCollection = lastLevelCollection;
        }
    }
        */
    [HarmonyPatch(typeof(GameEnergyCounter))]
    [HarmonyPatch("ProcessEnergyChange", MethodType.Normal)]
    class StandardLevelGameplayManagerEnergyReachedZero
    {
        public static bool Prefix(ref GameEnergyCounter __instance, ref float energyChange)
        {
            if ((GMPUI.EndlessMode && Config.EndlessContinueOnFail))
            {
                GameObject endlessObj = GameObject.Find("GMP Endless Behavior");
                if (endlessObj == null) return true;
                if (energyChange > 0) return true;
                EndlessBehavior endlessBehavior = endlessObj.GetComponent<EndlessBehavior>();
                if (endlessBehavior.nextSong != null)
                {
                    bool willFail = (__instance.energy + energyChange) <= 0;

                    if (willFail && BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.gameplayModifiers.noFailOn0Energy)
                    {
                        __instance.ProcessEnergyChange(0.5f);
                        endlessBehavior.SongEnd();
                        return false;
                    }
                }
            }
            else if (GMPUI.chatIntegration && Plugin.twitchPluginInstalled && GameModifiersController.currentHealthType != GameModifiersController.HealthType.Normal)
            {
                energyChange = GameModifiersController.currentHealthType.GetHealthChangeForHealthType(energyChange);
            }
            return true;
        }
    }
    
}
