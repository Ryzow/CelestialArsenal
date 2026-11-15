using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Senjata awal: pedang hybrid melee/magic. Tiap ayunan melepas bintang pengejar.
    // Texture: StarfallBlade.png = strip 4 frame (32x32 per frame)
    public class StarfallBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Animasi item: 4 frame, ganti tiap 6 tick
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.damage = 14;
            Item.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Item.knockBack = 4.5f;
            Item.crit = 4;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = ModContent.ProjectileType<StarfallStarProjectile>();
            Item.shootSpeed = 9f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Bintang butuh mana; tanpa mana pedang tetap bisa diayun
            if (!CometMana.TryPay(player, ModContent.GetInstance<CelestialArsenalConfig>().StarManaCost))
                return false;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            CometMana.AddStarTooltip(Mod, tooltips);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height,
                    DustID.YellowStarDust, 0f, 0f, 100, default, 1f);
                d.noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            foreach (int bar in new[] { ItemID.GoldBar, ItemID.PlatinumBar })
            {
                CreateRecipe()
                    .AddIngredient(bar, 10)
                    .AddIngredient(ItemID.FallenStar, 8)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
        }
    }
}
