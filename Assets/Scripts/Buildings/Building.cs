using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int currentLevel = 3;
    [SerializeField] private Transform modelContainer;

    // Store child models for each level
    private GameObject[] levelModels;

    private void Awake()
    {
        // Find all child models (level_01_buildingName, level_02_buildingName, etc.)
        // They should be children of this building
        CacheLevelModels();
        UpdateModelDisplay();
    }

    private void CacheLevelModels()
    {
        // Get all children and sort them by name to ensure correct level ordering
        int childCount = transform.childCount;
        levelModels = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            levelModels[i] = transform.GetChild(i).gameObject;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void UpgradeBuilding()
    {
        currentLevel++;
        UpdateModelDisplay();
    }

    private void UpdateModelDisplay()
    {
        // Disable all models
        foreach (GameObject model in levelModels)
        {
            model.SetActive(false);
        }

        // Enable the correct level model (currentLevel is 1-indexed, array is 0-indexed)
        if (currentLevel - 1 < levelModels.Length)
        {
            levelModels[currentLevel - 1].SetActive(true);
        }
    }
}
