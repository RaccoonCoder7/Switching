using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kandooz.Common
{
    public enum Axe
    {
        X,
        Y,
        Z
    }
    public class RotateAround : MonoBehaviour
    {


       
        Quaternion initialRotation;
        public void Start()
        {
            initialRotation = this.transform.localRotation;
            StartIt();

        }
        [SerializeField]float rotationSpeed;
        [SerializeField] Axe axe;
        
        public void Reset()
        {
            StopAllCoroutines();
            StartCoroutine(ResetRotation());


        }
        public void StartIt()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateRotation());
        }
        IEnumerator UpdateRotation()
        {
            while (true)
            {
                switch (axe)
                {
                    case Axe.X:
                        this.transform.localRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
                        break;
                    case Axe.Y:
                        this.transform.localRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

                        break;
                    case Axe.Z:
                        this.transform.localRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

                        break;
                    default:
                        break;
                }
                yield return null;
            }

        }
        IEnumerator ResetRotation()
        {
            float t = 0;
            var start = this.transform.localRotation;
            while (t < 1)
            {
                t += Time.deltaTime * 3;
                transform.localRotation = Quaternion.Lerp(start, initialRotation, t);
                yield return null;
            }
        }
    }
}