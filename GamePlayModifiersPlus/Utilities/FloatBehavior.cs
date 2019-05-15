namespace GamePlayModifiersPlus
{
    using System;
    using UnityEngine;

    public class FloatBehavior : MonoBehaviour
    {
        public float originalY;

        public float originalX;

        internal float frequency = 8f + UnityEngine.Random.Range(-1f, 0.5f);

        public float floatStrength = 0.25f;// + UnityEngine.Random.Range(-0.8f, 0.5f);
        internal void Start()
        {
            this.originalY = transform.localPosition.y;
            this.originalX = transform.localPosition.x;
        }

        internal void Update()
        {
   //         Plugin.Log(transform.localPosition.ToString());
            transform.localPosition = new Vector3(
               transform.localPosition.x,
                (originalY) + ((float)Math.Sin(Time.time * frequency) * floatStrength),
                transform.localPosition.z);
        }
    }
}
