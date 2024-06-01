using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kandooz.Burger {

    [CreateAssetMenu (fileName = "GrabbedObject", menuName = "Grabbing", order = 0)]
    public class GrabbedObjectData : ScriptableObject {
        public bool usePosition = true;
        public bool useRotation = true;
        public Vector3 rightHandObjPosition;
        public Vector3 rightHandObjRotation;
        [Space (15)]
        public Vector3 leftHandObjPosition;
        public Vector3 leftHandObjRotation;
        [Space (15)]
        public bool useAnimationProfile;
        public HandAnimationProfile rightHandProfile;
        public HandAnimationProfile leftHandProfile;
    }

    [System.Serializable]
    public struct HandAnimationProfile {
        public bool grip;
        public bool index;
        public bool thumb;

        public float gripPercentage;
        public float indexPercentage;
        public int thumbState;
        public float thumbPercentage;

        public bool staticPose;
        public int pose;
    }
}