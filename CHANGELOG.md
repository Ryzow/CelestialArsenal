# Celestial Arsenal Changelog

## v0.2.1 - 2026-11-15
Early game update: the first weapons, armor set and accessory for the melee/magic hybrid class.

### Added
- **Starfall Blade** (pre-boss sword). Each swing releases a homing star. Animated 4-frame sprite.
- **Starfall Lance** (pre-boss spear). Thrusts forward and launches a homing star from the tip. Animated 4-frame sprite.
- **Starfall Star** projectile with a 4-frame animation, gentle homing and a star dust trail.
- **Starfall Set** (Hood, Cuirass, Greaves). Defense 4 / 5 / 3.
  - Hood: +4% melee/magic damage, +20 max mana.
  - Cuirass: +4% melee/magic critical strike chance.
  - Greaves: +5% movement speed.
  - **Set bonus:** -15% mana cost (also applies to comets and stars) and +5% melee/magic damage.
- **Starfall Charm** accessory: +6% melee/magic damage, +3% critical strike chance, +20 max mana.
- Server config: **Mana per star** (default 2, 0 = free).
- All new items are crafted at an Anvil from Gold or Platinum Bars and Fallen Stars.

### Notes
- The armor sprites worn on the body are a first pass and use a single pose for every animation frame, so the legs and arms can look slightly off while walking or swinging. They will be improved in a later update.

## v0.2 - 2026-11-15

### Added
- **Mana cost.** Comets from left click now use mana (default 6 per comet; Comet Sword Awakened fires 2 comets per swing). The giant meteor uses 60 mana by default, paid when the meteor is released.
  - Mana reduction (Mana Flower, armor bonuses, etc.) is respected.
  - Using the sword no longer drinks mana potions automatically.
  - Mana regeneration is delayed after use, like other magic weapons.
  - Without enough mana you can still swing the sword, only the comets do not fall.
- **Giant meteor cooldown.** After a giant meteor is released there is an 8 second cooldown, shown in the HUD as the **Comet Recharge** buff with a countdown. A sound and sparkle play when it is ready. The buff cannot be cancelled with a right click.
- **Server config** (Settings > Mod Configuration > Celestial Arsenal): mana per comet, mana per giant meteor, and cooldown seconds. Set a value to 0 to make it free.
- **Client config** (Celestial Arsenal (Client)): turn **screen shake** on or off, and a **screen shake intensity** slider from 0 to 1. This only affects your own game.
- Tooltips now show the mana cost and cooldown, using the numbers from the config.
- Config options now have readable names and descriptions.

### Changed
- **Screen shake** now runs through one shared system instead of separate code in each projectile, so every shake follows the client config.
  - The giant meteor explosion, the charge release and small comet impacts all scale with the intensity slider.
  - Shake strength still fades with distance from the explosion.
  - No shake is created at all when the toggle is off or the intensity is 0, and never on a dedicated server.
- Comet Sword Awakened cannot start charging while on cooldown or when mana is lower than the meteor cost.
- If your mana runs out during a charge, the meteor fizzles instead of firing.

### Fixed
- Compile error ("CelestialArsenal does not contain a definition for Content") caused by the mod class having the same name as the mod namespace.

## v0.1 - Initial release
- **Comet Sword**: pre-Hardmode melee/magic hybrid sword that calls a comet from the sky with each swing.
- **Comet Sword Awakened**: Hardmode upgrade (Comet Sword + 12 Soul of Light + 2 Unicorn Horn at a Mythril or Orichalcum Anvil). Fires 2 comets, and holding right click charges a giant meteor.
- Hybrid melee/magic damage class that benefits from both melee and magic bonuses.
- Glowmask on both swords, animated comet projectile, and charge particle effects.
- Screen shake on explosions.
