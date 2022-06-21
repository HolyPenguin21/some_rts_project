using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController scene = null;

    [Header("Camera settings :")]
    public float camMoveSpeed;
    public float[] BoundsX = new float[] { -4f, 10f };
    public float[] BoundsZ = new float[] { -4f, 4f };
    public StrategyCamera strategyCam;

    [Header("Input settings :")]
    public Input_Strategy input_Strategy;
    private Rect rect;

    [Header("Scene units :")]
    public List<GameObject> sceneUnits_obj = new List<GameObject>();
    public List<Unit> sceneUnits = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    [Header("Scene players :")]
    public List<Player> players = new List<Player>();

    [Header("Movement :")]
    private Vector3 basePos;
    private Vector3 offset;
    private Vector3 uPos;
    private Vector3 resultPos;

    private void Awake()
    {
        Setup_Camera();
        Setup_Input();

        Add_Player("hum_Player", false, Color.green);
        Add_Player("ai_Player", true, Color.red);
    }

    private void Start()
    {
        Set_Singletone();
    }

    private void Update()
    {
        input_Strategy.Update();
        Unit_Update();

        // Test
        if(Input.GetKey(KeyCode.Alpha1))
            Create_Unit(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()), 1, players[0]);
        if (Input.GetKey(KeyCode.Alpha2))
            Create_Unit(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()), 1, players[1]);
    }

    private void LateUpdate()
    {
        strategyCam.LateUpdate();
    }

    private void OnGUI()
    {
        if (input_Strategy.isDraggingMouseBox)
        {
            rect = input_Strategy.sb_constructor.GetScreenRect(input_Strategy.drag_StartPos, Input.mousePosition);
            input_Strategy.sb_constructor.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            input_Strategy.sb_constructor.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }

    #region Units
    public void Create_Unit(Vector3 pos, int unitId, Player owner)
    {
        GameObject unit_go = MonoBehaviour.Instantiate(Resources.Load("Units/Soldier", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
        unit_go.name = "Soldier_" + sceneUnits.Count;

        Soldier unit = new Soldier(unit_go, owner);
        unit.name = "Soldier_" + sceneUnits.Count;

        Add_UnitToScene(unit);
    }

    private void Add_UnitToScene(Unit unit)
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

    private void OnTriggerEnter(Collider other)
    {
        // NEED FOR SELECTION
        Unit someUnit = input_Strategy.Get_Unit_byGo(other.gameObject);
        someUnit.Select();
    }
    #endregion

    #region Effects
    public void Create_Bullet(Transform parent, Vector3 target)
    {
        Vector3 dir = target - parent.position;
        Vector3 dir_norm = dir.normalized * 1.5f;
        GameObject bullet_go = MonoBehaviour.Instantiate(Resources.Load("Effects/Bullet", typeof(GameObject)), parent.position + dir_norm, Quaternion.identity) as GameObject;
        
        ParticleSystem bullet_part = bullet_go.GetComponent<ParticleSystem>();
        var main = bullet_part.main;
        main.startLifetime = dir.magnitude / main.startSpeed.constant - 0.025f;

        bullet_go.transform.LookAt(target);
    }
    #endregion

    #region Orders
    public void Order_Move(Vector3 pos)
    {
        if (selectedUnits.Count == 0) return;

        if (selectedUnits.Count == 1)
        {
            basePos = selectedUnits[0].tr.position;
        }
        else
        {
            Bounds bounds = new Bounds(selectedUnits[0].tr.position, Vector3.zero);
            for (int i = 1; i < selectedUnits.Count; i++)
                bounds.Encapsulate(selectedUnits[i].tr.position);

            basePos = bounds.center;
        }

        foreach (Unit unit in selectedUnits)
        {
            unit.target = null;

            uPos = unit.tr.position;
            offset = uPos - basePos;
            resultPos = Utility.Get_NavMeshPoint(pos + offset);

            unit.Order_MoveTo(resultPos);
        }
    }

    public void Order_Attack(Unit targetUnit)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Order_Attack(targetUnit);
        }
    }
    #endregion

    #region Setup
    private void Add_Player(string playerName, bool isAi, Color color)
    {
        players.Add(new Player(playerName, isAi, color));
    }

    private void Set_Singletone()
    {
        if (scene == null) scene = this;
        else Destroy(gameObject);
    }

    private void Setup_Camera()
    {
        Transform camPivot_obj = Camera.main.transform.parent;
        strategyCam = new StrategyCamera(camPivot_obj, camMoveSpeed, BoundsX, BoundsZ);
    }

    private void Setup_Input()
    {
        input_Strategy = new Input_Strategy();
    }
    #endregion
}