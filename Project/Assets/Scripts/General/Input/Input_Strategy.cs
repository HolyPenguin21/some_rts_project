using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Strategy
{
    public SelectBox_Constructor sb_constructor;

    private Ray mouseRay;
    private RaycastHit mouseHit;

    public bool isDraggingMouseBox = false;
    public Vector3 drag_StartPos;
    private Vector3 drag_EndPos;

    public Input_Strategy()
    {
        sb_constructor = new SelectBox_Constructor();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            drag_StartPos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if ((drag_StartPos - Input.mousePosition).magnitude > 20)
            {
                isDraggingMouseBox = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (isDraggingMouseBox)
            {
                Select_MultipleUnits();
            }
            else
            {
                Select_Unit();
            }

            isDraggingMouseBox = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Mouse_RightClick();
        }
    }

    private void Mouse_RightClick()
    {
        GameObject clickedObject = Utility.Get_ClickedObject();
        Unit clickedUnit = Get_Unit_byGo(clickedObject);
        switch (clickedObject.tag)
        {
            // terrain click
            case "Terrain":
                SceneController.scene.Order_Move(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()));
                break;
            case "Unit":
                // enemy click
                if (SceneController.scene.selectedUnits[0].owner != clickedUnit.owner)
                {
                    SceneController.scene.Order_Attack(clickedUnit);
                }
                // ally click
                else
                {
                    SceneController.scene.Order_Move(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()));
                }
                break;
        }
    }

    private void Select_Unit()
    {
        Unit clickedUnit = Get_ClickedUnit();
        if (clickedUnit == null)
        {
            DeselectAll();
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            clickedUnit.Select();
        }
        else
        {
            DeselectAll();
            clickedUnit.Select();
        }
    }

    private void Select_MultipleUnits()
    {
        sb_constructor.Set_Verts(mouseRay, mouseHit, drag_StartPos, drag_EndPos);
        sb_constructor.selectionMesh = sb_constructor.generateSelectionMesh();

        sb_constructor.Set_Components(SceneController.scene.gameObject);

        if (!Input.GetKey(KeyCode.LeftShift))
            DeselectAll();

        MonoBehaviour.Destroy(sb_constructor.selectionBox, 0.02f);
    }

    #region Helpers
    private Unit Get_ClickedUnit()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            if (mouseHit.collider.CompareTag("Unit"))
            {
                return Get_Unit_byGo(mouseHit.collider.gameObject);
            }
        }

        return null;
    }

    public Unit Get_Unit_byGo(GameObject go)
    {
        foreach (Unit unit in SceneController.scene.sceneUnits)
            if (unit.go == go)
                return unit;

        return null;
    }

    private void DeselectAll()
    {
        foreach (Unit unit in SceneController.scene.sceneUnits)
        {
            unit.Deselect();
        }
    }
    #endregion
}