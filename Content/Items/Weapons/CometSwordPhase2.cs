using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // FASE 2 (Hardmode): upgrade dari CometSword.
    // Klik kiri: 2 komet. Klik kanan (tahan): charge lalu meteor besar.
    public class CometSwordPhase2 : ModItem
    {
        // Texture: CometSwordPhase2.png, glowmask: CometSwordPhase2_Glow.png

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;

            Item.damage = 70;
            Item.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Item.knockBack = 6.5f;
            Item.crit = 6;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Pink;

            SetLeftClickStats();
        }

        // ---------- Klik kiri: ayun pedang + 2 komet ----------
        private void SetLeftClickStats()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.channel = false;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<CometProjectile>();
            Item.shootSpeed = 15f;
        }

        // ---------- Klik kanan: charge (tahan) ----------
        private void SetRightClickStats()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.noUseGraphic = true; // pedang digambar sendiri oleh projectile charge (bilah tegak lurus ke atas)
            Item.channel = true;
            Item.autoReuse = false;
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<CometChargeProjectile>();
            Item.shootSpeed = 0f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CometChargeProjectile>()] > 0)
                return false;

            if (player.altFunctionUse == 2)
                SetRightClickStats();
            else
                SetLeftClickStats();

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<CometChargeProjectile>(),
                    (int)(damage * ModContent.GetInstance<CelestialArsenalConfig>().BigCometDamageMultiplier),
                    knockback * 2f, player.whoAmI);
                return false;
            }

            // Klik kiri: 2 komet dari langit, sedikit menyebar
            Vector2 target = Main.MouseWorld;
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawn = new Vector2(target.X + Main.rand.Next(-300, 301), player.Center.Y - 700f);
                Vector2 dir = (target - spawn).SafeNormalize(Vector2.UnitY) * Item.shootSpeed;
                Projectile.NewProjectile(source, spawn, dir, type, damage, knockback, player.whoAmI);
            }
            return false;
        }


        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height,
                    Main.rand.NextBool(3) ? DustID.BlueTorch : DustID.Torch, 0f, 0f, 100, default, 1.4f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            float rotation, float scale, int whoAmI)
        {
            CometGlow.DrawInWorld(this, spriteBatch, rotation, scale);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
            Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CometGlow.DrawInInventory(this, spriteBatch, position, frame, origin, scale);
        }

        // Upgrade: CometSword + 12 Soul of Light + 2 Unicorn Horn di Mythril/Orichalcum Anvil (Hardmode)
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CometSword>())
                .AddIngredient(ItemID.SoulofLight, 12)
                .AddIngredient(ItemID.UnicornHorn, 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}