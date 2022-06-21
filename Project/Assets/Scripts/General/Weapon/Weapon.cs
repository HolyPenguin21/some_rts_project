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

    public void Setup()
    {
        reload_Cur = reload_Max;
    }

    public void Update()
    {
        if (reload_Cur > 0)
            reload_Cur -= Time.deltaTime;
    }

    public IEnumerator Shoot()
    {
        unit.canMove = false;

        unit.animate.Play_AttackAnimation(attackAnimation[Random.Range(0, attackAnimation.Length)]);
        float aTimer = unit.animate.animClip_Cur.length;
        while (aTimer > 0)
        {
            aTimer -= Time.deltaTime;
            yield return null;
        }

        unit.animate.animClip_Cur = null;
        unit.canMove = true;
        reload_Cur = reload_Max;
    }
}
