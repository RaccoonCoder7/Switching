using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Vector3 Front(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center + (boxCollider.transform.forward * boxCollider.bounds.extents.z);
    }
    public static Vector3 Back(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center - (boxCollider.transform.forward * boxCollider.bounds.extents.z);
    }
    public static Vector3 Left(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center - (boxCollider.transform.right * boxCollider.bounds.extents.x);
    }
    public static Vector3 Right(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center + (boxCollider.transform.right * boxCollider.bounds.extents.x);
    }
    public static Vector3 Top(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center + (boxCollider.transform.up * boxCollider.bounds.extents.y);
    }
    public static Vector3 Bottom(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.center - (boxCollider.transform.up * boxCollider.bounds.extents.y);
    }

    public static Vector3 Scaled(this Vector3 vector3, Vector3 other)
    {
        return new Vector3(vector3.x * other.x, vector3.y * other.y, vector3.z * other.z);
    }

    public static Vector3 Scaled(this Vector3 vector3, float scale)
    {
        return new Vector3(vector3.x * scale, vector3.y * scale, vector3.z * scale);
    }
}
