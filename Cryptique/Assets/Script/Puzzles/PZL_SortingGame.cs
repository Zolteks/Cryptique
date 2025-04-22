using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PZL_SortingGame : Puzzle
{
    private List<bool> etatsPlacement;
    private bool bIsAllPlaced = false;

    [SerializeField] private int iElementNumber;
    [SerializeField] private OBJ_Item OBJ_LanternOil;
    [SerializeField] private GameObject m_Game;
    [SerializeField] private GameObject m_Oil;
 
    [SerializeField] public Camera cam;

    private bool bOilTake = false;
    private GameObject m_doors;
    private BoxCollider m_boxCollider;

    void Start()
    {
        m_doors = GameObject.Find("Doors");
        m_boxCollider = m_doors.GetComponent<BoxCollider>();
        m_boxCollider.enabled = false;

        etatsPlacement = new List<bool>(new bool[iElementNumber]);
        SGL_InteractManager.Instance.ChangeCamera(cam);

        PC_PlayerController.Instance.DisableInput();
        SGL_InteractManager.Instance.EnableInteraction();
    }

    public void UpdateEtatPlacement(int index, bool estBienPlace)
    {
        if (index >= 0 && index < etatsPlacement.Count)
        {
            etatsPlacement[index] = estBienPlace;
            CheckPlacementComplet();
        }
    }

    private void CheckPlacementComplet()
    {
        if (etatsPlacement.All(etat => etat))
        {
            bIsAllPlaced = true;
            m_Game.SetActive(false);
            m_Oil.SetActive(true);
        }
    }

    private void Update()
    {
        if (!bIsAllPlaced || bOilTake) return;

        // Vérifie si l'objet n'est plus actif OU a été détruit
        if (m_Oil == null || !m_Oil.activeInHierarchy)
        {
            bOilTake = true;
            Debug.Log("Huile récupérée !");

            PC_PlayerController.Instance.EnableInput();
            Complete();
        }
    }

    public bool GetisAllPlaced()
    {
        return bIsAllPlaced;
    }
}
