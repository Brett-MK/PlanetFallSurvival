// Planetfall Survival — Screen components
// Heroes, Backpack, Alliance, Battle (new).
// Relies on window.I + window.{TopBar,ResourceStrip,BottomNav,Canvas,Avatar,...}

// ---------- Screen Header (back + title + optional right slot) ----------
function ScreenHeader({ title, onBack, right }) {
  return (
    <div className="pf-screen-header">
      <button className="pf-back-btn" onClick={onBack} aria-label="Back">
        <window.I.Chevron dir="left" size={18} color="#F4EEFF"/>
      </button>
      <div className="pf-screen-title">{title}</div>
      {right}
    </div>
  );
}

// ---------- Sort dropdown ----------
function Sort({ label = 'Power' }) {
  return (
    <button className="pf-sort">
      <span className="pf-sort-label">Sort:</span>
      <span>{label}</span>
      <window.I.Chevron dir="down" size={14} color="#C6B8E8"/>
    </button>
  );
}

// ---------- Tabs ----------
function Tabs({ tabs, active, onChange }) {
  return (
    <div className="pf-tabs">
      {tabs.map(t => (
        <button
          key={t}
          className={`pf-tab ${active === t ? 'is-active' : ''}`}
          onClick={() => onChange?.(t)}
        >{t}</button>
      ))}
    </div>
  );
}

// ---------- Stars row ----------
function Stars({ count = 5, filled = 5, type = 'lit' }) {
  return (
    <div className="pf-hero-stars">
      {Array.from({length: count}).map((_, i) => (
        <span key={i} className={`pf-star ${i < filled ? type : 'dim'}`}/>
      ))}
    </div>
  );
}

// ---------- Hero Card ----------
// Portrait SVG placeholder — colored silhouette with role tint
function HeroPortrait({ hue = 260, icon = 'user' }) {
  const Icon = {
    user:   <path d="M50 40a14 14 0 1 0 0-28 14 14 0 0 0 0 28Zm-24 38c0-14 10-22 24-22s24 8 24 22" fill="currentColor"/>,
    robot:  <g fill="currentColor"><rect x="30" y="22" width="40" height="34" rx="6"/><rect x="38" y="60" width="24" height="20" rx="3"/><circle cx="40" cy="36" r="3" fill="#fff"/><circle cx="60" cy="36" r="3" fill="#fff"/><rect x="44" y="44" width="12" height="4" rx="1" fill="#fff"/></g>,
    alien:  <g fill="currentColor"><ellipse cx="50" cy="36" rx="20" ry="24"/><ellipse cx="42" cy="34" rx="5" ry="8" fill="#fff"/><ellipse cx="58" cy="34" rx="5" ry="8" fill="#fff"/><path d="M30 64c4 10 12 16 20 16s16-6 20-16" stroke="#fff" strokeWidth="2" fill="none"/></g>,
    pilot:  <g fill="currentColor"><circle cx="50" cy="36" r="18"/><rect x="34" y="54" width="32" height="26" rx="4"/><rect x="40" y="28" width="20" height="10" rx="2" fill="#8B6CFF"/></g>,
    heavy:  <g fill="currentColor"><rect x="28" y="30" width="44" height="44" rx="6"/><rect x="36" y="24" width="28" height="10" rx="3"/><circle cx="40" cy="48" r="3" fill="#FF5A6E"/><circle cx="60" cy="48" r="3" fill="#FF5A6E"/></g>,
  }[icon];
  return (
    <svg viewBox="0 0 100 90" width="80%" height="80%" style={{ color: `hsl(${hue}, 70%, 72%)` }}>
      {Icon}
    </svg>
  );
}

function HeroCard({ name, level, tier, stars = 5, filled = 5, starType = 'lit', rank = 'S', newDot, portrait }) {
  return (
    <div className="pf-hero-card">
      <div className="pf-hero-portrait">
        <HeroPortrait hue={portrait?.hue} icon={portrait?.icon}/>
      </div>
      <div className="pf-hero-rank">
        <window.I.Shield size={14} color="#FFC24A"/>
      </div>
      <div className="pf-hero-tier">{rank}{tier}</div>
      {newDot && <span className="pf-hero-new-dot"/>}
      <div className="pf-hero-meta">
        <div className="pf-hero-level">Lv. {level}</div>
        <Stars count={stars} filled={filled} type={starType}/>
      </div>
    </div>
  );
}

// ---------- Heroes Screen ----------
function HeroesScreen() {
  const heroes = [
    { name: 'Voss',    level: 80, tier: 8,  rank:'S', filled:5, icon:'heavy',  hue: 260 },
    { name: 'Lyra',    level: 80, tier: 10, rank:'S', filled:5, icon:'alien',  hue: 320 },
    { name: 'Doran',   level: 80, tier: 9,  rank:'S', filled:5, icon:'pilot',  hue: 200 },
    { name: 'Kael',    level: 80, tier: 7,  rank:'S', filled:5, icon:'user',   hue: 30  },
    { name: 'Mira',    level: 80, tier: 7,  rank:'R', filled:4, icon:'pilot',  hue: 280 },
    { name: 'Ven',     level: 80, tier: 7,  rank:'R', filled:4, icon:'alien',  hue: 180 },
    { name: 'Torin',   level: 80, tier: 6,  rank:'S', filled:4, icon:'user',   hue: 220 },
    { name: 'Zara',    level: 80, tier: 6,  rank:'S', filled:4, icon:'robot',  hue: 310 },
    { name: 'Rook',    level: 80, tier: 5,  rank:'R', filled:3, icon:'heavy',  hue: 20, newDot:true },
    { name: 'Pax',     level: 80, tier: 8,  rank:'S', filled:3, icon:'alien',  hue: 140, newDot:true },
    { name: 'Greaves', level: 80, tier: 6,  rank:'R', filled:3, icon:'heavy',  hue: 240, newDot:true },
    { name: 'Soren',   level: 80, tier: 11, rank:'S', filled:3, icon:'pilot',  hue: 340, newDot:true },
    { name: 'Ada',     level: 80, tier: 4,  rank:'R', filled:2, icon:'user',   hue: 20  },
    { name: 'Nox',     level: 80, tier: 4,  rank:'R', filled:2, icon:'user',   hue: 260 },
    { name: 'Rhee',    level: 80, tier: 2,  rank:'R', filled:2, icon:'user',   hue: 300 },
    { name: 'Brond',   level: 80, tier: 10, rank:'S', filled:2, icon:'heavy',  hue: 40  },
  ];
  return (
    <div className="pf-screen-shell">
      <ScreenHeader title="Heroes" right={<Sort label="Power"/>}/>
      <div className="pf-heroes-body" style={{marginTop: 10}}>
        <div className="pf-heroes-grid">
          {heroes.map((h, i) => <HeroCard key={i} {...h}/>)}
        </div>
        <div style={{display:'grid', gridTemplateColumns:'1fr 1fr', gap: 10, marginTop: 14}}>
          <button className="pf-btn pf-btn--ghost" style={{width:'100%'}}>DRILL CAMP</button>
          <button className="pf-btn pf-btn--gold" style={{width:'100%'}}>RECRUIT</button>
        </div>
      </div>
    </div>
  );
}

// ---------- Backpack Cells ----------
function LootCell({ variant, icon, qty, count, glyph }) {
  return (
    <div className={`pf-cell pf-cell--${variant}`}>
      {qty && <span className="pf-cell-qty">{qty}</span>}
      <div className="pf-cell-icon">{icon || <span style={{fontSize: 28}}>{glyph}</span>}</div>
      <span className="pf-cell-count">{count}</span>
    </div>
  );
}
// Inline illustrated loot glyphs (sci-fi) — not emoji
function LootIcon({ kind, size = 40 }) {
  const wrap = (content) => <svg viewBox="0 0 48 48" width={size} height={size}>{content}</svg>;
  switch(kind){
    case 'diamond': return wrap(<><defs><linearGradient id="dg" x1="0" x2="0" y1="0" y2="1"><stop offset="0" stopColor="#C9F0FF"/><stop offset="1" stopColor="#1F7EB3"/></linearGradient></defs><path d="M10 18 24 6l14 12L24 42Z" fill="url(#dg)" stroke="#0E4E75" strokeWidth="1.2"/></>);
    case 'crystal': return wrap(<><defs><linearGradient id="cg" x1="0" x2="0" y1="0" y2="1"><stop offset="0" stopColor="#FFB8D2"/><stop offset="1" stopColor="#8B1E5C"/></linearGradient></defs><path d="M14 22 24 8l10 14-10 20Z" fill="url(#cg)" stroke="#4A0A2E" strokeWidth="1.2"/></>);
    case 'chest':   return wrap(<><rect x="8" y="18" width="32" height="22" rx="3" fill="#7C5A2C" stroke="#3A2A14" strokeWidth="1.5"/><path d="M8 24h32" stroke="#3A2A14" strokeWidth="1.5"/><rect x="20" y="22" width="8" height="6" fill="#FFC24A" stroke="#8F4E08"/><path d="M10 18c0-6 6-10 14-10s14 4 14 10" fill="#8E6C34" stroke="#3A2A14" strokeWidth="1.5"/></>);
    case 'vial':    return wrap(<><rect x="14" y="10" width="20" height="6" rx="1" fill="#6DD5FF" stroke="#0E4E75"/><path d="M16 16 14 40a3 3 0 0 0 3 3h14a3 3 0 0 0 3-3L32 16Z" fill="#6DD5FF" stroke="#0E4E75" strokeWidth="1.5"/><path d="M16 30h16" stroke="rgba(255,255,255,0.6)" strokeWidth="2"/></>);
    case 'ration':  return wrap(<><ellipse cx="24" cy="26" rx="16" ry="10" fill="#D9543D" stroke="#5C1A10" strokeWidth="1.5"/><path d="M10 26c2-4 8-6 14-6s12 2 14 6" stroke="#fff" strokeWidth="1.5" fill="none" opacity=".6"/></>);
    case 'ore':     return wrap(<><path d="M8 30 16 14h16l8 16-12 10h-8Z" fill="#C44848" stroke="#6B1818" strokeWidth="1.5"/><path d="M16 14l4 10 8-6" stroke="rgba(255,255,255,0.45)" strokeWidth="1.5" fill="none"/></>);
    case 'can':     return wrap(<><rect x="14" y="8" width="20" height="32" rx="3" fill="#D87516" stroke="#5C3208" strokeWidth="1.5"/><rect x="14" y="14" width="20" height="20" fill="#FFE0A8" stroke="#5C3208"/><path d="M20 24h8M20 28h8" stroke="#8F4E08" strokeWidth="1.2"/></>);
    case 'token':   return wrap(<><circle cx="24" cy="24" r="16" fill="#FFC24A" stroke="#8F4E08" strokeWidth="1.5"/><circle cx="24" cy="24" r="10" fill="none" stroke="#8F4E08" strokeWidth="1"/><path d="M19 24l4 4 7-8" stroke="#8F4E08" strokeWidth="2.2" fill="none" strokeLinecap="round" strokeLinejoin="round"/></>);
    case 'key':     return wrap(<><circle cx="16" cy="24" r="8" fill="none" stroke="#C6B8E8" strokeWidth="3"/><path d="M24 24h18M36 24v6M40 24v4" stroke="#C6B8E8" strokeWidth="3" strokeLinecap="round"/></>);
    default: return wrap(<circle cx="24" cy="24" r="14" fill="#8B6CFF"/>);
  }
}

function BackpackScreen() {
  const [tab, setTab] = React.useState('Resources');
  const resources = [
    ['epic','diamond','1','11,220'], ['rare','diamond','10','2,687'], ['epic','diamond','100','3,134'], ['legend','diamond','1K','343'],
    ['myth','crystal',null,'3,228'], ['legend','chest','',211], ['legend','ore',null,'350'], ['energy','vial','1K','1,787'],
    ['rare','vial','5K','661'], ['epic','vial','10K','128'], ['legend','vial','50K','8'], ['rare','can','10','115'],
    ['rare','chest',null,'2,910'], ['rare','chest',null,'2,037'], ['epic','chest',null,'7,977'], ['epic','chest',null,'340'],
    ['energy','chest',null,'11,863'], ['legend','chest',null,'14,672'], ['epic','chest',null,'187'], ['rare','chest',null,'25,242'],
    ['rare','ration','1','1,991'], ['epic','ration','10','370'], ['common','ration','100','1,060'], ['energy','ration','1K','113,213'],
  ];
  return (
    <div className="pf-screen-shell">
      <ScreenHeader title="Backpack" right={
        <button className="pf-back-btn"><window.I.Bell size={18} color="#C6B8E8"/></button>
      }/>
      <Tabs tabs={['Resources','Speedup','Bonus','Gear','Other']} active={tab} onChange={setTab}/>
      <div className="pf-heroes-body" style={{marginTop: 0, borderTopLeftRadius: 0, borderTopRightRadius: 0}}>
        <div className="pf-cells-grid">
          {resources.map(([v, icon, qty, count], i) => (
            <LootCell key={i} variant={v} qty={qty} count={count} icon={<LootIcon kind={icon} size={40}/>}/>
          ))}
        </div>
      </div>
    </div>
  );
}

// ---------- Alliance ----------
function AllianceScreen() {
  const items = [
    ['War',           'Sword',    1],
    ['Chests',        'Backpack', 5],
    ['Territory',     'Flame',    null],
    ['Battle',        'Shield',   null],
    ['Shop',          'Cart',     null],
    ['Tech',          'Gear',     null],
    ['Rankings',      'Bolt',     null],
    ['Help',          'Alliance', null],
  ];
  return (
    <div className="pf-screen-shell">
      <ScreenHeader title="Alliance"/>
      <div className="pf-alliance-card">
        <div className="pf-crest">
          <svg viewBox="0 0 60 70" width="60" height="70">
            <path d="M30 2 L58 12 L54 46 L30 66 L6 46 L2 12 Z" fill="#F59A2B" stroke="#8F4E08" strokeWidth="1.5"/>
            <path d="M30 14 L46 20 L43 40 L30 52 L17 40 L14 20 Z" fill="#D87516"/>
            <path d="M20 26 L30 20 L40 26 L40 38 L30 44 L20 38 Z" fill="#FFE08A" stroke="#8F4E08" strokeWidth="1"/>
            <text x="30" y="36" textAnchor="middle" fontFamily="Orbitron" fontSize="12" fontWeight="800" fill="#8F4E08">PF</text>
          </svg>
        </div>
        <div className="pf-alliance-info">
          <div className="pf-alliance-name">[PF] Nightfall</div>
          <div className="pf-alliance-row"><span className="k">Leader</span><span>Cmdr. Voss</span></div>
          <div className="pf-alliance-row"><span className="k">Power</span><span>76,545,810,157</span></div>
          <div className="pf-alliance-row"><span className="k">Rank</span><span>1</span></div>
          <div className="pf-alliance-row"><span className="k">Members</span><span>100/100</span></div>
        </div>
      </div>
      <div style={{margin: '10px', display:'flex', gap: 6, alignItems:'center'}}>
        <div className="pf-bar-lvl">11</div>
        <div className="pf-bar" style={{flex:1}}>
          <span className="pf-bar-fill" style={{width:'100%'}}></span>
          <span className="pf-bar-label">MAXED</span>
        </div>
      </div>
      <div className="pf-menu-grid">
        {items.map(([label, icon, badge]) => {
          const Icon = window.I[icon];
          return (
            <button key={label} className="pf-menu-btn">
              <span className="pf-menu-btn-icon"><Icon size={20} color="#C6B8E8"/></span>
              <span className="pf-menu-btn-label">{label}</span>
              {badge && <span className="pf-badge">{badge}</span>}
            </button>
          );
        })}
      </div>
    </div>
  );
}

// ---------- Battle Screen (NEW — PvE stage ladder) ----------
function BattleScreen() {
  const stages = [
    { n: 1, name: 'Crash Site',      sub: 'Cleared · 3/3', locked: false, stars: 3 },
    { n: 2, name: 'Ruined Hab',      sub: 'Cleared · 3/3', locked: false, stars: 3 },
    { n: 3, name: 'Fungal Grove',    sub: 'Cleared · 2/3', locked: false, stars: 2 },
    { n: 4, name: 'Xenomorph Nest',  sub: 'In progress',    locked: false, stars: 1, active: true },
    { n: 5, name: 'Plasma Fault',    sub: 'Locked',        locked: true,  stars: 0 },
    { n: 6, name: 'Derelict Cruiser',sub: 'Locked',        locked: true,  stars: 0 },
    { n: 7, name: 'Rogue AI Core',   sub: 'Locked',        locked: true,  stars: 0 },
  ];
  return (
    <div className="pf-screen-shell">
      <ScreenHeader title="Battle"/>
      <div className="pf-battle-hero">
        <div className="pf-battle-hero-bg"/>
        <div className="pf-chapter">
          <div>
            <div className="pf-chapter-num">CHAPTER 04</div>
            <div className="pf-chapter-name">XENO BIOME</div>
          </div>
          <div className="pf-chapter-nav">
            <button><window.I.Chevron dir="left" size={14} color="#F4EEFF"/></button>
            <button><window.I.Chevron dir="right" size={14} color="#F4EEFF"/></button>
          </div>
        </div>
        <div style={{display:'flex', justifyContent:'space-between', alignItems:'center', position:'relative'}}>
          <div style={{display:'flex', gap: 8}}>
            <div style={{width:48, height:48, borderRadius:12, background:'var(--grad-cell-epic)', display:'grid', placeItems:'center', boxShadow:'var(--shadow-cell)'}}>
              <LootIcon kind="diamond" size={30}/>
            </div>
            <div style={{width:48, height:48, borderRadius:12, background:'var(--grad-cell-energy)', display:'grid', placeItems:'center', boxShadow:'var(--shadow-cell)'}}>
              <LootIcon kind="vial" size={30}/>
            </div>
            <div style={{width:48, height:48, borderRadius:12, background:'var(--grad-cell-legend)', display:'grid', placeItems:'center', boxShadow:'var(--shadow-cell)'}}>
              <LootIcon kind="token" size={30}/>
            </div>
          </div>
          <button className="pf-btn pf-btn--gold">DEPLOY</button>
        </div>
      </div>

      <div className="pf-stage-list">
        {stages.map(s => (
          <div key={s.n} className={`pf-stage ${s.locked ? 'is-locked' : ''}`}>
            <div className="pf-stage-num">{s.locked ? <window.I.Shield size={14} color="#8A7BB8"/> : `04-${s.n}`}</div>
            <div className="pf-stage-info">
              <div className="pf-stage-name">{s.name}</div>
              <div className="pf-stage-sub">{s.sub}</div>
            </div>
            <div className="pf-stars-row pf-stage-stars">
              {[0,1,2].map(i => <span key={i} className={`pf-star ${i < s.stars ? 'gold' : 'dim'}`}/>)}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

Object.assign(window, {
  ScreenHeader, Sort, Tabs, Stars, HeroCard, HeroesScreen,
  LootCell, LootIcon, BackpackScreen, AllianceScreen, BattleScreen,
});
