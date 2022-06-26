using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public Soldier(GameObject go, Player owner)
    {
        attackPoint = go.transform.Find("Mesh").
            transform.Find("Bip001").
            transform.Find("Bip001 Pelvis").
            transform.Find("Bip001 Spine").
            transform.Find("Bip001 R Clavicle").
            transform.Find("Bip001 R UpperArm").
            transform.Find("Bip001 R Forearm").
            transform.Find("Bip001 R Hand").
            transform.Find("WeaponContainer");

        Setup_Unit(go, owner);

        health = new Health(this, 100);
        weapon = new Rifle(this);
    }
}