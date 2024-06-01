using System.Collections;
using UnityEngine;
using UnityEngine.XR;
namespace Kandooz.Burger
{
    public class GrabbedObjectDataReader : MonoBehaviour
    {
        public GrabbedObjectData data;
        public bool active = false;
        public float speed = 20;
        private Vector3 targetPos;
        private Vector3 targetRot;
        private HandAnimationProfile targetAnimationProfile;

        public void ApplyData(XRNode handNode, AnimationController handAnimator, XRNodeHandController handController)
        {
            if (data != null)
            {
                switch (handNode)
                {

                    case XRNode.LeftHand:
                        if(data.useRotation)
                        this.transform.localEulerAngles = data.leftHandObjRotation;

                        active = data.usePosition;

                        targetPos = data.leftHandObjPosition;
                        targetRot = data.leftHandObjRotation;
                        if (handAnimator != null && data.useAnimationProfile)
                        {
                            if (handController != null) handController.active = false;
                            handAnimator.SetAnimatorWithProfile(data.leftHandProfile);
                        }

                        break;
                    case XRNode.RightHand:
                        this.transform.localEulerAngles = data.rightHandObjRotation;
                        active = true;
                        targetPos = data.rightHandObjPosition;
                        targetRot = data.rightHandObjRotation;
                        if (handAnimator != null && data.useAnimationProfile)
                        {
                            if(handController != null) handController.active = false;
                            handAnimator.SetAnimatorWithProfile(data.rightHandProfile);
                        }

                        break;

                    default:
                        Debug.Log("Wrong Node Sent");
                        break;
                }
            }
        }

        private void Start()
        {
            active = false;
        }
        private void Update()
        {
            if (!active) return;
            if(this.transform.localPosition != targetPos)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPos, Time.deltaTime * speed);
            }
   
        }
    }
}