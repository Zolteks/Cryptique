using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class PZL_Flipper : MonoBehaviour
{

    public Button bLeftButtonBumper;
    public Button bRightButtonBumper;
    public Slider bSliderLauncher;

    public GameObject gLeftPivot;
    public GameObject gRightPivot;
    public GameObject gLauncher;
    public GameObject gLastPos;

    public Bumper bLeftBumper;
    public Bumper bRightBumper;

    private float fZStartPosition;
    private float fZEndPosition;

    private Quaternion qLeftPivotRotation;
    private Quaternion qRightPivotRotation;


    // Start is called before the first frame update
    void Start()
    {
        fZStartPosition = gLauncher.transform.position.z;
        fZEndPosition = gLastPos.transform.position.z;

        qLeftPivotRotation = gLeftPivot.transform.rotation;
        qRightPivotRotation = gRightPivot.transform.rotation;
    }

    public void ActivateLeftBumper()
    {
        Quaternion rotation = Quaternion.Euler(0, -60, 0);

        gLeftPivot.transform.rotation = qLeftPivotRotation * rotation;

        bLeftBumper.LaunchBalls();

        StartCoroutine(ResetBumperAfterDelay(0.3f, gLeftPivot, qLeftPivotRotation));
    }

    public void ActivateRightBumper()
    {
        Quaternion rotation = Quaternion.Euler(0, 60, 0);

        gRightPivot.transform.rotation = qRightPivotRotation * rotation;

        bRightBumper.LaunchBalls();

        StartCoroutine(ResetBumperAfterDelay(0.3f, gRightPivot, qRightPivotRotation));
    }

    private IEnumerator ResetBumperAfterDelay(float delay, GameObject bumper, Quaternion initialRotation)
    {
        yield return new WaitForSeconds(delay);
        bumper.transform.rotation = initialRotation;
    }

}
