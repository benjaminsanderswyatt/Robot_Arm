using UnityEngine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    [System.Serializable]
    public class JointControl
    {
        public Transform joint;
        public Vector3 rotationAxis = Vector3.up;
        public float minAngle = -90f;
        public float maxAngle = 90f;
        public Slider controlSlider;

        [HideInInspector]
        public float currentAngle;
    }

    public JointControl baseControl;
    public JointControl joint1Control;
    public JointControl joint2Control;
    public JointControl joint3Control;

    void Start()
    {
        // Setup slider listeners
        SetupSlider(baseControl);
        SetupSlider(joint1Control);
        SetupSlider(joint2Control);
        SetupSlider(joint3Control);
    }

    void SetupSlider(JointControl jointControl)
    {
        if (jointControl.controlSlider != null)
        {
            jointControl.controlSlider.minValue = jointControl.minAngle;
            jointControl.controlSlider.maxValue = jointControl.maxAngle;
            jointControl.controlSlider.onValueChanged.AddListener((value) =>
            {
                jointControl.currentAngle = value;
                RotateJoint(jointControl);
            });
        }
    }

    void RotateJoint(JointControl jointControl)
    {
        if (jointControl.joint != null)
        {
            Quaternion rotation = Quaternion.AngleAxis(jointControl.currentAngle, jointControl.rotationAxis);
            jointControl.joint.localRotation = rotation;
        }
    }
}