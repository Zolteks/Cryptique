using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassOrientationSetter : MonoBehaviour
{
    [SerializeField] Compass compass;
    [SerializeField] Compass.Orientation orientation;
   Compass.Orientation baseOrientation;

    CameraDirdection lastDir;

    private void Start()
    {
        CameraRotator rotator = GameManager.GetInstance().GetCamera().GetComponent<CameraRotator>();
        lastDir = rotator.GetDirection();
        rotator.eDirectionUpdate += OnCameraMovement;
        baseOrientation = orientation;
    }

    void OnCameraMovement(CameraDirdection newDir)
    {
        if (newDir == lastDir) return;

        // fucking hell to retrieve the correct increment value in order to change orientation
        int increment;
        if((int)newDir == 3)
            increment = (int)lastDir == 2 ? -1 : 1;
        else if((int)lastDir == 3)
            increment = (int)newDir == 2 ? 1 : -1;
        else
            increment = (int)lastDir < (int)newDir ? -1 : 1;
        int orientationIncremnet = ((int)orientation + increment);


        orientationIncremnet = orientationIncremnet < 0 ? 3 : orientationIncremnet % 4;
        print("lastDir : " + lastDir + " || newDir : " + newDir + "  || orientationIncrement : " + (Compass.Orientation)orientationIncremnet + "  || baseOrientation : "+ baseOrientation + "  || Increment : "+increment);
        orientation = (Compass.Orientation)orientationIncremnet;
        lastDir = newDir;

        // In case we're already in collision with the camera
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(250, 10, 250), Quaternion.identity, 128);
        if (hits != null)
        {
            foreach (Collider col in hits)
            {
                if(col.TryGetComponent<CameraRotator>(out _))
                    compass.SetOrientation(orientation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("I DO FUCKIN COLLIDE SALE FILS DE PUTE *************");
        if (other.gameObject.TryGetComponent<CameraRotator>(out _))
            compass.SetOrientation(orientation);
    }
}
