using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders
{
    private SceneController scene;
    private Vector3 basePos;
    private Vector3 offset;
    private Vector3 uPos;
    private Vector3 resultPos;

    public void Order_Move(Vector3 pos)
    {
        scene = SceneController.scene;

        if (scene.selectedUnits.Count == 0) return;

        if (scene.selectedUnits.Count == 1)
        {
            basePos = scene.selectedUnits[0].tr.position;
        }
        else
        {
            Bounds bounds = new Bounds(scene.selectedUnits[0].tr.position, Vector3.zero);
            for (int i = 1; i < scene.selectedUnits.Count; i++)
                bounds.Encapsulate(scene.selectedUnits[i].tr.position);

            basePos = bounds.center;
        }

        foreach (Unit unit in scene.selectedUnits)
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
        foreach (Unit unit in SceneController.scene.selectedUnits)
        {
            unit.Order_Attack(targetUnit);
        }
    }
}
