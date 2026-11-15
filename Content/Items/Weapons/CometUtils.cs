using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Pembayaran mana yang menghormati pengurang mana (Mana Flower, armor, dll)
    public static class CometMana
    {
        public static int Cost(Player player, int baseCost)
        {
            if (baseCost <= 0)
                return 0;
            return Math.Max(1, (int)(baseCost * player.manaCost));
        }

        // true jika berhasil bayar (atau gratis). Tidak memicu minum potion otomatis.
        public static bool TryPay(Player player, int baseCost)
        {
            int cost = Cost(player, baseCost);
            if (cost <= 0)
                return true;

            if (player.CheckMana(cost, true, true))
            {
                player.manaRegenDelay = (int)player.maxRegenDelay; // regen mana tertunda seperti senjata magic
                return true;
            }
            return false;
        }

        // Baris tooltip dinamis (angka mengikuti config)
        public static void AddTooltips(Mod mod, List<TooltipLine> tooltips, bool hasCharge)
        {
            var cfg = ModContent.GetInstance<CelestialArsenalConfig>();

            if (cfg.CometManaCost > 0)
                tooltips.Add(new TooltipLine(mod, "CometMana",
                    Language.GetTextValue("Mods.CelestialArsenal.Common.CometMana", cfg.CometManaCost)));

            if (hasCharge && cfg.BigCometManaCost > 0)
                tooltips.Add(new TooltipLine(mod, "BigCometMana",
                    Language.GetTextValue("Mods.CelestialArsenal.Common.BigCometMana", cfg.BigCometManaCost)));

            if (hasCharge && cfg.RightClickCooldownSeconds > 0f)
                tooltips.Add(new TooltipLine(mod, "BigCometCooldown",
                    Language.GetTextValue("Mods.CelestialArsenal.Common.BigCometCooldown",
                        cfg.RightClickCooldownSeconds.ToString("0.#"))));
        }
    }

    // Screen shake yang menghormati config client
    public static class CometShake
    {
        public static void Punch(Vector2 position, float strength, float cyclesPerSecond, int frames,
            float falloffDistance, string id)
        {
            if (Main.dedServ)
                return;

            var cfg = ModContent.GetInstance<CelestialArsenalClientConfig>();
            if (!cfg.ScreenShake || cfg.ScreenShakeIntensity <= 0f)
                return;

            Main.instance.CameraModifiers.Add(new PunchCameraModifier(position, Main.rand.NextVector2Unit(),
                strength * cfg.ScreenShakeIntensity, cyclesPerSecond, frames, falloffDistance, id));
        }
    }
}
