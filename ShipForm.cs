using System;
using System.Windows.Forms;

namespace SpaceShipsApp
{
    public partial class ShipForm : Form
    {
        public CosmicShip Ship { get; private set; }
        private readonly bool isCombat;

        public ShipForm(bool isCombat, CosmicShip ship = null)
        {
            this.isCombat = isCombat;
            InitializeComponent(ship);
        }

        private void InitializeComponent(CosmicShip ship)
        {
            this.Text = isCombat ? "Добавить/Редактировать боевой корабль" : "Добавить/Редактировать исследовательский корабль";
            this.Size = new System.Drawing.Size(400, 300);

            var nameLabel = new Label { Text = "Название:", Location = new System.Drawing.Point(10, 10) };
            var nameTextBox = new TextBox { Location = new System.Drawing.Point(120, 10), Size = new System.Drawing.Size(200, 20), Text = ship?.Name };
            var speedLabel = new Label { Text = "Макс. скорость (км/с):", Location = new System.Drawing.Point(10, 40) };
            var speedTextBox = new TextBox { Location = new System.Drawing.Point(120, 40), Size = new System.Drawing.Size(200, 20), Text = ship?.MaxSpeed.ToString() };
            var saveButton = new Button { Text = "Сохранить", Location = new System.Drawing.Point(120, 220), Size = new System.Drawing.Size(100, 30) };
            var cancelButton = new Button { Text = "Отмена", Location = new System.Drawing.Point(230, 220), Size = new System.Drawing.Size(100, 30) };

            Control[] specificControls;
            if (isCombat)
            {
                var weaponLabel = new Label { Text = "Мощность оружия:", Location = new System.Drawing.Point(10, 70) };
                var weaponTextBox = new TextBox { Location = new System.Drawing.Point(120, 70), Size = new System.Drawing.Size(200, 20), Text = ship is CombatShip ? ((CombatShip)ship).WeaponPower.ToString() : "80" };
                var armorLabel = new Label { Text = "Уровень брони:", Location = new System.Drawing.Point(10, 100) };
                var armorTextBox = new TextBox { Location = new System.Drawing.Point(120, 100), Size = new System.Drawing.Size(200, 20), Text = ship is CombatShip ? ((CombatShip)ship).ArmorLevel.ToString() : "50" };
                specificControls = new Control[] { weaponLabel, weaponTextBox, armorLabel, armorTextBox };
            }
            else
            {
                var scanLabel = new Label { Text = "Радиус сканирования (км):", Location = new System.Drawing.Point(10, 70) };
                var scanTextBox = new TextBox { Location = new System.Drawing.Point(120, 70), Size = new System.Drawing.Size(200, 20), Text = ship is ResearchShip ? ((ResearchShip)ship).ScanRange.ToString() : "1000" };
                var storageLabel = new Label { Text = "Объем хранилища (ТБ):", Location = new System.Drawing.Point(10, 100) };
                var storageTextBox = new TextBox { Location = new System.Drawing.Point(120, 100), Size = new System.Drawing.Size(200, 20), Text = ship is ResearchShip ? ((ResearchShip)ship).StorageVolume.ToString() : "50" };
                specificControls = new Control[] { scanLabel, scanTextBox, storageLabel, storageTextBox };
            }

            saveButton.Click += (s, e) =>
            {
                try
                {
                    string name = nameTextBox.Text;
                    if (string.IsNullOrEmpty(name))
                        throw new Exception("Название корабля не может быть пустым.");

                    if (!double.TryParse(speedTextBox.Text, out double maxSpeed) || maxSpeed < 0)
                        throw new Exception("Некорректная максимальная скорость.");
                    maxSpeed = Math.Min(maxSpeed, 10000.0); // MAX_SPEED

                    if (isCombat)
                    {
                        if (!int.TryParse(specificControls[1].Text, out int weaponPower) || weaponPower < 0)
                            throw new Exception("Некорректная мощность оружия.");
                        if (!int.TryParse(specificControls[3].Text, out int armorLevel) || armorLevel < 0)
                            throw new Exception("Некорректный уровень брони.");
                        weaponPower = Math.Min(weaponPower, 100); // MAX_WEAPON_POWER
                        armorLevel = Math.Min(armorLevel, 100); // MAX_ARMOR_LEVEL
                        Ship = new CombatShip(name, maxSpeed, weaponPower, armorLevel) { Id = ship?.Id ?? 0 };
                    }
                    else
                    {
                        if (!int.TryParse(specificControls[1].Text, out int scanRange) || scanRange < 0)
                            throw new Exception("Некорректный радиус сканирования.");
                        if (!int.TryParse(specificControls[3].Text, out int storageVolume) || storageVolume < 0)
                            throw new Exception("Некорректный объем хранилища.");
                        scanRange = Math.Min(scanRange, 5000); // MAX_SCAN_RANGE
                        storageVolume = Math.Min(storageVolume, 1000); // MAX_STORAGE_VOLUME
                        Ship = new ResearchShip(name, maxSpeed, scanRange, storageVolume) { Id = ship?.Id ?? 0 };
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { nameLabel, nameTextBox, speedLabel, speedTextBox, saveButton, cancelButton });
            this.Controls.AddRange(specificControls);
        }
    }
}