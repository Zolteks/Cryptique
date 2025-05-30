using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PZL_SortingGame : Puzzle
{
    private List<bool> etatsPlacement;
    private bool bIsAllPlaced = false;

    [SerializeField] private int iElementNumber;
    [SerializeField] private OBJ_Item OBJ_LanternOil;
    [SerializeField] private GameObject m_Game;
    [SerializeField] private GameObject m_Oil;
 
    [SerializeField] public Camera cam;

    [SerializeField] private GameObject m_UIIndications;

    private bool bOilTake = false;
    private GameObject m_doors;
    private BoxCollider m_boxCollider;
    private GameObject m_UIplayGameObject;

    void Start()
    {
        m_doors = GameObject.Find("Doors");
        m_boxCollider = m_doors.GetComponent<BoxCollider>();
        m_boxCollider.enabled = false;

        m_UIplayGameObject = GameObject.Find("UIPlay");
        m_UIplayGameObject.SetActive(false);


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

    public void QuitGame()
    {
        m_boxCollider.enabled = true;
        m_UIplayGameObject.SetActive(true);
        Quit();
    }

    private void Update()
    {
        if (!bIsAllPlaced || bOilTake) return;

        m_UIplayGameObject.SetActive(true);

        // Dsactier toute les composants Images ans les enfants de m_UIplayGameObject
        foreach (Image image in m_UIplayGameObject.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
        m_UIIndications.SetActive(false);

        // V�rifie si l'objet n'est plus actif OU a �t� d�truit
        if (m_Oil == null || !m_Oil.activeInHierarchy)
        {
           

            bOilTake = true;

            PC_PlayerController.Instance.EnableInput();
            SGL_InteractManager.Instance.ChangeCamera(Camera.main);
            Complete();

            //Reactive les composants Images
            foreach (Image image in m_UIplayGameObject.GetComponentsInChildren<Image>())
            {
                image.enabled = true;
            }
            m_UIIndications.SetActive(true);
        }
    }

    public bool GetisAllPlaced()
    {
        return bIsAllPlaced;
    }
}
