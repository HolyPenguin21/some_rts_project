using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit
{
    public GameObject go;
    public Transform tr;

    public NavMeshAgent agent;
    public Animate animate;

    private GameObject selectionCircle;
    public Transform attackPoint;

    public string name;
    public Player owner;
    public Unit target;
    public Weapon weapon;
    public Health health;

    public IEnumerator attack_cor;

    private Vector3 nextMove = Vector3.zero;

    public void Update()
    {
        Target_Track();

        Check_MovementStop();

        weapon.Update();
        animate.Update();
    }

    #region Target
    private void Target_Track()
    {
        if (target == null) return;

        if (Get_DistToTarget() > weapon.range)
        {
            Order_MoveTo(Utility.Get_NavMeshPoint(target.tr.position));
        }
        else
        {
            Target_Rotation();
            Target_Attack();
        }
    }

    private void Target_Rotation()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.LookRotation(target.tr.position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, rotation, Time.deltaTime * 20.0f);
    }

    private void Target_Attack()
    {
        if (agent.hasPath) agent.ResetPath();
        if (weapon.reload_Cur > 0 || attack_cor != null) return;

        attack_cor = Attack_cor();

        SceneController.scene.StartCoroutine(attack_cor);
        SceneController.scene.Activate_BulletEffect(attackPoint, target);
    }

    private IEnumerator Attack_cor()
    {
        string animName = weapon.Get_AttackAnimation();
        AnimationClip anim = animate.Get_AnimationClip(animName);
        float animTime = anim.length;
        
        animate.Play_AttackAnimation(animName);

        while (animTime > 0)
        {
            animTime -= Time.deltaTime;
            yield return null;
        }

        attack_cor = null;
        weapon.Set_Reload();

        Check_NextMove();
    }
    #endregion

    #region Orders
    public void Order_Attack(Unit target)
    {
        this.target = target;

        if (Get_DistToTarget() > weapon.range)
            Order_MoveTo(Utility.Get_NavMeshPoint(this.target.tr.position));
    }

    public void Order_MoveTo(Vector3 pos)
    {
        if (attack_cor != null)
        {
            nextMove = pos;
            return;
        }

        agent.ResetPath();
        agent.destination = pos;
    }

    public void Check_MovementStop()
    {
        if (!agent.hasPath) return;

        if (agent.remainingDistance < agent.stoppingDistance + 0.1f)
        {
            agent.ResetPath();
            animate.End_Of_Action();
        }
    }

    public void Check_NextMove()
    {
        if (nextMove == Vector3.zero) return;

        Order_MoveTo(nextMove);
        nextMove = Vector3.zero;
    }
    #endregion

    #region Setup
    public void Setup_Unit(GameObject go, Player owner)
    {
        this.go = go;
        this.tr = this.go.transform;
        this.owner = owner;

        Set_SelectionCircle();
        agent = this.go.GetComponent<NavMeshAgent>();
        animate = new Animate(this);
    }

    private void Set_SelectionCircle()
    {
        selectionCircle = this.go.transform.Find("Selector_sprite").gameObject;
        selectionCircle.GetComponent<SpriteRenderer>().color = owner.color;
        selectionCircle.SetActive(false);
    }
    #endregion

    #region Select/Deselect
    public void Select()
    {
        if (SceneController.scene.selectedUnits.Contains(this)) return;

        SceneController.scene.selectedUnits.Add(this);
        selectionCircle.SetActive(true);

        agent.avoidancePriority = 49;

        //Debug.Log(go.name + " named " + name + " owned by " + owner.name + " selected");
    }

    public void Deselect()
    {
        if (!SceneController.scene.selectedUnits.Contains(this)) return;

        SceneController.scene.selectedUnits.Remove(this);
        selectionCircle.SetActive(false);

        agent.avoidancePriority = 50;

        //Debug.Log(go.name + " named " + name + " owned by " + owner.name + " deselected");
    }
    #endregion

    #region Helpers
    private float Get_DistToTarget()
    {
        return Vector3.Distance(target.tr.position, tr.position);
    }
    #endregion
}