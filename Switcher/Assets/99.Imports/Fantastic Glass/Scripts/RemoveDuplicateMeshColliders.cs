using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveDuplicateMeshColliders : MonoBehaviour
{
    public GameObject objectToRemoveFrom;

    public static void RemoveDuplicatesFromObject(GameObject obj)
    {
        if (obj == null)
            return;
        //
        MeshCollider[] foundColliders = obj.GetComponentsInChildren<MeshCollider>();
        List<MeshCollider> testedColliders = new List<MeshCollider>();
        List<MeshCollider> collidersToDelete = new List<MeshCollider>();
        //
        for (int i = foundColliders.Length - 1; i >= 0; i--)
        {
            MeshCollider foundCollider = foundColliders[i];
            if (testedColliders.Contains(foundCollider))
            {
                collidersToDelete.Add(foundCollider);
            }
            testedColliders.Add(foundCollider);
        }
        //
        for (int i = collidersToDelete.Count - 1; i >= 0; i--)
        {
            Debug.Log("Remove mesh '" + collidersToDelete[i].name + "' from object '" + obj.name + "'.");
            Destroy(collidersToDelete[i]);
        }
    }

    void Start()
    {
        RemoveDuplicatesFromObject(objectToRemoveFrom);
    }
}
