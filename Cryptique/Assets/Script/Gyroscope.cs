using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    public int bufferSize = 30;
    public float sensitivity = 2.0f; // 2x la variation moyenne
    public float minMovementAngle = 1.5f; // pour ignorer les variations trop faibles

    private List<float> rotationSpeeds = new();
    private Quaternion lastRotation;
    private bool hasReference = false;

    private bool yoooooo = false;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        Quaternion current = ConvertGyro(Input.gyro.attitude);

        if (!hasReference)
        {
            lastRotation = current;
            hasReference = true;
            return;
        }

        float angle = Quaternion.Angle(lastRotation, current);
        lastRotation = current;

        if (rotationSpeeds.Count >= bufferSize)
            rotationSpeeds.RemoveAt(0);

        rotationSpeeds.Add(angle);

        float avg = rotationSpeeds.Average();
        float var = Mathf.Abs(rotationSpeeds.Average() - angle); // variation simple

        float threshold = avg + (var * sensitivity);

        if (angle > threshold && angle > minMovementAngle)
        {
            Debug.LogWarning(" Mouvement suspect détecté !");
            // Tu peux déclencher une action ici (reset, effet, message…)
            yoooooo = true;
        }
    }

    Quaternion ConvertGyro(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
        GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);

        if(yoooooo)
        {
            GUI.skin.label.fontSize = Screen.width / 20;
            GUILayout.Label("Mouvement suspect détecté !");
            GUI.skin.label.fontSize = Screen.width / 40;
        }

    }

}
