using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // Account info
    public string name { get; set; }
    public string pinCode { get; set; }
    
    // Player stats
    public int maxHp { get; set; } = 100; 
    public int currentHp { get; set; }
    public int dmg { get; set; }
    public float moveSpeed { get; set; }
    public int hpRegen { get; set; }
    public int level { get; set; }
    public int exp { get; set; }
    public int statPoints { get; set; }
    public int score { get; set; }
    public float fireRate { get; set; }
    
    // Ui info
    public float hpProgressBar { get; set; }
    public float dmgProgressBar { get; set; }
    public float firerateProgressBar { get; set; }
    public float moveSpeedProgressBar { get; set; }
    public float hpRegenProgressBar { get; set; }

    public Player(string name, string pinCode) {
        this.name = name;
        this.pinCode = pinCode;
    }
    
    public Player(string name, string pinCode, int maxHp, int currentHp, int dmg, float moveSpeed, int hpRegen, int level, int exp, int statPoints, int score, float fireRate, float hpProgressBar, float dmgProgressBar, float firerateProgressBar, float moveSpeedProgressBar, float hpRegenProgressBar) {
        this.name = name;
        this.pinCode = pinCode;
        this.maxHp = maxHp = 100;
        this.currentHp = currentHp;
        this.dmg = dmg;
        this.moveSpeed = moveSpeed;
        this.hpRegen = hpRegen;
        this.level = level;
        this.exp = exp;
        this.statPoints = statPoints;
        this.score = score;
        this.fireRate = fireRate;
        this.hpProgressBar = hpProgressBar;
        this.dmgProgressBar = dmgProgressBar;
        this.firerateProgressBar = firerateProgressBar;
        this.moveSpeedProgressBar = moveSpeedProgressBar;
        this.hpRegenProgressBar = hpRegenProgressBar;
    }

    
}
