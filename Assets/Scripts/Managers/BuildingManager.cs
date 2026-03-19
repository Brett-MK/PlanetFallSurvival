using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private BuildingUpgradeUI buildingUpgradeUI;
    [SerializeField] private float buildingZoomSize = 3f;

    public bool IsOpen { get; private set; }
    public Building CurrentBuilding { get; private set; }

    public void OpenBuilding(Building building, Vector3 center)
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

    public void CloseBuilding()
    {
        IsOpen = false;
        CurrentBuilding = null;
        cameraController.ZoomOut();

        if (buildingUI != null)
            buildingUI.SetActive(false);
    }
}
