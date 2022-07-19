using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IKCalculator
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="bone1"></param>
    /// <param name="bone2"></param>
    /// <param name="foot"></param>
    /// <returns></returns>
    public static Vector3 GetElbowPoint(Transform parent, Transform bone1, Transform bone2, Vector3 target)
    {
        float d = Vector3.Distance(parent.position, target);
        float l1 = bone1.lossyScale.z;
        float l2 = bone2.lossyScale.z;

        // Calculate the vertical angle from the origingoing up to the elbow
        float phi = Mathf.Deg2Rad * 90 - Mathf.Acos(((l1 * l1) + (d * d) - (l2 * l2)) / (2 * l1 * d));

        if (float.IsNaN(phi)) phi = 0f;

        // Calculate the angle between forward and the target strictly on the XZ plane
        Vector3 targetAngle = target - parent.position;
        targetAngle.y = 0;
        float theta = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, targetAngle);
        float sign = (target.z < parent.position.z) ? -1.0f : 1.0f;
        theta = theta * sign;

        // Distance to the elbow
        float rho = l1;

        Vector3 ePointLocal = new Vector3(rho * Mathf.Sin(phi) * Mathf.Cos(theta), rho * Mathf.Cos(phi), rho * Mathf.Sin(phi) * Mathf.Sin(theta));

        Vector3 ePoint = ePointLocal + parent.position;

        Debug.DrawLine(parent.position, ePoint, Color.cyan);
        Debug.DrawLine(ePoint, target, Color.cyan);

        return ePoint;
    }
}
