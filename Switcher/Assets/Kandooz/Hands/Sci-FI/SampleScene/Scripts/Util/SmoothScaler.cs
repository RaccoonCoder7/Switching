//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Lerp Scale of object smoothly when object enabled 
// It's lerping with extra steps
//
//=============================================================================


using UnityEngine;
namespace Kandooz
{
    public class SmoothScaler : MonoBehaviour
    {
        public Vector3 startScale;
        public Vector3 targetScale = Vector3.one;
        public float scaleDuration = 1;

        private float delta = 0;

        void Start()
        {
            transform.localScale = startScale;
        }

        private void OnEnable()
        {
            transform.localScale = startScale;
        }

        void Update()
        {
            if (targetScale != transform.localScale)
            {
                delta = Time.deltaTime / scaleDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, delta);

            }

        }
    }
}