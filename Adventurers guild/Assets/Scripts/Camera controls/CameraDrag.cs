using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float maxZoom = 5;
    public float minZoom = 20;
    public float sensitivity = 1;
    public float speed = 30;
    float targetZoom;

    void LateUpdate()
    {
        Zoom();

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        transform.Translate(move, Space.World);


    }

    void Zoom()
    {
        targetZoom -= Input.mouseScrollDelta.y * sensitivity;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, speed * Time.deltaTime);
        Camera.main.orthographicSize = newSize;
    }
}