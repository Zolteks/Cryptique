using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IN_DecoyArrow : OBJ_Interactable
{
    [SerializeField] Transform fallbackTile;
    float m_fFadeInDuration = 1.5f;
    float m_fFadeOutDuration = .8f;

    public override bool Interact()
    {
        if (false == CanInteract())
            return false;

        StartCoroutine(CoroutineBackToFallback());

        return true;
    }

    IEnumerator CoroutineBackToFallback()
    {
        float timer = 0;
        GameObject mask = (GameObject)Instantiate(Resources.Load("WhiteMask"));
        MaskOpacityHandler opacityHandler = mask.GetComponent<MaskOpacityHandler>();

        while (timer <= m_fFadeInDuration)
        {
            timer += Time.deltaTime;
            opacityHandler.SetOpacity(timer / m_fFadeInDuration);
            yield return null;
        }

        GameManager.GetInstance().GetCamera().position = fallbackTile.position;
        timer = 0;

        while (timer <= m_fFadeOutDuration)
        {
            timer += Time.deltaTime;
            opacityHandler.SetOpacity(1- timer / m_fFadeOutDuration);
            yield return null;
        }
        
        Destroy(mask);
    }

    // Is that definitive? Unsure
    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        Interact();
    }
}
