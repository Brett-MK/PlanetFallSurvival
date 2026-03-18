using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject buildingUI;

    private Camera cam;
    private bool buildingOpen = false;
    private InputAction backAction;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        backAction = new InputAction("Back");
        backAction.AddBinding("<Keyboard>/escape");
        backAction.AddBinding("<Mouse>/rightButton");
        backAction.Enable();
    }

    private void OnDestroy()
    {
        backAction.Dispose();
    }

    private void Update()
    {
        if (backAction.WasPressedThisFrame() && buildingOpen)
        {
            CloseBuilding();
            return;
        }

        if (buildingOpen) return;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Building building = hit.collider.GetComponent<Building>();
                if (building != null)
                    OpenBuilding(hit.collider.bounds.center);
                else
                    Debug.Log("No Building component on hit object");
            }
            else
            {
                Debug.Log("Raycast missed");
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
