using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private BuildingManager buildingManager;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        if (inputActions.Player.Back.WasPressedThisFrame() && buildingManager.IsOpen)
        {
            buildingManager.CloseBuilding();
            return;
        }

        if (buildingManager.IsOpen) return;

        if (inputActions.Player.Click.WasPressedThisFrame())
            TrySelectBuilding();
    }

    private void TrySelectBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        if (!hit.collider.TryGetComponent(out Building building)) return;

        Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
        Vector3 center = building.transform.position;

        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer r in renderers)
                bounds.Encapsulate(r.bounds);
            center = bounds.center;
        }

        buildingManager.OpenBuilding(building, center);
    }
}
