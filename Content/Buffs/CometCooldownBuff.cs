using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialArsenal.Content.Buffs
{
    // Cooldown meteor besar. Tampil di HUD dengan hitung mundur otomatis.
    // Texture: CometCooldownBuff.png (32x32)
    public class CometCooldownBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        // Cooldown tidak bisa dibatalkan dengan klik kanan pada ikon buff
        public override bool RightClick(int buffIndex) => false;

        public override void Update(Player player, ref int buffIndex)
        {
            // Tanda "siap dipakai lagi" saat cooldown habis
            if (player.buffTime[buffIndex] == 1 && player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                for (int i = 0; i < 12; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.Center, DustID.BlueTorch,
                        Main.rand.NextVector2Circular(3f, 3f), 100, default, 1.4f);
                    d.noGravity = true;
                }
            }
        }
    }
}
