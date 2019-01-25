using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace GamePlayModifiersPlus.Harmony_Patches
{

    [HarmonyPatch(typeof(ColorNoteVisuals),
new Type[] {
            typeof(NoteController)
})]
    [HarmonyPatch("HandleNoteControllerDidInitEvent", MethodType.Normal)]
    class ColorNoteVisualsHandleNoteControllerDidInitEvent
    {
        static bool Prefix(NoteController noteController, ref ColorManager ____colorManager, ref SpriteRenderer ____arrowGlowSpriteRenderer, ref SpriteRenderer ____circleGlowSpriteRenderer, ref MaterialPropertyBlockController[] ____materialPropertyBlockControllers, ref int ____colorID, ref Action ___didInitEvent, ref MeshRenderer ____arrowMeshRenderer)
        {
            NoteData noteData = noteController.noteData;
            NoteType noteType = noteData.noteType;
            if (noteData.cutDirection == NoteCutDirection.Any || (int)noteData.cutDirection >= 2000)
            {
                ____arrowMeshRenderer.enabled = false;
                ____arrowGlowSpriteRenderer.enabled = false;
                ____circleGlowSpriteRenderer.enabled = true;
            }
            else
            {
                ____arrowMeshRenderer.enabled = true;
                ____arrowGlowSpriteRenderer.enabled = true;
                ____circleGlowSpriteRenderer.enabled = false;
            }
            Color color = ____colorManager.ColorForNoteType(noteType);
            ____arrowGlowSpriteRenderer.color = color.ColorWithAlpha(0.3f);
            ____circleGlowSpriteRenderer.color = color;
            foreach (MaterialPropertyBlockController materialPropertyBlockController in ____materialPropertyBlockControllers)
            {
                MaterialPropertyBlock materialPropertyBlock = materialPropertyBlockController.materialPropertyBlock;
                materialPropertyBlock.SetColor(____colorID, color.ColorWithAlpha(1f));
                materialPropertyBlockController.ApplyChanges();
            }
            ___didInitEvent?.Invoke();

            return false;
        }





    }
}
