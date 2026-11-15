using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Projectile tombak: menusuk maju-mundur lalu melepas satu bintang dari ujung.
    // Texture: StarfallLanceProjectile.png (48x48, ujung menghadap kanan atas)
    public class StarfallLanceProjectile : ModProjectile
    {
        private const float HoldoutRangeMin = 16f;
        private const float HoldoutRangeMax = 56f;
        private const float HalfLength = 30f; // jarak pusat sprite ke ujung tombak

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI;

            if (Projectile.timeLeft > duration)
                Projectile.timeLeft = duration;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            // Maju di paruh pertama, mundur di paruh kedua
            float half = duration * 0.5f;
            float progress = Projectile.timeLeft < half
                ? Projectile.timeLeft / half
                : (duration - Projectile.timeLeft) / half;

            Projectile.Center = player.MountedCenter +
                Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // Sprite menghadap kanan atas (-45 derajat) -> putar agar ujung searah gerak
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // Lepas satu bintang saat tombak mencapai titik terjauh
            if (Projectile.localAI[0] == 0f && Projectile.timeLeft <= half)
            {
                Projectile.localAI[0] = 1f;
                if (Main.myPlayer == Projectile.owner &&
                    CometMana.TryPay(player, ModContent.GetInstance<CelestialArsenalConfig>().StarManaCost))
                {
                    Vector2 tip = Projectile.Center + Projectile.velocity * HalfLength;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), tip, Projectile.velocity * 10f,
                        ModContent.ProjectileType<StarfallStarProjectile>(),
                        (int)(Projectile.damage * 0.8f), Projectile.knockBack, Projectile.owner);
                }
            }

            // Percikan bintang di ujung
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * HalfLength,
                    DustID.YellowStarDust, Vector2.Zero, 100, default, 1f);
                d.noGravity = true;
            }

            return false; // lewati AI bawaan
        }

        // Hitbox memanjang di sepanjang tombak, bukan hanya di pusat sprite
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 start = Projectile.Center - Projectile.velocity * HalfLength;
            Vector2 end = Projectile.Center + Projectile.velocity * HalfLength;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                start, end, 14f, ref point);
        }
    }
}
