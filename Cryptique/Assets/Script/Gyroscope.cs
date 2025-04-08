using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Gyroscope : MonoBehaviour
{
    [Header("Paramètres")]
    public float calibrationDuration = 3f;
    public float sensitivityMultiplier = 2f;
    public float minMovementThreshold = 0.05f;

    [Header("Événements")]
    public UnityEvent OnMotionDetected;

    private float calibrationTimer = 0f;
    private bool isCalibrating = false;
    private bool isCalibrated = false;
    private bool movementDetected = false;

    private Vector3 lastAcceleration;
    private bool hasReference = false;

    private List<float> calibrationData = new();
    private float calibratedAverage = 0f;
    private float calibratedVariation = 0f;

    void OnEnable()
    {
        StartCalibration();
    }

    void Update()
    {
        Vector3 current = Input.acceleration;

        if (!hasReference)
        {
            lastAcceleration = current;
            hasReference = true;
            return;
        }

        float delta = Vector3.Distance(lastAcceleration, current);
        lastAcceleration = current;

        if (isCalibrating)
        {
            calibrationTimer += Time.deltaTime;
            calibrationData.Add(delta);

            if (calibrationTimer >= calibrationDuration)
            {
                Calibrate();
            }

            return;
        }

        if (isCalibrated && !movementDetected)
        {
            float threshold = calibratedAverage + calibratedVariation * sensitivityMultiplier;

            if (delta > threshold && delta > minMovementThreshold)
            {
                movementDetected = true;
                OnMotionDetected?.Invoke();
            }
        }
    }

    private void StartCalibration()
    {
        calibrationTimer = 0f;
        calibrationData.Clear();
        isCalibrating = true;
        isCalibrated = false;
        movementDetected = false;
        hasReference = false;
    }

    private void Calibrate()
    {
        if (calibrationData.Count == 0) return;

        calibratedAverage = calibrationData.Average();
        float sumSquaredDiff = calibrationData.Sum(d => Mathf.Pow(d - calibratedAverage, 2));
        calibratedVariation = Mathf.Sqrt(sumSquaredDiff / calibrationData.Count);

        isCalibrated = true;
        isCalibrating = false;
    }
}