using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_RepairLadder : OBJ_InteractOnDrop
{
    [SerializeField] private List<GameObject> testBase;
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private GameObject LadderGameObject;

    public override bool Interact()
    {
        GameObject test = Instantiate(GetItemDropped().GetPrefab(), parentGameObject.transform);

        test.transform.position = testBase[0].transform.position;
        testBase.Remove(testBase[0]);

        if (testBase.Count <= 0)
        {
            GameObject Ladder = Instantiate(LadderGameObject);

            Ladder.transform.position = parentGameObject.transform.position;

            Destroy(parentGameObject);
        }

        return true;
    }
}
