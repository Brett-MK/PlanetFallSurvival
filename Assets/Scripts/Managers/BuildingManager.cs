using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private BuildingUpgradeUI buildingUpgradeUI;
    [SerializeField] private float buildingZoomSize = 3f;

    public bool IsOpen { get; private set; }
    public Building CurrentBuilding { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnBuildingSelected += OpenBuilding;
        InputManager.Instance.OnBackPressed += CloseBuilding;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnBuildingSelected -= OpenBuilding;
        InputManager.Instance.OnBackPressed -= CloseBuilding;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OpenBuilding(Building building, Vector3 center)
    {
        IsOpen = true;
        CurrentBuilding = building;
        cameraController.ZoomToBuilding(center, buildingZoomSize);

        if (buildingUI != null)
        {
            buildingUI.SetActive(true);
            if (buildingUpgradeUI != null)
                buildingUpgradeUI.SetBuilding(building);
        }
    }

    private void CloseBuilding()
    {
        IsOpen = false;
        CurrentBuilding = null;
        cameraController.ZoomOut();

        if (buildingUI != null)
            buildingUI.SetActive(false);
    }
}
