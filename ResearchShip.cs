using System;

namespace SpaceShipsApp
{
    public class ResearchShip : CosmicShip
    {
        public int ScanRange { get; set; }
        public int StorageVolume { get; set; }

        public ResearchShip(string name, double maxSpeed, int scanRange, int storageVolume)
            : base(name, maxSpeed)
        {
            ScanRange = scanRange;
            StorageVolume = storageVolume;
        }

        public override string ToString()
        {
            return $"{Name} (Исследовательский)";
        }

        public override string ShowInfo()
        {
            return base.ShowInfo() + $"\nРадиус сканирования: {ScanRange} км\nОбъем хранилища: {StorageVolume} ед.";
        }

        public override string SpecialAction1()
        {
            return $"Сканирование области выполнено. Радиус сканирования: {ScanRange} км.";
        }

        public override string SpecialAction2()
        {
            return $"Данные сохранены. Объем хранилища: {StorageVolume} ед.";
        }
    }
}