using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Data-binder for BottomNav. Finds nav items by name, wires onClick,
/// and updates active state when UIManager.OnScreenChanged fires.
/// Hierarchy expected (built by PlanetfallUIBuilder):
///   BottomNav > BASE, HEROES, BATTLE, BACKPACK, ALLIANCE
///     each > IconWrap > NavIcon (Image), ActiveBar (Image)
///     each > NavLabel (TMP_Text)
/// </summary>
public class BottomNavController : MonoBehaviour
{
    static readonly Color ActiveColor   = Hex("#F4EEFF");
    static readonly Color InactiveColor = Hex("#8A7BB8");

    private struct NavItem
    {
        public ScreenType screen;
        public Image      icon;
        public Image      activeBar;
        public TMP_Text   label;
    }

    private readonly List<NavItem> _items = new();

    private static readonly (string name, ScreenType screen)[] Map =
    {
        ("BASE",     ScreenType.Base),
        ("HEROES",   ScreenType.Heroes),
        ("BATTLE",   ScreenType.Battle),
        ("BACKPACK", ScreenType.Backpack),
        ("ALLIANCE", ScreenType.Alliance),
    };

    private void Awake()
    {
        foreach (var m in Map)
        {
            var tf = transform.Find(m.name);
            if (tf == null) { Debug.LogWarning($"[BottomNav] Child '{m.name}' not found."); continue; }

            ScreenType captured = m.screen;
            var btn = tf.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(() => UIManager.Instance.ShowScreen(captured));

            var iconTf     = tf.Find("IconWrap/NavIcon");
            var activeBarTf = tf.Find("IconWrap/ActiveBar");
            var labelTf    = tf.Find("NavLabel");

            _items.Add(new NavItem
            {
                screen    = m.screen,
                icon      = iconTf      != null ? iconTf.GetComponent<Image>()       : null,
                activeBar = activeBarTf != null ? activeBarTf.GetComponent<Image>()  : null,
                label     = labelTf     != null ? labelTf.GetComponent<TMP_Text>()   : null,
            });
        }
    }

    private void OnEnable()
    {
        UIManager.OnScreenChanged += Refresh;
        if (UIManager.Instance != null) Refresh(UIManager.Instance.CurrentScreen);
    }

    private void OnDisable() => UIManager.OnScreenChanged -= Refresh;

    private void Refresh(ScreenType active)
    {
        foreach (var item in _items)
        {
            bool on = item.screen == active;
            Color c = on ? ActiveColor : InactiveColor;
            if (item.icon      != null) item.icon.color = c;
            if (item.label     != null) item.label.color = c;
            if (item.activeBar != null) item.activeBar.gameObject.SetActive(on);
        }
    }

    static Color Hex(string hex) { ColorUtility.TryParseHtmlString(hex, out Color c); return c; }
}
