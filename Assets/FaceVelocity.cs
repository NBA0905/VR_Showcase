using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceVelocity : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}