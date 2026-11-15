using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Senjata awal: tombak hybrid melee/magic. Ujungnya memancarkan bintang saat menusuk.
    // Texture: StarfallLance.png = strip 4 frame (48x48 per frame)
    public class StarfallLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;

            Item.damage = 17;
            Item.DamageType = ModContent.GetInstance<MeleeMagicDamageClass>();
            Item.knockBack = 5.5f;
            Item.crit = 4;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true; // yang digambar adalah projectile tombaknya
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = ModContent.ProjectileType<StarfallLanceProjectile>();
            Item.shootSpeed = 3.2f;
        }

        // Hanya satu tombak aktif pada satu waktu
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            CometMana.AddStarTooltip(Mod, tooltips);
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
