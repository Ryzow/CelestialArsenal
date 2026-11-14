using Terraria.ModLoader;

namespace CelestialArsenal
{
    // Damage class hybrid: mendapat bonus dari MELEE dan MAGIC sekaligus
    // (emblem, armor, aksesoris, buff, dll yang menambah melee ATAU magic akan berlaku)
    public class MeleeMagicDamageClass : DamageClass
    {
        // Stat apa saja yang diwarisi dari class lain
        // (damage, crit, attack speed, armor penetration, knockback)
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic
                || damageClass == DamageClass.Melee
                || damageClass == DamageClass.Magic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        // Efek spesial (misal on-hit effect) yang menganggap senjata ini sebagai melee / magic
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == DamageClass.Melee || damageClass == DamageClass.Magic;
        }
    }
}