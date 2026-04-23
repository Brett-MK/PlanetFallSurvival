// Shared icon primitives — clean line/duotone SVGs in sci-fi style.
// 24px base grid, 1.75 stroke weight, round caps. currentColor aware.
// Exported to window for cross-script access.

const I = {
  // ========= Top bar =========
  Bolt: ({ size = 18, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} aria-hidden>
      <path d="M13.5 2 4 14h6l-1.5 8L20 10h-6l-.5-8Z" fill={color} stroke={color} strokeWidth="0.5" strokeLinejoin="round"/>
    </svg>
  ),
  Cart: ({ size = 20, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <path d="M3 4h2l2.4 11.3a2 2 0 0 0 2 1.7h7.8a2 2 0 0 0 2-1.6L20.9 8H6"/>
      <circle cx="9.5" cy="20" r="1.4"/>
      <circle cx="17" cy="20" r="1.4"/>
    </svg>
  ),
  Gear: ({ size = 20, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <circle cx="12" cy="12" r="3"/>
      <path d="M19.4 15a1.7 1.7 0 0 0 .3 1.8l.1.1a2 2 0 1 1-2.8 2.8l-.1-.1a1.7 1.7 0 0 0-1.8-.3 1.7 1.7 0 0 0-1 1.5V21a2 2 0 1 1-4 0v-.1a1.7 1.7 0 0 0-1.1-1.5 1.7 1.7 0 0 0-1.8.3l-.1.1a2 2 0 1 1-2.8-2.8l.1-.1a1.7 1.7 0 0 0 .3-1.8 1.7 1.7 0 0 0-1.5-1H3a2 2 0 1 1 0-4h.1a1.7 1.7 0 0 0 1.5-1.1 1.7 1.7 0 0 0-.3-1.8l-.1-.1a2 2 0 1 1 2.8-2.8l.1.1a1.7 1.7 0 0 0 1.8.3h.1a1.7 1.7 0 0 0 1-1.5V3a2 2 0 1 1 4 0v.1a1.7 1.7 0 0 0 1 1.5 1.7 1.7 0 0 0 1.8-.3l.1-.1a2 2 0 1 1 2.8 2.8l-.1.1a1.7 1.7 0 0 0-.3 1.8v.1a1.7 1.7 0 0 0 1.5 1H21a2 2 0 1 1 0 4h-.1a1.7 1.7 0 0 0-1.5 1Z"/>
    </svg>
  ),
  Calendar: ({ size = 14, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <rect x="3" y="5" width="18" height="16" rx="2"/>
      <path d="M16 3v4M8 3v4M3 10h18"/>
    </svg>
  ),

  // ========= Resources =========
  Diamond: ({ size = 16 }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} aria-hidden>
      <defs>
        <linearGradient id="diaG" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0" stopColor="#C9F0FF"/>
          <stop offset=".5" stopColor="#6DD5FF"/>
          <stop offset="1" stopColor="#1F7EB3"/>
        </linearGradient>
      </defs>
      <path d="M4 9 12 3l8 6-8 12L4 9Z" fill="url(#diaG)" stroke="#0E4E75" strokeWidth="0.7" strokeLinejoin="round"/>
      <path d="M4 9h16M12 3v18M8 9l4 12M16 9l-4 12" stroke="#0E4E75" strokeWidth="0.5" fill="none" opacity=".55"/>
      <path d="M8 5.5 10 4" stroke="#fff" strokeWidth="1.2" strokeLinecap="round" opacity=".8"/>
    </svg>
  ),
  Leaf: ({ size = 16 }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} aria-hidden>
      <defs>
        <linearGradient id="leafG" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0" stopColor="#B8F5CC"/>
          <stop offset=".5" stopColor="#4CD980"/>
          <stop offset="1" stopColor="#1F8A48"/>
        </linearGradient>
      </defs>
      <path d="M20 4C10 4 4 10 4 18c0 1 .2 2 .4 2.6 1.1-4.3 4-7.6 8-9.3-3.1 2-5.3 5-6.2 8.9.6.2 1.3.3 2 .3 8 0 14-6 14-16Z" fill="url(#leafG)" stroke="#0E5F2D" strokeWidth="0.8" strokeLinejoin="round"/>
    </svg>
  ),

  // ========= Bottom nav =========
  Compass: ({ size = 26, color = 'currentColor', filled = false }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill={filled ? color : 'none'} stroke={color} strokeWidth="1.8" strokeLinejoin="round" aria-hidden>
      <circle cx="12" cy="12" r="9" fill={filled ? color : 'none'}/>
      <path d="m15.5 8.5-5 2-2 5 5-2 2-5Z" fill={filled ? '#fff' : 'none'} stroke={filled ? '#fff' : color} strokeLinejoin="round"/>
    </svg>
  ),
  Hero: ({ size = 24, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <circle cx="12" cy="8" r="4"/>
      <path d="M4 21c1.5-4 4.5-6 8-6s6.5 2 8 6"/>
    </svg>
  ),
  Sword: ({ size = 24, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <path d="m14.5 3 6.5.5.5 6.5L9 22l-7-7L14.5 3Z"/>
      <path d="m5 18 3 3M13.5 9.5l1.5 1.5"/>
    </svg>
  ),
  Backpack: ({ size = 24, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <path d="M6 8a4 4 0 0 1 4-4h4a4 4 0 0 1 4 4v10a3 3 0 0 1-3 3H9a3 3 0 0 1-3-3V8Z"/>
      <path d="M9 4a3 3 0 0 1 6 0M6 12h12M10 15h4"/>
    </svg>
  ),
  Alliance: ({ size = 24, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <circle cx="9" cy="9" r="3.2"/>
      <circle cx="17" cy="10" r="2.5"/>
      <path d="M3 19c1-3.2 3.5-5 6-5s5 1.8 6 5"/>
      <path d="M14.5 14.5c1-1 2-1.5 3-1.5 2 0 3.5 1.5 4 4.5"/>
    </svg>
  ),
  Globe: ({ size = 24, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <circle cx="12" cy="12" r="9"/>
      <path d="M3 12h18M12 3a14 14 0 0 1 0 18M12 3a14 14 0 0 0 0 18"/>
    </svg>
  ),

  // ========= Misc =========
  Plus: ({ size = 14, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="3" strokeLinecap="round" aria-hidden>
      <path d="M12 5v14M5 12h14"/>
    </svg>
  ),
  Chevron: ({ size = 14, color = 'currentColor', dir = 'right' }) => {
    const rot = { right: 0, left: 180, up: -90, down: 90 }[dir];
    return (
      <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="2.2" strokeLinecap="round" strokeLinejoin="round" style={{ transform: `rotate(${rot}deg)` }} aria-hidden>
        <path d="m9 6 6 6-6 6"/>
      </svg>
    );
  },
  Bell: ({ size = 18, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <path d="M6 8a6 6 0 0 1 12 0c0 5 2 6 2 7H4c0-1 2-2 2-7Z"/>
      <path d="M10 20a2 2 0 0 0 4 0"/>
    </svg>
  ),
  Shield: ({ size = 18, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden>
      <path d="M12 3 4 6v6c0 5 3.5 8 8 9 4.5-1 8-4 8-9V6l-8-3Z"/>
    </svg>
  ),
  Flame: ({ size = 16, color = 'currentColor' }) => (
    <svg viewBox="0 0 24 24" width={size} height={size} fill="none" stroke={color} strokeWidth="1.8" strokeLinejoin="round" aria-hidden>
      <path d="M12 3c1 4 5 5 5 10a5 5 0 1 1-10 0c0-3 2-4 2-7 2 1 3 1 3-3Z"/>
    </svg>
  ),
};

window.I = I;
