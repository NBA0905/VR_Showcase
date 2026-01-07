using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportation : MonoBehaviour
{
    public GameObject leftTeleportation;

    public InputActionProperty leftAction;
    public InputActionProperty leftCancel;

    public XRRayInteractor leftRay;

    // Controlled by teleport orb
    private bool canTeleport = false;

    void Update()
    {
        if (!canTeleport)
        {
            leftTeleportation.SetActive(false);
            return;
        }

        bool isLeftHovering = leftRay.TryGetHitInfo(
            out Vector3 leftPos,
            out Vector3 leftNormal,
            out int leftNo,
            out bool leftValid
        );

        leftTeleportation.SetActive(
            !isLeftHovering &&
            leftCancel.action.ReadValue<float>() == 0 &&
            leftAction.action.ReadValue<float>() > 0.1f
        );
    }

    // Called by teleport orb
    public void SetTeleportEnabled(bool enabled)
    {
        canTeleport = enabled;
    }
}
