using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayModifiersPlus.Utilities
{
    public static class EnumExtensions
    {
        public static float GetHealthChangeForHealthType(this GameModifiersController.HealthType type, float initialChange)
        {
            switch (type)
            {
                case GameModifiersController.HealthType.Instafail:
                    if (initialChange < 0)
                        return -1f;
                    break;
                case GameModifiersController.HealthType.Poison:
                    if (initialChange > 0)
                        return 0;
                    break;
                case GameModifiersController.HealthType.Invincible:
                    if (initialChange < 0)
                        return 0;
                    break;
                default:
                    break;
            }
            return initialChange;
        }
    }
}
