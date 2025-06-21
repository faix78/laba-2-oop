using System;

namespace SpaceShipsApp
{
    public class CombatShip : CosmicShip
    {
        public int WeaponPower { get; set; }
        public int ArmorLevel { get; set; }

        public CombatShip(string name, double maxSpeed, int weaponPower, int armorLevel)
            : base(name, maxSpeed)
        {
            WeaponPower = weaponPower;
            ArmorLevel = armorLevel;
        }

        public override string ToString()
        {
            return $"{Name} (Боевой)";
        }

        public override string ShowInfo()
        {
            return base.ShowInfo() + $"\nМощность оружия: {WeaponPower}\nУровень брони: {ArmorLevel}";
        }

        public override string SpecialAction1()
        {
            return $"Атака на цель выполнена. Мощность оружия: {WeaponPower}.";
        }

        public override string SpecialAction2()
        {
            return $"Броня укреплена. Уровень брони: {ArmorLevel}.";
        }
    }
}