using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomedInSize = 10f;
    [SerializeField] private float zoomedInHeight = 60f;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera cam;
    private float defaultSize;
    private Vector3 defaultPosition;
    private float targetSize;
    private Vector3 targetPosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        defaultPosition = transform.position;
        targetSize = defaultSize;
        targetPosition = defaultPosition;
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }

    public void ZoomToBuilding(Vector3 worldPosition)
    {
        targetPosition = worldPosition - transform.forward * zoomedInHeight;
        targetSize = zoomedInSize;
    }

    public void ZoomOut()
    {
        targetPosition = defaultPosition;
        targetSize = defaultSize;
    }
}
