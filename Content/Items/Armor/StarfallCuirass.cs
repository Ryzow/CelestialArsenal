using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Armor
{
    // Texture: StarfallCuirass.png (ikon), _Body.png dan _Arms.png (dipakai di badan)
    [AutoloadEquip(EquipType.Body)]
    public class StarfallCuirass : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 29;
            Item.value = Item.sellPrice(silver: 120);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<MeleeMagicDamageClass>() += 4;
        }

        // Set lengkap: Hood + Cuirass + Greaves
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<StarfallHood>()
                && legs.type == ModContent.ItemType<StarfallGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.CelestialArsenal.SetBonus.Starfall");
            player.manaCost -= 0.15f; // juga menurunkan biaya mana komet dan bintang
            player.GetDamage<MeleeMagicDamageClass>() += 0.05f;
        }

        public override void AddRecipes()
        {
            foreach (int bar in new[] { ItemID.GoldBar, ItemID.PlatinumBar })
            {
                CreateRecipe()
                    .AddIngredient(bar, 12)
                    .AddIngredient(ItemID.FallenStar, 10)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
        }
    }
}
