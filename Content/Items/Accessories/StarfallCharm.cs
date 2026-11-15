using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Accessories
{
    // Texture: StarfallCharm.png
    public class StarfallCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 23;
            Item.accessory = true;
            Item.value = Item.sellPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<MeleeMagicDamageClass>() += 0.06f;
            player.GetCritChance<MeleeMagicDamageClass>() += 3;
            player.statManaMax2 += 20;
        }

        public override void AddRecipes()
        {
            foreach (int bar in new[] { ItemID.GoldBar, ItemID.PlatinumBar })
            {
                CreateRecipe()
                    .AddIngredient(bar, 6)
                    .AddIngredient(ItemID.FallenStar, 8)
                    .AddIngredient(ItemID.Chain, 1)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
        }
    }
}
