using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animate
{
    private Unit unit;
    private NavMeshAgent agent;
    private Animation anim;

    private float idleTimer;

    public AnimationClip animClip_Cur;

    public Animate(Unit unit)
    {
        this.unit = unit;
        this.agent = this.unit.agent;
        this.anim = this.unit.go.transform.Find("Mesh").GetComponent<Animation>();
    }

    public void Update()
    {
        Movement();
        Timers_Update();
    }

    private void Movement()
    {
        if (unit.attack_cor != null) return;

        if (agent.velocity.magnitude > 0.1f)
        {
            anim.CrossFade("infantry_combat_run");
            anim["infantry_combat_run"].speed = (agent.velocity.magnitude * 1) / agent.speed;
        }
        else
        {
            if (idleTimer > 0 || unit.target != null)
                anim.CrossFade("infantry_combat_idle");
            else
                anim.CrossFade("infantry_guard_idle");
        }
    }

    public void Play_AttackAnimation(string animName)
    {
        anim.CrossFade(animName);
        animClip_Cur = anim.GetClip(animName);
    }

    public void End_Of_Action()
    {
        idleTimer = Random.Range(3.0f, 7.0f);
    }

    private void Timers_Update()
    {
        if (idleTimer > 0)
            idleTimer -= Time.deltaTime;
    }
}