namespace GamePlayModifiersPlus
{
    using System;
    using UnityEngine;

    public class FloatBehavior : MonoBehaviour
    {
        public float originalY;

        public float originalX;

        internal float frequency = 8f + UnityEngine.Random.Range(-1f, 0.5f);

        public float floatStrength = 0.2f;// + UnityEngine.Random.Range(-0.8f, 0.5f);// You can change this in the Unity Editor to

        // change the range of y positions that are possible.
        internal void Start()
        {
            this.originalY = this.transform.localPosition.y;
            this.originalX = this.transform.localPosition.x;
        }

        internal void Update()
        {
            transform.localPosition = new Vector3(
               transform.localPosition.x,
                originalY + ((float)Math.Sin(Time.time * frequency) * floatStrength),
                transform.localPosition.z);
        }
    }
}
