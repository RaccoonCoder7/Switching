using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Kandooz.Burger
{

    public enum HandState
    {
        Free,
        Grabbing
    }
    public class HandInteraction : MonoBehaviour
    {
        public string GrabbingAxis;
        public XRNode handNode = XRNode.RightHand;
        public HandState handState;

        public float castRadius = 4;
        public Vector3 offset = Vector3.zero;
        public Transform sphereCastPos;

        private InteractableObject currentInteractable;

        private AnimationController currentHandAnimation;
        private XRNodeHandController currentHandController;


        void Awake()
        {
            handState = HandState.Free;
            currentHandAnimation = this.GetComponent<AnimationController>();
            currentHandController = this.GetComponentInParent<XRNodeHandController>();
            
        }

        private float distance = 0;
        private float newDis = 0;
        private int currentObjectIndex = 0;
        private List<InteractableObject> nearObjects;

        private InteractableObject intObj;
        private InteractableObject CurrentInteractable
        {
            get
            {
                return currentInteractable;
            }
            set
            {
                if(currentInteractable != value)
                {
                    if (currentInteractable != null) currentInteractable.OnUnHover();
                    currentInteractable = value;
                    if(currentInteractable != null) currentInteractable.OnHover(handNode);
                }

            }
        }

        #region MonoBehaviour Methods

        void Update()
        {
            if (handState == HandState.Free)
            {
                nearObjects = new List<InteractableObject>();
                //var colliders = Physics.OverlapSphere(this.transform.position + offset, castRadius);
                var colliders = Physics.OverlapSphere(sphereCastPos.position, castRadius);

                foreach (var collider in colliders)
                {
                    //Debug.Log("Object " + collider.name + " Dist= " + Vector3.Distance(this.transform.position, collider.transform.position));
                    intObj = collider.gameObject.GetComponent<InteractableObject>();
                    if (intObj != null)
                    {
                        if(intObj.inUse == false && (intObj.hoveredHand == XRNode.CenterEye || intObj.hoveredHand == handNode))
                        nearObjects.Add(intObj);
                    }
                }

                if (nearObjects.Count > 0)
                {
                    if (nearObjects.Count == 1) CurrentInteractable = nearObjects[0];
                    else
                    {
                        currentObjectIndex = 0;
                        distance = Vector3.Distance(this.transform.position, nearObjects[0].transform.position);
                        for (int i = 1; i < nearObjects.Count; i++)
                        {
                            newDis = Vector3.Distance(this.transform.position, nearObjects[i].transform.position);
                            if (newDis < distance)
                            {
                                distance = newDis;
                                currentObjectIndex = i;
                            }
                        }
                        CurrentInteractable = nearObjects[currentObjectIndex];

                    }
                }
                else
                {
                    CurrentInteractable = null;
                }
            }

            if (Input.GetAxis(GrabbingAxis) > 0.9f && handState == HandState.Free && CurrentInteractable != null )
            {
                handState = HandState.Grabbing;
                CurrentInteractable.OnUnHover();
                CurrentInteractable.OnInteract(handNode,this.gameObject, currentHandAnimation, currentHandController);
            }


            if (Input.GetAxis(GrabbingAxis) <= 0.9f && handState != HandState.Free )
            {
                if (CurrentInteractable != null)
                {
                    CurrentInteractable.OnUnInteract(handNode, this.gameObject, currentHandAnimation, currentHandController);
                }
                handState = HandState.Free;
            }

        }

        #endregion

        //private void Grab()
        //{
        //    originalParent = GrabbedObject.transform.parent;
        //    handState = HandState.Grabbing;
        //    GrabbedObject.transform.SetParent(this.gameObject.transform);
        //    GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;

        //    VelocityEstimator estimator = GrabbedObject.GetComponent<VelocityEstimator>();
        //    if(estimator == null) estimator = GrabbedObject.AddComponent<VelocityEstimator>();
        //    estimator.BeginEstimatingVelocity();

        //    GrabbedObjectDataReader dataReader = GrabbedObject.GetComponent<GrabbedObjectDataReader>();
        //    if (dataReader != null) dataReader.ApplyData(handNode, currentHandAnimationController, currentHandController);


        //}
        //private void UnGrab()
        //{
        //    handState = HandState.Free;      

        //    if (GrabbedObject == null) return;

        //    GrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        //    InheritVelocity();
        //    GrabbedObject.transform.SetParent(originalParent);
        //    GrabbedObject = null;

        //    if(currentHandController != null) currentHandController.active = true;
        //}

        //private void InheritVelocity()
        //{

        //    VelocityEstimator estimator = GrabbedObject.GetComponent<VelocityEstimator>();
        //    if (estimator != null)
        //    {
        //        estimator.FinishEstimatingVelocity();
        //        HandVelocity = estimator.GetVelocityEstimate();
        //        HandAngularVelocity = estimator.GetAngularVelocityEstimate();

        //        GrabbedObject.GetComponent<Rigidbody>().velocity = HandVelocity;
        //        GrabbedObject.GetComponent<Rigidbody>().angularVelocity = HandAngularVelocity;
        //    }
        //}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(this.transform.position + offset, castRadius);
            Gizmos.DrawWireSphere(sphereCastPos.position, castRadius);
        }

    }
}
