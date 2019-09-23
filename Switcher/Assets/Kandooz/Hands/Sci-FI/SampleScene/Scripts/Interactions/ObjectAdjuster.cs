using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine;
namespace Kandooz.Burger
{
    public class ObjectAdjuster : MonoBehaviour
    {

        public GameObject adjustableObject;
        public AnimationController handAnimationController;
        [Space(5)]
        public GrabbedObjectData dataFile;
        public XRNode hand = XRNode.RightHand;

        [Space(10)]
        public float rotationSpeed = 0.1f;
        public float moveSpeed = 0.1f;
        public float fingerIncDecRate = 0.05f;
        public float speedRate = 0.05f;
        [Header("Controls")]
        [Space(5)]
        public KeyCode moveForward = KeyCode.W;
        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode moveBackward = KeyCode.S;
        public KeyCode moveUp = KeyCode.E;
        public KeyCode moveDown = KeyCode.Q;
        [Space(10)]
        public KeyCode rotateAroundZ = KeyCode.Z;
        public KeyCode rotateAroundY = KeyCode.Y;
        public KeyCode rotateAroundX = KeyCode.X;
        public KeyCode toggleRotationDirection = KeyCode.C;
        [Space(10)]
        public KeyCode selectIndex = KeyCode.I;
        public KeyCode selectGrip = KeyCode.G;
        public KeyCode selectThumb = KeyCode.T;
        [Space(10)]
        public KeyCode enableDisableIndex = KeyCode.Keypad7;
        public KeyCode enableDisableGrip = KeyCode.Keypad4;
        public KeyCode enableDisableThumb = KeyCode.Keypad1;
        [Space(5)]
        public KeyCode increaseFinger = KeyCode.RightArrow;
        public KeyCode decreaseFinger = KeyCode.LeftArrow;
        [Space(10)]
        public KeyCode increaseSpeed = KeyCode.KeypadPlus;
        public KeyCode decreaseSpeed = KeyCode.KeypadMinus;
        [Space(20)]
        public KeyCode saveTransform = KeyCode.RightShift;
        public KeyCode saveHand = KeyCode.RightControl;
        [Space(20)]
        public Text currentFingerText;

        private float rotationDirection = 1;
        private int currentFinger = 0; // 0 Index , 1 Grip , 2 Thumb
        private void Update()
        {

            // Movement using the Parent Object (this object)
            if (Input.GetKey(moveForward))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.forward * moveSpeed;
            }
            if (Input.GetKey(moveBackward))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.back * moveSpeed;
            }
            if (Input.GetKey(moveRight))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.right * moveSpeed;
            }
            if (Input.GetKey(moveLeft))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.left * moveSpeed;
            }
            if (Input.GetKey(moveUp))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.up * moveSpeed;
            }
            if (Input.GetKey(moveDown))
            {
                this.transform.localPosition = this.transform.localPosition + Vector3.down * moveSpeed;
            }

            // Rotation rotating the object it self;
            if (Input.GetKeyDown(toggleRotationDirection))
            {
                rotationDirection *= -1;
            }
            if (Input.GetKey(rotateAroundX))
            {
                adjustableObject.transform.Rotate(Vector3.right * rotationDirection, rotationSpeed, Space.Self);
            }
            if (Input.GetKey(rotateAroundY))
            {
                adjustableObject.transform.Rotate(Vector3.up * rotationDirection, rotationSpeed, Space.Self);
            }
            if (Input.GetKey(rotateAroundZ))
            {
                adjustableObject.transform.Rotate(Vector3.forward * rotationDirection, rotationSpeed, Space.Self);
            }

            // Hand Setting
            if (handAnimationController != null)
            {
                if (Input.GetKeyDown(selectIndex))
                {
                    currentFinger = 0;
                }
                if (Input.GetKeyDown(selectGrip))
                {
                    currentFinger = 1;
                }
                if (Input.GetKeyDown(selectThumb))
                {
                    currentFinger = 2;
                }

                if (Input.GetKeyDown(enableDisableIndex))
                {
                    handAnimationController.Index = !handAnimationController.Index;
                }
                if (Input.GetKeyDown(enableDisableGrip))
                {
                    handAnimationController.Grip = !handAnimationController.Grip;
                }
                if (Input.GetKeyDown(enableDisableThumb))
                {
                    handAnimationController.Thumb = !handAnimationController.Thumb;
                }

                if (Input.GetKeyDown(increaseFinger))
                {
                    switch (currentFinger)
                    {
                        case 0:
                            handAnimationController.IndexPercentage = Mathf.Min(handAnimationController.IndexPercentage + fingerIncDecRate, 1);
                            break;
                        case 1:
                            handAnimationController.GripPercentage = Mathf.Min(handAnimationController.GripPercentage + fingerIncDecRate, 1);
                            break;                   
                        case 2:
                            handAnimationController.ThumbPercentage = Mathf.Min(handAnimationController.ThumbPercentage + fingerIncDecRate, 1);
                            break;
                    }
                }
                if (Input.GetKeyDown(decreaseFinger))
                {
                    switch (currentFinger)
                    {
                        case 0:
                            handAnimationController.IndexPercentage = Mathf.Max(handAnimationController.IndexPercentage - fingerIncDecRate, 0);
                            break;
                        case 1:
                            handAnimationController.GripPercentage = Mathf.Max(handAnimationController.GripPercentage - fingerIncDecRate, 0);
                            break;
                        case 2:
                            handAnimationController.ThumbPercentage = Mathf.Max(handAnimationController.ThumbPercentage - fingerIncDecRate, 0);
                            break;
                    }
                }
            }


            // Adjust Rotation Speed

            if (Input.GetKeyDown(increaseSpeed))
            {
                rotationSpeed = Mathf.Min(1, rotationSpeed + speedRate);
            } else
            if (Input.GetKeyDown(decreaseSpeed))
            {
                rotationSpeed = Mathf.Max(0, rotationSpeed - speedRate);
            }

            // Save Whats done

            if (Input.GetKeyDown(saveTransform))
            {
                SaveTransformDataInFile();
            }
            if (Input.GetKeyDown(saveHand))
            {
                SaveHandDataInFile();
            }
        }
        private void SetAsInData()
        {

        }
        private void SaveTransformDataInFile()
        {
            if(dataFile == null)
            {
                Debug.LogError("No Data File");
                return;
            }

            switch (hand)
            {
                case XRNode.LeftHand:
                    dataFile.leftHandObjPosition = this.transform.localPosition;
                    dataFile.leftHandObjRotation = adjustableObject.transform.localEulerAngles;
                    break;
                case XRNode.RightHand:
                    dataFile.rightHandObjPosition = this.transform.localPosition;
                    dataFile.rightHandObjRotation = adjustableObject.transform.localEulerAngles;
                    break;   
            }


        }
        private void SaveHandDataInFile()
        {
            if (dataFile == null)
            {
                Debug.LogError("No Data File");
                return;
            }
            switch (hand)
            {
                case XRNode.LeftHand:
                    dataFile.leftHandProfile = new HandAnimationProfile();

                    dataFile.leftHandProfile.grip = (handAnimationController.GripPercentage != 0);
                    dataFile.leftHandProfile.index = (handAnimationController.IndexPercentage != 0);
                    dataFile.leftHandProfile.thumb = (handAnimationController.ThumbPercentage != 0);

                    if (dataFile.leftHandProfile.grip) dataFile.leftHandProfile.gripPercentage = handAnimationController.GripPercentage;
                    if (dataFile.leftHandProfile.index) dataFile.leftHandProfile.indexPercentage = handAnimationController.IndexPercentage;
                    if (dataFile.leftHandProfile.thumb) dataFile.leftHandProfile.thumbPercentage = handAnimationController.ThumbPercentage;

                    break;
                case XRNode.RightHand:
                    dataFile.rightHandProfile = new HandAnimationProfile();

                    dataFile.rightHandProfile.grip = (handAnimationController.GripPercentage != 0);
                    dataFile.rightHandProfile.index = (handAnimationController.IndexPercentage != 0);
                    dataFile.rightHandProfile.thumb = (handAnimationController.ThumbPercentage != 0);

                    if (dataFile.rightHandProfile.grip) dataFile.rightHandProfile.gripPercentage = handAnimationController.GripPercentage;
                    if (dataFile.rightHandProfile.index) dataFile.rightHandProfile.indexPercentage = handAnimationController.IndexPercentage;
                    if (dataFile.rightHandProfile.thumb) dataFile.rightHandProfile.thumbPercentage = handAnimationController.ThumbPercentage;
                    break;
            }

        }

    }
}