using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class ActivateTeleportation : MonoBehaviour
{
    public GameObject leftTeleportation;

    public InputActionProperty leftAction;
    public InputActionProperty leftCancel;

    public XRRayInteractor leftRay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeftHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNo, out bool leftValid);
        leftTeleportation.SetActive(!isLeftHovering && leftCancel.action.ReadValue<float>() == 0 && leftAction.action.ReadValue<float>() > 0.1f);
    }
}
