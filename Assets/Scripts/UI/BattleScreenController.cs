using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data-binder for BattleScreen. Hierarchy built by PlanetfallUIBuilder.
/// </summary>
public class BattleScreenController : MonoBehaviour
{
    private void Awake()
    {
        WireBack("ScreenHeader/BackButton");
        WireLog("ChapterPanel/ChapterInner/DeployButton", "[Battle] Deploy tapped");
    }

    void WireBack(string path)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenType.Base));
        else Debug.LogWarning($"[BattleScreen] '{path}' not found.");
    }

    void WireLog(string path, string msg)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => Debug.Log(msg));
        else Debug.LogWarning($"[BattleScreen] '{path}' not found.");
    }
}
