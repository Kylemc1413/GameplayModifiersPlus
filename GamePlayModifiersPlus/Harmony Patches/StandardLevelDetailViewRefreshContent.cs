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
    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("RefreshContent", MethodType.Normal)]

    class StandardLevelDetailViewRefreshContent
    {
        static void Postfix(StandardLevelDetailViewController __instance, ref LevelParamsPanel ____levelParamsPanel, ref TextMeshProUGUI ____highScoreText, ref IDifficultyBeatmap ____selectedDifficultyBeatmap, ref PlayerData ____playerData)
        {

            try
            {
                IBeatmapLevel level = ____selectedDifficultyBeatmap.level;
                PlayerData localPlayer = ____playerData;
                if (localPlayer != null)
                {
                    PlayerLevelStatsData playerLevelStats = localPlayer.GetPlayerLevelStatsData(level.levelID, ____selectedDifficultyBeatmap.difficulty, ____selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
                    if (playerLevelStats != null)
                        if (playerLevelStats.validScore)
                        {
                            int highScore = int.Parse(____highScoreText.text);
                            int maxScore = ScoreModel.MaxRawScoreForNumberOfNotes(____selectedDifficultyBeatmap.beatmapData.notesCount);
                            float percent = (float)highScore / maxScore;
                            percent *= 100;
                            ____highScoreText.overflowMode = TextOverflowModes.Overflow;
                            ____highScoreText.enableWordWrapping = false;
                            ____highScoreText.richText = true;
                            ____highScoreText.text += "<size=75%> <#FFFFFF> (" + "<#FFD42A>" + percent.ToString("F2") + "%" + "<#FFFFFF>)";

                        }
                }
            }
            catch(Exception ex)
            {
                Plugin.Log("Exception in DetailView Postfix: \n " + ex);
            }
 
        

        }
    }
    */
}
