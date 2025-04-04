using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBehaviour : MonoBehaviour
{
    [SerializeField] List<Transform> faceDetectors;
    [SerializeField] float velocityFactor = 1;
    [SerializeField] float angularVelocityFactor = 1;

    public bool bRigged;

    bool bDead = false;

    Rigidbody rb;
    Vector3 initialPos;

    Vector3 launchRotation;
    Vector3 launchForce;
    Vector3 launchTorque;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
        Init();

        if (bRigged)
            RiggDie();

        LaunchDice();
    }

    private void Init()
    {
        bDead = false;

        int x = Random.Range(0, 360);
        int y = Random.Range(0, 360);
        int z = Random.Range(0, 360);
        launchRotation = new Vector3(x, y, z);

        x = Random.Range(0, 25);
        y = Random.Range(0, 25);
        z = Random.Range(0, 25);
        launchForce = new Vector3(x, y, z);

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);
        launchTorque = new Vector3(x, y, z);

        rb.maxAngularVelocity = 1000;
    }

    private void LaunchDice()
    {
        transform.position = initialPos;
        transform.rotation = Quaternion.Euler(launchRotation);
        rb.velocity = launchForce * velocityFactor;

        rb.AddTorque(launchTorque * angularVelocityFactor, ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            Init();

            if (bRigged)
                RiggDie();

            LaunchDice();
        }

        if (bDead) return;

        if (CheckStop())
        {
            int resultIndex = FindFaceResult();
            print(resultIndex+1);
            bDead = true;
        }
    }

    private bool CheckStop()
    {
        if(rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
            return true;

        return false;
    }

    private int FindFaceResult()
    {
        int maxIndex = 0;
        for(int i = 0; i<faceDetectors.Count; i++)
        {
            if (faceDetectors[maxIndex].position.y < faceDetectors[i].position.y)
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private void RiggDie()
    {
        LaunchDice();
        Physics.simulationMode = SimulationMode.Script;
        for(int i = 0; i<5000; i++)
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }
        Physics.simulationMode = SimulationMode.FixedUpdate;

        int id = FindFaceResult();

        int res = id + 1;
        print("previous result was : " + res);

        if (id <= 3)
        {
            launchRotation = Quaternion.AngleAxis(180, Vector3.forward) * launchRotation;
        }
    }
}
