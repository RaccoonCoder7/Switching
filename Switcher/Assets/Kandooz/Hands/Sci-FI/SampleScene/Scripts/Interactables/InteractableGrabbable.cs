//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Compenent added to grabbable object where it handle grabbing, ungrabbing, hovering, retrieving data regarding snaping position and custom grabbing animation
// and other cool things
//
//=============================================================================



using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace Kandooz.Burger
{
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableGrabbable : InteractableObject
    {
        public bool twoHanded = false; // If object can be grabbed by two hands, one of them is a decoy


        [Tooltip ("Add functions to be called when object grabbed")]
        public UnityEvent OnGrab;
        [Tooltip("Add functions to be called when object ungrabbed")]
        public UnityEvent OnUnGrab;
        [Tooltip("Add functions to be called when object is hovered")]
        public UnityEvent OnHandsOn;
        [Tooltip("Add functions to be called when object is unhovered")]
        public UnityEvent OnHandsOff;

        private Transform originalParent; //Original parent of grabbed object

        private AnimationController currentHandAnimation;  //Hand animation controller
        private XRNodeHandController currentHandController; // Hand Unity XR input controller

        private Vector3 HandVelocity;          // Velocity of hand
        private Vector3 HandAngularVelocity;   // Angular Velocity of hand

        private Rigidbody rb;                  // Grabbed object Rigidbody
        private GrabbedObjectDataReader dataReader;     //Grabbed object data, snapping position, custom animatoin

        public GameObject hoverIndicator;  // mesh enabled on hover

        public override void OnHover(XRNode hand = XRNode.CenterEye)
        {
            hoveredOn = true; // Is hovered by hoveredHand
            hoveredHand = hand; // Which hand is hovering first
            OnHandsOn.Invoke();

            if (hoverIndicator != null) hoverIndicator.SetActive(true);

        }
        public override void OnUnHover(XRNode hand = XRNode.CenterEye)
        {
            hoveredOn = true;
            hoveredHand = hand;

            OnHandsOff.Invoke();

            if (hoverIndicator != null) hoverIndicator.SetActive(false);
        }

        public override void OnInteract(XRNode hand, GameObject handObject = null, AnimationController handAnimation = null, XRNodeHandController handController = null)
        {
            inUse = true;

            currentHandController = handController;
            currentHandAnimation = handAnimation;

            this.transform.SetParent(handObject.transform);
            rb.isKinematic = true;

            VelocityEstimator estimator = this.GetComponent<VelocityEstimator>();
            if (estimator == null) estimator = this.gameObject.AddComponent<VelocityEstimator>();
            estimator.BeginEstimatingVelocity();

            if (dataReader != null) dataReader.ApplyData(hand, currentHandAnimation, currentHandController);

            OnGrab.Invoke();
        }


        public override void OnUnInteract(XRNode hand, GameObject handObject = null, AnimationController handAnimation = null, XRNodeHandController handController = null)
        {
            inUse = false;


            rb.isKinematic = false;

            this.transform.SetParent(originalParent);
            InheritVelocity();

            if (currentHandController != null) currentHandController.active = true;
            if (currentHandAnimation != null) currentHandAnimation.ResetController();
             currentHandAnimation = null;
            currentHandController = null;
            if (dataReader != null) dataReader.active = false;

            OnUnGrab.Invoke();
        }


        private void InheritVelocity()
        {

            VelocityEstimator estimator = this.GetComponent<VelocityEstimator>();
            if (estimator != null)
            {
                estimator.FinishEstimatingVelocity();
                HandVelocity = estimator.GetVelocityEstimate();
                HandAngularVelocity = estimator.GetAngularVelocityEstimate();

                rb.velocity = HandVelocity;
                rb.angularVelocity = HandAngularVelocity;
            }
        }

        private void Start()
        {
            originalParent = this.transform.parent;
            rb = this.GetComponent<Rigidbody>();
            hoveredHand = XRNode.CenterEye;
            dataReader = this.GetComponent<GrabbedObjectDataReader>();
        }
    }
}