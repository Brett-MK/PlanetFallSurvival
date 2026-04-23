using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data-binder for HeroesScreen. Hierarchy built by PlanetfallUIBuilder.
/// </summary>
public class HeroesScreenController : MonoBehaviour
{
    private void Awake()
    {
        WireBack("ScreenHeader/BackButton", ScreenType.Base);
        WireLog("HeroBottomBar/DrillCampButton", "[Heroes] Drill Camp");
        WireLog("HeroBottomBar/RecruitButton",   "[Heroes] Recruit");
    }

    void WireBack(string path, ScreenType screen)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => UIManager.Instance.ShowScreen(screen));
    }

    void WireLog(string path, string msg)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => Debug.Log(msg));
    }
}
