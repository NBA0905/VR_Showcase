using UnityEngine;

public class WristHUDVisibility : MonoBehaviour
{
    public Transform head;
    public float showAngle = 35f;

    void Update()
    {
        Vector3 toWrist = (transform.position - head.position).normalized;
        float angle = Vector3.Angle(head.forward, toWrist);

        gameObject.SetActive(angle < showAngle);
    }
}
