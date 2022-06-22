using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private Unit unit;

    public int health_Cur;
    private int health_Max;

    public Health(Unit unit, int health_Max)
    {
        this.unit = unit;
        this.health_Max = health_Max;
        this.health_Cur = this.health_Max;
    }

    public void Reduce(int value)
    {
        health_Cur -= value;

        if (health_Cur <= 0)
        {
            Debug.Log("dead");
        }
    }

    public void Add(int value)
    {
        health_Cur += value;

        if (health_Cur > health_Max)
            health_Cur = health_Max;
    }
}
