//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Twhat does this thing do that animation controller isn't doing?
// 
//
//=============================================================================


using UnityEngine;
namespace Kandooz.Burger {
    public abstract class AbstractHandController : MonoBehaviour {
        private AnimationController controller;
        protected virtual void Awake()
        {
            controller = GetComponentInChildren<AnimationController>();
        }
        public bool Grip
        {
            set { controller.Grip = value; }
        }
        public bool Index
        {
            set { controller.Index = value; }
        }
        public bool Thumb
        {
            set
            { controller.Thumb = value; }
        }

        public float GripPercentage
        {
            set { controller.GripPercentage = value; }
        }
        public float IndexPercentage
        {
            set { controller.IndexPercentage = value; }
        }
        public float ThumbPercentage
        {
            set
            { controller.ThumbPercentage = value; }
        }


    }
}