using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    public class CometProjectile : ModProjectile
    {
        // Texture otomatis diambil dari CometProjectile.png

        // ai[0] == 1 -> komet BESAR (dari charge klik kanan)
        // ai[1] == 1 -> sedang meledak
        private bool Big => Projectile.ai[0] == 1f;
        private bool Exploding => Projectile.ai[1] == 1f;

        public override void SetStaticDefaults()
        {
            // CometProjectile.png = strip vertikal 4 frame (48x26 per frame)
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.light = 0.8f;

            // Tiap musuh hanya kena sekali per projectile (langsung + ledakan tidak dobel)
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            // Inisialisasi sekali di frame pertama
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                if (Big)
                {
                    Vector2 c = Projectile.Center;
                    Projectile.width = 56;
                    Projectile.height = 56;
                    Projectile.Center = c;
                    Projectile.scale = 2f; // kelipatan bulat agar pixel art tetap tajam
                    Projectile.penetrate = -1;
                    Projectile.timeLeft = 240;
                    Projectile.light = 1.5f;
                }
            }

            if (Exploding)
                return;

            // Sprite sudah diputar agar kepala menghadap kanan
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Animasi api: ganti frame tiap 4 tick
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            // Ekor api
            int count = Big ? 3 : 1;
            for (int i = 0; i < count; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Torch, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f,
                        100, default, Big ? 2.2f : 1.4f);
                    Main.dust[d].noGravity = true;
                }
            }
            if (Main.rand.NextBool(Big ? 2 : 5))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.BlueTorch, 0f, 0f, 100, default, Big ? 1.8f : 1f);
                Main.dust[d].noGravity = true;
            }
        }

        // Komet besar: meledak jadi area besar saat kena musuh / tanah
        private void Detonate()
        {
            if (Exploding)
                return;

            Projectile.ai[1] = 1f;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;

            Vector2 c = Projectile.Center;
            int size = ModContent.GetInstance<CelestialArsenalConfig>().ExplosionSize;
            Projectile.width = size;
            Projectile.height = size;
            Projectile.Center = c;

            Projectile.netUpdate = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Big)
            {
                Detonate();
                return false; // jangan hancur dulu, tunggu ledakan
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, Big ? 360 : 180);
            if (Big)
                Detonate();
        }

        // Sembunyikan sprite saat fase ledakan
        public override bool PreDraw(ref Color lightColor) => !Exploding;

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            // Screen shake: kuat untuk meteor besar, ringan untuk komet kecil
            if (!Main.dedServ)
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center,
                    Main.rand.NextVector2Unit(), Big ? 16f : 2.5f, 7f, Big ? 35 : 10, 2000f, "CometImpact"));

            int fire = Big ? 90 : 30;
            int blue = Big ? 40 : 12;
            float speed = Big ? 14f : 6f;

            for (int i = 0; i < fire; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch,
                    Main.rand.NextVector2Circular(speed, speed), 100, default, Big ? 3f : 2f);
                d.noGravity = true;
            }
            for (int i = 0; i < blue; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BlueTorch,
                    Main.rand.NextVector2Circular(speed * 0.8f, speed * 0.8f), 100, default, Big ? 2.2f : 1.5f);
                d.noGravity = true;
            }
        }
    }
}