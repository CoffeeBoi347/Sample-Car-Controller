using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetObj;
    public Vector3 cameraPos = new Vector3(0f, 0f, -10f);
    public float speed;
    private void Update()
    {
        Vector3 desiredPos = targetObj.transform.position + cameraPos;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, desiredPos, speed);
        transform.position = newPosition;
    }
}