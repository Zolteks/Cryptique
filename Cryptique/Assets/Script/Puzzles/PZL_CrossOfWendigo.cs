using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.InputSystem;

public class PZL_CrossOfWendigo : Puzzle
{
    [SerializeField] private Transform lastTile;
    [SerializeField] private GameObject player;

    [SerializeField] private List<GameObject> buisons;

    [SerializeField] private Gyroscope gyroscope;


    private InputManager inputManager;

    public void Awake()
    {
        inputManager = InputManager.Instance;
    }

    public void Start()
    {
    }

    public void Update()
    {
        
    }

    public void isDetectMovement()
    {
        player.transform.position = lastTile.position;
    }

    public void Success()
    {
        Debug.Log($"Puzzle completed successfully.");
    }

}
