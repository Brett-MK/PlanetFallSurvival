// Base screen — main "hub" of Planetfall Survival.
// Uses TopBar, ResourceStrip, Canvas, BottomNav + floating base overlays.

const { useState: useStateBase } = React;

function BaseScreen() {
  const [active, setActive] = React.useState('base');

  return (
    <div className="pf-device">
      <div className="pf-screen">
        <div className="pf-notch" aria-hidden/>
        <div className="pf-statusbar">
          <span>9:41</span>
          <div className="pf-statusbar-icons">
            <svg width="16" height="10" viewBox="0 0 16 10" aria-hidden><path d="M1 9h2V5H1zM5 9h2V3H5zM9 9h2V1H9zM13 9h2V0h-2z" fill="#F4EEFF"/></svg>
            <svg width="14" height="10" viewBox="0 0 14 10" aria-hidden><path d="M7 0a9 9 0 0 0-6.5 2.8L7 9.5l6.5-6.7A9 9 0 0 0 7 0Z" fill="#F4EEFF"/></svg>
            <svg width="22" height="10" viewBox="0 0 22 10" aria-hidden>
              <rect x=".5" y=".5" width="18" height="9" rx="2" fill="none" stroke="#F4EEFF" opacity=".4"/>
              <rect x="2" y="2" width="13" height="6" rx="1" fill="#F4EEFF"/>
              <rect x="19" y="3" width="1.5" height="4" rx=".75" fill="#F4EEFF" opacity=".5"/>
            </svg>
          </div>
        </div>

        <window.TopBar power={813126736} />
        <window.ResourceStrip
          time="UTC 03/09 · 03:42:52"
          vip={12}
          diamonds={35961}
          energy="652M"
        />

        <window.Canvas>
          {/* Scene placeholder — "Outpost K-7" */}
          <div className="pf-scene">
            <div className="pf-planet" aria-hidden/>
            <div className="pf-outpost" aria-hidden>
              <svg viewBox="0 0 220 180" width="220" height="180">
                <defs>
                  <linearGradient id="domeG" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0" stopColor="#8B6CFF" stopOpacity=".55"/>
                    <stop offset="1" stopColor="#2E1E5C" stopOpacity=".3"/>
                  </linearGradient>
                  <linearGradient id="hullG" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0" stopColor="#4A3A7A"/>
                    <stop offset="1" stopColor="#1A0F3D"/>
                  </linearGradient>
                </defs>
                {/* ground */}
                <ellipse cx="110" cy="160" rx="100" ry="14" fill="#1A0F3D" opacity=".8"/>
                {/* dome */}
                <path d="M40 150 Q110 40 180 150 Z" fill="url(#domeG)" stroke="#8B6CFF" strokeWidth="1.5" opacity=".9"/>
                {/* hab modules */}
                <rect x="55" y="120" width="40" height="32" rx="4" fill="url(#hullG)" stroke="#8B6CFF" strokeWidth="1"/>
                <rect x="100" y="108" width="50" height="44" rx="5" fill="url(#hullG)" stroke="#8B6CFF" strokeWidth="1"/>
                <rect x="155" y="122" width="30" height="30" rx="3" fill="url(#hullG)" stroke="#8B6CFF" strokeWidth="1"/>
                {/* lights */}
                <circle cx="72" cy="132" r="2" fill="#FFC24A"/>
                <circle cx="82" cy="132" r="2" fill="#FFC24A"/>
                <circle cx="120" cy="122" r="2" fill="#6DD5FF"/>
                <circle cx="135" cy="122" r="2" fill="#6DD5FF"/>
                <circle cx="165" cy="135" r="2" fill="#7BE89B"/>
                <circle cx="175" cy="135" r="2" fill="#7BE89B"/>
                {/* antenna */}
                <path d="M125 108 L125 75 M120 78 L130 78 M117 83 L133 83" stroke="#8B6CFF" strokeWidth="1.2" fill="none"/>
                <circle cx="125" cy="72" r="2.5" fill="#FF5A6E"/>
              </svg>
            </div>
          </div>

          {/* Left-side floating action stack */}
          <div className="pf-float-group pf-float-group--left">
            <button className="pf-float-btn" aria-label="Quests">
              <window.I.Flame size={22} color="#FFC24A"/>
              <span className="pf-float-btn-label">QUESTS</span>
            </button>
            <button className="pf-float-btn" aria-label="Mail">
              <window.I.Bell size={22} color="#6DD5FF"/>
              <span className="pf-float-btn-label">MAIL</span>
            </button>
          </div>

          {/* Right-side */}
          <div className="pf-float-group pf-float-group--right">
            <button className="pf-float-btn" aria-label="Shield">
              <window.I.Shield size={22} color="#7BE89B"/>
              <span className="pf-float-btn-label">SHIELD</span>
            </button>
          </div>

          {/* Bottom CTA row */}
          <div className="pf-float-group pf-float-group--bottom">
            <window.SecondaryButton size="sm">RESEARCH</window.SecondaryButton>
            <window.PrimaryButton size="sm">RECRUIT</window.PrimaryButton>
          </div>
        </window.Canvas>

        <window.BottomNav active={active} onChange={setActive} />
      </div>
    </div>
  );
}

window.BaseScreen = BaseScreen;
