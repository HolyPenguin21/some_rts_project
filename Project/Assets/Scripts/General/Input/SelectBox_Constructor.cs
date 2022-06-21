using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox_Constructor
{
    public MeshCollider selectionBox;
    public Mesh selectionMesh;
    public Vector2[] corners;
    public Vector3[] verts;

    private Texture2D _whiteTexture;

    #region Create mesh
    public void Set_Verts(Ray mouseRay, RaycastHit mouseHit, Vector3 drag_StartPos, Vector3 drag_EndPos)
    {
        verts = new Vector3[4];
        int i = 0;
        drag_EndPos = Input.mousePosition;
        corners = getBoundingBox(drag_StartPos, drag_EndPos);

        foreach (Vector2 corner in corners)
        {
            mouseRay = Camera.main.ScreenPointToRay(corner);

            if (Physics.Raycast(mouseRay, out mouseHit, 75.0f))
            {
                verts[i] = new Vector3(mouseHit.point.x, mouseHit.point.y, mouseHit.point.z);
                //Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), mouseHit.point, Color.red, 1.0f);
            }
            i++;
        }
    }

    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(p1, p2);
        var topRight = Vector3.Max(p1, p2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;
    }

    public Mesh generateSelectionMesh()
    {
        Vector3[] someVerts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            someVerts[i] = verts[i];
            someVerts[i].y = -1;
        }

        for (int j = 4; j < 8; j++)
        {
            someVerts[j] = verts[j - 4] + Vector3.up * 20;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = someVerts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    public void Set_Components(GameObject obj)
    {
        selectionBox = obj.AddComponent<MeshCollider>();
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;
    }
    #endregion

    #region Draw select box
    public Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }
    #endregion
}