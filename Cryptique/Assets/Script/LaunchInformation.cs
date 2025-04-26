using System.Collections;
using UnityEngine;

public class LaunchInformation : OBJ_Collectable
{
    [SerializeField] private GameObject m_UIInfo;

    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        StartCoroutine(CoroutineLaunchInfo(3f));
        
        return true;
    }

    private IEnumerator CoroutineLaunchInfo(float delay)
    {
        m_UIInfo.SetActive(true);
        PC_PlayerController.Instance.DisableInput();

        yield return new WaitForSeconds(delay);

        // Add to inventory
        SGL_InventoryManager.Instance.AddItem(m_item);

        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(m_item.GetRegion(), m_item.GetName());
        Debug.Log($"Item {m_item.name} collected ! add to GameProgressionManager ");

        m_UIInfo.SetActive(false);
        Destroy(gameObject);
        PC_PlayerController.Instance.EnableInput();
    }
}
