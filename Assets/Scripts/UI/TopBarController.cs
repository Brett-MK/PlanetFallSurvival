using UnityEngine;
using TMPro;

/// <summary>
/// Data-binder for TopBar. Finds elements by hierarchy path and updates
/// display values. Hierarchy built by PlanetfallUIBuilder.
///
/// TopBar > AvatarButton > LevelBadge > LevelText
/// TopBar > CombatPowerGroup > CPRow > CPValue
/// </summary>
public class TopBarController : MonoBehaviour
{
    // Swap these for real data sources when ready
    [SerializeField] private int  playerLevel = 42;
    [SerializeField] private long combatPower = 813126736;

    private void Start()
    {
        SetText("AvatarButton/LevelBadge/LevelText", playerLevel.ToString());
        SetText("CombatPowerGroup/CPRow/CPValue",    combatPower.ToString("N0"));
    }

    void SetText(string path, string value)
    {
        var tf = transform.Find(path);
        if (tf == null) { Debug.LogWarning($"[TopBar] '{path}' not found."); return; }
        var t = tf.GetComponent<TMP_Text>();
        if (t != null) t.text = value;
    }
}
