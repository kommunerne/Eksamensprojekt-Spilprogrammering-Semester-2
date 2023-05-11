using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DBScript : MonoBehaviour {

    private string dbName;

    private void Start() {
        dbName = "URI=file:" + Application.dataPath + "/Database/SpilprogrammeringDB.db";
        
        CreateUpgrade(0);
        CreatePlayer("Arnold");
    }


    public void CreateDB() {}
    

    public void CreatePlayer(string name) {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {
                
                
                string playerInsert = "INSERT INTO Players (name, EXP, Health, UpgradeID, Score) VALUES ('{0}','{1}','{2}','{3}','{4}');";
                command.CommandText = string.Format(playerInsert, name, 0, 100, 0, 0);
                
                command.ExecuteNonQuery();
                Debug.Log("attempt made to insert player into db");
            }
        }
    }

    public void CreateUpgrade(int type) {
        using (SqliteConnection connection = new SqliteConnection(dbName)) {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand()) {

                string upgradeInsert = "INSERT INTO Upgrades (Type, MilestoneUpgrade) VALUES ('{0}','{1}');";
                command.CommandText = string.Format(upgradeInsert, type, 0);

                command.ExecuteNonQuery();
                
                Debug.Log("attempt made to insert upgrade into db");

            }
        }
    }


    
}
