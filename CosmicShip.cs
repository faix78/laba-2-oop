using System;

namespace SpaceShipsApp
{
    public abstract class CosmicShip
    {
        public long Id { get; set; } // Изменено с int на long
        public string Name { get; set; }
        public double MaxSpeed { get; set; }

        public CosmicShip(string name, double maxSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public virtual string ShowInfo()
        {
            return $"Название: {Name}\nМаксимальная скорость: {MaxSpeed} км/с";
        }

        public virtual string CalculateTrajectory()
        {
            return "Расчет траектории не реализован.";
        }

        public virtual string DetermineFuelConsumption()
        {
            return "Расчет расхода топлива не реализован.";
        }

        public virtual string SelectMission(int choice)
        {
            return "Выбор миссии: Подготовка к стандартной миссии.";
        }

        public virtual string SpecialAction1()
        {
            return "Действие не поддерживается для этого типа корабля.";
        }

        public virtual string SpecialAction2()
        {
            return "Действие не поддерживается для этого типа корабля.";
        }

        public virtual int GetSpecialProperty1() => 0;
        public virtual int GetSpecialProperty2() => 0;
    }
}