using System;
using System.Windows.Forms;

namespace SpaceShipsApp
{
    public partial class ActionForm : Form
    {
        public string Result { get; private set; }
        private readonly CosmicShip ship;

        public ActionForm(CosmicShip ship)
        {
            this.ship = ship;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Специальные действия";
            this.Size = new System.Drawing.Size(300, 200);

            var actionComboBox = new ComboBox { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(260, 30) };
            var executeButton = new Button { Text = "Выполнить", Location = new System.Drawing.Point(10, 50), Size = new System.Drawing.Size(100, 30) };
            var cancelButton = new Button { Text = "Отмена", Location = new System.Drawing.Point(120, 50), Size = new System.Drawing.Size(100, 30) };

            actionComboBox.Items.AddRange(ship is CombatShip
                ? new[] { "Атака на цель", "Укрепление брони" }
                : new[] { "Сканирование области", "Сохранение данных" });

            executeButton.Click += (s, e) =>
            {
                Result = actionComboBox.SelectedIndex == 0 ? ship.SpecialAction1() : ship.SpecialAction2();
                this.Close();
            };
            cancelButton.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { actionComboBox, executeButton, cancelButton });
        }
    }
}