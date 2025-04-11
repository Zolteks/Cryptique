using UnityEngine;

public class RotationGutters : MonoBehaviour
{
    PipePieceTrigger pipePieceTrigger;
    private Quaternion targetRotation;
    public float rotationSpeed = 360f;
    private bool isRotating = false;
    PZL_GutterLabyrinth PZL_GutterLabyrinth;

    //private void Start()
    //{
    //    PZL_GutterLabyrinth = Object.FindAnyObjectByType<PZL_GutterLabyrinth>();
    //}
    void Update()
    {
        if (Input.touchCount > 0 && !isRotating)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 0f, -90f));
                        isRotating = true;
                        //PZL_GutterLabyrinth.Solve();
                    }
                }
            }
        }

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
}
