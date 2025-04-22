using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IN_DecoyArrow : OBJ_Interactable
{
    [Header("Arrow Settings")]
    [SerializeField] Transform fallbackTile;
    [SerializeField] GameObject teleportPoint;
    float m_fFadeInDuration = 1.5f;
    float m_fFadeOutDuration = .8f;
    private PC_PlayerController m_playerController;
    
    private void Awake()
    {
        m_playerController = PC_PlayerController.Instance;
        if (m_playerController == null)
            Debug.LogError("PlayerController not found");
    }

    public override bool Interact()
    {
        if (Vector3.Distance(m_playerController.transform.position, transform.position) > 100)
        {
            InteractionCallback();
            return true;
        }
        m_playerController.OnMoveCallback += InteractionCallback;
        if (teleportPoint)
            m_playerController.MoveToTile(teleportPoint.transform.position);
        else
            m_playerController.MoveTo();
        return true;
    }
    
    private void InteractionCallback()
    {
        StartCoroutine(CoroutineBackToFallback());
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
        m_playerController.OnMoveCallback -= InteractionCallback;
    }
}
