using Npgsql;
using System;
using System.Collections.Generic;

namespace SpaceShipsApp
{
    public class DatabaseManager
    {
        private readonly string connectionString;

        public DatabaseManager(string connString)
        {
            connectionString = connString;
        }

        public void AddShip(CosmicShip ship)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd1 = new NpgsqlCommand(
                            "INSERT INTO cosmic_ships (name, max_speed, ship_type) VALUES (@name, @maxspeed, @shiptype) RETURNING id",
                            conn);
                        cmd1.Parameters.AddWithValue("name", ship.Name);
                        cmd1.Parameters.AddWithValue("maxspeed", ship.MaxSpeed);
                        cmd1.Parameters.AddWithValue("shiptype", ship is CombatShip ? "Combat" : "Research");
                        long id = (long)cmd1.ExecuteScalar(); // Изменено на long

                        if (ship is CombatShip combatShip)
                        {
                            var cmd2 = new NpgsqlCommand(
                                "INSERT INTO combat_ships (id, weapon_power, armor_level) VALUES (@id, @weaponpower, @armorlevel)",
                                conn);
                            cmd2.Parameters.AddWithValue("id", id);
                            cmd2.Parameters.AddWithValue("weaponpower", combatShip.WeaponPower);
                            cmd2.Parameters.AddWithValue("armorlevel", combatShip.ArmorLevel);
                            cmd2.ExecuteNonQuery();
                        }
                        else if (ship is ResearchShip researchShip)
                        {
                            var cmd2 = new NpgsqlCommand(
                                "INSERT INTO research_ships (id, scan_range, storage_volume) VALUES (@id, @scanrange, @storagevolume)",
                                conn);
                            cmd2.Parameters.AddWithValue("id", id);
                            cmd2.Parameters.AddWithValue("scanrange", researchShip.ScanRange);
                            cmd2.Parameters.AddWithValue("storagevolume", researchShip.StorageVolume);
                            cmd2.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        ship.Id = id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<CosmicShip> GetAllShips()
        {
            var ships = new List<CosmicShip>();
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(
                        "SELECT cs.id, cs.name, cs.max_speed, cs.ship_type, co.weapon_power, co.armor_level, re.scan_range, re.storage_volume " +
                        "FROM cosmic_ships cs " +
                        "LEFT JOIN combat_ships co ON cs.id = co.id " +
                        "LEFT JOIN research_ships re ON cs.id = re.id",
                        conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt64(0); // Используем GetInt64
                            var name = reader["name"].ToString();
                            var maxSpeed = Convert.ToDouble(reader["max_speed"]);
                            var shipType = reader["ship_type"].ToString();
                            if (shipType == "Combat")
                            {
                                var weaponPower = reader["weapon_power"] != DBNull.Value ? Convert.ToInt32(reader["weapon_power"]) : 0;
                                var armorLevel = reader["armor_level"] != DBNull.Value ? Convert.ToInt32(reader["armor_level"]) : 0;
                                ships.Add(new CombatShip(name, maxSpeed, weaponPower, armorLevel) { Id = id }); // Используем id как long
                            }
                            else if (shipType == "Research")
                            {
                                var scanRange = reader["scan_range"] != DBNull.Value ? Convert.ToInt32(reader["scan_range"]) : 0;
                                var storageVolume = reader["storage_volume"] != DBNull.Value ? Convert.ToInt32(reader["storage_volume"]) : 0;
                                ships.Add(new ResearchShip(name, maxSpeed, scanRange, storageVolume) { Id = id }); // Используем id как long
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return ships;
            }
            return ships;
        }

        public void UpdateShip(CosmicShip ship)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd1 = new NpgsqlCommand(
                            "UPDATE cosmic_ships SET name = @name, max_speed = @maxspeed, ship_type = @shiptype WHERE id = @id",
                            conn);
                        cmd1.Parameters.AddWithValue("id", ship.Id);
                        cmd1.Parameters.AddWithValue("name", ship.Name);
                        cmd1.Parameters.AddWithValue("maxspeed", ship.MaxSpeed);
                        cmd1.Parameters.AddWithValue("shiptype", ship is CombatShip ? "Combat" : "Research");
                        cmd1.ExecuteNonQuery();

                        if (ship is CombatShip combatShip)
                        {
                            var cmd2 = new NpgsqlCommand(
                                "INSERT INTO combat_ships (id, weapon_power, armor_level) VALUES (@id, @weaponpower, @armorlevel) " +
                                "ON CONFLICT (id) DO UPDATE SET weapon_power = @weaponpower, armor_level = @armorlevel",
                                conn);
                            cmd2.Parameters.AddWithValue("id", ship.Id);
                            cmd2.Parameters.AddWithValue("weaponpower", combatShip.WeaponPower);
                            cmd2.Parameters.AddWithValue("armorlevel", combatShip.ArmorLevel);
                            cmd2.ExecuteNonQuery();
                        }
                        else if (ship is ResearchShip researchShip)
                        {
                            var cmd2 = new NpgsqlCommand(
                                "INSERT INTO research_ships (id, scan_range, storage_volume) VALUES (@id, @scanrange, @storagevolume) " +
                                "ON CONFLICT (id) DO UPDATE SET scan_range = @scanrange, storage_volume = @storagevolume",
                                conn);
                            cmd2.Parameters.AddWithValue("id", ship.Id);
                            cmd2.Parameters.AddWithValue("scanrange", researchShip.ScanRange);
                            cmd2.Parameters.AddWithValue("storagevolume", researchShip.StorageVolume);
                            cmd2.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteShip(long id) // Изменено с int на long
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd2 = new NpgsqlCommand("DELETE FROM combat_ships WHERE id = @id", conn);
                        cmd2.Parameters.AddWithValue("id", id);
                        cmd2.ExecuteNonQuery();

                        var cmd3 = new NpgsqlCommand("DELETE FROM research_ships WHERE id = @id", conn);
                        cmd3.Parameters.AddWithValue("id", id);
                        cmd3.ExecuteNonQuery();

                        var cmd1 = new NpgsqlCommand("DELETE FROM cosmic_ships WHERE id = @id", conn);
                        cmd1.Parameters.AddWithValue("id", id);
                        cmd1.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}