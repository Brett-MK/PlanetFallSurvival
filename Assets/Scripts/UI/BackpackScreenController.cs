using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data-binder for BackpackScreen. Hierarchy built by PlanetfallUIBuilder.
/// </summary>
public class BackpackScreenController : MonoBehaviour
{
    private void Awake()
    {
        var tf  = transform.Find("ScreenHeader/BackButton");
        var btn = tf != null ? tf.GetComponent<Button>() : null;
        if (btn != null) btn.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenType.Base));
    }
}
