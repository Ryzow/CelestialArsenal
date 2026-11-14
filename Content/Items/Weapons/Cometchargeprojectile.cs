using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Projectile tak terlihat yang mengatur proses charge klik kanan
    public class CometChargeProjectile : ModProjectile
    {
        // Tidak perlu gambar sendiri, pakai texture kosong bawaan Terraria
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.None;

        // Lama charge diambil dari config (detik -> tick)
        private static int ChargeTime => (int)(ModContent.GetInstance<CelestialArsenalConfig>().ChargeTimeSeconds * 60f);

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
        }

        // Gambar pedang fase 2 TEGAK LURUS ke atas kepala (bukan miring seperti sprite aslinya)
        public override bool PreDraw(ref Color lightColor)
        {
            int itemType = ModContent.ItemType<CometSwordPhase2>();
            Texture2D tex = TextureAssets.Item[itemType].Value;
            Texture2D glow = CometGlow.GetGlow(ModContent.GetModItem(itemType));

            float progress = MathHelper.Clamp(Projectile.ai[0] / ChargeTime, 0f, 1f);
            float bob = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f) * 2f;
            Vector2 pos = Projectile.Center + new Vector2(0f, bob) - Main.screenPosition;

            // Sprite miring 45 derajat (ujung kanan atas) -> putar -45 derajat agar ujung menghadap atas
            float rotation = -MathHelper.PiOver4;
            float scale = 1f + 0.12f * progress;
            Vector2 origin = tex.Size() * 0.5f;

            Main.EntitySpriteDraw(tex, pos, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
            // Bagian menyala makin terang saat charge bertambah
            Main.EntitySpriteDraw(glow, pos, null, Color.White * (0.5f + 0.5f * progress), rotation, origin, scale,
                SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead || player.CCed)
            {
                Projectile.Kill();
                return;
            }

            // Pemilik melepas klik kanan sebelum charge penuh -> batal
            // (pakai controlUseTile = tombol klik kanan, bukan player.channel
            //  karena channel hanya mengikuti klik kiri)
            if (Main.myPlayer == Projectile.owner && !player.controlUseTile)
            {
                Projectile.Kill();
                return;
            }

            // Tahan player dalam pose "mengangkat pedang"
            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            if (Main.myPlayer == Projectile.owner)
                player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);

            Projectile.Center = player.Top + new Vector2(0f, -62f); // pusat pedang tegak di atas kepala

            Projectile.ai[0]++;
            float progress = MathHelper.Clamp(Projectile.ai[0] / ChargeTime, 0f, 1f);

            // ---------- Efek partikel charge ----------
            // Partikel api & biru tersedot ke tengah
            for (int i = 0; i < 2 + (int)(progress * 3f); i++)
            {
                Vector2 offset = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(90f, 150f);
                Vector2 pos = Projectile.Center + offset;
                Vector2 vel = -offset / 12f; // sampai ke tengah dalam ~12 tick
                int type = Main.rand.NextBool(3) ? DustID.BlueTorch : DustID.Torch;

                Dust d = Dust.NewDustPerfect(pos, type, vel, 100, default, 1.1f + progress * 1.2f);
                d.noGravity = true;
            }

            // Inti yang membesar di tengah
            if (Main.rand.NextFloat() < 0.4f + progress * 0.6f)
            {
                Dust core = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f) * progress,
                    DustID.YellowTorch, Vector2.Zero, 100, default, 0.8f + progress * 1.6f);
                core.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 1.0f * progress, 0.55f * progress, 0.2f * progress);

            // Suara tiap 30 tick agar terasa "mengisi"
            if (Projectile.ai[0] % 30 == 0 && Projectile.ai[0] < ChargeTime)
                SoundEngine.PlaySound(SoundID.Item15 with { Volume = 0.5f, Pitch = progress }, Projectile.Center);

            // ---------- Charge penuh ----------
            if (Projectile.ai[0] >= ChargeTime)
            {
                FireBigComet(player);
                Projectile.Kill();
            }
        }

        private void FireBigComet(Player player)
        {
            // Ledakan partikel saat charge selesai
            SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
            if (!Main.dedServ)
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center,
                    Main.rand.NextVector2Unit(), 5f, 6f, 20, 1500f, "CometCharge"));
            for (int i = 0; i < 50; i++)
            {
                int type = i % 3 == 0 ? DustID.BlueTorch : DustID.Torch;
                Dust d = Dust.NewDustPerfect(Projectile.Center, type,
                    Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(3f, 9f),
                    100, default, 2f);
                d.noGravity = true;
            }

            if (Main.myPlayer != Projectile.owner)
                return;

            // Komet besar jatuh dari langit ke arah kursor (ai[0] = 1 -> mode besar)
            Vector2 target = Main.MouseWorld;
            Vector2 spawn = new Vector2(target.X + Main.rand.Next(-120, 121), player.Center.Y - 800f);
            Vector2 dir = (target - spawn).SafeNormalize(Vector2.UnitY) * 16f;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawn, dir,
                ModContent.ProjectileType<CometProjectile>(),
                Projectile.damage, Projectile.knockBack, Projectile.owner, ai0: 1f);
        }
    }
}