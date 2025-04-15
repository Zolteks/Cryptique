using UnityEngine;

public class RotationGutters : OBJ_Interactable
{
    private Quaternion targetRotation;
    public float rotationSpeed = 360f;
    private bool isRotating = false;
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    override public bool Interact()
    {
        if (isRotating) return false;

        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 0f, -90f));
        isRotating = true;

        return true;
    }
}
