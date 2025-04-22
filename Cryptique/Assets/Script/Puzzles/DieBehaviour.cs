
using System.Collections.Generic;
using UnityEngine;
using static DieBehaviour;

public class DieBehaviour : MonoBehaviour
{
    public enum RigState
    {
        regular = 0,
        lose,
        win,
    }

    [SerializeField] List<Transform> faceDetectors;
    //[SerializeField] float velocityFactor = 1;
    //[SerializeField] float angularVelocityFactor = 1;

    int result;

    public System.Func<int, bool> onRollOver;

    public RigState rigState;

    //bool bDead = false;

    //Rigidbody rb;
    //Vector3 initialPos;

    //Vector3 launchRotation;
    //Vector3 launchForce;
    //Vector3 launchTorque;

    //    void Start()
    //    {
    //        rb = GetComponent<Rigidbody>();
    //        initialPos = transform.position;
    //        Init();

    //        if (rigState != RigState.regular)
    //            RiggDie();

    //        LaunchDice();
    //    }

    //    private void Init()
    //    {
    //        bDead = false;

    //        int x = Random.Range(0, 360);
    //        print("x: "+x);
    //        int y = Random.Range(0, 360);
    //        int z = Random.Range(0, 360);
    //        launchRotation = new Vector3(x, y, z);

    //        x = Random.Range(0, 25);
    //        y = Random.Range(0, 25);
    //        z = Random.Range(0, 25);
    //        launchForce = new Vector3(x, y, z);

    //        x = Random.Range(0, 50);
    //        y = Random.Range(0, 50);
    //        z = Random.Range(0, 50);
    //        launchTorque = new Vector3(x, y, z);

    //        rb.maxAngularVelocity = 1000;
    //    }

    //    private void LaunchDice()
    //    {
    //        transform.position = initialPos;
    //        transform.rotation = Quaternion.Euler(launchRotation);
    //        rb.velocity = launchForce * velocityFactor;

    //        rb.AddTorque(launchTorque * angularVelocityFactor, ForceMode.VelocityChange);
    //    }

    //    private void Update()
    //    {
    //#if UNITY_EDITOR
    //        if (Input.GetKey(KeyCode.Space)){
    //            Init();

    //            if (rigState != RigState.regular)
    //                RiggDie();

    //            LaunchDice();
    //        }
    //#endif

    //        if (bDead) return;

    //        if (CheckStop())
    //        {
    //            int resultIndex = FindFaceResult();
    //            print(resultIndex+1);
    //            bDead = true;
    //            if(onRollOver != null)
    //            {
    //                onRollOver(resultIndex + 1);
    //            }
    //        }
    //    }

    //    private bool CheckStop()
    //    {
    //        if(rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
    //            return true;

    //        return false;
    //    }

    //    private int FindFaceResult()
    //    {
    //        int maxIndex = 0;
    //        for(int i = 0; i<faceDetectors.Count; i++)
    //        {
    //            if (faceDetectors[maxIndex].position.y < faceDetectors[i].position.y)
    //            {
    //                maxIndex = i;
    //            }
    //        }
    //        return maxIndex;
    //    }

    //    private void RiggDie()
    //    {
    //        LaunchDice();
    //        Physics.simulationMode = SimulationMode.Script;
    //        for(int i = 0; i<500; i++)
    //        {
    //            Physics.Simulate(Time.fixedDeltaTime);
    //        }
    //        Physics.simulationMode = SimulationMode.FixedUpdate;

    //        int id = FindFaceResult();

    //        int res = id + 1;
    //        print("previous result was : " + res);

    //        switch (rigState)
    //        {
    //            case RigState.win:
    //                switch (id)
    //                {
    //                    case 0:
    //                        launchRotation = Quaternion.AngleAxis(180, Vector3.forward) * launchRotation;
    //                        break;
    //                    case 1:
    //                        launchRotation = Quaternion.AngleAxis(90, Vector3.forward) * launchRotation;
    //                        break;
    //                    case 2:
    //                        launchRotation = Quaternion.AngleAxis(180, Vector3.forward) * launchRotation;
    //                        break;
    //                }
    //                break;

    //            case RigState.lose:
    //                switch (id)
    //                {
    //                    case 3:
    //                        launchRotation = Quaternion.AngleAxis(-90, Vector3.forward) * launchRotation;
    //                        break;
    //                    case 4:
    //                        launchRotation = Quaternion.AngleAxis(180, Vector3.forward) * launchRotation;
    //                        break;
    //                    case 5:
    //                        launchRotation = Quaternion.AngleAxis(180, Vector3.forward) * launchRotation;
    //                        break;
    //                }
    //                break;
    //        }


    //    }

    public void Roll()
    {
        Animator animator = GetComponent<Animator>();
        int min = 1;
        int max = 6;

        switch (rigState)
        {
            case RigState.lose:
                max = 3;
                break;

            case RigState.win:
                min = 4;
                break;

            default:
                break;
        }

        result = Random.Range(min, max+1);
        int animID = Random.Range(0, 6);
        animator.SetTrigger(result + "_"+animID);
    }

    public void OnRollOverAnimCallback()
    {
        onRollOver(result);
    }
}
