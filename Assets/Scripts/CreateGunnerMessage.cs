using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public struct CreateGunnerMessage : NetworkMessage
{
    public string name; // Navn!... Burde v�re obvious hvad det her 
    public string pinCode; // String fordi en tekstfelt i main menu ikke kunne v�re andet end en string :( 
    public int prefabSelector; // V�lger type af tanks -- igennem en int

    public int level; // Spillerens nuværende level
    public int exp; // Spillerens nuværende exp
    public int score; // spillerens nuværende score

}
