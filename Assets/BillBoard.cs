using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target; // player or camera

    void LateUpdate()
    {
        if (target == null)
            target = Camera.main.transform;

        transform.LookAt(target);
        transform.Rotate(0, 180, 0); // TMP text faces backward by default
    }
}
