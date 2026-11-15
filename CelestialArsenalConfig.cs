using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CelestialArsenal
{
    // Bisa diubah langsung di game: Settings -> Mod Configuration -> Celestial Arsenal
    public class CelestialArsenalConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        // ----- Meteor besar (klik kanan) -----
        [DefaultValue(1.5f)]
        [Range(0.3f, 5f)]
        [Increment(0.1f)]
        public float ChargeTimeSeconds;

        [DefaultValue(4f)]
        [Range(1f, 15f)]
        [Increment(0.5f)]
        public float BigCometDamageMultiplier;

        [DefaultValue(260)]
        [Range(100, 600)]
        [Increment(20)]
        public int ExplosionSize;

        // ----- Mana & cooldown -----
        // Mana per komet (klik kiri). 0 = gratis
        [DefaultValue(6)]
        [Range(0, 50)]
        public int CometManaCost;

        // Mana untuk satu meteor besar (klik kanan). 0 = gratis
        [DefaultValue(60)]
        [Range(0, 200)]
        public int BigCometManaCost;

        // Cooldown setelah meteor besar keluar. 0 = tanpa cooldown
        [DefaultValue(8f)]
        [Range(0f, 60f)]
        [Increment(1f)]
        public float RightClickCooldownSeconds;
    }
}
