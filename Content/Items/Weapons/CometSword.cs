using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // FASE 1 (sebelum Hardmode): ayunan pedang + komet kecil dari langit.
    // Upgrade ke fase 2 di Mythril/Orichalcum Anvil.
    public class CometSword : ModItem
    {
        // Texture: CometSword.png, glowmask: CometSword_Glow.png

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;

            Item.damage = 28;
            Item.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Item.knockBack = 5f;
            Item.crit = 4;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Orange;

            Item.shoot = ModContent.ProjectileType<CometProjectile>();
            Item.shootSpeed = 14f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Komet butuh mana; kalau mana kurang, pedang tetap bisa diayun tanpa komet
            if (!CometMana.TryPay(player, ModContent.GetInstance<CelestialArsenalConfig>().CometManaCost))
                return false;

            // Komet kecil jatuh dari langit ke arah kursor
            Vector2 target = Main.MouseWorld;
            Vector2 spawn = new Vector2(target.X + Main.rand.Next(-250, 251), player.Center.Y - 700f);
            Vector2 dir = (target - spawn).SafeNormalize(Vector2.UnitY) * Item.shootSpeed;

            Projectile.NewProjectile(source, spawn, dir, type, damage, knockback, player.whoAmI);
            return false;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            CometMana.AddTooltips(Mod, tooltips, hasCharge: false);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height,
                    DustID.Torch, 0f, 0f, 100, default, 1.2f);
                Main.dust[d].noGravity = true;
            }
        }

        // ---------- Glowmask saat jatuh di tanah & di inventory ----------
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 12)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
