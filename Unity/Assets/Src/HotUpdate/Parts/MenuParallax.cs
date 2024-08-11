using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = 0.3f;

    private Vector2 startPosition;
    private Vector2 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector2 offset = Camera.main.ScreenToViewportPoint(mousePos);
        Vector3 newPos = Vector2.SmoothDamp(transform.position, startPosition + offset * offsetMultiplier, ref velocity, smoothTime);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}