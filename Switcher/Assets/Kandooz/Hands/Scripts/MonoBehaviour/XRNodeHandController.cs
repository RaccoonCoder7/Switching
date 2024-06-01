//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Map unity XR input to hand animator
//
//=============================================================================


using UnityEngine;
using UnityEngine.SpatialTracking;
namespace Kandooz.Burger
{
    [RequireComponent(typeof(TrackedPoseDriver))]
    public class XRNodeHandController : AbstractHandController
    {

        // Axis to be created and assigned in the inspector
        public string leftIndexAxeName;     // Left controller trigger input, 9th axis for Oculus Rift
        public string leftGripAxeName;      // Left controller grip input, 11th axis for Oculus Rift
        public string rightIndexAxeName;    // Right controller trigger input, 10th axis for Oculus Rift
        public string rightGripAxeName;     // Right controller grip input, 12th axis for Oculus Rift

        public bool active = true;          // set active to false to stop mapping input to hand animation

        private TrackedPoseDriver pose;     // left||right controller


        private void Start()
        {
            pose = GetComponent<TrackedPoseDriver>();
        }
        private void Update()
        {
            if (!active) return;
            switch (pose.poseSource)
            {

                case TrackedPoseDriver.TrackedPose.LeftPose:
                    //why these aren't axis like the others ya bakr?
                    Index = Input.GetKey(KeyCode.JoystickButton14);
                    Thumb = Input.GetKey(KeyCode.JoystickButton8)|| Input.GetKey(KeyCode.JoystickButton16);

                    if (IsAxisAvailable(leftIndexAxeName))
                        IndexPercentage = Input.GetAxis(leftIndexAxeName);
                    if (IsAxisAvailable(leftGripAxeName))
                    {
                    
                        var grip = Input.GetAxis(leftGripAxeName);
                        GripPercentage = grip;
                        if (grip > .1f)
                        {
                            Grip = true;
                        }
                    }
                    break;

                case TrackedPoseDriver.TrackedPose.RightPose:
                    //w dool kaman ya bakr?

                    Index = Input.GetKey(KeyCode.JoystickButton15);
                    Thumb = Input.GetKey(KeyCode.JoystickButton9)|| Input.GetKey(KeyCode.JoystickButton17);

                    if (IsAxisAvailable(rightIndexAxeName))
                        IndexPercentage = Input.GetAxis(rightIndexAxeName);
                    if (IsAxisAvailable(rightGripAxeName))
                    {

                        var grip = Input.GetAxis(rightGripAxeName);
                        GripPercentage = grip;
                        if (grip > .1f)
                        {
                            Grip = true;
                        }
                    }

                    break;
                default:
                    break;
            }
        }


        private bool IsAxisAvailable(string axisName)
        {
            try
            {
                Input.GetAxis(axisName);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }

}