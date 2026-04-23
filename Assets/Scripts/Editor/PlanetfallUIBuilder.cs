#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// One-time editor script. Run via Planetfall > Build Complete UI.
/// Builds the entire Planetfall UI hierarchy inside Main Canvas.
/// Clears all existing Canvas children before building.
/// Delete this file after running if desired.
/// </summary>
public static class PlanetfallUIBuilder
{
    // ── Design tokens ─────────────────────────────────────────────────────────
    static readonly Color SpaceVoid    = H("#0A0618");
    static readonly Color SpaceBeam    = H("#8B6CFF");
    static readonly Color PanelBg      = H("#1A1248");
    static readonly Color BorderDim    = H("#452E99");
    static readonly Color Fg1          = H("#F4EEFF");
    static readonly Color Fg2          = H("#C6B8E8");
    static readonly Color Fg3          = H("#8A7BB8");
    static readonly Color Gold         = H("#FFC24A");
    static readonly Color GoldDark     = H("#3A1A00");
    static readonly Color Diamond      = H("#6DD5FF");
    static readonly Color Energy       = H("#7BE89B");
    static readonly Color Danger       = H("#FF5A6E");
    static readonly Color RarityLegend = H("#3D1F6E");
    static readonly Color RarityEpic   = H("#6B1F4A");
    static readonly Color RarityRare   = H("#1F3A5F");
    static readonly Color RarityCommon = H("#1A1248");

    // ── Entry point ───────────────────────────────────────────────────────────

    [MenuItem("Planetfall/Build Complete UI")]
    static void BuildAll()
    {
        var canvas = FindCanvas();
        if (canvas == null) { Debug.LogError("[Builder] No Canvas found in scene."); return; }

        // Clear existing children
        for (int i = canvas.transform.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(canvas.transform.GetChild(i).gameObject);

        // ── Root layers (order = render order, last = on top) ─────────────────
        var bg = MakeImg("Background", canvas.transform, SpaceVoid);
        Stretch(bg.rectTransform);
        bg.raycastTarget = false;

        var screens = MakeRT("Screens", canvas.transform); Stretch(screens);
        var hud     = MakeRT("HUD",     canvas.transform); Stretch(hud);

        // ── Screens ───────────────────────────────────────────────────────────
        var baseGo     = MakeScreen("BaseScreen",     screens, false);
        var heroesGo   = MakeScreen("HeroesScreen",   screens, true);
        var backpackGo = MakeScreen("BackpackScreen", screens, true);
        var battleGo   = MakeScreen("BattleScreen",   screens, true);
        var allianceGo = MakeScreen("AllianceScreen", screens, true);

        BuildBaseScreen(baseGo.transform);
        BuildHeroesScreen(heroesGo.transform);
        BuildBackpackScreen(backpackGo.transform);
        BuildBattleScreen(battleGo.transform);
        BuildAllianceScreen(allianceGo.transform);

        baseGo    .SetActive(true);
        heroesGo  .SetActive(false);
        backpackGo.SetActive(false);
        battleGo  .SetActive(false);
        allianceGo.SetActive(false);

        // ── HUD ───────────────────────────────────────────────────────────────
        var topBarGo        = BuildTopBar(hud);
        var resourceStripGo = BuildResourceStrip(hud);
        var bottomNavGo     = BuildBottomNav(hud);

        // Add runtime controller components
        if (topBarGo       .GetComponent<TopBarController>()        == null) topBarGo       .AddComponent<TopBarController>();
        if (resourceStripGo.GetComponent<ResourceStripController>() == null) resourceStripGo.AddComponent<ResourceStripController>();
        if (bottomNavGo    .GetComponent<BottomNavController>()     == null) bottomNavGo    .AddComponent<BottomNavController>();

        // ── Wire UIManager ────────────────────────────────────────────────────
        WireUIManager(canvas, baseGo, heroesGo, backpackGo, battleGo, allianceGo,
                      topBarGo, resourceStripGo);

        // Force layout rebuild so ContentSizeFitter calculates heights in edit mode
        Canvas.ForceUpdateCanvases();
        foreach (var csf in canvas.GetComponentsInChildren<ContentSizeFitter>(true))
            LayoutRebuilder.ForceRebuildLayoutImmediate(csf.GetComponent<RectTransform>());

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("[Builder] Done. Save the scene (Ctrl+S) to persist.");
    }

    // ── HUD: TopBar ───────────────────────────────────────────────────────────

    static GameObject BuildTopBar(RectTransform parent)
    {
        var bar = MakeImg("TopBar", parent, SpaceVoid);
        AnchorTop(bar.rectTransform, 0, 80);
        AddHLG(bar.gameObject, 12, 12, 12, 12, 10, TextAnchor.MiddleLeft);

        // Avatar
        var shell = MakeImg("AvatarButton", bar.transform, SpaceBeam);
        shell.raycastTarget = true;
        shell.gameObject.AddComponent<Button>();
        AddLE(shell.gameObject, pw: 56, ph: 56);

        var frame = MakeImg("Frame", shell.transform, PanelBg);
        Stretch(frame.rectTransform);
        frame.rectTransform.offsetMin = new Vector2(1.5f, 1.5f);
        frame.rectTransform.offsetMax = new Vector2(-1.5f, -1.5f);

        var avatarIcon = MakeImg("AvatarIcon", frame.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.4f));
        CenterAnchor(avatarIcon.rectTransform, 0, 4);
        avatarIcon.rectTransform.sizeDelta = new Vector2(32, 32);
        // ICON: character silhouette sprite

        var badge = MakeImg("LevelBadge", shell.transform, Gold);
        badge.rectTransform.anchorMin = badge.rectTransform.anchorMax = new Vector2(1, 0);
        badge.rectTransform.pivot     = new Vector2(1, 0);
        badge.rectTransform.anchoredPosition = new Vector2(-2, 2);
        badge.rectTransform.sizeDelta = new Vector2(22, 18);
        var badgeTmp = MakeTMP("LevelText", badge.transform, "42", 9, GoldDark);
        badgeTmp.fontStyle = FontStyles.Bold;
        Stretch(badgeTmp.rectTransform);
        // FONT: Barlow

        // Combat Power group
        var cpGo = MakeRT("CombatPowerGroup", bar.transform);
        AddLE(cpGo.gameObject, fw: 1, ph: 56);
        AddVLG(cpGo.gameObject, spacing: 2, align: TextAnchor.MiddleLeft, cw: true, ch: true, ew: true);

        var cpRow = new GameObject("CPRow", typeof(RectTransform));
        cpRow.transform.SetParent(cpGo, false);
        AddLE(cpRow, ph: 22);
        AddHLG(cpRow, spacing: 4, align: TextAnchor.MiddleLeft, cw: false, ch: false);

        var bolt = MakeImg("BoltIcon", cpRow.transform, Gold);
        bolt.rectTransform.sizeDelta = new Vector2(16, 16);
        AddLE(bolt.gameObject, pw: 16, ph: 16);

        var cpVal = MakeTMP("CPValue", cpRow.transform, "813,126,736", 17, Gold, TextAlignmentOptions.MidlineLeft);
        cpVal.fontStyle = FontStyles.Bold;
        cpVal.enableWordWrapping = false;
        // FONT: Orbitron

        var cpLabel = MakeTMP("CPLabel", cpGo, "COMBAT POWER", 9, Fg3, TextAlignmentOptions.MidlineLeft);
        cpLabel.characterSpacing = 1f;
        AddLE(cpLabel.gameObject, ph: 12);
        // FONT: Barlow

        // Right buttons
        var rightGroup = MakeRT("TopRightButtons", bar.transform);
        AddLE(rightGroup.gameObject, ph: 56);
        AddHLG(rightGroup.gameObject, spacing: 8, align: TextAnchor.MiddleRight);

        // Shop button
        var shopImg = MakeImg("ShopButton", rightGroup, Gold);
        shopImg.raycastTarget = true;
        shopImg.gameObject.AddComponent<Button>();
        AddLE(shopImg.gameObject, pw: 44, ph: 44);
        var cartIcon = MakeImg("CartIcon", shopImg.transform, GoldDark);
        cartIcon.rectTransform.sizeDelta = new Vector2(22, 22);
        CenterAnchor(cartIcon.rectTransform);
        // ICON: cart_icon

        // Settings button
        var settShell = MakeImg("SettingsButton", rightGroup, SpaceBeam);
        settShell.raycastTarget = true;
        settShell.gameObject.AddComponent<Button>();
        AddLE(settShell.gameObject, pw: 44, ph: 44);
        var settInner = MakeImg("Inner", settShell.transform, PanelBg);
        Stretch(settInner.rectTransform);
        settInner.rectTransform.offsetMin = new Vector2(1, 1);
        settInner.rectTransform.offsetMax = new Vector2(-1, -1);
        var gearIcon = MakeImg("GearIcon", settShell.transform, Fg2);
        gearIcon.rectTransform.sizeDelta = new Vector2(20, 20);
        CenterAnchor(gearIcon.rectTransform);
        // ICON: settings_icon

        return bar.gameObject;
    }

    // ── HUD: ResourceStrip ────────────────────────────────────────────────────

    static GameObject BuildResourceStrip(RectTransform parent)
    {
        var strip = MakeImg("ResourceStrip", parent, SpaceVoid);
        AnchorTop(strip.rectTransform, 80, 44);
        AddHLG(strip.gameObject, 8, 8, 6, 6, 5, TextAnchor.MiddleLeft);

        // Border bottom
        var border = MakeImg("BorderBottom", strip.transform, BorderDim);
        border.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        border.rectTransform.anchorMin = Vector2.zero;
        border.rectTransform.anchorMax = new Vector2(1, 0);
        border.rectTransform.pivot     = new Vector2(0.5f, 0);
        border.rectTransform.anchoredPosition = Vector2.zero;
        border.rectTransform.sizeDelta = new Vector2(0, 1);

        BuildChip("TimeChip",    strip.transform, 128, "UTC 03/09 · 03:42:52", Fg3,     Fg3);
        BuildChip("VipBadge",    strip.transform,  54, "VIP 12",               Gold,    H("#C07820"), bold: true);
        BuildChip("DiamondChip", strip.transform,  70, "35,961",               Diamond, Diamond);
        BuildChip("EnergyChip",  strip.transform,  58, "652M",                 Energy,  Energy);

        return strip.gameObject;
    }

    static void BuildChip(string name, Transform parent, float width, string text, Color textColor, Color iconColor, bool bold = false)
    {
        var chip = MakeImg(name, parent, PanelBg);
        AddLE(chip.gameObject, pw: width, ph: 28);
        AddHLG(chip.gameObject, 6, 6, 0, 0, 4, TextAnchor.MiddleLeft, cw: true, ch: true);

        var icon = MakeImg("Icon", chip.transform, iconColor);
        icon.rectTransform.sizeDelta = new Vector2(12, 12);
        AddLE(icon.gameObject, pw: 12, ph: 12);

        var tmp = MakeTMP("ChipText", chip.transform, text, 10, textColor, TextAlignmentOptions.MidlineLeft);
        if (bold) tmp.fontStyle = FontStyles.Bold;
        // FONT: Barlow
    }

    // ── HUD: BottomNav ────────────────────────────────────────────────────────

    static GameObject BuildBottomNav(RectTransform parent)
    {
        var nav = MakeImg("BottomNav", parent, PanelBg);
        AnchorBottom(nav.rectTransform, 80);

        // Border top
        var border = MakeImg("BorderTop", nav.transform, BorderDim);
        border.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        border.rectTransform.anchorMin = new Vector2(0, 1);
        border.rectTransform.anchorMax = Vector2.one;
        border.rectTransform.pivot     = new Vector2(0.5f, 1);
        border.rectTransform.anchoredPosition = Vector2.zero;
        border.rectTransform.sizeDelta = new Vector2(0, 1);

        AddHLG(nav.gameObject, 0, 0, 0, 0, 0, TextAnchor.MiddleCenter, cw: true, ch: true, ew: true, eh: true);

        var tabs = new (string label, int badge)[]
        {
            ("BASE", 0), ("HEROES", 2), ("BATTLE", 0), ("BACKPACK", 99), ("ALLIANCE", 0),
        };

        bool first = true;
        foreach (var tab in tabs)
        {
            BuildNavItem(nav.transform, tab.label, tab.badge, first);
            first = false;
        }

        return nav.gameObject;
    }

    static void BuildNavItem(Transform parent, string label, int badge, bool active)
    {
        var root = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
        root.transform.SetParent(parent, false);
        root.GetComponent<Image>().color = Color.clear;

        var rootVLG = root.AddComponent<VerticalLayoutGroup>();
        rootVLG.childAlignment = TextAnchor.MiddleCenter;
        rootVLG.spacing = 4;
        rootVLG.padding = new RectOffset(0, 0, 8, 8);
        rootVLG.childControlWidth = true; rootVLG.childControlHeight = true;
        rootVLG.childForceExpandWidth = false; rootVLG.childForceExpandHeight = false;

        // Active bg pill
        if (active)
        {
            var pill = MakeImg("ActivePill", root.transform, H("#2A1B6E"));
            pill.raycastTarget = false;
            pill.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
            CenterAnchor(pill.rectTransform);
            pill.rectTransform.sizeDelta = new Vector2(44, 44);
        }

        // IconWrap (32x32)
        var wrap = new GameObject("IconWrap", typeof(RectTransform));
        wrap.transform.SetParent(root.transform, false);
        AddLE(wrap, pw: 32, ph: 32, fw: 0);

        // ActiveBar (gold strip, anchored bottom of wrap)
        var bar = MakeImg("ActiveBar", wrap.transform, Gold);
        bar.raycastTarget = false;
        bar.rectTransform.anchorMin = new Vector2(0.5f, 0);
        bar.rectTransform.anchorMax = new Vector2(0.5f, 0);
        bar.rectTransform.pivot     = new Vector2(0.5f, 0);
        bar.rectTransform.anchoredPosition = Vector2.zero;
        bar.rectTransform.sizeDelta = new Vector2(20, 4);
        bar.gameObject.SetActive(active);

        // Icon
        var icon = MakeImg("NavIcon", wrap.transform, active ? Fg1 : Fg3);
        icon.preserveAspect = true;
        icon.raycastTarget  = false;
        Stretch(icon.rectTransform);
        icon.rectTransform.offsetMin = new Vector2(3, 6);
        icon.rectTransform.offsetMax = new Vector2(-3, -3);
        // ICON: assign base_icon / heroes_icon / battle_icon / backpack_icon / alliance_icon

        // Badge
        if (badge > 0)
        {
            var badgeBg = MakeImg("Badge", wrap.transform, Danger);
            badgeBg.raycastTarget = false;
            badgeBg.rectTransform.anchorMin = badgeBg.rectTransform.anchorMax = new Vector2(1, 1);
            badgeBg.rectTransform.pivot     = new Vector2(1, 1);
            badgeBg.rectTransform.anchoredPosition = new Vector2(6, 6);
            badgeBg.rectTransform.sizeDelta = new Vector2(16, 16);
            string badgeStr = badge > 99 ? "99+" : badge.ToString();
            var badgeTmp = MakeTMP("BadgeText", badgeBg.transform, badgeStr, 8, Color.white);
            badgeTmp.fontStyle = FontStyles.Bold;
            Stretch(badgeTmp.rectTransform);
            // FONT: Barlow
        }

        // Label
        var lbl = MakeTMP("NavLabel", root.transform, label, 9, active ? Fg1 : Fg3);
        lbl.fontStyle = FontStyles.Bold;
        AddLE(lbl.gameObject, ph: 11);
        // FONT: Barlow
    }

    // ── BaseScreen ────────────────────────────────────────────────────────────

    static void BuildBaseScreen(Transform parent)
    {
        // FloatGroup_Left
        var left = new GameObject("FloatGroup_Left", typeof(RectTransform));
        left.transform.SetParent(parent, false);
        var leftRT = left.GetComponent<RectTransform>();
        leftRT.anchorMin = new Vector2(0, 0.5f);
        leftRT.anchorMax = new Vector2(0, 0.5f);
        leftRT.pivot     = new Vector2(0, 0.5f);
        leftRT.anchoredPosition = new Vector2(12, 0);
        leftRT.sizeDelta = new Vector2(52, 160);
        AddVLG(left, spacing: 8, align: TextAnchor.UpperLeft, cw: true, ch: true);

        BuildFloatButton("QuestsButton", left.transform, Gold,    "QUESTS");
        BuildFloatButton("MailButton",   left.transform, Diamond, "MAIL");

        // FloatGroup_Right
        var right = new GameObject("FloatGroup_Right", typeof(RectTransform));
        right.transform.SetParent(parent, false);
        var rightRT = right.GetComponent<RectTransform>();
        rightRT.anchorMin = new Vector2(1, 0.5f);
        rightRT.anchorMax = new Vector2(1, 0.5f);
        rightRT.pivot     = new Vector2(1, 0.5f);
        rightRT.anchoredPosition = new Vector2(-12, 0);
        rightRT.sizeDelta = new Vector2(52, 80);
        AddVLG(right, spacing: 8, align: TextAnchor.UpperRight, cw: true, ch: true);

        BuildFloatButton("ShieldButton", right.transform, Energy, "SHIELD");

        // FloatGroup_Bottom
        var bottom = new GameObject("FloatGroup_Bottom", typeof(RectTransform));
        bottom.transform.SetParent(parent, false);
        var bottomRT = bottom.GetComponent<RectTransform>();
        bottomRT.anchorMin = new Vector2(0.5f, 0);
        bottomRT.anchorMax = new Vector2(0.5f, 0);
        bottomRT.pivot     = new Vector2(0.5f, 0);
        bottomRT.anchoredPosition = new Vector2(0, 100);
        bottomRT.sizeDelta = new Vector2(280, 44);
        AddHLG(bottom, spacing: 8, align: TextAnchor.MiddleCenter, cw: true, ch: true);

        // Research — ghost button
        var resShell = MakeImg("ResearchButton", bottom.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.35f));
        resShell.raycastTarget = true;
        resShell.gameObject.AddComponent<Button>();
        AddLE(resShell.gameObject, ph: 40, fw: 1);
        var resInner = MakeImg("Inner", resShell.transform, new Color(SpaceVoid.r, SpaceVoid.g, SpaceVoid.b, 0.8f));
        Stretch(resInner.rectTransform);
        resInner.rectTransform.offsetMin = new Vector2(1, 1);
        resInner.rectTransform.offsetMax = new Vector2(-1, -1);
        MakeTMP("Label", resShell.transform, "RESEARCH", 11, Fg2).SetAllDirty();
        // FONT: Barlow

        // Recruit — gold button
        var recBtn = MakeImg("RecruitButton", bottom.transform, Gold);
        recBtn.raycastTarget = true;
        recBtn.gameObject.AddComponent<Button>();
        AddLE(recBtn.gameObject, ph: 40, fw: 1);
        var recLabel = MakeTMP("Label", recBtn.transform, "RECRUIT", 11, GoldDark);
        recLabel.fontStyle = FontStyles.Bold;
        Stretch(recLabel.rectTransform);
        // FONT: Barlow
    }

    static void BuildFloatButton(string name, Transform parent, Color accent, string labelText)
    {
        // Outer = border color at low opacity
        var shell = MakeImg(name, parent, new Color(accent.r, accent.g, accent.b, 0.25f));
        shell.raycastTarget = true;
        shell.gameObject.AddComponent<Button>();
        AddLE(shell.gameObject, pw: 52, ph: 52);

        var inner = MakeImg("Inner", shell.transform, PanelBg);
        Stretch(inner.rectTransform);
        inner.rectTransform.offsetMin = new Vector2(1.5f, 1.5f);
        inner.rectTransform.offsetMax = new Vector2(-1.5f, -1.5f);

        var icon = MakeImg("Icon", shell.transform, accent);
        icon.rectTransform.sizeDelta = new Vector2(22, 22);
        CenterAnchor(icon.rectTransform, 0, 4);
        // ICON: assign quest/mail/shield sprite

        var lbl = MakeTMP("Label", shell.transform, labelText, 8, accent);
        lbl.rectTransform.anchorMin = new Vector2(0, 0);
        lbl.rectTransform.anchorMax = new Vector2(1, 0);
        lbl.rectTransform.pivot     = new Vector2(0.5f, 0);
        lbl.rectTransform.anchoredPosition = new Vector2(0, 5);
        lbl.rectTransform.sizeDelta = new Vector2(0, 12);
        // FONT: Barlow
    }

    // ── HeroesScreen ──────────────────────────────────────────────────────────

    static void BuildHeroesScreen(Transform parent)
    {
        BuildScreenHeader(parent, "Heroes", true, true);

        // Bottom bar
        var bottomBar = MakeImg("HeroBottomBar", parent, SpaceVoid);
        AnchorBottom(bottomBar.rectTransform, 56);
        AddHLG(bottomBar.gameObject, 12, 12, 6, 6, 8, TextAnchor.MiddleCenter, cw: true, ch: true);

        var drillShell = MakeImg("DrillCampButton", bottomBar.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.3f));
        drillShell.raycastTarget = true;
        drillShell.gameObject.AddComponent<Button>();
        AddLE(drillShell.gameObject, ph: 44, fw: 1);
        var drillInner = MakeImg("Inner", drillShell.transform, PanelBg);
        Stretch(drillInner.rectTransform);
        drillInner.rectTransform.offsetMin = new Vector2(1, 1);
        drillInner.rectTransform.offsetMax = new Vector2(-1, -1);
        var drillLabel = MakeTMP("Label", drillShell.transform, "DRILL CAMP", 12, Fg2);
        drillLabel.fontStyle = FontStyles.Bold;
        Stretch(drillLabel.rectTransform);
        // FONT: Barlow

        var recBtn = MakeImg("RecruitButton", bottomBar.transform, Gold);
        recBtn.raycastTarget = true;
        recBtn.gameObject.AddComponent<Button>();
        AddLE(recBtn.gameObject, ph: 44, fw: 1);
        var recLabel = MakeTMP("Label", recBtn.transform, "RECRUIT", 12, GoldDark);
        recLabel.fontStyle = FontStyles.Bold;
        Stretch(recLabel.rectTransform);
        // FONT: Barlow

        // ScrollView (below 56px header, above 56px bottom bar)
        var sr = BuildScrollView("HeroScrollView", parent, 56, 56);
        var content = sr.content.gameObject;

        var grid = content.AddComponent<GridLayoutGroup>();
        grid.cellSize        = new Vector2(72, 110);
        grid.spacing         = new Vector2(8, 8);
        grid.padding         = new RectOffset(12, 12, 12, 12);
        grid.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.startCorner     = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis       = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment  = TextAnchor.UpperLeft;
        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var heroes = new (string name, string rarity)[]
        {
            ("Voss","Legendary"),("Lyra","Legendary"),("Doran","Legendary"),("Kael","Legendary"),
            ("Mira","Rare"),    ("Ven","Rare"),      ("Torin","Epic"),     ("Zara","Epic"),
            ("Rook","Rare"),    ("Pax","Legendary"), ("Greaves","Rare"),   ("Soren","Epic"),
            ("Ada","Rare"),     ("Nox","Rare"),      ("Rhee","Rare"),      ("Brond","Legendary"),
        };
        foreach (var h in heroes) BuildHeroCard(content.transform, h.name, h.rarity);
    }

    static void BuildHeroCard(Transform parent, string heroName, string rarity)
    {
        Color pc    = RarityColor(rarity);
        string rank = rarity == "Legendary" ? "S" : rarity == "Epic" ? "A" : "R";
        int stars   = rarity == "Legendary" ? 5 : rarity == "Epic" ? 4 : 3;

        var card = MakeImg(heroName, parent, pc);

        // Portrait silhouette (centered upper half)
        var sil = MakeImg("Portrait", card.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.4f));
        sil.rectTransform.anchorMin = new Vector2(0.25f, 0.38f);
        sil.rectTransform.anchorMax = new Vector2(0.75f, 0.82f);
        sil.rectTransform.offsetMin = sil.rectTransform.offsetMax = Vector2.zero;

        // Rank badge (top-right)
        var rankBg = MakeImg("RankBadge", card.transform, Gold);
        rankBg.rectTransform.anchorMin = rankBg.rectTransform.anchorMax = new Vector2(1, 1);
        rankBg.rectTransform.pivot     = new Vector2(1, 1);
        rankBg.rectTransform.anchoredPosition = new Vector2(-4, -4);
        rankBg.rectTransform.sizeDelta = new Vector2(20, 16);
        var rankTmp = MakeTMP("RankText", rankBg.transform, rank, 9, GoldDark);
        rankTmp.fontStyle = FontStyles.Bold;
        Stretch(rankTmp.rectTransform);
        // FONT: Barlow

        // Meta strip (bottom 38%)
        var meta = MakeImg("Meta", card.transform, new Color(0.04f, 0.02f, 0.09f, 0.9f));
        meta.rectTransform.anchorMin = Vector2.zero;
        meta.rectTransform.anchorMax = new Vector2(1, 0.38f);
        meta.rectTransform.offsetMin = meta.rectTransform.offsetMax = Vector2.zero;
        AddVLG(meta.gameObject, 4, 4, 4, 4, 2, TextAnchor.MiddleCenter, cw: true, ch: false, ew: true);

        var nameTmp = MakeTMP("HeroName", meta.transform, heroName, 8, Fg1);
        nameTmp.fontStyle = FontStyles.Bold;
        AddLE(nameTmp.gameObject, ph: 11);
        // FONT: Barlow

        var lvlTmp = MakeTMP("LevelText", meta.transform, "Lv.80", 8, Fg2);
        AddLE(lvlTmp.gameObject, ph: 10);
        // FONT: Barlow

        // Stars row
        var starsGo = new GameObject("StarRow", typeof(RectTransform));
        starsGo.transform.SetParent(meta.transform, false);
        AddHLG(starsGo, spacing: 2, align: TextAnchor.MiddleCenter, cw: false, ch: false);
        AddLE(starsGo, ph: 11);
        for (int i = 0; i < 5; i++)
        {
            var star = MakeImg($"Star{i}", starsGo.transform, i < stars ? Gold : H("#2A2060"));
            star.rectTransform.sizeDelta = new Vector2(10, 10);
        }
    }

    // ── BackpackScreen ────────────────────────────────────────────────────────

    static void BuildBackpackScreen(Transform parent)
    {
        BuildScreenHeader(parent, "Backpack", true, false);

        // Category tabs row (anchored below 56px header)
        var tabs = MakeImg("CategoryTabs", parent, SpaceVoid);
        tabs.rectTransform.anchorMin = new Vector2(0, 1);
        tabs.rectTransform.anchorMax = new Vector2(1, 1);
        tabs.rectTransform.pivot     = new Vector2(0.5f, 1);
        tabs.rectTransform.anchoredPosition = new Vector2(0, -56);
        tabs.rectTransform.sizeDelta = new Vector2(0, 40);
        AddHLG(tabs.gameObject, 8, 8, 6, 6, 6, TextAnchor.MiddleLeft, cw: false, ch: false);

        var tabBorder = MakeImg("BorderBottom", tabs.transform, BorderDim);
        tabBorder.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        tabBorder.rectTransform.anchorMin = Vector2.zero;
        tabBorder.rectTransform.anchorMax = new Vector2(1, 0);
        tabBorder.rectTransform.pivot     = new Vector2(0.5f, 0);
        tabBorder.rectTransform.anchoredPosition = Vector2.zero;
        tabBorder.rectTransform.sizeDelta = new Vector2(0, 1);

        var tabNames = new[] { "Resources", "Speedup", "Bonus", "Gear", "Other" };
        bool firstTab = true;
        foreach (var t in tabNames)
        {
            var tabBtn = MakeImg($"Tab_{t}", tabs.transform, firstTab ? H("#2A1B6E") : PanelBg);
            tabBtn.raycastTarget = true;
            tabBtn.gameObject.AddComponent<Button>();
            AddLE(tabBtn.gameObject, ph: 28, pw: t.Length * 7 + 16);
            MakeTMP("Label", tabBtn.transform, t, 10, firstTab ? Fg2 : Fg3);
            // FONT: Barlow
            firstTab = false;
        }

        // ScrollView (offset top = 56 header + 40 tabs = 96)
        var sr = BuildScrollView("ItemScrollView", parent, 96, 0);
        var content = sr.content.gameObject;

        var grid = content.AddComponent<GridLayoutGroup>();
        grid.cellSize        = new Vector2(72, 80);
        grid.spacing         = new Vector2(8, 8);
        grid.padding         = new RectOffset(12, 12, 12, 12);
        grid.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.childAlignment  = TextAnchor.UpperLeft;
        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        string[] rarities  = { "Legendary", "Epic", "Rare", "Common" };
        int[]    qtys      = { 12,5,3,1,25,8,50,2,7,3,1,14,6,99,2,11,4,8,1,33,7,2,9,15 };
        for (int i = 0; i < 24; i++)
            BuildItemCell(content.transform, i, rarities[i % rarities.Length], qtys[i], i % 3 == 0);
    }

    static void BuildItemCell(Transform parent, int idx, string rarity, int qty, bool isNew)
    {
        var cell = MakeImg($"Item_{idx:00}", parent, RarityColor(rarity));

        var icon = MakeImg("ItemIcon", cell.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.5f));
        icon.rectTransform.sizeDelta = new Vector2(32, 32);
        CenterAnchor(icon.rectTransform);
        // ICON: assign item sprite

        var qtyTmp = MakeTMP("QtyLabel", cell.transform, $"x{qty}", 9, Fg1, TextAlignmentOptions.BottomRight);
        qtyTmp.rectTransform.anchorMin = Vector2.zero;
        qtyTmp.rectTransform.anchorMax = new Vector2(1, 0.4f);
        qtyTmp.rectTransform.offsetMin = new Vector2(4, 4);
        qtyTmp.rectTransform.offsetMax = new Vector2(-4, 0);
        // FONT: Barlow

        if (isNew)
        {
            var dot = MakeImg("NewDot", cell.transform, Danger);
            dot.rectTransform.anchorMin = dot.rectTransform.anchorMax = new Vector2(1, 1);
            dot.rectTransform.pivot     = new Vector2(1, 1);
            dot.rectTransform.anchoredPosition = new Vector2(-4, -4);
            dot.rectTransform.sizeDelta = new Vector2(8, 8);
        }
    }

    // ── BattleScreen ──────────────────────────────────────────────────────────

    static void BuildBattleScreen(Transform parent)
    {
        BuildScreenHeader(parent, "Battle", true, false);

        // Chapter panel (border shell + inner, margin 12 from edges, top 56+12=68)
        var chShell = MakeImg("ChapterPanel", parent, SpaceBeam);
        chShell.rectTransform.anchorMin = new Vector2(0, 1);
        chShell.rectTransform.anchorMax = new Vector2(1, 1);
        chShell.rectTransform.pivot     = new Vector2(0.5f, 1);
        chShell.rectTransform.anchoredPosition = new Vector2(0, -68);
        chShell.rectTransform.sizeDelta = new Vector2(-24, 120);

        var chInner = MakeImg("ChapterInner", chShell.transform, PanelBg);
        Stretch(chInner.rectTransform);
        chInner.rectTransform.offsetMin = new Vector2(1, 1);
        chInner.rectTransform.offsetMax = new Vector2(-1, -1);
        AddVLG(chInner.gameObject, 12, 12, 12, 12, 8, TextAnchor.MiddleCenter, cw: true, ch: true, ew: false);

        // Chapter nav row (prev / title / next)
        var chNav = new GameObject("ChapterNav", typeof(RectTransform));
        chNav.transform.SetParent(chInner.transform, false);
        AddLE(chNav, ph: 32, fw: 1);
        AddHLG(chNav, spacing: 8, align: TextAnchor.MiddleCenter, cw: true, ch: true);

        var prevBtn = MakeImg("PrevButton", chNav.transform, Color.clear);
        prevBtn.raycastTarget = true; prevBtn.gameObject.AddComponent<Button>();
        AddLE(prevBtn.gameObject, pw: 32, ph: 32);
        var prevTmp = MakeTMP("Arrow", prevBtn.transform, "‹", 22, Fg2); Stretch(prevTmp.rectTransform);

        var chTitle = MakeTMP("ChapterTitle", chNav.transform, "CHAPTER 04 — XENO BIOME", 13, Fg1);
        chTitle.fontStyle = FontStyles.Bold; chTitle.enableWordWrapping = false;
        AddLE(chTitle.gameObject, fw: 1);
        // FONT: Orbitron

        var nextBtn = MakeImg("NextButton", chNav.transform, Color.clear);
        nextBtn.raycastTarget = true; nextBtn.gameObject.AddComponent<Button>();
        AddLE(nextBtn.gameObject, pw: 32, ph: 32);
        var nextTmp = MakeTMP("Arrow", nextBtn.transform, "›", 22, Fg2); Stretch(nextTmp.rectTransform);

        // Reward icons
        var rewards = new GameObject("RewardIcons", typeof(RectTransform));
        rewards.transform.SetParent(chInner.transform, false);
        AddLE(rewards, ph: 28);
        AddHLG(rewards, spacing: 8, align: TextAnchor.MiddleCenter, cw: false, ch: false);
        for (int i = 0; i < 3; i++)
        {
            var rew = MakeImg($"Reward{i}", rewards.transform, new Color(SpaceBeam.r, SpaceBeam.g, SpaceBeam.b, 0.4f));
            rew.rectTransform.sizeDelta = new Vector2(28, 28);
        }

        // Deploy button
        var deployBtn = MakeImg("DeployButton", chInner.transform, Gold);
        deployBtn.raycastTarget = true; deployBtn.gameObject.AddComponent<Button>();
        AddLE(deployBtn.gameObject, pw: 140, ph: 40);
        var deployLabel = MakeTMP("Label", deployBtn.transform, "DEPLOY", 13, GoldDark);
        deployLabel.fontStyle = FontStyles.Bold;
        Stretch(deployLabel.rectTransform);
        // FONT: Barlow

        // Stage scroll view (below header 56 + 12 margin + 120 panel + 12 margin = 200)
        var sr = BuildScrollView("StageScrollView", parent, 200, 0);
        var content = sr.content.gameObject;
        AddVLG(content, 12, 12, 12, 12, 8, cw: true, ch: true, ew: true);
        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var stages = new (string num, string name, string status, int stars)[]
        {
            ("1-1","Alpha Landing",   "Completed",   3),
            ("1-2","Crash Site",      "Completed",   3),
            ("1-3","Fungal Grove",    "Completed",   2),
            ("1-4","Xenomorph Nest",  "In Progress", 1),
            ("1-5","Plasma Fault",    "Locked",      0),
            ("1-6","Derelict Cruiser","Locked",      0),
            ("1-7","Rogue AI Core",   "Locked",      0),
        };
        foreach (var s in stages) BuildStageRow(content.transform, s.num, s.name, s.status, s.stars);
    }

    static void BuildStageRow(Transform parent, string num, string name, string status, int stars)
    {
        bool locked = stars == 0;
        var row = MakeImg($"Stage_{num.Replace("-","_")}", parent, PanelBg);
        AddLE(row.gameObject, ph: 64);
        AddHLG(row.gameObject, 12, 12, 0, 0, 10, TextAnchor.MiddleLeft, cw: true, ch: false);
        if (locked) row.gameObject.AddComponent<CanvasGroup>().alpha = 0.55f;

        // Stage circle
        var circleShell = MakeImg("StageCircle", row.transform, locked ? H("#2A2060") : SpaceBeam);
        AddLE(circleShell.gameObject, pw: 40, ph: 40, fw: 0);
        if (!locked)
        {
            var circleInner = MakeImg("Inner", circleShell.transform, SpaceVoid);
            Stretch(circleInner.rectTransform);
            circleInner.rectTransform.offsetMin = new Vector2(1, 1);
            circleInner.rectTransform.offsetMax = new Vector2(-1, -1);
        }
        var numTmp = MakeTMP("StageNum", circleShell.transform, num, 11, Fg1);
        Stretch(numTmp.rectTransform);
        // FONT: Orbitron

        // Info column
        var info = new GameObject("StageInfo", typeof(RectTransform));
        info.transform.SetParent(row.transform, false);
        AddVLG(info, spacing: 2, align: TextAnchor.MiddleLeft, cw: true, ch: true, ew: true);
        AddLE(info, fw: 1);
        var stageName = MakeTMP("StageName", info.transform, name, 12, Fg1, TextAlignmentOptions.MidlineLeft);
        stageName.fontStyle = FontStyles.Bold;
        // FONT: Barlow
        MakeTMP("StageStatus", info.transform, status, 10, Fg3, TextAlignmentOptions.MidlineLeft);
        // FONT: Barlow

        // Stars
        var starsGo = new GameObject("StarRow", typeof(RectTransform));
        starsGo.transform.SetParent(row.transform, false);
        AddHLG(starsGo, spacing: 3, align: TextAnchor.MiddleRight, cw: false, ch: false);
        AddLE(starsGo, pw: 52, ph: 14, fw: 0);
        for (int i = 0; i < 3; i++)
        {
            var star = MakeImg($"Star{i}", starsGo.transform, i < stars ? Gold : H("#2A2060"));
            star.rectTransform.sizeDelta = new Vector2(14, 14);
        }
    }

    // ── AllianceScreen ────────────────────────────────────────────────────────

    static void BuildAllianceScreen(Transform parent)
    {
        BuildScreenHeader(parent, "Alliance", true, false);

        // Alliance card (top margin 68 = 56 header + 12)
        var cardShell = MakeImg("AllianceCard", parent, SpaceBeam);
        cardShell.rectTransform.anchorMin = new Vector2(0, 1);
        cardShell.rectTransform.anchorMax = new Vector2(1, 1);
        cardShell.rectTransform.pivot     = new Vector2(0.5f, 1);
        cardShell.rectTransform.anchoredPosition = new Vector2(0, -68);
        cardShell.rectTransform.sizeDelta = new Vector2(-24, 140);

        var cardInner = MakeImg("CardInner", cardShell.transform, PanelBg);
        Stretch(cardInner.rectTransform);
        cardInner.rectTransform.offsetMin = new Vector2(1, 1);
        cardInner.rectTransform.offsetMax = new Vector2(-1, -1);
        AddHLG(cardInner.gameObject, 16, 16, 16, 16, 12, TextAnchor.MiddleLeft, cw: false, ch: false);

        // Crest icon
        var crest = MakeImg("CrestIcon", cardInner.transform, SpaceBeam);
        crest.rectTransform.sizeDelta = new Vector2(56, 56);
        AddLE(crest.gameObject, pw: 56, ph: 56, fw: 0);
        // ICON: alliance crest sprite

        // Alliance info column
        var info = new GameObject("AllianceInfo", typeof(RectTransform));
        info.transform.SetParent(cardInner.transform, false);
        AddVLG(info, spacing: 4, align: TextAnchor.UpperLeft, cw: true, ch: true, ew: true);
        AddLE(info, fw: 1);

        var allianceName = MakeTMP("AllianceName", info.transform, "[PF] Nightfall", 16, Fg1, TextAlignmentOptions.MidlineLeft);
        allianceName.fontStyle = FontStyles.Bold;
        AddLE(allianceName.gameObject, ph: 20);
        // FONT: Orbitron
        var leaderText = MakeTMP("LeaderText", info.transform, "Leader: Commander Voss", 10, Fg3, TextAlignmentOptions.MidlineLeft);
        AddLE(leaderText.gameObject, ph: 14);
        // FONT: Barlow

        // Stats row
        var statsRow = new GameObject("StatsRow", typeof(RectTransform));
        statsRow.transform.SetParent(info.transform, false);
        AddLE(statsRow, ph: 14);
        AddHLG(statsRow, spacing: 12, align: TextAnchor.MiddleLeft, cw: true, ch: true);
        var powerStat = MakeTMP("PowerStat", statsRow.transform, "PWR 48.2B", 10, Gold, TextAlignmentOptions.MidlineLeft);
        AddLE(powerStat.gameObject, fw: 1);
        MakeTMP("MembersStat", statsRow.transform, "94 / 100", 10, Fg2, TextAlignmentOptions.MidlineLeft);

        // Level bar
        var levelBar = new GameObject("LevelBar", typeof(RectTransform));
        levelBar.transform.SetParent(info.transform, false);
        AddLE(levelBar, ph: 22);
        AddVLG(levelBar, spacing: 3, align: TextAnchor.UpperLeft, cw: true, ch: true, ew: true);
        var lvlLabel = MakeTMP("LevelLabel", levelBar.transform, "Level 11  MAXED", 10, Gold, TextAlignmentOptions.MidlineLeft);
        AddLE(lvlLabel.gameObject, ph: 12);
        // FONT: Barlow
        var barBg = MakeImg("BarBG", levelBar.transform, SpaceVoid);
        AddLE(barBg.gameObject, ph: 6);
        var barFill = MakeImg("BarFill", barBg.transform, Gold);
        Stretch(barFill.rectTransform);
        barFill.rectTransform.offsetMax = Vector2.zero;  // 100% fill for maxed

        // Menu grid (below card: 68 + 140 + 12 = 220 from top)
        var menuGo = new GameObject("MenuGrid", typeof(RectTransform));
        menuGo.transform.SetParent(parent, false);
        menuGo.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        menuGo.GetComponent<RectTransform>().anchorMax = Vector2.one;
        menuGo.GetComponent<RectTransform>().offsetMin = new Vector2(12, 12);
        menuGo.GetComponent<RectTransform>().offsetMax = new Vector2(-12, -220);

        var grid = menuGo.AddComponent<GridLayoutGroup>();
        grid.cellSize        = new Vector2(72, 72);
        grid.spacing         = new Vector2(8, 8);
        grid.padding         = new RectOffset(0, 0, 0, 0);
        grid.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.startCorner     = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis       = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment  = TextAnchor.UpperLeft;

        foreach (var item in new[] { "War","Chests","Territory","Battle","Shop","Tech","Rankings","Help" })
            BuildMenuButton(menuGo.transform, item);
    }

    static void BuildMenuButton(Transform parent, string label)
    {
        var btn = MakeImg($"MenuBtn_{label}", parent, PanelBg);
        btn.color = BorderDim;
        btn.raycastTarget = true;
        btn.gameObject.AddComponent<Button>();

        var inner = MakeImg("Inner", btn.transform, PanelBg);
        Stretch(inner.rectTransform);
        inner.rectTransform.offsetMin = new Vector2(1, 1);
        inner.rectTransform.offsetMax = new Vector2(-1, -1);
        AddVLG(inner.gameObject, 0, 0, 10, 8, 4, TextAnchor.MiddleCenter, cw: true, ch: true, ew: true);

        var icon = MakeImg("MenuIcon", inner.transform, SpaceBeam);
        icon.rectTransform.sizeDelta = new Vector2(28, 28);
        AddLE(icon.gameObject, pw: 28, ph: 28);
        // ICON: assign menu icon sprites

        var lbl = MakeTMP("MenuLabel", inner.transform, label, 10, Fg2);
        AddLE(lbl.gameObject, ph: 14);
        // FONT: Barlow
    }

    // ── Shared ────────────────────────────────────────────────────────────────

    static void BuildScreenHeader(Transform parent, string title, bool hasBack, bool hasSort)
    {
        var header = MakeImg("ScreenHeader", parent, SpaceVoid);
        AnchorTop(header.rectTransform, 0, 56);
        AddHLG(header.gameObject, 12, 12, 8, 8, 8, TextAnchor.MiddleLeft, cw: true, ch: true);

        if (hasBack)
        {
            var backBtn = MakeImg("BackButton", header.transform, Color.clear);
            backBtn.raycastTarget = true;
            backBtn.gameObject.AddComponent<Button>();
            backBtn.rectTransform.sizeDelta = new Vector2(32, 32);
            AddLE(backBtn.gameObject, pw: 32, ph: 32);
            var icon = MakeImg("BackIcon", backBtn.transform, Fg2);
            icon.rectTransform.sizeDelta = new Vector2(20, 20);
            CenterAnchor(icon.rectTransform);
            // ICON: back chevron sprite
        }

        var titleTmp = MakeTMP("ScreenTitle", header.transform, title, 18, Fg1, TextAlignmentOptions.MidlineLeft);
        titleTmp.fontStyle = FontStyles.Bold;
        AddLE(titleTmp.gameObject, fw: 1);
        // FONT: Orbitron

        if (hasSort)
        {
            var sortShell = MakeImg("SortButton", header.transform, SpaceBeam);
            sortShell.raycastTarget = true;
            sortShell.gameObject.AddComponent<Button>();
            AddLE(sortShell.gameObject, pw: 110, ph: 32);
            var sortInner = MakeImg("Inner", sortShell.transform, PanelBg);
            Stretch(sortInner.rectTransform);
            sortInner.rectTransform.offsetMin = new Vector2(1, 1);
            sortInner.rectTransform.offsetMax = new Vector2(-1, -1);
            var sortLabel = MakeTMP("SortLabel", sortShell.transform, "Sort: Power  ▾", 11, Fg2);
            Stretch(sortLabel.rectTransform);
            // FONT: Barlow
        }

        // Border bottom
        var border = MakeImg("BorderBottom", header.transform, BorderDim);
        border.raycastTarget = false;
        border.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        border.rectTransform.anchorMin = Vector2.zero;
        border.rectTransform.anchorMax = new Vector2(1, 0);
        border.rectTransform.pivot     = new Vector2(0.5f, 0);
        border.rectTransform.anchoredPosition = Vector2.zero;
        border.rectTransform.sizeDelta = new Vector2(0, 1);
    }

    static ScrollRect BuildScrollView(string name, Transform parent, float topOffset, float bottomOffset)
    {
        var svGo = new GameObject(name, typeof(RectTransform), typeof(ScrollRect));
        svGo.transform.SetParent(parent, false);
        var svRT = svGo.GetComponent<RectTransform>();
        svRT.anchorMin = Vector2.zero;
        svRT.anchorMax = Vector2.one;
        svRT.offsetMin = new Vector2(0, bottomOffset);
        svRT.offsetMax = new Vector2(0, -topOffset);

        var sr = svGo.GetComponent<ScrollRect>();
        sr.horizontal = false; sr.vertical = true; sr.scrollSensitivity = 30f;

        var vpGo = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
        vpGo.transform.SetParent(svGo.transform, false);
        vpGo.GetComponent<Image>().color = Color.clear;
        vpGo.GetComponent<Mask>().showMaskGraphic = false;
        Stretch(vpGo.GetComponent<RectTransform>());

        var contentGo = new GameObject("Content", typeof(RectTransform));
        contentGo.transform.SetParent(vpGo.transform, false);
        var contentRT = contentGo.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 1);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.pivot     = new Vector2(0.5f, 1);
        contentRT.offsetMin = contentRT.offsetMax = Vector2.zero;

        sr.viewport = vpGo.GetComponent<RectTransform>();
        sr.content  = contentRT;
        return sr;
    }

    // ── UIManager wiring ──────────────────────────────────────────────────────

    static void WireUIManager(Canvas canvas, GameObject baseGo, GameObject heroesGo,
        GameObject backpackGo, GameObject battleGo, GameObject allianceGo,
        GameObject topBarGo, GameObject resourceStripGo)
    {
        var mgr = canvas.GetComponent<UIManager>() ?? canvas.gameObject.AddComponent<UIManager>();
        var so  = new SerializedObject(mgr);
        so.FindProperty("baseScreen")    .objectReferenceValue = baseGo;
        so.FindProperty("heroesScreen")  .objectReferenceValue = heroesGo;
        so.FindProperty("backpackScreen").objectReferenceValue = backpackGo;
        so.FindProperty("battleScreen")  .objectReferenceValue = battleGo;
        so.FindProperty("allianceScreen").objectReferenceValue = allianceGo;
        so.FindProperty("topBar")        .objectReferenceValue = topBarGo;
        so.FindProperty("resourceStrip") .objectReferenceValue = resourceStripGo;
        so.ApplyModifiedProperties();
    }

    // ── Low-level helpers ─────────────────────────────────────────────────────

    static RectTransform MakeRT(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }

    static Image MakeImg(string name, Transform parent, Color color)
    {
        var go  = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var img = go.GetComponent<Image>();
        img.color = color;
        return img;
    }

    static TextMeshProUGUI MakeTMP(string name, Transform parent, string text, float size, Color color,
        TextAlignmentOptions align = TextAlignmentOptions.Center)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = text; t.fontSize = size; t.color = color;
        t.alignment = align; t.enableWordWrapping = false;
        return t;
    }

    static LayoutElement AddLE(GameObject go, float pw = -1, float ph = -1, float fw = -1, float fh = -1)
    {
        var le = go.AddComponent<LayoutElement>();
        if (pw >= 0) le.preferredWidth  = pw;
        if (ph >= 0) le.preferredHeight = ph;
        if (fw >= 0) le.flexibleWidth   = fw;
        if (fh >= 0) le.flexibleHeight  = fh;
        return le;
    }

    static LayoutElement AddLE(RectTransform rt, float pw = -1, float ph = -1, float fw = -1, float fh = -1)
        => AddLE(rt.gameObject, pw, ph, fw, fh);

    static HorizontalLayoutGroup AddHLG(GameObject go,
        int l = 0, int r = 0, int t = 0, int b = 0, float spacing = 0,
        TextAnchor align = TextAnchor.MiddleLeft,
        bool cw = true, bool ch = true, bool ew = false, bool eh = false)
    {
        var hlg = go.GetComponent<HorizontalLayoutGroup>() ?? go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(l, r, t, b); hlg.spacing = spacing;
        hlg.childAlignment = align;
        hlg.childControlWidth = cw; hlg.childControlHeight = ch;
        hlg.childForceExpandWidth = ew; hlg.childForceExpandHeight = eh;
        return hlg;
    }

    static HorizontalLayoutGroup AddHLG(RectTransform rt,
        int l = 0, int r = 0, int t = 0, int b = 0, float spacing = 0,
        TextAnchor align = TextAnchor.MiddleLeft,
        bool cw = true, bool ch = true, bool ew = false, bool eh = false)
        => AddHLG(rt.gameObject, l, r, t, b, spacing, align, cw, ch, ew, eh);

    static VerticalLayoutGroup AddVLG(GameObject go,
        int l = 0, int r = 0, int t = 0, int b = 0, float spacing = 0,
        TextAnchor align = TextAnchor.MiddleCenter,
        bool cw = true, bool ch = true, bool ew = false, bool eh = false)
    {
        var vlg = go.GetComponent<VerticalLayoutGroup>() ?? go.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(l, r, t, b); vlg.spacing = spacing;
        vlg.childAlignment = align;
        vlg.childControlWidth = cw; vlg.childControlHeight = ch;
        vlg.childForceExpandWidth = ew; vlg.childForceExpandHeight = eh;
        return vlg;
    }

    static VerticalLayoutGroup AddVLG(RectTransform rt,
        int l = 0, int r = 0, int t = 0, int b = 0, float spacing = 0,
        TextAnchor align = TextAnchor.MiddleCenter,
        bool cw = true, bool ch = true, bool ew = false, bool eh = false)
        => AddVLG(rt.gameObject, l, r, t, b, spacing, align, cw, ch, ew, eh);

    static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }

    static void AnchorTop(RectTransform rt, float yOffset, float height)
    {
        rt.anchorMin = new Vector2(0, 1); rt.anchorMax = new Vector2(1, 1);
        rt.pivot     = new Vector2(0.5f, 1);
        rt.anchoredPosition = new Vector2(0, -yOffset);
        rt.sizeDelta = new Vector2(0, height);
    }

    static void AnchorBottom(RectTransform rt, float height)
    {
        rt.anchorMin = Vector2.zero; rt.anchorMax = new Vector2(1, 0);
        rt.pivot     = new Vector2(0.5f, 0);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(0, height);
    }

    static void CenterAnchor(RectTransform rt, float offsetX = 0, float offsetY = 0)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(offsetX, offsetY);
    }

    static GameObject MakeScreen(string name, RectTransform parent, bool hasBg)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Stretch(go.GetComponent<RectTransform>());
        if (hasBg) { var img = go.AddComponent<Image>(); img.color = SpaceVoid; img.raycastTarget = false; }
        return go;
    }

    static Color RarityColor(string rarity) => rarity switch
    {
        "Legendary" => RarityLegend,
        "Epic"      => RarityEpic,
        "Rare"      => RarityRare,
        _           => RarityCommon,
    };

    static Canvas FindCanvas()
    {
        foreach (var c in Object.FindObjectsOfType<Canvas>())
            if (c.name == "Main Canvas") return c;
        var all = Object.FindObjectsOfType<Canvas>();
        return all.Length > 0 ? all[0] : null;
    }

    static Color H(string hex) { ColorUtility.TryParseHtmlString(hex, out Color c); return c; }
}
#endif
