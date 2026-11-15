using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CelestialArsenal
{
    // Pengaturan per-pemain (tidak mempengaruhi pemain lain di server)
    // Settings -> Mod Configuration -> Celestial Arsenal (Client)
    public class CelestialArsenalClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool ScreenShake;

        [DefaultValue(1f)]
        [Range(0f, 1f)]
        [Increment(0.1f)]
        public float ScreenShakeIntensity;
    }
}
