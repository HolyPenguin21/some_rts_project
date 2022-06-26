using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders
{
    public Orders()
    {
        GlobalEvents.onUnitMove += Move;
        GlobalEvents.onUnitAttack += Attack;
    }

    private void Move(Vector3 pos)
    {
        List<Unit> selectedUnits = Utility.scene.selectedUnits;
        Vector3 basePos;

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

            Vector3 uPos = unit.tr.position;
            Vector3 offset = uPos - basePos;
            Vector3 resultPos = Utility.Get_NavMeshPoint(pos + offset);

            unit.Order_MoveTo(resultPos);
        }
    }

    private void Attack(Unit targetUnit)
    {
        foreach (Unit unit in Utility.scene.selectedUnits)
        {
            unit.Order_Attack(targetUnit);
        }
    }

    public void UnsubscribeEvents()
    {
        GlobalEvents.onUnitMove -= Move;
        GlobalEvents.onUnitAttack -= Attack;
    }
}
