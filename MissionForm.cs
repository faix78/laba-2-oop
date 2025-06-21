using System;
using System.Windows.Forms;

namespace SpaceShipsApp
{
    public partial class MissionForm : Form
    {
        public string Result { get; private set; }
        private readonly CosmicShip ship;

        public MissionForm(CosmicShip ship)
        {
            this.ship = ship;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Выбор миссии";
            this.Size = new System.Drawing.Size(300, 200);

            var missionComboBox = new ComboBox { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(260, 30) };
            var selectButton = new Button { Text = "Выбрать", Location = new System.Drawing.Point(10, 50), Size = new System.Drawing.Size(100, 30) };
            var cancelButton = new Button { Text = "Отмена", Location = new System.Drawing.Point(120, 50), Size = new System.Drawing.Size(100, 30) };

            missionComboBox.Items.AddRange(ship is CombatShip
                ? new[] { "Атака", "Защита базы" }
                : new[] { "Исследование космоса", "Приземление успешно" });

            selectButton.Click += (s, e) =>
            {
                Result = ship.SelectMission(missionComboBox.SelectedIndex + 1);
                this.Close();
            };
            cancelButton.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { missionComboBox, selectButton, cancelButton });
        }
    }
}