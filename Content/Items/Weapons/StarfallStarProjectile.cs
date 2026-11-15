using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Bintang kecil dari Starfall Blade / Starfall Lance. Texture: strip 4 frame (16x16 per frame)
    public class StarfallStarProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.light = 0.4f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            // Animasi 4 frame
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            // Mengejar musuh terdekat dengan lembut
            NPC target = null;
            float best = 350f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(Projectile))
                    continue;
                float dist = Vector2.Distance(Projectile.Center, npc.Center);
                if (dist < best)
                {
                    best = dist;
                    target = npc;
                }
            }
            if (target != null)
            {
                float speed = Projectile.velocity.Length();
                Vector2 desired = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.08f);
            }

            // Jejak bintang
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.YellowStarDust,
                    -Projectile.velocity * 0.1f, 100, default, 1f);
                d.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.YellowStarDust,
                    Main.rand.NextVector2Circular(3f, 3f), 100, default, 1.2f);
                d.noGravity = true;
            }
        }
    }
}
