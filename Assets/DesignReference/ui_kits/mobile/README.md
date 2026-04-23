# Mobile UI Kit — Planetfall Survival

Recreation of the in-game **Base UI** from the reference screenshot, componentized for use in mocks and promo assets.

## Files
- `index.html` — renders the full Base screen inside a device frame
- `styles.css` — all chrome CSS (imports tokens from `../../colors_and_type.css`)
- `Icons.jsx` — SVG icon set used in HUD chrome
- `Components.jsx` — TopBar, ResourceStrip, Canvas, BottomNav, Avatar, CombatPower, ResourceChip, VipBadge, IconButton, Badge, PrimaryButton, SecondaryButton, Panel
- `BaseScreen.jsx` — assembled Base hub

## Components

| Component | Purpose |
|---|---|
| `<TopBar>` | Avatar + combat power + shop + settings |
| `<Avatar>` | Square-rounded portrait with level chip |
| `<CombatPower>` | Gold stat numeral with bolt icon + caps label |
| `<IconButton>` | 44×44 chrome button; `variant="gold"` or `"ghost"` |
| `<ResourceStrip>` | Time chip, VIP, resource chips row |
| `<ResourceChip>` | Pill with icon + value + optional `+` button; `variant="diamond" \| "energy"` |
| `<VipBadge>` | VIP level pill (gold) |
| `<TimeChip>` | Calendar icon + server time pill |
| `<Canvas>` | 3D viewport region; starfield + vignette overlays |
| `<BottomNav>` | 6-tab nav with active-tab glow and notification badges |
| `<PrimaryButton>` / `<SecondaryButton>` | Gold CTA / ghost button |
| `<Panel>` | Generic chrome card |

## Variants & sizes
Buttons: `size="sm" \| "md" \| "lg"`. Icon buttons: 44px fixed. Device design width 412px (iPhone-class portrait).

## Known placeholders
- Avatar uses a generic SVG silhouette — swap in real portraits.
- The 3D "outpost" scene is a line-art SVG placeholder; production should render 3D colony art here.
- No game logic; nav tab selection is local state only.
