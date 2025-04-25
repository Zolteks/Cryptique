using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public enum Orientation
    {
        North = 0,
        West,
        South,
        East,
    }

    [SerializeField] Animator animator;

    public void SetOrientation(Orientation orientation)
    {
        switch (orientation)
        {
            case Orientation.North:
                //animator.SetTrigger("North");
                animator.SetInteger("Orientation", 0);
                print("set to north");
                break;
            case Orientation.South:
                //animator.SetTrigger("South");
                animator.SetInteger("Orientation", 2);
                print("set to south");
                break;
            case Orientation.East:
                //animator.SetTrigger("East");
                animator.SetInteger("Orientation", 1);
                print("set to east");
                break;
            case Orientation.West:
                //animator.SetTrigger("West");
                animator.SetInteger("Orientation", 3);
                print("set to west");
                break;
        }
    }
}
