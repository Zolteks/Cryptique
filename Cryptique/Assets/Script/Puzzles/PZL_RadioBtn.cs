using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PZL_RadioBtn : MonoBehaviour
{
    float _angle;
    public float angle
    {
        get { return _angle; }
    }

    Vector3 mPrevPos;
    Vector3 mPosDelta;
    bool isSelected;

    private void OnMouseDown()
    {
        isSelected = true;
    }
    private void OnMouseUp()
    {
        isSelected = false;
    }

    private void Update()
    {
        if (isSelected)
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);

            _angle = transform.rotation.eulerAngles.x;
        }

        mPrevPos = Input.mousePosition;
    }
}
