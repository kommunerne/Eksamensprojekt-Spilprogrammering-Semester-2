using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    
    public string username { get; set; }
    public int pinCode { get; set; }
    public int prefabNr { get; set; }
    public int level { get; set; }
    public int exp { get; set; }
    public int score { get; set; }

    
    
    public Player(string username, int pinCode, int prefabNr, int level, int exp, int score) {
        this.username = username;
        this.pinCode = pinCode;
        this.prefabNr = prefabNr;
        this.level = level;
        this.exp = exp;
        this.score = score;
    }
}
