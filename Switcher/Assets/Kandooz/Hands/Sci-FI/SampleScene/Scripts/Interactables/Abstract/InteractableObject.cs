//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Abstract interactable object behaviours 
//
//=============================================================================



using UnityEngine.XR;
using UnityEngine;

namespace Kandooz.Burger
{

    public abstract class InteractableObject : MonoBehaviour
    {
        public bool inUse = false;
        public bool hoveredOn = false;
        public string interactButton;
        public XRNode hoveredHand; 

        public abstract void OnHover(XRNode hand = XRNode.CenterEye);
        public abstract void OnUnHover(XRNode hand = XRNode.CenterEye);
        public abstract void OnInteract(XRNode hand, GameObject handObject = null, AnimationController handAnimation = null, XRNodeHandController handController = null);
        public abstract void OnUnInteract(XRNode hand, GameObject handObject = null, AnimationController handAnimation = null, XRNodeHandController handController = null);
    }
}