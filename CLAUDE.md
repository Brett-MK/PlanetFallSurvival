# Planetfall Survival — Project Guide

## What We're Building

**Planetfall Survival** is a mobile-first colony-building survival strategy game built in Unity. Players manage a colony on an alien world: constructing and upgrading buildings, commanding heroes, running expeditions, and competing in alliances. The genre is mobile 4X / colony survival (think Rise of Kingdoms or Whiteout Survival). The game runs in portrait mode on phones.

**Target platform:** iOS / Android (developed and previewed in Unity Editor)  
**Unity version:** Unity 6 (URP pipeline)  
**Screen reference resolution:** 412 × 892 px (Canvas Scaler: Scale With Screen Size)

---

## Project Structure

```
Assets/
  Art/                        # Textures, sprites, 3D art
  DesignReference/            # Claude Design export — source of truth for all UI
  Prefabs/                    # Building prefabs, UI prefabs
  Scenes/                     # Unity scenes
  Scripts/
    Buildings/                # Building.cs — level-based model swapping
    Camera/                   # CameraController.cs — orthographic pan/pinch/zoom
    Managers/                 # BuildingManager.cs, InputManager.cs (singletons)
    UI/                       # All UI scripts (controllers, helpers)
    Editor/                   # PlanetfallUIBuilder.cs — one-time hierarchy builder
  UI/
    Icons/                    # Icon sprites: base_icon, heroes_icon, battle_icon,
                              #   backpack_icon, alliance_icon, power_icon,
                              #   cart_icon, settings_icon, alliance_icon, etc.
  ScriptableObjects/
  Settings/
  ThirdParty/
```

---

## The Design Reference System

The design lives in `Assets/DesignReference/ui_kits/mobile/`. It is a React/JSX browser-based UI kit that acts as the **single source of truth** for all visual design decisions.

### Files
| File | Purpose |
|------|---------|
| `index.html` | Preview app — open in browser to see all screens with screen picker |
| `styles.css` | All CSS for every component — measurements, colors, layout |
| `colors_and_type.css` | Design tokens — colors, gradients, shadows, fonts, border radii |
| `Components.jsx` | TopBar, ResourceStrip, BottomNav, Avatar, Buttons, Canvas |
| `BaseScreen.jsx` | Base screen layout: float groups, outpost scene, CTA row |
| `Screens.jsx` | HeroesScreen, BackpackScreen, AllianceScreen, BattleScreen |
| `Icons.jsx` | SVG icon library (Compass, Sword, Shield, Cart, Gear, etc.) |

**How to use:** Open `Assets/DesignReference/ui_kits/mobile/index.html` in a browser. Use the screen picker buttons to switch between Base, Heroes, Battle, Backpack, Alliance. This is the exact visual target for Unity UI.

### Key design constraints from the reference
- TopBar and ResourceStrip appear **only on the Base screen** — not on Heroes/Battle/Backpack/Alliance
- Other screens use `ScreenHeader` (back button + title + optional right slot) instead
- BottomNav is always visible on all screens
- The Base screen canvas area is transparent — the 3D world shows through

---

## Design Tokens

All colors, sizes, and typography come from `colors_and_type.css` and `styles.css`. The Unity builder mirrors these exactly.

### Colors
| Token | Hex | Usage |
|-------|-----|-------|
| `--space-void` | `#0A0618` | Background, screen bg |
| `--space-beam` | `#8B6CFF` | Purple accent, borders, icons |
| `--panel-bg` | `#1A1248` | Card/chip/panel backgrounds |
| `--border-dim` | `#452E99` | Subtle panel borders (beam at 30% opacity) |
| `--fg-1` | `#F4EEFF` | Primary text, active icons |
| `--fg-2` | `#C6B8E8` | Secondary text |
| `--fg-3` | `#8A7BB8` | Muted text, inactive nav |
| `--gold-1` | `#FFC24A` | Gold CTA, combat power, rank badges |
| `--gold-dark` | `#3A1A00` | Text on gold backgrounds |
| `--diamond` | `#6DD5FF` | Diamond resource color |
| `--energy` | `#7BE89B` | Bio-energy resource color |
| `--danger` | `#FF5A6E` | Badge/notification color |
| Rarity Legendary | `#3D1F6E` | Hero card bg |
| Rarity Epic | `#6B1F4A` | Hero card bg |
| Rarity Rare | `#1F3A5F` | Hero card bg |
| Rarity Common | `#1A1248` | Hero card bg (= PanelBg) |

### Typography
- **Display font:** Orbitron (not yet imported — TMP default LiberationSans in use). Mark all display font usages with `// FONT: Orbitron`
- **UI / Body font:** Barlow (not yet imported). Mark all body font usages with `// FONT: Barlow`
- **How to import:** Download from Google Fonts, generate TMP font assets, assign to `[SerializeField]` font slots in the controller scripts

---

## Unity Canvas Architecture

The scene has one **Main Canvas** (Screen Space - Overlay, Canvas Scaler Scale With Screen Size 412×892).

### Hierarchy built by `PlanetfallUIBuilder.cs`
```
Main Canvas  [UIManager]
  ├── Background        (Image #0A0618, stretch fill, sibling 0 = renders behind)
  ├── Screens           (RectTransform, stretch fill)
  │   ├── BaseScreen    (active on load; transparent bg — 3D shows through)
  │   ├── HeroesScreen  (Image #0A0618 bg; inactive)
  │   ├── BackpackScreen(Image #0A0618 bg; inactive)
  │   ├── BattleScreen  (Image #0A0618 bg; inactive)
  │   └── AllianceScreen(Image #0A0618 bg; inactive)
  └── HUD               (RectTransform, stretch fill; renders on top of Screens)
      ├── TopBar        [TopBarController]       — Base screen only (hidden on other screens)
      ├── ResourceStrip [ResourceStripController]— Base screen only (hidden on other screens)
      └── BottomNav     [BottomNavController]    — Always visible
```

### Key rule: TopBar + ResourceStrip visibility
Per the design reference, TopBar and ResourceStrip are **only shown when BaseScreen is active**. `UIManager.OnScreenChanged` drives this — `TopBarController` and `ResourceStripController` subscribe and hide/show themselves based on the active screen.

---

## Scripts Reference

### Core Game Scripts

**`Assets/Scripts/Buildings/Building.cs`**  
Attach to building prefabs. Children are level models (index 0 = level 1). Calling `UpgradeBuilding()` activates the next child model and deactivates the rest.

**`Assets/Scripts/Managers/BuildingManager.cs`** (singleton)  
Listens for `InputManager.OnBuildingSelected`. Calls `ZoomToBuilding` on camera, activates building upgrade UI. Closes on `OnBackPressed`.

**`Assets/Scripts/Managers/InputManager.cs`** (singleton)  
New Input System. Raycasts clicks/taps to detect building selection. Fires `OnBuildingSelected(Building, Vector3)` and `OnBackPressed`.

**`Assets/Scripts/Camera/CameraController.cs`**  
Orthographic camera. Drag-to-pan (clamped to world bounds), scroll/pinch-to-zoom. `ZoomToBuilding(pos, size)` and `ZoomOut()` for building inspection mode.

### UI Scripts

**`Assets/Scripts/UI/UIManager.cs`** (singleton on Main Canvas)  
Owns `ScreenType` enum `{ Base, Heroes, Battle, Backpack, Alliance }`. `ShowScreen(type)` activates the matching screen GO and deactivates the rest. Fires `static event Action<ScreenType> OnScreenChanged`.

**`Assets/Scripts/UI/BottomNavController.cs`**  
Attached to BottomNav. Subscribes to `UIManager.OnScreenChanged`. Updates active tab colors (`Fg1` active, `Fg3` inactive), shows/hides ActiveBar strip. Wires each tab Button onClick → `UIManager.ShowScreen(type)`.

**`Assets/Scripts/UI/TopBarController.cs`**  
Attached to TopBar. Hides itself when any non-Base screen is active.

**`Assets/Scripts/UI/ResourceStripController.cs`**  
Attached to ResourceStrip. Same visibility logic as TopBarController.

**Screen controllers** (BaseScreenController, HeroesScreenController, BackpackScreenController, BattleScreenController, AllianceScreenController)  
Simple data binders — find buttons by hierarchy path in `Awake()`, wire `onClick`. No UI generation. All follow the same `WireBack` / `WireLog` pattern.

**`Assets/Scripts/UI/BuildingUpgradeUI.cs`**  
TMP refs for building level/name, upgrade button. Called by BuildingManager.

### Editor Scripts

**`Assets/Scripts/Editor/PlanetfallUIBuilder.cs`**  
One-time setup tool. **Planetfall → Build Complete UI** in the Unity menu clears Main Canvas children and rebuilds the entire hierarchy. All RectTransform anchors, colors, Layout Groups, and text values are set by this script. After running it, save the scene (Ctrl+S). This file can be deleted after the hierarchy is finalized.

---

## How We Build UI

**Philosophy:** Build the hierarchy visually in the editor using the `PlanetfallUIBuilder` editor script, then attach lightweight controller scripts that only find existing elements and bind data or events. No runtime GameObject creation in controller scripts.

### Workflow
1. Make changes to `PlanetfallUIBuilder.cs`
2. Run **Planetfall → Build Complete UI** in Unity menu
3. Save scene (Ctrl+S)
4. Hit Play to test

### Layout system notes
- **`childControlHeight = true`** is required on any VLG/HLG whose children have explicit `LayoutElement.preferredHeight` values — without it, children default to 100px height
- ScrollView: Viewport has `Image` (clear) + `Mask` (showMaskGraphic=false). Content is anchored top with `pivot=(0.5,1)`, has `GridLayoutGroup` or `VerticalLayoutGroup` + `ContentSizeFitter (verticalFit=PreferredSize)`
- Border simulation: outer Image (border color) + inner Image (background color) inset by `offsetMin/offsetMax`

### Icon assignment (post-build)
After running the builder, assign these sprites from `Assets/UI/Icons/`:
- BottomNav items: `base_icon`, `heroes_icon`, `battle_icon`, `backpack_icon`, `alliance_icon`
- TopBar: `cart_icon` (ShopButton/CartIcon), `settings_icon` (SettingsButton/GearIcon)
- BaseScreen float buttons: quest, mail, shield sprites
- Mark with `// ICON: sprite_name` comments throughout the builder

---

## Screen-by-Screen Reference

### Base Screen
- Transparent background (3D world shows through)
- TopBar + ResourceStrip shown (HUD layer, above Screens layer)
- FloatGroup_Left: QUESTS (gold) + MAIL (diamond blue) — pill buttons, left-center
- FloatGroup_Right: SHIELD (green) — pill button, right-center
- FloatGroup_Bottom: RESEARCH (ghost) + RECRUIT (gold) — above BottomNav, 100px from bottom

### Heroes Screen
- Header: back button + "Heroes" title + "Sort: Power ▾" dropdown
- Scrollable 4-column hero card grid (16 heroes, `GridLayoutGroup` 72×110 cells)
- Hero rarities: Legendary (S, 5★, `#3D1F6E`), Epic (A, 4★, `#6B1F4A`), Rare (R, 3★, `#1F3A5F`)
- Bottom bar: DRILL CAMP (ghost) + RECRUIT (gold)
- Hero names: Voss, Lyra, Doran, Kael, Mira, Ven, Torin, Zara, Rook, Pax, Greaves, Soren, Ada, Nox, Rhee, Brond

### Backpack Screen
- Header: back button + "Backpack" title + bell button
- Category tabs: Resources / Speedup / Bonus / Gear / Other
- Scrollable 4-column item grid (24 cells, `GridLayoutGroup` 72×80 cells)
- Cell rarity variants: common/rare/epic/legendary backgrounds

### Battle Screen
- Header: back button + "Battle" title
- Chapter panel: "CHAPTER 04 — XENO BIOME", prev/next nav, 3 reward icons, DEPLOY button
- Scrollable stage list (7 stages, first 4 unlocked, last 3 locked/greyed)
- Stage circle: gold for unlocked, dark purple for locked
- Stars: 3,3,2,1,0,0,0

### Alliance Screen
- Header: back button + "Alliance" title
- Alliance card: crest icon (56×56) + name "[PF] Nightfall" + leader/power/members stats
- Level bar: level badge circle + progress bar (100% = "MAXED")
- Menu grid: 4-column, 8 items — War, Chests, Territory, Battle, Shop, Tech, Rankings, Help

---

## BottomNav Items
| Tab | Icon sprite | Badge |
|-----|-------------|-------|
| BASE | `base_icon` | none |
| HEROES | `heroes_icon` | 2 |
| BATTLE | `battle_icon` | none |
| BACKPACK | `backpack_icon` | 99+ |
| ALLIANCE | `alliance_icon` | none |

Active state: icon + label color `#F4EEFF`, gold ActiveBar (20×4px) at bottom of icon wrap, purple pill bg behind icon.

---

## Pending / Known Issues (as of last session)

1. **Fonts not imported** — Orbitron and Barlow needed from Google Fonts → TMP Font Asset Creator. All usage sites marked `// FONT: Orbitron` / `// FONT: Barlow`.
2. **TopBar + ResourceStrip must hide on non-Base screens** — UIManager.OnScreenChanged subscription needed in both controllers.
3. **Float buttons too tall** — `AddVLG` on FloatGroup containers uses `ch: false`; needs `ch: true` so LayoutElement preferredHeight=52 is respected.
4. **ResourceStrip chips clipped** — TimeChip fixed width (150px) too large; Diamond/Energy chips pushed off-screen.
5. **ScrollView screens blank** — Heroes, Backpack, Battle stage list appear empty; ContentSizeFitter + Viewport Mask interaction may need a forced layout rebuild.
6. **Alliance level bar oversized** — BarBG uses `ch: false` in parent VLG, defaults to 100px tall instead of 6px.
7. **Icon sprites not assigned** — All nav, TopBar, float button sprite slots still show white squares.
