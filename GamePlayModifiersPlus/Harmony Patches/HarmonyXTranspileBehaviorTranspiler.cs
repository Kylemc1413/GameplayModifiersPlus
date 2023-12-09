using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace GamePlayModifiersPlus.Harmony_Patches
{
    [HarmonyPatch(typeof(BeatmapCallbacksController))]
    internal static class HarmonyXTranspileBehaviorTranspiler
    {
        [HarmonyTranspiler]
        [HarmonyPatch("ManualUpdate")]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions, null);

            return codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Leave))
                .Advance(1)
                .Insert(new CodeInstruction(OpCodes.Nop)).InstructionEnumeration();
        }
    }
}
