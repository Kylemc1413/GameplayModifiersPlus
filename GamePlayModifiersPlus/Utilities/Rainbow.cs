namespace GamePlayModifiersPlus.Utilities
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    public class Rainbow
    {
        public static List<Color> LeftColors = new List<Color>
        {
            { new Color(1f,0,0) },
            { new Color(0.756f, 0, 0.901f) },
            { new Color(0.901f, 0, 0.631f)},
            { new Color(0.901f, 0, 0.105f) },
            { new Color(0.901f, 0.498f, 0) },
            { new Color(1f, 0.992f, 0) },
            { new Color(0.678f, 0, 0.364f) },
        };
        public static List<Color> RightColors = new List<Color>
        {
            { new Color(0,0,1f) },
            { new Color(0.223f, 0, 0.478f) },
            { new Color(0,0.7f,1f) },
            { new Color(0,0.5f,1f) },
            { new Color(0, 0.207f, 0.658f) },
            { new Color(0.109f, 0.490f, 0.337f) },
            { new Color(0, 0.980f, 0.980f) },
            { new Color(0, 0.901f, 0.196f) },
        };


        public static Color GetLeftColor()
        {
            Color color = new Color();
            int random2 = 7;
            int random1 = (int)UnityEngine.Random.Range(0f, 9f);

            if (random1 <= 4)
            {
                random1 = (int)UnityEngine.Random.Range(0f, 6f);
                color = LeftColors[random1];
            }
            else
            {
                random2 = (int)UnityEngine.Random.Range(7f, 9f);
                switch (random2)
                {
                    case 7:
                        color = new Color(UnityEngine.Random.Range(0.5f, 1f), 0f, UnityEngine.Random.Range(0.25f, 0.5f));
                        break;
                    case 8:
                        color = new Color(1f, UnityEngine.Random.Range(0f, 0.5f), 0f);
                        break;
                    case 9:
                        color = new Color(1f, UnityEngine.Random.Range(0.8f, 1f), 0f);
                        break;
                    default:
                        color = new Color(UnityEngine.Random.Range(0.5f, 1f), 0f, UnityEngine.Random.Range(0.25f, 0.5f));
                        break;
                }
            }

<<<<<<< HEAD
        //    if (color.r <= 0.75f && (color.b <= .4f || color.g <= 0.5f))
                color *= 1.4f;
=======
            //    if (color.r <= 0.75f && (color.b <= .4f || color.g <= 0.5f))
            color *= 1.4f;
>>>>>>> master
            color *= UnityEngine.Random.Range(.7f, 1.5f);




            return color;
        }

        public static Color GetRightColor()
        {
            Color color = new Color();
            int random2 = 7;
            int random1 = (int)UnityEngine.Random.Range(0f, 9f);

            if (random1 <= 4)
            {
                random1 = (int)UnityEngine.Random.Range(0f, 7f);
<<<<<<< HEAD
            color = RightColors[random1];
=======
                color = RightColors[random1];
>>>>>>> master
            }
            else

            {
                random2 = (int)UnityEngine.Random.Range(8f, 9f);
                switch (random2)
                {
                    case 8:
                        color = new Color(0f, UnityEngine.Random.Range(0.35f, 1.1f), UnityEngine.Random.Range(0.35f, 1.1f));
                        break;
                    case 9:
                        color = new Color(UnityEngine.Random.Range(0.25f, 0.35f), 0f, UnityEngine.Random.Range(0.6f, 1f));
                        break;
                    default:
                        color = new Color(0f, UnityEngine.Random.Range(0.25f, 1f), UnityEngine.Random.Range(0.25f, 1f));
                        break;
                }
            }


<<<<<<< HEAD
   //         if (color.b <= 0.75f &&( color.g <= .4f || color.r <= 0.35f))
                color *= 1.4f;
=======
            //         if (color.b <= 0.75f &&( color.g <= .4f || color.r <= 0.35f))
            color *= 1.4f;
>>>>>>> master
            color *= UnityEngine.Random.Range(.7f, 1.5f);





            return color;
        }

        public static void RandomizeColors()
        {
            Plugin.colorA.SetColor(GetLeftColor());
            Plugin.colorB.SetColor(GetRightColor());
        }

    }
}
