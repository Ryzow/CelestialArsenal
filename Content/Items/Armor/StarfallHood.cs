using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Armor
{
    // Texture: StarfallHood.png (ikon), StarfallHood_Head.png (dipakai di kepala)
    [AutoloadEquip(EquipType.Head)]
    public class StarfallHood : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.value = Item.sellPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MeleeMagicDamageClass>() += 0.04f;
            player.statManaMax2 += 20;
        }

        public override void AddRecipes()
        {
            foreach (int bar in new[] { ItemID.GoldBar, ItemID.PlatinumBar })
            {
                CreateRecipe()
                    .AddIngredient(bar, 8)
                    .AddIngredient(ItemID.FallenStar, 6)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
        }
    }
}
