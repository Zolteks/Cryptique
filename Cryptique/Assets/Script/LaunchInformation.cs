using System.Collections;
using UnityEngine;

public class LaunchInformation : OBJ_Collectable
{
    [Header("Information Settings")]
    [SerializeField] private float m_delay = 3f;
    [SerializeField] private GameObject m_UIInfo;

    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        StartCoroutine(CoroutineLaunchInfo());
        
        return true;
    }

    private IEnumerator CoroutineLaunchInfo()
    {
        yield return new WaitForEndOfFrame();
        
        m_UIInfo.SetActive(true);
        PC_PlayerController.Instance.DisableInput();

        yield return new WaitForSeconds(m_delay);

        // Add to inventory
        SGL_InventoryManager.Instance.AddItem(m_item);

        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(m_item.GetRegion(), m_item.GetName());
        Debug.Log($"Item {m_item.name} collected ! add to GameProgressionManager ");

        m_UIInfo.SetActive(false);
        if (PC_PlayerController.Instance.GetInputActive() == false)
            PC_PlayerController.Instance.EnableInput();
        Destroy(gameObject);
    }
}
