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
    private GameObject selectionCircle;
    public Transform attackPoint;

    public string name;
    public Player owner;
    public Unit target;
    public Weapon weapon;
    public Health health;

    public Animate animate;

    public bool canMove = true;

    public void Update()
    {
        TrackTarget();

        Check_MovementStop();

        weapon.Update();
        animate.Update();
    }

    #region Target
    private void TrackTarget()
    {
        if (target == null) return;

        if (Get_DistToTarget() > weapon.range)
        {
            Order_MoveTo(Utility.Get_NavMeshPoint(target.tr.position));
        }
        else
        {
            Target_Rotation();
            Attack();
        }
    }

    private void Target_Rotation()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.LookRotation(target.tr.position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, rotation, Time.deltaTime * 20.0f);
    }

    private void Attack()
    {
        agent.ResetPath();

        if (weapon.reload_Cur > 0 || animate.animClip_Cur != null) return;

        SceneController.scene.StartCoroutine(weapon.Shoot());
        SceneController.scene.Create_Bullet(attackPoint, target.tr.position + Vector3.up);
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
        if (!canMove)
        {
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