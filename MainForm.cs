using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpaceShipsApp
{
    public class MainForm : Form
    {
        private readonly DatabaseManager db;
        private List<CosmicShip> ships;

        public MainForm()
        {
            InitializeComponent();
            string connectionString = "Host=localhost;Port=5432;Database=SpaceShips;Username=postgres;Password=4579123";
            db = new DatabaseManager(connectionString);
            LoadShips();
        }

        private void InitializeComponent()
        {
            this.Text = "Управление космическими кораблями";
            this.Size = new System.Drawing.Size(800, 600);

            var shipsListBox = new ListBox { Name = "shipsListBox", Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(300, 400) };
            var addCombatButton = new Button { Name = "addCombatButton", Text = "Создать боевой корабль", Location = new System.Drawing.Point(320, 10), Size = new System.Drawing.Size(250, 30) };
            var addResearchButton = new Button { Name = "addResearchButton", Text = "Создать исследовательский корабль", Location = new System.Drawing.Point(320, 50), Size = new System.Drawing.Size(250, 30) };
            var editButton = new Button { Name = "editButton", Text = "Редактировать", Location = new System.Drawing.Point(320, 90), Size = new System.Drawing.Size(250, 30) };
            var deleteButton = new Button { Name = "deleteButton", Text = "Удалить", Location = new System.Drawing.Point(320, 130), Size = new System.Drawing.Size(250, 30) };
            var shipMenuButton = new Button { Name = "shipMenuButton", Text = "Меню корабля", Location = new System.Drawing.Point(320, 170), Size = new System.Drawing.Size(250, 30) };
            var outputTextBox = new TextBox { Name = "outputTextBox", Location = new System.Drawing.Point(10, 420), Size = new System.Drawing.Size(760, 150), Multiline = true, ReadOnly = true };

            shipsListBox.SelectedIndexChanged += (s, e) => { outputTextBox.Text = (shipsListBox.SelectedItem as CosmicShip)?.ShowInfo(); };
            addCombatButton.Click += (s, e) => AddShip(true);
            addResearchButton.Click += (s, e) => AddShip(false);
            editButton.Click += (s, e) => EditShip(shipsListBox.SelectedItem as CosmicShip);
            deleteButton.Click += (s, e) => DeleteShip(shipsListBox.SelectedItem as CosmicShip);
            shipMenuButton.Click += (s, e) => ShowShipMenu(shipsListBox.SelectedItem as CosmicShip);

            this.Controls.AddRange(new Control[] { shipsListBox, addCombatButton, addResearchButton, editButton, deleteButton, shipMenuButton, outputTextBox });
        }

        private void LoadShips()
        {
            ships = db.GetAllShips();
            var shipsListBox = this.Controls.OfType<ListBox>().FirstOrDefault(c => c.Name == "shipsListBox");
            if (shipsListBox != null)
            {
                shipsListBox.Items.Clear();
                foreach (var ship in ships)
                    shipsListBox.Items.Add(ship);
            }
        }

        private void AddShip(bool isCombat)
        {
            var form = new ShipForm(isCombat);
            if (form.ShowDialog() == DialogResult.OK)
            {
                db.AddShip(form.Ship);
                LoadShips();
            }
        }

        private void EditShip(CosmicShip ship)
        {
            if (ship == null) return;
            var form = new ShipForm(ship is CombatShip, ship);
            if (form.ShowDialog() == DialogResult.OK)
            {
                db.UpdateShip(form.Ship);
                LoadShips();
            }
        }

        private void DeleteShip(CosmicShip ship)
        {
            if (ship == null) return;
            if (MessageBox.Show($"Удалить корабль {ship.Name}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.DeleteShip(ship.Id);
                LoadShips();
            }
        }

        private void ShowShipMenu(CosmicShip ship)
        {
            if (ship == null) return;
            var form = new ShipMenuForm(ship);
            form.ShowDialog();
            db.UpdateShip(ship); 
            LoadShips();
        }
    }
}