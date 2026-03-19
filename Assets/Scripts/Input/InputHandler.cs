using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private BuildingUpgradeUI buildingUpgradeUI;

    private Camera cam;
    private InputSystem_Actions inputActions;
    private bool buildingOpen;
    private Building currentBuilding;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        if (inputActions.Player.Back.WasPressedThisFrame() && buildingOpen)
        {
            CloseBuilding();
            return;
        }

        if (buildingOpen) return;

        if (inputActions.Player.Click.WasPressedThisFrame())
        {
            Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Building building = hit.collider.GetComponent<Building>();
                if (building != null)
                {
                    currentBuilding = building;

                    Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
                    Vector3 center = building.transform.position;

                    if (renderers.Length > 0)
                    {
                        Bounds bounds = renderers[0].bounds;
                        foreach (Renderer renderer in renderers)
                            bounds.Encapsulate(renderer.bounds);
                        center = bounds.center;
                    }

                    OpenBuilding(center);
                }
            }
        }
    }

    private void OpenBuilding(Vector3 center)
    {
        buildingOpen = true;
        cameraController.ZoomToBuilding(center);
        if (buildingUI != null)
        {
            buildingUI.SetActive(true);
            if (buildingUpgradeUI != null && currentBuilding != null)
                buildingUpgradeUI.SetBuilding(currentBuilding);
        }
    }

    private void CloseBuilding()
    {
        buildingOpen = false;
        cameraController.ZoomOut();
        if (buildingUI != null) buildingUI.SetActive(false);
    }
}
