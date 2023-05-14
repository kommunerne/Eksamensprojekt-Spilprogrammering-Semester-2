using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBScript : MonoBehaviour {

    private string dbName;
    
    private void Start() {
        dbName = "URI=file:" + Application.dataPath + "/Database/SpilprogrammeringDB.db";
        //dbName = "URI=file:spilprogrammeringDB.db"; // For use in the build version
        
        CreateDB();
        Player player = new Player("RonaldMacDonald", 1234, 1, 1, 0, 0);
        CreatePlayer(player);
        GetPlayer("RonaldMacDonald", 1234);
        Player playerUpdate = new Player("ArnoldMacGarnold", 4321, 1, 1, 0, 0);

        UpdatePlayer("RonaldMacDonald", 1234, playerUpdate);
    }

    public void CreateDB() {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {

                string commandText = "CREATE TABLE IF NOT EXISTS players " +
                                     "(Username TEXT, " +
                                     "PinCode INT, " +
                                     "PrefabNr INT, " +
                                     "Level INT, " +
                                     "Exp INT, " +
                                     "Score INT);";

                command.CommandText = commandText;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
    

    public void CreatePlayer(Player playerToCreate) {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {
                
                string playerInsert = 
                    "INSERT INTO Players (Username, PinCode, PrefabNr, Level, Exp, Score) " +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');";

                command.CommandText = string.Format(playerInsert, playerToCreate.username, playerToCreate.pinCode, playerToCreate.prefabNr, playerToCreate.level, playerToCreate.exp, playerToCreate.score);
                Debug.Log("After player insert statement creation in CreatePlayer");

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public void UpdatePlayer(string username, int pinCode, Player playerToUpdate) {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {
                string playerUpdate = "UPDATE Players " +
                                      "SET Username = @Username, " +
                                      "PinCode = @PinCode, " +
                                      "PrefabNr = @PrefabNr, " +
                                      "Level = @Level, " +
                                      "Exp = @Exp, " +
                                      "Score = @Score " +
                                      "WHERE Username = @UsernameOriginal AND PinCode = @PinCodeOriginal;";
                
                command.Parameters.Add(new SqliteParameter("@Username", playerToUpdate.username));
                command.Parameters.Add(new SqliteParameter("@PinCode", playerToUpdate.pinCode));
                command.Parameters.Add(new SqliteParameter("@PrefabNr", playerToUpdate.prefabNr));
                command.Parameters.Add(new SqliteParameter("@Level", playerToUpdate.level));
                command.Parameters.Add(new SqliteParameter("@Exp", playerToUpdate.exp));
                command.Parameters.Add(new SqliteParameter("@Score", playerToUpdate.score));
                
                command.Parameters.Add(new SqliteParameter("@UsernameOriginal", username));
                command.Parameters.Add(new SqliteParameter("@PinCodeOriginal", pinCode));

                command.CommandText = playerUpdate;
                command.ExecuteNonQuery();
                connection.Close();
                
            }
        }
    }

    public Player GetPlayer(string username, int pinCode) {
        Player playerToGet;
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {

                command.CommandText = "SELECT * FROM players WHERE Username LIKE @param1 AND PinCode LIKE @param2;";
                command.CommandType = CommandType.Text; // Done to make things easier on the database
                command.Parameters.Add(new SqliteParameter("@param1", username));
                command.Parameters.Add(new SqliteParameter("@param2", pinCode));
                
                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        playerToGet = new Player(reader["Username"].ToString(), Convert.ToInt32(reader["PinCode"]), Convert.ToInt32(reader["PrefabNr"]), Convert.ToInt32(reader["Level"]), Convert.ToInt32(reader["Exp"]), Convert.ToInt32(reader["Score"]));
                        
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
