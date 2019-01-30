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
    [HarmonyPatch(typeof(StandardLevelDetailViewController))]
    [HarmonyPatch("RefreshContent", MethodType.Normal)]

    class StandardLevelDetailViewRefreshContent
    {
        static void Postfix(ref LevelParamsPanel ____levelParamsPanel, ref TextMeshProUGUI ____highScoreText, ref IDifficultyBeatmap ____difficultyBeatmap, ref IPlayer ____player)
        {
            IBeatmapLevel level = ____difficultyBeatmap.level;
           PlayerDataModelSO.LocalPlayer localPlayer = ____player as PlayerDataModelSO.LocalPlayer;
            if(localPlayer != null)
            { 
    PlayerLevelStatsData playerLevelStats = localPlayer.GetPlayerLevelStatsData(level.levelID, ____difficultyBeatmap.difficulty);
                if(playerLevelStats != null)
            if(playerLevelStats.validScore)
            {
                
               int highScore =  int.Parse(____highScoreText.text);
                int maxScore = ScoreController.MaxScoreForNumberOfNotes(____difficultyBeatmap.beatmapData.notesCount);
                float percent = (float)highScore / maxScore;
                percent *= 100;
                ____highScoreText.overflowMode = TextOverflowModes.Overflow;
                ____highScoreText.enableWordWrapping = false;
                ____highScoreText.richText = true;
                ____highScoreText.text += "<size=75%> <#FFFFFF> (" + "<#FFD42A>" + percent.ToString("F2") + "%" + "<#FFFFFF>)";


            }
}
        

        }
    }
}
