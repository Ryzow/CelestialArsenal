using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Armor
{
    // Texture: StarfallGreaves.png (ikon), StarfallGreaves_Legs.png (dipakai di kaki)
    [AutoloadEquip(EquipType.Legs)]
    public class StarfallGreaves : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 100);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
            player.statDefense += 12;
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
