using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAccess : MonoBehaviour
{
    [SerializeField] private List<OBJ_Item> conds = new();
    [SerializeField] private GameObject placeHorse;
    [SerializeField] private GameObject enterHorse;
    [SerializeField] private Transform horseTarget;
    [SerializeField] private float horseSpeed = 2f;

    private SGL_InventoryManager inventoryManager;
    private IN_Character character;
    private Animator horseAnimator;

    private bool isInteract = false;
    private bool horseMoving = false;

    private void Start()
    {
        inventoryManager = SGL_InventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("ForestAccess -> InventoryManager not found");
            return;
        }

        if (placeHorse != null)
        {
            character = placeHorse.GetComponent<IN_Character>();
            horseAnimator = placeHorse.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (conds.Count == 0 || inventoryManager == null || character == null)
            return;

        List<OBJ_Item> foundConds = new List<OBJ_Item>();

        foreach (var cond in conds)
        {
            if (inventoryManager.CheckForItem(cond))
            {
                foundConds.Add(cond);
            }
        }

        foreach (var cond in foundConds)
        {
            conds.Remove(cond);
        }

        if (character.getWasInteracting())
        { 
            isInteract = true;
        }

        if (conds.Count == 0 && isInteract && !horseMoving)
        {
            StartCoroutine(MoveHorseCoroutine());
        }
    }

    private IEnumerator MoveHorseCoroutine()
    {
        horseMoving = true;

        if (horseAnimator != null)
            horseAnimator.SetBool("isWalk", true);

        while (Vector3.Distance(placeHorse.transform.position, horseTarget.position) > 0.1f)
        {
            placeHorse.transform.position = Vector3.MoveTowards(
                placeHorse.transform.position,
                horseTarget.position,
                horseSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (horseAnimator != null)
            horseAnimator.SetBool("isWalk", false);

        placeHorse.SetActive(false);
        enterHorse.SetActive(true);
    }
}
