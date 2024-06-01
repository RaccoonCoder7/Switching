using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kandooz
{
    public class InputManager : MonoBehaviour
    {

        public InputData inputData;
        private static InputManager instance;

        bool leftGripDown;
        bool leftIndexDown;
        bool leftGripUp;
        bool leftIndexUp;


        bool rightGripDown;
        bool rightIndexDown;
        bool rightGripUp;
        bool rightIndexUp;

        bool leftGripDownSticky;
        bool leftIndexDownSticky;
        bool leftGripUpSticky;
        bool leftIndexUpSticky;


        bool rightGripDownSticky;
        bool rightIndexDownSticky;
        bool rightGripUpSticky;
        bool rightIndexUpSticky;


        

        public bool LeftGripDown
        {
            get
            {
                return leftGripDown;
            }
        }

        public bool GetLeftIndexDown()
        {
            return leftIndexDown;
        }

  
        
        public static InputManager Instance {
            get
            {
                return instance;
            }
        }

        private void Start()
        {
            if (instance == null) instance = this;
            if (inputData == null) inputData = Resources.Load<InputData>("inputData");
        }

        private void FixedUpdate()
        {
            ResetUpsDowns();


        }

        private void ResetUpsDowns()
        {
            leftGripDown = false;
            leftIndexDown = false;
            leftGripUp = false;
            leftIndexUp = false;
            rightGripDown = false;
            rightIndexDown = false;
            rightGripUp = false;
            rightIndexUp = false;
        }
    }
}