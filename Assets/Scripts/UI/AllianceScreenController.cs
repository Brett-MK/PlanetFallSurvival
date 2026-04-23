using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data-binder for AllianceScreen. Hierarchy built by PlanetfallUIBuilder.
/// </summary>
public class AllianceScreenController : MonoBehaviour
{
    private void Awake()
    {
        WireBack("ScreenHeader/BackButton");
        WireLog("MenuGrid/MenuBtn_War",       "[Alliance] War");
        WireLog("MenuGrid/MenuBtn_Chests",    "[Alliance] Chests");
        WireLog("MenuGrid/MenuBtn_Territory", "[Alliance] Territory");
        WireLog("MenuGrid/MenuBtn_Battle",    "[Alliance] Battle");
        WireLog("MenuGrid/MenuBtn_Shop",      "[Alliance] Shop");
        WireLog("MenuGrid/MenuBtn_Tech",      "[Alliance] Tech");
        WireLog("MenuGrid/MenuBtn_Rankings",  "[Alliance] Rankings");
        WireLog("MenuGrid/MenuBtn_Help",      "[Alliance] Help");
    }

    void WireBack(string path)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenType.Base));
        else Debug.LogWarning($"[AllianceScreen] '{path}' not found.");
    }

    void WireLog(string path, string msg)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => Debug.Log(msg));
        else Debug.LogWarning($"[AllianceScreen] '{path}' not found.");
    }
}
