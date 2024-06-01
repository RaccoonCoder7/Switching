using UnityEngine;

namespace Kandooz
{
    [CreateAssetMenu(fileName = "inputData", menuName = "InputData", order = 0)]
    public class InputData : ScriptableObject
    {
        public string rightThumbAxisHorizontal = "RightThumbAxis-H";
        public string rightThumbAxisVertical = "RightThumbAxis-V";
        public string leftThumbAxisHorizontal = "LeftThumbAxis-H";
        public string leftThumbAxisVertical = "LeftThumbAxis-V";
        public string rightTrigger = "TriggerRight";
        public string rightTriggerSnap = "TriggerRightSnap";
        public string leftTrigger = "TriggerLeft";
        public string leftTriggerSnap = "TriggerLeftSnap";
        public string rightGrip = "GripRight";
        public string rightGripSnap = "GripRightSnap";
        public string leftGrip = "GripLeft";
        public string leftGripSnap = "GripLeftSnap";
        [Space(15)]
        public KeyCode buttonA = KeyCode.JoystickButton0;
        public KeyCode buttonB = KeyCode.JoystickButton1;
        public KeyCode buttonX = KeyCode.JoystickButton2;
        public KeyCode buttonY = KeyCode.JoystickButton3;
        [Space(15)]
        public KeyCode leftStickPress = KeyCode.JoystickButton8;
        public KeyCode rightStickPress = KeyCode.JoystickButton9;
    }
}