using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float speed = 2.0f; // Speed of the movement
    public float height = 1.0f; // Amplitude of vertical movement
    public float width = 1.0f; // Amplitude of horizontal movement

    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position; // Store the initial position
    }

    void Update()
    {
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * speed) * height; // Vertical movement
        newPos.x += Mathf.Cos(Time.time * speed) * width; // Horizontal movement

        Debug.Log($"New Position: {newPos}"); // Log the new position

        if (rb != null)
        {
            rb.MovePosition(newPos); // Use Rigidbody to move
        }
        else
        {
            transform.position = newPos; // Update the position directly if no Rigidbody
        }
    }
}