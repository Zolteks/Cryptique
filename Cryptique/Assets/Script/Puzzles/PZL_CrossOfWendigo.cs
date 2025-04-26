using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PZL_CrossOfWendigo : Puzzle
{
    [SerializeField]
    private GameObject wendigo;
    [SerializeField]
    private Animator wendigoAnimator;
    [SerializeField] 
    private PC_PlayerController playerController;
    [SerializeField]
    private GameObject target;


    private bool isMoved = false;


    private void Update()
    {
        if(isMoved)
        {
            //Walk to the target by speed
            wendigo.transform.position = Vector3.MoveTowards(wendigo.transform.position, target.transform.position, Time.deltaTime * 2f);
            //Play the animation
            wendigoAnimator.SetBool("isWalking", true);

            //Check if the wendigo reached the target
            if (Vector3.Distance(wendigo.transform.position, target.transform.position) < 0.1f)
            {
                wendigoAnimator.SetBool("isWalking", false);
                Debug.Log("Wendigo reached the target");
                //Complete the puzzle
                Complete();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            isMoved = true;
            playerController.DisableInput();
        }
    }



    //[SerializeField] private Transform lastTile;
    //[SerializeField] private GameObject player;

    //[SerializeField] private List<IN_Bush> bush;

    //[SerializeField] private Gyroscope gyroscope;

    //bool isHidden = false;

    //private SGL_InputManager m_inputManager;

    //public void Awake()
    //{
    //    m_inputManager = SGL_InputManager.Instance;
    //}


    //public void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        StartCoroutine(StartTimer());
    //    }

    //    if (!isHidden)
    //    {
    //        for (int i = 0; i < bush.Count; i++)
    //        {
    //            if (bush[i].getIsInteract())
    //            {
    //                Debug.Log("Player Interact");
    //                StopAllCoroutines();
    //                isHidden = true;
    //                gyroscope.enabled = true;
    //            }
    //        }
    //    }
    //    else if(gyroscope.isCalibrated)
    //    {
    //        StartCoroutine(HiddenTimmer());
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        isDetectMovement();
    //        Debug.Log("Movement detected");
    //    }
    //}

    //public void isDetectMovement()
    //{
    //    LoseGame();
    //}

    //public void Success()
    //{
    //    Debug.Log($"Puzzle completed successfully.");
    //}

    //public void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        StartCoroutine(StartTimer());
    //    }
    //}

    //private IEnumerator StartTimer()
    //{
    //    Debug.Log("Start timer");

    //    //If the player didn't move for 5 fists seconds he dies
    //    yield return new WaitForSeconds(5f);

    //    if (!isHidden)
    //    {
    //        Debug.Log("Player is not hidden");
    //        LoseGame();
    //    }
    //}
    //private IEnumerator HiddenTimmer()
    //{
    //    yield return new WaitForSeconds(5f);
    //    Debug.Log("Player is hidden");
    //    Complete();
    //}

    //private void LoseGame()
    //{
    //    player.transform.position = lastTile.position;
    //    gyroscope.enabled = false;
    //    StopAllCoroutines();
    //}
}
