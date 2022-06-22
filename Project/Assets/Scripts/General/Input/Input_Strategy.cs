using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Strategy : MonoBehaviour
{
    public Color selectBoxColor = new Color(0.5f, 1f, 0.4f, 0.2f);
    public Color selectBoxBorderColor = new Color(0.5f, 1f, 0.4f);

    private SelectBox_Constructor sb_constructor;

    private bool isDraggingMouseBox = false;
    private Vector3 drag_StartPos;
    private Vector3 drag_EndPos;

    private void Awake()
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

    private void OnGUI()
    {
        if (!isDraggingMouseBox) return;

        Rect rect_ui = sb_constructor.GetScreenRect(drag_StartPos);
        sb_constructor.DrawScreenRect(rect_ui, selectBoxColor);
        sb_constructor.DrawScreenRectBorder(rect_ui, 1, selectBoxBorderColor);

    }

    private void OnTriggerEnter(Collider other)
    {
        Unit someUnit = Utility.Get_Unit_byGo(other.gameObject);
        someUnit.Select();
    }

    private void Mouse_RightClick()
    {
        GameObject clickedObject = Utility.Get_ClickedObject();
        Unit clickedUnit = Utility.Get_Unit_byGo(clickedObject);

        switch (clickedObject.tag)
        {
            case "Terrain":
                SceneController.scene.orders.Order_Move(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()));
                break;

            case "Unit":
                // Enemy click
                if (SceneController.scene.selectedUnits[0].owner != clickedUnit.owner)
                {
                    SceneController.scene.orders.Order_Attack(clickedUnit);
                }
                // Ally click
                else
                {
                    SceneController.scene.orders.Order_Move(Utility.Get_NavMeshPoint(Utility.Get_MouseWorldPos()));
                }
                break;
        }
    }

    private void Select_Unit()
    {
        Unit clickedUnit = Utility.Get_Unit_byGo(Utility.Get_ClickedObject());
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
        sb_constructor.Create_SelectBoxMesh(drag_StartPos, drag_EndPos);

        if (!Input.GetKey(KeyCode.LeftShift))
            DeselectAll();
    }

    private void DeselectAll()
    {
        foreach (Unit unit in SceneController.scene.sceneUnits)
        {
            unit.Deselect();
        }
    }
}