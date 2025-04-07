using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // N�cessaire pour EventTrigger

public class PZL_Flipper : MonoBehaviour
{
    public Button bLeftButtonBumper;
    public Button bRightButtonBumper;
    public Slider sSliderLauncher;

    public GameObject gLeftPivot;
    public GameObject gRightPivot;
    public Transform gLauncher;
    public GameObject gLastPos;

    public Bumper bLeftBumper;
    public Bumper bRightBumper;
    public Bumper pZL_Launcher;

    private Quaternion qLeftPivotRotation;
    private Quaternion qRightPivotRotation;

    private float fZStartPosition;
    private float fZEndPosition;

    private bool bIsSliding = false;

    // Variables pour l'animation fluide du cube
    private float animationDuration = 0.5f; // Dur�e de l'animation
    private float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fZStartPosition = gLauncher.position.z;
        fZEndPosition = gLastPos.transform.position.z;

        qLeftPivotRotation = gLeftPivot.transform.rotation;
        qRightPivotRotation = gRightPivot.transform.rotation;

        sSliderLauncher.onValueChanged.AddListener(OnSliderValueChanged);

        // Ajouter un EventTrigger pour d�tecter quand l'utilisateur commence et termine d'interagir avec le slider
        EventTrigger trigger = sSliderLauncher.GetComponent<EventTrigger>();

        // D�tecter le d�but de l'interaction
        EventTrigger.Entry entryStart = new EventTrigger.Entry();
        entryStart.eventID = EventTriggerType.PointerDown;
        entryStart.callback.AddListener((data) => { OnSliderPointerDown(); });
        trigger.triggers.Add(entryStart);

        // D�tecter la fin de l'interaction
        EventTrigger.Entry entryEnd = new EventTrigger.Entry();
        entryEnd.eventID = EventTriggerType.PointerUp;
        entryEnd.callback.AddListener((data) => { OnSliderPointerUp(); });
        trigger.triggers.Add(entryEnd);
    }

    // M�thode pour activer le bumper gauche
    public void ActivateLeftBumper()
    {
        Quaternion rotation = Quaternion.Euler(0, -60, 0);
        StartCoroutine(MoveBumperAfterDelay(0f, 0.1f, gLeftPivot, qLeftPivotRotation * rotation));
        bLeftBumper.Returnball();
        StartCoroutine(MoveBumperAfterDelay(0.3f, 0.3f, gLeftPivot, qLeftPivotRotation));
    }

    // M�thode pour activer le bumper droit
    public void ActivateRightBumper()
    {
        Quaternion rotation = Quaternion.Euler(0, 60, 0);
        StartCoroutine(MoveBumperAfterDelay(0f, 0.1f, gRightPivot, qRightPivotRotation * rotation));
        bRightBumper.Returnball();
        StartCoroutine(MoveBumperAfterDelay(0.3f, 0.3f, gRightPivot, qRightPivotRotation));
    }

    // M�thode appel�e chaque fois que la valeur du slider change
    void OnSliderValueChanged(float value)
    {
        if (bIsSliding)
        {
            float newZPosition = Mathf.Lerp(fZStartPosition, fZEndPosition, value);

            gLauncher.localPosition = new Vector3(gLauncher.localPosition.x, gLauncher.localPosition.y, newZPosition);
        }
    }

    // M�thode appel�e lorsque l'utilisateur commence � interagir avec le slider
    void OnSliderPointerDown()
    {
        bIsSliding = true;
    }

    // M�thode appel�e lorsque l'utilisateur rel�che le clic sur le slider
    void OnSliderPointerUp()
    {
        float sliderValue = sSliderLauncher.value;
        float defaultBumperForce = pZL_Launcher.GetBumperForce();

        pZL_Launcher.SetBumperForce(defaultBumperForce * sliderValue);

        bIsSliding = false;
        // R�initialiser la valeur du slider � 0 lorsque l'utilisateur rel�che
        sSliderLauncher.value = 0;

        // Appeler la fonction de lancement de la balle ou d'autres actions
        pZL_Launcher.LaunchBalls();

        StartCoroutine(SmoothReturnToInitialPosition(gLauncher));
    }

    // Coroutine pour actionner la rotation du bumper apr�s un d�lai
    private IEnumerator MoveBumperAfterDelay(float delay, float duration, GameObject bumper, Quaternion initialRotation)
    {
        // Attend avant de commencer la remise en place
        yield return new WaitForSeconds(delay);

        float timeElapsed = 0f;

        Quaternion startRotation = bumper.transform.rotation;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            bumper.transform.rotation = Quaternion.Lerp(startRotation, initialRotation, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // S'assure qu'on atteint bien la rotation finale
        bumper.transform.rotation = initialRotation;
    }


    private IEnumerator SmoothReturnToInitialPosition(Transform startPos)
    {
        Vector3 targetPos = new Vector3(startPos.localPosition.x, startPos.localPosition.y, fZStartPosition); // Position initiale souhait�e

        timeElapsed = 0f; // R�initialisation du temps �coul�

        while (timeElapsed < animationDuration)
        {
            // Calcul de la nouvelle position en fonction du temps �coul�
            float t = timeElapsed / animationDuration;

            // Calcul de la position interm�diaire avec Lerp
            gLauncher.localPosition = Vector3.Lerp(startPos.localPosition, targetPos, t);

            // Incr�mentation du temps �coul�
            timeElapsed += Time.deltaTime;

            yield return null; // Attendre la prochaine frame
        }

        // Assurer que le cube arrive bien � la position finale � la fin de l'animation
        gLauncher.localPosition = targetPos;
    }
}
