using UnityEngine;

public class StrategyCamera
{
    private float camMoveSpeed = 1;
    private float[] BoundsX = new float[] { -4f, 10f };
    private float[] BoundsZ = new float[] { -4f, 4f };

    private Transform tr;
    private Vector3 nextPos;
    private Vector3 velocity = Vector3.zero;

    public StrategyCamera(Transform camPivot_obj, float camMoveSpeed, float[] BoundsX, float[] BoundsZ)
    {
        tr = camPivot_obj;
        this.camMoveSpeed = camMoveSpeed;
        this.BoundsX = BoundsX;
        this.BoundsZ = BoundsZ;
    }

    public void LateUpdate()
    {
        nextPos = tr.position;

        nextPos += new Vector3(Input.GetAxis("Horizontal") * camMoveSpeed, 0, Input.GetAxis("Vertical") * camMoveSpeed);
        nextPos.x = Mathf.Clamp(nextPos.x, BoundsX[0], BoundsX[1]);
        nextPos.z = Mathf.Clamp(nextPos.z, BoundsZ[0], BoundsZ[1]);

        tr.position = Vector3.SmoothDamp(tr.position, nextPos, ref velocity, 0.3f);
    }
}