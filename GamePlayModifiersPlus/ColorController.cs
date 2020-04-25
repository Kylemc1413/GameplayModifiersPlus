using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GamePlayModifiersPlus.Utilities;
namespace GamePlayModifiersPlus
{
    public static class ColorController
    {
        public static ColorScheme GMPColorScheme = new ColorScheme("GMPColorScheme", "GMP Color Scheme", false, Color.white, Color.white, Color.white, Color.white, Color.white);
        public static ColorScheme oldColorScheme = null;

        public static void ResetColors()
        {
            if (!GameObjects.ColorManager) return;
            if (oldColorScheme == null)
                SetupColors();
            GameObjects.ColorManager.SetField("_colorScheme", oldColorScheme);
            //  ColorManager.GetField<SimpleColorSO>("_saberAColor").SetColor(ColorManager.GetField<ColorScheme>("_colorScheme").saberAColor);
            //  ColorManager.GetField<SimpleColorSO>("_saberBColor").SetColor(ColorManager.GetField<ColorScheme>("_colorScheme").saberBColor);
        }
        public static void SetupColors()
        {
            oldColorScheme = GameObjects.ColorManager.GetField<ColorScheme>("_colorScheme");
            GMPColorScheme.SetField("_saberAColor", oldColorScheme.saberAColor);
            GMPColorScheme.SetField("_saberBColor", oldColorScheme.saberBColor);
            GMPColorScheme.SetField("_environmentColor0", oldColorScheme.environmentColor0);
            GMPColorScheme.SetField("_environmentColor1", oldColorScheme.environmentColor1);
            GMPColorScheme.SetField("_obstaclesColor", oldColorScheme.obstaclesColor);
        }
        public static void SetColors(Color left, Color right)
        {
            if (!GameObjects.ColorManager) return;
            if (oldColorScheme == null)
                SetupColors();
            if (GameObjects.ColorManager.GetField<ColorScheme>("_colorScheme") != GMPColorScheme)
                GameObjects.ColorManager.SetField("_colorScheme", GMPColorScheme);

            GMPColorScheme.SetField("_saberAColor", left);
            GMPColorScheme.SetField("_saberBColor", right);

        }
    }
}
