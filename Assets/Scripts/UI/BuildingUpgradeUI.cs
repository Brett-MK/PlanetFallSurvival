using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUpgradeUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI levelText;

    private Building currentBuilding;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
        closeButton.onClick.RemoveListener(OnCloseClicked);
    }

    public void SetBuilding(Building building)
    {
        currentBuilding = building;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentBuilding == null) return;

        levelText.text = $"Level: {currentBuilding.GetCurrentLevel()}";
        // You can add building name here if Building script tracks it
    }

    private void OnUpgradeClicked()
    {
        if (currentBuilding != null)
        {
            currentBuilding.UpgradeBuilding();
            UpdateUI();
        }
    }

    private void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }
}
