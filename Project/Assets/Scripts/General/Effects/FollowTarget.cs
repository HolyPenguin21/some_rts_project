using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget
{
    public GameObject go;
    private Transform tr;
    private ParticleSystem bullet_part;

    private Transform attackPoint;
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

    public void Set_InitialPosition(Transform attackPoint)
    {
        if (target == null) return;

        this.attackPoint = attackPoint;
        Vector3 dir = (target.tr.position - attackPoint.position).normalized * 1.5f;

        tr.position = this.attackPoint.position + dir;
    }

    private Vector3 Set_CurrentPosition()
    {
        if (target == null) return attackPoint.position;

        Vector3 dir = (target.tr.position - attackPoint.position).normalized * 1.5f;

        return attackPoint.position + dir;
    }

    public void Set_Lifetime()
    {
        if (target == null) return;

        Vector3 dist_Vector = target.tr.position - tr.position;

        var main = bullet_part.main;
        main.startLifetime = dist_Vector.magnitude / main.startSpeed.constant - 0.025f;
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

        tr.position = Set_CurrentPosition();

        Vector3 lookPos = target.tr.position + Vector3.up;
        tr.LookAt(lookPos);
    }
}
