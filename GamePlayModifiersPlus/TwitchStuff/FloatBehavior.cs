using IllusionPlugin;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using AsyncTwitch;
using IllusionInjector;
using TMPro;
using CustomUI.GameplaySettings;
namespace GamePlayModifiersPlus
{
    public class FloatBehavior : MonoBehaviour
    {
        public float originalY;
        public float originalX;
        float frequency = 8f + UnityEngine.Random.Range(-1f, 0.5f);
        public float floatStrength = 0.2f; // + UnityEngine.Random.Range(-0.8f, 0.5f);// You can change this in the Unity Editor to 
                                                                              // change the range of y positions that are possible.

        void Start()
        {
            this.originalY = this.transform.localPosition.y;
            this.originalX = this.transform.localPosition.x;
        }

        void Update()
        {// originalX + ((float)Math.Cos(Time.time * frequency) * floatStrength)
            transform.localPosition = new Vector3(
               transform.localPosition.x,
                originalY + ((float)Math.Sin(Time.time * frequency) * floatStrength ),
                transform.localPosition.z);
        }
    }
}
