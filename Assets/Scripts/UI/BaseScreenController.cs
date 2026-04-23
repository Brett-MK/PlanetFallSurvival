using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data-binder for BaseScreen. Finds buttons by hierarchy path and wires onClick.
/// Hierarchy built by PlanetfallUIBuilder — no manual wiring needed.
/// </summary>
public class BaseScreenController : MonoBehaviour
{
    private void Awake()
    {
        Wire("FloatGroup_Left/QuestsButton",     "[Base] Quests");
        Wire("FloatGroup_Left/MailButton",       "[Base] Mail");
        Wire("FloatGroup_Right/ShieldButton",    "[Base] Shield");
        Wire("FloatGroup_Bottom/ResearchButton", "[Base] Research");
        Wire("FloatGroup_Bottom/RecruitButton",  "[Base] Recruit");
    }

    void Wire(string path, string logMsg)
    {
        var tf  = transform.Find(path);
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => Debug.Log(logMsg));
        else Debug.LogWarning($"[BaseScreen] '{path}' not found.");
    }
}
