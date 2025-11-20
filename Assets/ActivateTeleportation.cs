using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleportation : MonoBehaviour
{
    public GameObject leftTeleportation;

    public InputActionProperty leftAction;
    public InputActionProperty leftCancel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && leftAction.action.ReadValue<float>() > 0.1f);
    }
}
