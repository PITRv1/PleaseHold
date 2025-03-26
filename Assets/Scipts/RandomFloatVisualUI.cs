using UnityEngine;

public class RandomFloatVisualUI : MonoBehaviour
{
    [SerializeField] private float xMovementRange = .45f; // Left-Right distance
    [SerializeField] private float yMovementRange = .2f; // Up-Down distance
    [SerializeField] private float xSpeed = 1f; // Speed for X Lerp
    [SerializeField] private float ySpeed = 0.25f; // Speed for Y Lerp

    private float startX;
    private float startY;
    private float timeElapsed; // Time counter for smooth movement

    private void Start()
    {
        // Store the initial position
        startX = transform.position.x;
        startY = transform.position.y;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Lerp X position (left to right)
        float newX = Mathf.Lerp(startX - xMovementRange, startX + xMovementRange, Mathf.PingPong(timeElapsed * xSpeed, 1));

        // Lerp Y position (up to down)
        float newY = Mathf.Lerp(startY - yMovementRange, startY + yMovementRange, Mathf.PingPong(timeElapsed * ySpeed, 1));

        // Apply the new position
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
