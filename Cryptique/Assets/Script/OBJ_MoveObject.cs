using UnityEngine;

public class OBJ_MoveObject : OBJ_Interactable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject targetPosition;
    private bool isMoving = false;
    private void Update()
    {
        if (isMoving)
        {
            MoveObject();
        }
    }
    public override bool Interact()
    {
        isMoving = !isMoving;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        return true;
    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition.transform.position) < 0.1f)
        {
            isMoving = false;
        }
    }
}

