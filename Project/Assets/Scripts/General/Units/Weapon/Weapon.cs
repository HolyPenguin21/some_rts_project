using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public Unit unit;

    public string name;
    public int dmg;
    public float range;
    public float reload_Max;
    public float reload_Cur;

    public string[] attackAnimation;

    public void Update()
    {
        Timers_Update();
    }

    public string Get_AttackAnimation()
    { 
        return attackAnimation[Random.Range(0, attackAnimation.Length)];
    }

    public void Set_Reload()
    {
        reload_Cur = reload_Max;
    }

    private void Timers_Update()
    {
        if (reload_Cur > 0)
            reload_Cur -= Time.deltaTime;
    }


}
