using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBScript : MonoBehaviour {

    private string dbName;

    private void Awake()
    {
        // dbName = "URI=file:" + Application.dataPath + "/Database/SpilprogrammeringDB.db";
        // dbName = Directory.GetCurrentDirectory(); // For use in the build version

        // CreateDB();
    }
    private void Start() {
        dbName = "URI=file:" + Application.dataPath + "/Database/SpilprogrammeringDB.db";
        
        CreateDB();
        CreatePlayer("Arnold", "1234DummyPass");
        GetPlayer("Arnold", "1234DummyPass");
    }

    public void CreateDB() {
        Debug.Log("Begun creation of DB");
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {
                string commandText = "CREATE TABLE IF NOT EXISTS players (name TEXT, pinCode TEXT, " +
                                     "maxHp INTEGER, currentHp INTEGER, dmg INTEGER, fireRate REAL, moveSpeed REAL, " +
                                     "hpRegen INTEGER, level INTEGER, Exp INTEGER, statPoints INTEGER, score INTEGER, " +
                                     "hpProgressBar REAL, dmgProgressBar REAL, firerateProgressBar REAL, " +
                                     "moveSpeedProgressBar REAL, hpRegenProgressBar REAL);";

                command.CommandText = commandText;
                command.ExecuteNonQuery();
                connection.Close();
                Debug.Log("Executed CreateDB Sql command");
            }
        }
    }
    

    public void CreatePlayer(string playerName, string pinCode) {
        Debug.Log("Begun creation of player");
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            Debug.Log("Passed using statement");
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {
                
                
                string playerInsert = 
                    "INSERT INTO Players (name, pinCode, maxHp, currentHp, dmg, fireRate, moveSpeed, hpRegen, " +
                    "level, Exp, statPoints, score, hpProgressBar, dmgProgressBar, firerateProgressBar, " +
                    "moveSpeedProgressBar, hpRegenProgressBar) " +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'," +
                    "'{14}','{15}','{16}');";
                
                command.CommandText = string.Format(playerInsert, playerName, pinCode, 100, 100, 10, 1.0, 1.0, 1, 1, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0.0);
                
                command.ExecuteNonQuery();
                connection.Close();
                Debug.Log("Executed CreatePlayer SQL Command");
            }
        }
    }

    public void UpdatePlayer(string name) {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();
            
            
        }
    }

    public Player GetPlayer(string name, string pinCode) {
        Debug.Log("Begun creation of getPlayer");
        Player playerToGet;
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {

                command.CommandText = "SELECT * FROM players WHERE Name LIKE @param1 AND PinCode LIKE @param2";
                command.CommandType = CommandType.Text; // Done to make things easier on the database
                command.Parameters.Add(new SqliteParameter("@param1", name));
                command.Parameters.Add(new SqliteParameter("@param2", pinCode));
                
                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        
                        // playerToGet = new Player(reader["name"].ToString(), reader["pinCode"].ToString(), 
                        //     (int)reader["maxHp"], (int)reader["currentHp"],(int)reader["dmg"],
                        //     (float)(int)reader["moveSpeed"],(int)reader["hpRegen"],(int)reader["level"],
                        //     (int)reader["Exp"],(int)reader["statPoints"], (int)reader["score"],
                        //     (float)reader["fireRate"] ,(float)reader["hpProgressBar"],
                        //     (float)reader["dmgProgressBar"],
                        //     (float)reader["firerateProgressBar"],(float)reader["moveSpeedProgressBar"],
                        //     (float)reader["hpRegenProgressBar"]);
                        //

                        playerToGet = new Player(reader["name"].ToString(), reader["PinCode"].ToString());
                        
                        Debug.Log(playerToGet.name);
                        return playerToGet;

                    }
                }
                
                connection.Close();
            }
        }
        Debug.Log("failed to retrieve player");
        return null;
    }


    
}
