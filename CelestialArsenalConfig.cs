using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CelestialArsenal
{
    // Bisa diubah langsung di game: Settings -> Mod Configuration -> Celestial Arsenal
    public class CelestialArsenalConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

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
    }
}