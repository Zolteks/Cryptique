using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_DropMirror : OBJ_InteractOnDrop
{
    public GameObject DropBorder;
    public PZL_UndergroundLakeDoor LockedDoor;

    public override bool Interact()
    {
        if (DropBorder != null)
        {
            LockedDoor.Begin();
            GameObject mirror = GameObject.Find("PZL_Mirror(Clone)");
            if (mirror != null)
            {
                Debug.Log("PZL_Mirror found " + gameObject.name);
                mirror.transform.position = DropBorder.transform.position;
                DropBorder.SetActive(false);
                return true;
            }
            return false;
        }
        else
        {
            Debug.LogError("PZL_Mirror not found " + gameObject.name);
            return false;
        }
    }
}