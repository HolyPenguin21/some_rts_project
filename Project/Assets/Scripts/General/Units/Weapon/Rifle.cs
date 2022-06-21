using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public Rifle(Unit unit)
    {
        this.unit = unit;

        name = "Rifle";
        dmg = 10;
        range = 15;
        reload_Max = 2;

        attackAnimation = new string[1];
        attackAnimation[0] = "infantry_combat_shoot_burst";

        Setup();
    }
}
