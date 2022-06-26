using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEvents
{
    public delegate Unit OnUnitCreation(Vector3 pos, int unitId, Player owner);
    public static event OnUnitCreation onUnitCreation;

    public delegate void OnUnitMove(Vector3 pos);
    public static event OnUnitMove onUnitMove;

    public delegate void OnUnitAttack(Unit target);
    public static event OnUnitAttack onUnitAttack;

    public static void CreateUnit(Vector3 pos, int unitId, Player owner)
    {
        Unit unit = onUnitCreation?.Invoke(pos, unitId, owner);

        Utility.scene.Add_UnitToScene(unit);
    }

    public static void Move(Vector3 pos)
    {
        onUnitMove?.Invoke(pos);
    }

    public static void Attack(Unit target)
    {
        onUnitAttack?.Invoke(target);
    }
}
