using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 followOffset;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + followOffset;
    }
}