using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreation
{
    public UnitCreation()
    {
        GlobalEvents.onUnitCreation += Spawn_Unit;
    }

    private Unit Spawn_Unit(Vector3 pos, int unitId, Player owner)
    {
        GameObject unit_go = Create_UnitObject(pos, unitId);
        Unit unit = Create_Unit(unit_go, owner);

        return unit;
    }

    private GameObject Create_UnitObject(Vector3 pos, int unitId)
    {
        GameObject unit_go;

        switch (unitId)
        {
            case 1:
                unit_go = MonoBehaviour.Instantiate(Resources.Load("Units/Soldier", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
                unit_go.name = "Soldier";
                break;
            default:
                unit_go = MonoBehaviour.Instantiate(Resources.Load("Units/Soldier", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
                unit_go.name = "Soldier";
                break;
        }

        return unit_go;
    }

    private Unit Create_Unit(GameObject unit_go, Player owner)
    {
        Soldier unit = new Soldier(unit_go, owner);
        unit.name = "Soldier";

        return unit;
    }

    public void UnsubscribeEvents()
    {
        GlobalEvents.onUnitCreation -= Spawn_Unit;
    }
}
