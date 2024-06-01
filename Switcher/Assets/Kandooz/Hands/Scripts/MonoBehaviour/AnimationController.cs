//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Set animation values of fingers in the animation controller, using blends between poses, the values are between 0 and 1 where 0 indicate closed finger and
// 1 indicate open finger. 
//
//=============================================================================


using UnityEngine;

namespace Kandooz.Burger
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        #region variables
        [HideInInspector] public Animator animator; // Hand animator

        [HideInInspector] [SerializeField] private bool staticPose; // if checked you can choose between 3 static poses..
        [HideInInspector] [SerializeField] private int pose;        //  static poses => (1- Big Grip, 2- Small Grip, 3- Cylinder Grip)

        [HideInInspector] [SerializeField] private bool grip;       // if checked it means the grip animation is derived from input
        [HideInInspector] [SerializeField] [Range(0, 1)] private float gripPercentage = 1;
        [HideInInspector] [SerializeField] private bool index;      // if checked it means the index animation is derived from input
        [HideInInspector] [SerializeField] [Range(0, 1)] private float indexPercentage = 1;
        [HideInInspector] [SerializeField] private bool thumb;      // if checked it means the thumb animation is derived from input
        [HideInInspector] [SerializeField] private int thumbState;
        [HideInInspector] [SerializeField] [Range(0, 1)] private float thumbPercentage = 1;

        [Space]
        [Tooltip("select if the hand is following a predefined sequence")]
        [HideInInspector] [SerializeField] private bool drivedByAnimation;

        private HandAnimationProfile defaultProfile;  // the values of the thumb, index and grip when grabbing an object
        private bool defaultAnimationDrive;  // enables mapping animations from input
        #endregion
        #region properties
        public bool Grip
        {
            get
            {
                return grip;
            }

            set
            {
                animator.SetBool("grip", value);
                grip = value;
            }
        }

        public bool Index
        {
            get
            {
                return index;
            }

            set
            {
                animator.SetBool("index", value);

                index = value;
            }
        }

        public bool Thumb
        {
            get
            {
                return thumb;
            }

            set
            {
                animator.SetBool("thumb", value);
                thumb = value;
            }
        }

        public float GripPercentage
        {
            get
            {
                return gripPercentage;
            }

            set
            {
                value = Mathf.Clamp01(value);
                animator.SetFloat("gripPercentage", value);
                gripPercentage = value;
            }
        }

        public float IndexPercentage
        {
            get
            {
                return indexPercentage;
            }

            set
            {
                value = Mathf.Clamp01(value);
                animator.SetFloat("indexPercentage", value);

                indexPercentage = value;
            }
        }

        public int ThumbState
        {
            get
            {
                return thumbState;
            }

            set
            {
                animator.SetInteger("thumbState", value);
                thumbState = value;
            }
        }

        public float ThumbPercentage
        {
            get
            {
                return thumbPercentage;
            }

            set
            {
                value = Mathf.Clamp01(value);
                animator.SetFloat("thumbPercentage", value);

                thumbPercentage = value;
            }
        }

        public bool StaticPose
        {
            get
            {
                return staticPose;
            }

            set
            {
                Pose = 0;
                staticPose = value;
            }
        }

        public int Pose
        {
            get
            {
                return pose;
            }

            set
            {
                animator.SetInteger("AnimationState", value);

                pose = value;

            }
        }

        #endregion
        #region monobehaviours
        void Awake()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }

            StaticPose = StaticPose;
            Pose = Pose;
            Grip = Grip;
            Index = Index;
            Thumb = Thumb;
            GripPercentage = GripPercentage;
            IndexPercentage = IndexPercentage;
            ThumbPercentage = ThumbPercentage;
            ThumbState = ThumbState;

            defaultProfile = CopyAnimatorToProfile();
            defaultAnimationDrive = drivedByAnimation;
        }
        private void Update()
        {
            if (drivedByAnimation)
            {
                StaticPose = StaticPose;
                Pose = Pose;
                Grip = Grip;
                Index = Index;
                Thumb = Thumb;
                GripPercentage = GripPercentage;
                IndexPercentage = IndexPercentage;
                ThumbPercentage = ThumbPercentage;
                ThumbState = ThumbState;
            }

        }
        #endregion
        #region User Methods
        // apply custom values of hand animation profile, mainly used when grabbing objects
        public void SetAnimatorWithProfile(HandAnimationProfile profile)
        {
            Grip = profile.grip;
            Index = profile.index;
            Thumb = profile.thumb;
            GripPercentage = profile.gripPercentage;
            IndexPercentage = profile.indexPercentage;
            ThumbState = profile.thumbState;
            ThumbPercentage = profile.thumbPercentage;
            StaticPose = profile.staticPose;
            Pose = profile.pose;
        }
        // Returns a Profile of the current Hand
        public HandAnimationProfile CopyAnimatorToProfile()
        {
            HandAnimationProfile profile = new HandAnimationProfile
            {
                grip = Grip,
                index = Index,
                thumb = Thumb,
                gripPercentage = GripPercentage,
                indexPercentage = IndexPercentage,
                thumbState = ThumbState,
                thumbPercentage = ThumbPercentage,
                staticPose = StaticPose,
                pose = Pose
            };
            return profile;
        }
        // Resets the Controller to default values
        public void ResetController()
        {
            SetAnimatorWithProfile(defaultProfile);
            drivedByAnimation = defaultAnimationDrive;
        }

        #endregion
    }

 
}


