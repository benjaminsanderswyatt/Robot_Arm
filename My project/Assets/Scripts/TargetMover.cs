using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        // Horizontal plane movement
        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        // Vertical movement
        if (Input.GetKey(KeyCode.Space)) move += Vector3.up;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) move += Vector3.down;

        transform.position += move * moveSpeed * Time.deltaTime;
    }

}
