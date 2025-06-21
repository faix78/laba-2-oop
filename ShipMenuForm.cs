using System;
using System.Windows.Forms;

namespace SpaceShipsApp
{
    public partial class ShipMenuForm : Form
    {
        private readonly CosmicShip ship;

        public ShipMenuForm(CosmicShip ship)
        {
            this.ship = ship;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"Меню корабля ({ship.Name})";
            this.Size = new System.Drawing.Size(400, 400);

            var outputTextBox = new TextBox { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(360, 200), Multiline = true, ReadOnly = true };
            var infoButton = new Button { Text = "Информация", Location = new System.Drawing.Point(10, 220), Size = new System.Drawing.Size(100, 30) };
            var propertiesButton = new Button { Text = "Свойства", Location = new System.Drawing.Point(120, 220), Size = new System.Drawing.Size(100, 30) };
            var missionButton = new Button { Text = "Миссии", Location = new System.Drawing.Point(230, 220), Size = new System.Drawing.Size(100, 30) };
            var actionsButton = new Button { Text = "Действия", Location = new System.Drawing.Point(10, 260), Size = new System.Drawing.Size(100, 30) };
            var closeButton = new Button { Text = "Закрыть", Location = new System.Drawing.Point(230, 300), Size = new System.Drawing.Size(100, 30) };

            infoButton.Click += (s, e) => outputTextBox.Text = ship.ShowInfo();
            propertiesButton.Click += (s, e) =>
            {
                string type = ship is CombatShip ? "Боевой" : "Исследовательский";
                outputTextBox.Text = $"*** Свойства ***\nТип: {type}\nСвойство 1: {ship.GetSpecialProperty1()}\nСвойство 2: {ship.GetSpecialProperty2()}";
            };
            missionButton.Click += (s, e) =>
            {
                var missionForm = new MissionForm(ship);
                missionForm.ShowDialog();
                outputTextBox.Text = missionForm.Result;
            };
            actionsButton.Click += (s, e) =>
            {
                var actionForm = new ActionForm(ship);
                actionForm.ShowDialog();
                outputTextBox.Text = actionForm.Result;
            };
            closeButton.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { outputTextBox, infoButton, propertiesButton, missionButton, actionsButton, closeButton });
        }
    }
}