// Planetfall Survival — Core UI chrome components.
// Matches reference screenshot. Uses window.I for iconography.

const { useState } = React;

// --------------- Avatar ---------------
function Avatar({ src, level = 42, size = 56 }) {
  return (
    <div className="pf-avatar" style={{ width: size, height: size }}>
      <div className="pf-avatar-frame">
        {src ? (
          <img src={src} alt="" />
        ) : (
          <div className="pf-avatar-placeholder">
            <svg viewBox="0 0 40 40" width="60%" height="60%" aria-hidden>
              <circle cx="20" cy="15" r="6" fill="#8B6CFF" opacity=".6"/>
              <path d="M8 34c2-6 7-9 12-9s10 3 12 9" fill="#8B6CFF" opacity=".6"/>
            </svg>
          </div>
        )}
      </div>
      <div className="pf-avatar-level">{level}</div>
    </div>
  );
}

// --------------- CombatPower ---------------
function CombatPower({ value = 813126736 }) {
  const formatted = value.toLocaleString('en-US');
  return (
    <div className="pf-cp">
      <div className="pf-cp-row">
        <span className="pf-cp-bolt"><window.I.Bolt size={20} color="#FFC24A"/></span>
        <span className="pf-cp-value">{formatted}</span>
      </div>
      <div className="pf-cp-label">COMBAT POWER</div>
    </div>
  );
}

// --------------- IconButton (square, for top-right) ---------------
function IconButton({ variant = 'ghost', children, badge, onClick, ariaLabel }) {
  return (
    <button
      className={`pf-iconbtn pf-iconbtn--${variant}`}
      onClick={onClick}
      aria-label={ariaLabel}
      type="button"
    >
      {children}
      {badge != null && <span className="pf-badge pf-badge--corner">{badge}</span>}
    </button>
  );
}

// --------------- ResourceChip ---------------
function ResourceChip({ icon, value, variant = 'default', onAdd, label }) {
  return (
    <div className={`pf-chip pf-chip--${variant}`} role="group" aria-label={label}>
      <span className="pf-chip-icon">{icon}</span>
      <span className="pf-chip-value">{value}</span>
      {onAdd && (
        <button className="pf-chip-add" onClick={onAdd} aria-label={`Add ${label}`}>
          <window.I.Plus size={10} color="#fff"/>
        </button>
      )}
    </div>
  );
}

// --------------- VIP Badge ---------------
function VipBadge({ level = 12 }) {
  return (
    <div className="pf-vip">
      <span className="pf-vip-label">VIP</span>
      <span className="pf-vip-level">{level}</span>
    </div>
  );
}

// --------------- Time Chip ---------------
function TimeChip({ children }) {
  return (
    <div className="pf-time">
      <window.I.Calendar size={12} color="#8A7BB8"/>
      <span>{children}</span>
    </div>
  );
}

// --------------- TopBar ---------------
function TopBar({ power = 813126736, onAvatar, onShop, onSettings }) {
  return (
    <div className="pf-topbar">
      <button className="pf-topbar-left" onClick={onAvatar} aria-label="Profile">
        <Avatar level={42} />
      </button>
      <div className="pf-topbar-center">
        <CombatPower value={power} />
      </div>
      <div className="pf-topbar-right">
        <IconButton variant="gold" onClick={onShop} ariaLabel="Shop" badge="NEW">
          <window.I.Cart size={22} color="#3A1A00"/>
        </IconButton>
        <IconButton variant="ghost" onClick={onSettings} ariaLabel="Settings">
          <window.I.Gear size={20} color="#C6B8E8"/>
        </IconButton>
      </div>
    </div>
  );
}

// --------------- ResourceStrip ---------------
function ResourceStrip({ time = 'UTC 03/09 · 03:42:52', vip = 12, diamonds = 35961, energy = '652M' }) {
  return (
    <div className="pf-resources">
      <TimeChip>{time}</TimeChip>
      <VipBadge level={vip} />
      <ResourceChip
        variant="diamond"
        icon={<window.I.Diamond size={16}/>}
        value={diamonds.toLocaleString('en-US')}
        label="Diamonds"
        onAdd={() => {}}
      />
      <ResourceChip
        variant="energy"
        icon={<window.I.Leaf size={16}/>}
        value={energy}
        label="Bio-energy"
        onAdd={() => {}}
      />
    </div>
  );
}

// --------------- BottomNav ---------------
const NAV_ITEMS = [
  { id: 'base',     label: 'BASE',     icon: 'Compass' },
  { id: 'heroes',   label: 'HEROES',   icon: 'Hero',     badge: 2 },
  { id: 'battle',   label: 'BATTLE',   icon: 'Sword' },
  { id: 'backpack', label: 'BACKPACK', icon: 'Backpack', badge: '99+' },
  { id: 'alliance', label: 'ALLIANCE', icon: 'Alliance' },
];

function BottomNav({ active = 'base', onChange }) {
  return (
    <nav className="pf-nav" aria-label="Primary">
      {NAV_ITEMS.map(item => {
        const Icon = window.I[item.icon];
        const isActive = item.id === active;
        return (
          <button
            key={item.id}
            className={`pf-nav-item ${isActive ? 'is-active' : ''}`}
            onClick={() => onChange?.(item.id)}
            aria-current={isActive ? 'page' : undefined}
          >
            <span className="pf-nav-icon-wrap">
              {isActive && <span className="pf-nav-active-ring" aria-hidden/>}
              <Icon size={26} color={isActive ? '#F4EEFF' : '#8A7BB8'}/>
              {item.badge != null && (
                <span className="pf-badge pf-badge--nav">{item.badge}</span>
              )}
            </span>
            <span className="pf-nav-label">{item.label}</span>
          </button>
        );
      })}
    </nav>
  );
}

// --------------- Canvas (3D viewport placeholder) ---------------
function Canvas({ children }) {
  return (
    <div className="pf-canvas">
      <div className="pf-canvas-stars" aria-hidden/>
      <div className="pf-canvas-vignette" aria-hidden/>
      {children}
    </div>
  );
}

// --------------- PrimaryButton (genre-standard gold CTA) ---------------
function PrimaryButton({ children, onClick, icon, size = 'md', disabled }) {
  return (
    <button
      className={`pf-btn pf-btn--gold pf-btn--${size}`}
      onClick={onClick}
      disabled={disabled}
      type="button"
    >
      {icon && <span className="pf-btn-icon">{icon}</span>}
      <span className="pf-btn-label">{children}</span>
    </button>
  );
}

// --------------- SecondaryButton ---------------
function SecondaryButton({ children, onClick, icon, size = 'md' }) {
  return (
    <button className={`pf-btn pf-btn--ghost pf-btn--${size}`} onClick={onClick} type="button">
      {icon && <span className="pf-btn-icon">{icon}</span>}
      <span className="pf-btn-label">{children}</span>
    </button>
  );
}

// --------------- Panel (generic chrome card) ---------------
function Panel({ children, title, className = '' }) {
  return (
    <div className={`pf-panel ${className}`}>
      {title && <div className="pf-panel-title">{title}</div>}
      {children}
    </div>
  );
}

Object.assign(window, {
  Avatar, CombatPower, IconButton, ResourceChip, VipBadge, TimeChip,
  TopBar, ResourceStrip, BottomNav, Canvas,
  PrimaryButton, SecondaryButton, Panel,
});
