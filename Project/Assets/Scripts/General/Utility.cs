using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    private static Ray mouseRay;
    private static RaycastHit mouseHit;

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

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(somePos, out navHit, 50.0f, -1))
        {
            somePos = navHit.position;
        }

        return somePos;
    }

    public static string Get_ClickedObjectTag()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            return mouseHit.collider.tag;
        }

        return "terrain";
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
}