using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private InputActionReference backAction;

    private Camera cam;
    private bool buildingOpen = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        clickAction.action.Enable();
        backAction.action.Enable();
    }

    private void OnDisable()
    {
        clickAction.action.Disable();
        backAction.action.Disable();
    }

    private void Update()
    {
        if (backAction.action.WasPressedThisFrame() && buildingOpen)
        {
            CloseBuilding();
            return;
        }

        if (buildingOpen) return;

        if (clickAction.action.WasPressedThisFrame())
        {
            Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Building building = hit.collider.GetComponent<Building>();
                if (building != null)
                    OpenBuilding(hit.collider.bounds.center);
            }
        }
    }

    private void OpenBuilding(Vector3 center)
    {
        buildingOpen = true;
        cameraController.ZoomToBuilding(center);
        if (buildingUI != null) buildingUI.SetActive(true);
    }

    private void CloseBuilding()
    {
        buildingOpen = false;
        cameraController.ZoomOut();
        if (buildingUI != null) buildingUI.SetActive(false);
    }
}
