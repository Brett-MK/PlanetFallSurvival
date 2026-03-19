using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Camera Bounds")]
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minZ = -50f;
    [SerializeField] private float maxZ = 50f;

    [Header("Camera Zoom")]
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float zoomedInSize = 3f;
    [SerializeField] private float zoomedInCameraHeight = 20f;

    private Camera cam;
    private Vector3 velocity;
    private float zoomVelocity;

    private InputSystem_Actions inputActions;
    private bool isDragging;
    private Vector3 dragOrigin;

    private Vector3 defaultPosition;
    private float defaultSize;
    private Quaternion defaultRotation;
    private bool isZoomedToBuilding;

    private Vector3 targetPosition;
    private float targetZoomSize;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        targetPosition = defaultPosition;
        targetZoomSize = defaultSize;

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        inputActions.Camera.Enable();
    }

    private void OnDisable()
    {
        inputActions.Camera.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize, ref zoomVelocity, smoothTime);

        if (!isZoomedToBuilding)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, Time.deltaTime * 5f);
            HandlePan();
            HandleZoom();
            HandleTouchZoom();
        }
    }

    private void HandlePan()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Two fingers = pinch zoom, not pan
        if (Touch.activeTouches.Count >= 2)
        {
            isDragging = false;
            return;
        }

        bool isPressed = inputActions.Camera.PanEnable.IsPressed();

        if (isPressed && !isDragging)
        {
            isDragging = true;
            dragOrigin = GetPointerWorldPosition();
        }

        if (isDragging && isPressed)
        {
            Vector3 currentPos = GetPointerWorldPosition();
            Vector3 move = dragOrigin - currentPos;
            move.y = 0;
            Vector3 newTarget = transform.position + move;

            newTarget.x = Mathf.Clamp(newTarget.x, minX, maxX);
            newTarget.z = Mathf.Clamp(newTarget.z, minZ, maxZ);
            newTarget.y = transform.position.y;

            targetPosition = newTarget;
        }

        if (!isPressed && isDragging)
            isDragging = false;
    }

    private void HandleZoom()
    {
        float scrollDelta = inputActions.Camera.Zoom.ReadValue<float>();

        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            float newZoom = targetZoomSize - scrollDelta * zoomSpeed * 0.1f;
            targetZoomSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }

    private void HandleTouchZoom()
    {
        var activeTouches = Touch.activeTouches;
        if (activeTouches.Count < 2)
            return;

        Vector2 pos0 = activeTouches[0].screenPosition;
        Vector2 pos1 = activeTouches[1].screenPosition;
        Vector2 delta0 = activeTouches[0].delta;
        Vector2 delta1 = activeTouches[1].delta;

        float currentDist = Vector2.Distance(pos0, pos1);
        float prevDist = Vector2.Distance(pos0 - delta0, pos1 - delta1);
        float pinchDelta = currentDist - prevDist;

        if (Mathf.Abs(pinchDelta) > 0.01f)
        {
            float newZoom = targetZoomSize - pinchDelta * zoomSpeed * 0.005f;
            targetZoomSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }

    private Vector3 GetPointerWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        ground.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }

    public void ZoomToBuilding(Vector3 worldPosition)
    {
        isZoomedToBuilding = true;
        isDragging = false;

        float camY = transform.position.y;
        Vector3 forward = transform.forward;
        float s = (worldPosition.y - camY) / forward.y;
        targetPosition = new Vector3(
            worldPosition.x - forward.x * s,
            camY,
            worldPosition.z - forward.z * s
        );
        targetZoomSize = zoomedInSize;
    }

    public void ZoomOut()
    {
        isZoomedToBuilding = false;
        targetPosition = defaultPosition;
        targetZoomSize = defaultSize;
        transform.rotation = defaultRotation;
    }
}
