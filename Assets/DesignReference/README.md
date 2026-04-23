# Planetfall Survival — Design System

A visual language for **Planetfall Survival**, a mobile strategy/colony-building survival game set in a deep-space sci-fi universe. Players build and defend a colony on a hostile planet, recruit heroes, fight waves, raid other players, and explore the world map.

This design system documents the visual foundations, UI component library, and brand voice so that any new screen, promo asset, or feature mockup stays consistent with the in-game HUD.

## Sources used

- `uploads/Screenshot 2026-04-19 194840.png` — the reference **Base UI** screenshot provided by the user (preserved at `assets/reference-base-ui.png`). No codebase or Figma was attached; **all visual tokens were lifted from that single screenshot** and enhanced with common sci-fi mobile-strategy conventions (State of Survival, Whiteout Survival, Last Fortress, Call of Dragons, etc. as genre peers).

## Product at a glance

- **Platform:** Mobile (portrait, ~9:16, ~450×772 in the reference frame)
- **Genre:** 4X-lite / colony survival strategy — gacha-hero-driven, with an always-on world map
- **Core loop:** Build base → gather resources → train / recruit heroes → battle → expand alliance/world
- **Primary screens:** Base (home), Heroes, Battle, Backpack, Alliance, World
- **Tone:** Urgent, sci-fi, commander-addressed ("Commander, your outpost is under attack")

## Index — what's in this folder

| Path | What |
|------|------|
| `README.md` | This file — context, content rules, visual foundations, iconography |
| `SKILL.md` | Agent-Skills compatible entry point for Claude Code |
| `colors_and_type.css` | All design tokens — colors, gradients, shadows, radii, spacing, type scale |
| `fonts/` | Font references (Orbitron, Rajdhani, Inter — via Google Fonts) |
| `assets/` | Reference screenshot, placeholder logo, resource icons |
| `preview/` | Design-system card HTMLs (registered for the Design System tab) |
| `ui_kits/mobile/` | The mobile game UI kit — Base screen recreation + components |

## Quick start

```html
<link rel="stylesheet" href="colors_and_type.css"/>
<link rel="stylesheet" href="ui_kits/mobile/styles.css"/>
<!-- React + Babel, then: -->
<script type="text/babel" src="ui_kits/mobile/Icons.jsx"></script>
<script type="text/babel" src="ui_kits/mobile/Components.jsx"></script>
<script type="text/babel" src="ui_kits/mobile/BaseScreen.jsx"></script>
```

## CONTENT FUNDAMENTALS

The copy style is **commander-voice sci-fi military**, typical of mid-core mobile strategy. Terse, imperative, always addressing the player as a rank.

- **Voice.** The game talks to the player as **"Commander"** (never "you" alone, never "I"). UI speaks in imperatives: *Deploy*, *Recruit*, *Claim*, *Upgrade*, *Defend*.
- **Casing.**
  - **ALL-CAPS** for nav labels, button labels, stat labels (`BASE`, `HEROES`, `COMBAT POWER`, `VIP 12`, `CLAIM`).
  - **Title Case** for screen titles and hero/building names (`Stellar Forge`, `Plasma Refinery`).
  - **Sentence case** for longer descriptive text and dialog (`Your colony is running low on energy.`).
- **Numbers.** Always formatted for readability — thousands separators with commas at low ranges (`35,961`), then abbreviated with uppercase suffixes at high ranges (`652M`, `2.4B`, `813,126,736` combat power displayed in full to show power fantasy).
- **Punctuation.** Minimal. No trailing periods on labels or single-line notifications. En-dash `–` or middle dot `·` as separators (`UTC 03/09 · 03:42:52`).
- **Emoji.** **Never.** Use in-engine icons or the icon set instead — emoji breaks the sci-fi visual tone.
- **No idioms.** Keep copy culture-neutral for global localization.
- **Urgency words.** `NEW!`, `LIVE`, `ENDS IN`, `LIMITED`, `99+` — used sparingly, always uppercase.

**Examples from the reference and conventions:**
- Stat: `813,126,736` / `COMBAT POWER`
- Tier: `VIP 12`
- Timestamp: `UTC 03/09 · 03:42:52`
- Nav: `BASE`, `HEROES`, `BATTLE`, `BACKPACK`, `ALLIANCE`, `WORLD`
- Notification counts: `2`, `99+` (never `100`, always `99+`)
- CTAs (genre-standard): `DEPLOY`, `RECRUIT`, `CLAIM`, `UPGRADE`, `ATTACK`, `SHIELD`, `SCOUT`
- Toasts: `Alliance help received.` / `Research complete.` / `Attack inbound — 00:42`

## VISUAL FOUNDATIONS

The look is **deep-space noir with gold commanding accents** — a dark violet/plum canvas that feels like looking out of a colony viewport into the void, with HUD chrome that sits on top like holographic glass.

### Palette
- **Canvas:** deep violet-navy gradient (`--space-void` `#0A0618` → `--space-abyss` `#120A28`), almost black but unmistakably purple.
- **Chrome surfaces:** layered plums (`--space-nebula` / `--space-plum` / `--space-twilight`) — each raised surface is a single step lighter than the one it sits on.
- **Primary accent:** **gold** (`--gold-1` `#FFC24A` → `--gold-3` `#D87516`). Used for: combat power numeral, the shop button, VIP tier, primary CTAs. **One gold element at a time** per region — it's the "main action" color.
- **Resource accents:** diamond-blue (`#6DD5FF`) for premium currency, leaf-green (`#7BE89B`) for energy/food. Each has a matching glow token.
- **Danger:** crimson `#FF5A6E` for notification badges only.
- **Purple glow:** `--space-beam` `#8B6CFF` for the active-tab indicator and focus states.

### Typography
- **Display / numerals:** **Orbitron** 700–900 — the blocky sci-fi face for big stat numbers (`813,126,736`). Substitute for licensed game face when available.
- **UI labels:** **Rajdhani** 600/700 — narrower, caps-locked, lightly tracked (+0.08em). Used for all nav labels, button text, caps labels like `COMBAT POWER`.
- **Body:** **Inter** — for long descriptive copy, dialog, tooltips. Quietly professional.
- **Never mix** body and display in the same line — display is for numerals + caps labels only.

### Backgrounds
- Main canvas is a **radial gradient** fading from a lighter plum at the top-center to near-black at the edges — simulates a viewport with depth.
- Subtle **starfield** (tiny white/violet dots, 2–3% opacity) as an overlay texture in empty space.
- **No hand-drawn illustrations, no photography in UI chrome.** In-game 3D renders live inside the canvas area only; HUD is pure vector.
- **Full-bleed** 3D colony art only in the center canvas; chrome bars are opaque panels above it.

### Gradients
- Panels: **180° top-light** (`#261852` → `#1A0F3D`) — simulates a light source above.
- Gold buttons: 3-stop top-light (`#FFD066` → `#F59A2B` → `#D87516`) — gives the button a domed, chiseled feel.
- Resource chips: similar top-light, tinted to the resource's hue.

### Shadows & inner lighting
Every chrome element uses a **two-sided inset** pattern — a thin light edge at the top and a darker edge at the bottom — to read as a physical beveled plate, not flat material. Plus an outer drop shadow for lift. See `--shadow-panel`, `--shadow-raised`, `--shadow-gold`.

### Glows
Resource pills and the active nav tab carry a matching **outer glow** (`--glow-diamond`, `--glow-energy`, `--shadow-active-tab`). Glows are always colored, never neutral. Notification badges pulse with `--glow-danger`.

### Corners
- Large panels (top bar, resource strip, bottom nav): **22px** (`--r-xl`) — generous rounding, reads as an iOS-era mobile-game chrome.
- Buttons / pills: **12–16px** (`--r-md`/`--r-lg`) or fully rounded pill (`--r-pill`).
- Resource chips: `--r-pill`.
- Avatars: `--r-lg` (softer square, not a full circle).

### Borders
- **1px violet** (`rgba(139,108,255,0.18–0.28)`) on all chrome — barely there but defines the edge.
- Gold buttons use a warmer **1px gold** border at 55% alpha.

### Motion
- **Ease-out cubic** (`cubic-bezier(0.2, 0.8, 0.2, 1)`) for most UI transitions — 180ms standard, 240ms for panel swaps.
- **Spring/overshoot** for reward pops, badge counts, and resource number increments.
- Active tab has a subtle **pulsing glow** (2s ease-in-out, infinite).
- Notification badges have a **single bounce** on appear, then static.
- No fades; state changes are assertive.

### Interaction states
- **Hover (tablet/pointer):** +6% brightness on gradient, subtle glow ring.
- **Press:** darker gradient variant (`--grad-gold-press`), 2px translateY down, inner shadow deepens — makes the button feel "pushed in."
- **Disabled:** 38% opacity, grayscale applied, no glow.
- **Focus:** 2px `--space-beam` outer ring, offset 2px.

### Blur / transparency
- Used **sparingly** — only for modal backdrops (`rgba(10,6,24,0.65)` + 12px blur) and tooltip backgrounds.
- Chrome panels are **opaque** — they sit over the 3D canvas and must stay readable.

### Layout rules (mobile, portrait)
- **Fixed top bar** at top, ~72px tall, safe-area padded.
- **Fixed resource strip** directly under top bar, ~44px.
- **Fixed bottom nav** at bottom, ~80px including safe area.
- **Central canvas** fills everything between — this is where the 3D view or screen content lives.
- Corner-rounded outer frame (~22px) with a subtle inner plum tint, so the canvas feels inset into a device-within-device.
- 16px gutter on left/right for all chrome.

### Imagery vibe
When 3D renders or illustrations appear: **cool-toned**, high-contrast, rim-lit from purple/cyan light sources, with warm gold sparks/embers for drama. Never warm or sunlit. Slight film grain optional.

## ICONOGRAPHY

Icons are **inline SVG**, rendered by `ui_kits/mobile/Icons.jsx` (source of truth — import or copy from there). The HUD uses two icon treatments that coexist:

1. **Line/stroke icons** — 24×24 viewBox, **1.8px stroke**, round caps, round joins, `currentColor`-driven so they tint via the CSS fg tokens. Used for: nav (`Compass`, `Hero`, `Sword`, `Backpack`, `Alliance`, `Globe`), utility (`Gear`, `Cart`, `Bell`, `Shield`, `Calendar`, `Chevron`, `Plus`), overlays (`Flame`).
2. **Duotone/gradient icons** — resource icons only: `Diamond` (blue gradient + facet highlights) and `Leaf` (green gradient). These stay "real-object" looking because they represent in-game currency.

**Rules:**
- **No emoji, ever.** Breaks the sci-fi tone.
- **No unicode symbols** as icons (no `★`, `⚡`, `⚙`). The `Bolt` SVG replaces ⚡; the `Gear` SVG replaces ⚙.
- Icons always come from the `I.*` set in `Icons.jsx` — **never hand-roll new SVGs inline**. If a new icon is needed, add it to that file so it's reusable.
- CDN set as fallback for prototyping: **Lucide** (`https://unpkg.com/lucide-static`) is the closest match — 24px grid, 1.5–2px stroke, same visual weight. Flag any Lucide substitutions in PRs.
- Colors: nav/utility icons use `var(--fg-2)` inactive, `var(--fg-1)` active. Alert icons take semantic tokens (`--danger`, `--info`, `--energy-1`).

**Assets on disk:**
- `assets/reference-base-ui.png` — original screenshot reference
- `ui_kits/mobile/Icons.jsx` — full inline SVG icon set

> ⚠️ **Flagged substitutions.** No game-specific icon font or sprite sheet was provided, so the icon set is a **genre-faithful approximation**. Before shipping, replace with the game's real icon art (especially: resource icons for diamond/energy, nav icons, hero-role icons not yet included).

## Font substitutions (flagged for user)

| Role | Currently used | Ideal replacement |
|------|----------------|-------------------|
| Display / numerals | **Orbitron** (Google Fonts) | Licensed sci-fi face — Eurostile Extended, Bank Gothic, or a custom numeric |
| UI labels | **Rajdhani** (Google Fonts) | Teko, Oswald, or a custom condensed caps face |
| Body | **Inter** (Google Fonts) | Keep or swap for a humanist sans — SF Pro / Roboto Flex |

All three are loaded via `@import` at the top of `colors_and_type.css`. Drop new font files into `fonts/` and update the `@font-face` declarations to replace.
