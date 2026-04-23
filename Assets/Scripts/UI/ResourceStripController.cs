using UnityEngine;
using TMPro;

/// <summary>
/// Data-binder for ResourceStrip. Finds chip text elements by hierarchy path.
/// Hierarchy built by PlanetfallUIBuilder:
///   ResourceStrip > TimeChip / VipBadge / DiamondChip / EnergyChip
///     each > ChipText (TMP_Text)
/// </summary>
public class ResourceStripController : MonoBehaviour
{
    // Swap these for real data sources when ready
    [SerializeField] private string timeDisplay   = "UTC 03/09 · 03:42:52";
    [SerializeField] private int    vipLevel      = 12;
    [SerializeField] private long   diamonds      = 35961;
    [SerializeField] private string energyDisplay = "652M";

    private void Start()
    {
        SetText("TimeChip/ChipText",    timeDisplay);
        SetText("VipBadge/ChipText",    $"VIP {vipLevel}");
        SetText("DiamondChip/ChipText", diamonds.ToString("N0"));
        SetText("EnergyChip/ChipText",  energyDisplay);
    }

    void SetText(string path, string value)
    {
        var tf = transform.Find(path);
        if (tf == null) { Debug.LogWarning($"[ResourceStrip] '{path}' not found."); return; }
        var t = tf.GetComponent<TMP_Text>();
        if (t != null) t.text = value;
    }
}
