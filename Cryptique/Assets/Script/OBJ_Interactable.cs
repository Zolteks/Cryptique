using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class OBJ_Interactable : MonoBehaviour
{
    /* Variables */
    [SerializeField]
    private bool m_canInteract = true;

    /* Functions */

    /// <summary>
    /// Indique si une interaction est possible.
    /// </summary>
    public bool CanInteract() { return m_canInteract; }

    /// <summary>
    /// Applique le booleen m_canInteract.
    /// </summary>
    /// <param name="canInteract">True si l'interaction est possible, sinon false.</param>
    public void SetCanInteract(bool canInteract) => m_canInteract = canInteract;

    /// <summary>  
    /// Interagit avec l'objet.  
    /// </summary>  
    /// <returns>True si l'interaction a reussi, sinon false.</returns>  
    public abstract bool Interact();
}
