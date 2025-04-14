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

            _angle += mPosDelta.x;

            if (angle > 360 || _angle < -360)
            {
                _angle = Mathf.Clamp(_angle, -360, 360);
                return;
            }

            transform.Rotate(new Vector3(1, 0, 0), mPosDelta.x, Space.Self);
        }

        mPrevPos = Input.mousePosition;
    }
}
