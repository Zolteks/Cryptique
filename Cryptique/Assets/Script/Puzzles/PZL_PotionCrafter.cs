using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_PotionCrafter : OBJ_InteractOnDrop
{
    [SerializeField] private List<OBJ_Item> m_AllIngredients;
    [SerializeField] private GameObject m_FailedPotion;
    [SerializeField] private GameObject m_SuccessFullPotion;
    [SerializeField] private Camera m_Camera;

    private PZL_PotionCrafterComplete m_Complete;
    private OBJ_Item m_ItemsDropped;
    private bool bIsPotionFailed = false;
    private bool bIsSuccessPotionPickedUp = false;
    private bool bIsFailedPotionPickedUp = false;

    private void Start()
    {
        m_Complete = GetComponent<PZL_PotionCrafterComplete>();
    }

    public override bool Interact()
    {
        if (CanInteract())
        {
            m_ItemsDropped = GetItemDropped();
            CheckListIngredient();
            return true;
        }
        return false;
    }

    void CheckListIngredient()
    {
        bool bIngredientFind = false;
        if (m_AllIngredients.Count != 0)
        {
            foreach (OBJ_Item recipeItem in m_AllIngredients)
            {
                if (m_ItemsDropped == recipeItem)
                {
                    m_AllIngredients.Remove(recipeItem);
                    bIngredientFind = true;
                    break;
                }
            }
        }

        if (!bIngredientFind) bIsPotionFailed = true;

        Debug.Log(bIsPotionFailed);

        if (m_AllIngredients.Count <= 0)
        {
            if (!bIsPotionFailed)
            {
                m_SuccessFullPotion.SetActive(true);
            }
            else
            {
                m_FailedPotion.SetActive(true);
            }
            m_Complete.PotionComplete();
            SetCanInteract(false);
        }
    }
}
