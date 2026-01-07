using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class TeleportOrb : MonoBehaviour
{
    private ActivateTeleportation teleportController;
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // Find teleport controller in scene
        //teleportController = FindObjectOfType<ActivateTeleportation>();
        teleportController =
            GameObject.FindGameObjectWithTag("Player")
              .GetComponent<ActivateTeleportation>();

    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (teleportController != null)
            teleportController.SetTeleportEnabled(true);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (teleportController != null)
            teleportController.SetTeleportEnabled(false);
    }
}
