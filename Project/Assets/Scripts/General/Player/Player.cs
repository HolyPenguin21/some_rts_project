using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public bool isAi;
    public string name;
    public Color color;

    public Player(string name, bool isAi, Color color)
    {
        this.isAi = isAi;
        this.name = name;
        this.color = color;
    }
}