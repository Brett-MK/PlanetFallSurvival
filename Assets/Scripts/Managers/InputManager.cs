using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }


    public event Action<Building, Vector3> OnBuildingSelected;
    public event Action OnBackPressed;

    private InputSystem_Actions inputActions;
    private bool _selectionActive;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Update()
    {
        if (inputActions.Player.Back.WasPressedThisFrame() && _selectionActive)
        {
            _selectionActive = false;
            OnBackPressed?.Invoke();
            return;
        }

        if (_selectionActive) return;

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

        _selectionActive = true;
        OnBuildingSelected?.Invoke(building, center);
    }
}
