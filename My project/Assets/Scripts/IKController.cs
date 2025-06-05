using UnityEngine;
using UnityEngine.UI;

public class IKController : MonoBehaviour
{
    [System.Serializable]
    public class JointLimit
    {
        public Transform joint;
        public Vector3 rotationAxis = Vector3.right;
        public float minAngle = -90f;
        public float maxAngle = 90f;
        public Slider controlSlider;
    }

    public JointLimit[] joints;
    public Transform endEffector;
    public Transform target;
    public int iterations = 10;
    public float threshold = 0.01f;

    void Start()
    {
        // Make sliders read-only
        foreach (var joint in joints)
        {
            if (joint.controlSlider != null)
            {
                joint.controlSlider.minValue = joint.minAngle;
                joint.controlSlider.maxValue = joint.maxAngle;
                joint.controlSlider.interactable = false;
            }
        }
    }

    void LateUpdate()
    {
        SolveIK();
        UpdateSliders();
    }

    void SolveIK()
    {
        if (Vector3.Distance(endEffector.position, target.position) < threshold)
            return;

        for (int i = 0; i < iterations; i++)
        {
            for (int j = joints.Length - 1; j >= 0; j--)
            {
                var jointLimit = joints[j];
                Transform joint = jointLimit.joint;

                Vector3 toEnd = endEffector.position - joint.position;
                Vector3 toTarget = target.position - joint.position;

                Quaternion rotation = Quaternion.FromToRotation(toEnd, toTarget);
                joint.rotation = rotation * joint.rotation;

                ClampJoint(jointLimit);
            }

            if (Vector3.Distance(endEffector.position, target.position) < threshold)
                break;
        }
    }

    void ClampJoint(JointLimit jointLimit)
    {
        Transform joint = jointLimit.joint;
        Vector3 axis = jointLimit.rotationAxis.normalized;

        Vector3 localEuler = joint.localEulerAngles;
        float angle = Vector3.Dot(axis, localEuler);
        angle = NormalizeAngle(angle);

        angle = Mathf.Clamp(angle, jointLimit.minAngle, jointLimit.maxAngle);

        Vector3 newEuler = Vector3.zero;
        if (axis == Vector3.right) newEuler.x = angle;
        if (axis == Vector3.up) newEuler.y = angle;
        if (axis == Vector3.forward) newEuler.z = angle;

        joint.localEulerAngles = newEuler;
    }

    void UpdateSliders()
    {
        foreach (var jointLimit in joints)
        {
            if (jointLimit.controlSlider == null || jointLimit.joint == null) continue;

            Vector3 axis = jointLimit.rotationAxis.normalized;
            float angle = Vector3.Dot(axis, jointLimit.joint.localEulerAngles);
            angle = NormalizeAngle(angle);
            jointLimit.controlSlider.value = Mathf.Clamp(angle, jointLimit.minAngle, jointLimit.maxAngle);
        }
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
