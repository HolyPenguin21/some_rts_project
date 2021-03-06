using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Camera settings :")]
    public float camMoveSpeed;
    public float[] BoundsX = new float[] { -4f, 10f };
    public float[] BoundsZ = new float[] { -4f, 4f };

    [Header("Scene units :")]
    public List<GameObject> sceneUnits_obj = new List<GameObject>();
    public List<Unit> sceneUnits = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    [Header("Scene players :")]
    public List<Player> players = new List<Player>();

    private StrategyCamera strategyCam;
    private Orders orders;
    private UnitCreation unitCreation;

    // Pooling objects
    private FollowTarget[] bullet_Effects;

    private void Awake()
    {
        Utility.scene = this;

        Setup_Camera();
        Setup_Orders();
        Setup_UnitCreation();
        Setup_Bullet_Effects_Pooling(50);

        // Test
        Add_Player("hum_Player", false, Color.green);
        Add_Player("ai_Player", true, Color.red);
    }

    private void Update()
    {
        Unit_Update();
        Update_BulletEffects();

        // Test
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GlobalEvents.CreateUnit(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()), 1, players[0]);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            GlobalEvents.CreateUnit(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()), 1, players[1]);
        }
    }

    private void LateUpdate()
    {
        strategyCam.LateUpdate();
    }

    private void OnDisable()
    {
        unitCreation.UnsubscribeEvents();
        orders.UnsubscribeEvents();
    }

    #region Units
    public void Add_UnitToScene(Unit unit)
    {
        sceneUnits.Add(unit);
        sceneUnits_obj.Add(unit.go);
    }

    private void Unit_Update()
    {
        if (sceneUnits.Count == 0) return;

        foreach (Unit unit in sceneUnits)
            unit.Update();
    }
    #endregion

    #region Effects
    private void Update_BulletEffects()
    {
        foreach (FollowTarget bullet in bullet_Effects)
            bullet.Update();
    }

    public void Activate_BulletEffect(Transform attackPoint, Unit target)
    {
        FollowTarget bullet = Get_FreeBullet();

        if (bullet == null) return;

        bullet.Set_Target(target);
        bullet.Set_InitialPosition(attackPoint);
        bullet.Set_Lifetime();
        bullet.Set_Active();
    }

    private FollowTarget Get_FreeBullet()
    {
        foreach (FollowTarget bullet in bullet_Effects)
        {
            if (!bullet.go.activeInHierarchy)
            {
                return bullet;
            }
        }

        Debug.LogError("Missing bullet prefabs, add more into pool");
        return null;
    }
    #endregion

    #region Setup
    private void Add_Player(string playerName, bool isAi, Color color)
    {
        players.Add(new Player(playerName, isAi, color));
    }

    private void Setup_Camera()
    {
        Transform camPivot_obj = Camera.main.transform.parent;
        strategyCam = new StrategyCamera(camPivot_obj, camMoveSpeed, BoundsX, BoundsZ);
    }

    private void Setup_Orders()
    {
        orders = new Orders();
    }

    private void Setup_UnitCreation()
    {
        unitCreation = new UnitCreation();
    }

    private void Setup_Bullet_Effects_Pooling(int count)
    {
        GameObject holder_obj = GameObject.Find("Bullet_Effects");
        if (holder_obj == null)
        {
            holder_obj = new GameObject("Bullet_Effects");
        }

        bullet_Effects = new FollowTarget[count];

        for (int i = 0; i < count; i++)
        {
            GameObject bullet_go = MonoBehaviour.Instantiate(Resources.Load("Effects/Bullet", typeof(GameObject)), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
            bullet_go.transform.parent = holder_obj.transform;
            bullet_go.SetActive(false);

            ParticleSystem bullet_part = bullet_go.GetComponent<ParticleSystem>();

            FollowTarget followTarget = new FollowTarget(bullet_go, bullet_part);

            bullet_Effects[i] = followTarget;
        }
    }
    #endregion
}