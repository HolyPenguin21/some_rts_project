using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget
{
    public GameObject go;
    private Transform tr;
    private ParticleSystem bullet_part;

    private Unit target;

    public FollowTarget(GameObject go, ParticleSystem bullet_part)
    {
        this.go = go;
        this.tr = this.go.transform;
        this.bullet_part = bullet_part;
    }

    public void Set_Target(Unit target)
    {
        this.target = target;
    }

    public void Set_Position(Transform attackPoint)
    {
        Vector3 pos = (target.tr.position - attackPoint.position).normalized * 1.5f;
        tr.position = pos;
    }

    public void Set_Lifetime()
    {
        Vector3 dir = target.tr.position - tr.position;

        var main = bullet_part.main;
        main.startLifetime = dir.magnitude / main.startSpeed.constant - 0.025f;
    }

    public void Set_Active()
    {
        go.SetActive(true);
    }

    public void Remove_SomeTarget(Unit target)
    {
        if(this.target == target)

        this.target = null;
    }

    public void Remove_Target()
    {
        target = null;
    }

    public void Update()
    {
        if (target == null || !go.activeInHierarchy) return;

        tr.LookAt(target.tr);
    }
}
