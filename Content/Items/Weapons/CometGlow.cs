using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Items.Weapons
{
    // Helper glowmask (bagian pedang yang menyala tetap terang walau di tempat gelap)
    public static class CometGlow
    {
        public static Texture2D GetGlow(ModItem item) =>
            ModContent.Request<Texture2D>(item.Texture + "_Glow", AssetRequestMode.ImmediateLoad).Value;

        // Saat item tergeletak di tanah
        public static void DrawInWorld(ModItem modItem, SpriteBatch spriteBatch, float rotation, float scale)
        {
            Item item = modItem.Item;
            Texture2D tex = TextureAssets.Item[item.type].Value;
            Texture2D glow = GetGlow(modItem);

            Vector2 position = new Vector2(
                item.position.X - Main.screenPosition.X + item.width * 0.5f,
                item.position.Y - Main.screenPosition.Y + item.height - tex.Height * 0.5f + 2f);

            spriteBatch.Draw(glow, position, null, Color.White, rotation, glow.Size() * 0.5f, scale,
                SpriteEffects.None, 0f);
        }

        // Saat di inventory / hotbar
        public static void DrawInInventory(ModItem modItem, SpriteBatch spriteBatch, Vector2 position,
            Rectangle frame, Vector2 origin, float scale)
        {
            spriteBatch.Draw(GetGlow(modItem), position, frame, Color.White, 0f, origin, scale,
                SpriteEffects.None, 0f);
        }
    }

    // Glowmask saat pedang sedang diayun di tangan player
    public class CometGlowLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            int t = drawInfo.drawPlayer.HeldItem.type;
            return t == ModContent.ItemType<CometSword>() || t == ModContent.ItemType<CometSwordPhase2>();
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Item held = drawInfo.drawPlayer.HeldItem;
            if (held.ModItem == null || held.noUseGraphic)
                return;

            Texture2D itemTex = TextureAssets.Item[held.type].Value;
            Texture2D glow = CometGlow.GetGlow(held.ModItem);

            // Salin DrawData pedang yang baru digambar game, ganti texture-nya dengan glowmask
            for (int i = drawInfo.DrawDataCache.Count - 1; i >= 0; i--)
            {
                DrawData data = drawInfo.DrawDataCache[i];
                if (data.texture == itemTex)
                {
                    data.texture = glow;
                    data.color = Color.White;
                    drawInfo.DrawDataCache.Add(data);
                    break;
                }
            }
        }
    }
}