using System;
using UnityEngine;

public enum ScreenType { Base, Heroes, Battle, Backpack, Alliance }

/// <summary>
/// Controls which screen panel is active. Attach to Main Canvas.
/// Wire the five screen GameObjects in the Inspector.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public static event Action<ScreenType> OnScreenChanged;

    [SerializeField] private GameObject baseScreen;
    [SerializeField] private GameObject heroesScreen;
    [SerializeField] private GameObject backpackScreen;
    [SerializeField] private GameObject battleScreen;
    [SerializeField] private GameObject allianceScreen;

    [Header("HUD — Base screen only")]
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject resourceStrip;

    public ScreenType CurrentScreen { get; private set; } = ScreenType.Base;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start() => ShowScreen(ScreenType.Base);

    public void ShowScreen(ScreenType screen)
    {
        baseScreen     .SetActive(screen == ScreenType.Base);
        heroesScreen   .SetActive(screen == ScreenType.Heroes);
        backpackScreen .SetActive(screen == ScreenType.Backpack);
        battleScreen   .SetActive(screen == ScreenType.Battle);
        allianceScreen .SetActive(screen == ScreenType.Alliance);

        bool isBase = screen == ScreenType.Base;
        if (topBar        != null) topBar       .SetActive(isBase);
        if (resourceStrip != null) resourceStrip.SetActive(isBase);

        CurrentScreen = screen;
        OnScreenChanged?.Invoke(screen);
        Canvas.ForceUpdateCanvases();
    }
}
