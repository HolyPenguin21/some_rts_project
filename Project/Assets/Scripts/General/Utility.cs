using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    private static Ray mouseRay;
    private static RaycastHit mouseHit;
    private static NavMeshHit navHit;

    public static Vector3 Get_MouseWorldPos()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            return mouseHit.point;
        }

        return Vector3.zero;
    }

    public static Vector3 Get_NavMeshPoint(Vector3 pos)
    {
        Vector3 somePos = pos;

        if (NavMesh.SamplePosition(somePos, out navHit, 50.0f, -1))
        {
            somePos = navHit.position;
        }

        return somePos;
    }

    public static GameObject Get_ClickedObject()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            return mouseHit.collider.gameObject;
        }

        return null;
    }

    public static Unit Get_Unit_byGo(GameObject go)
    {
        foreach (Unit unit in SceneController.scene.sceneUnits)
            if (unit.go == go)
                return unit;

        return null;
    }
}