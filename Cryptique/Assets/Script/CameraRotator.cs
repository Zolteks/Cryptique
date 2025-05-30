using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraDirdection
{
    bot,
    right,
    top,
    left,
}
public delegate void DirUpdateDelegate(CameraDirdection newDir);

public class CameraRotator : MonoBehaviour
{
    public event DirUpdateDelegate eDirectionUpdate;

    CameraDirdection m_currentDir;
    bool m_busy = false;

    Dictionary<CameraDirdection, bool> allowedRotations;

    bool isDragging = false;
    private Vector2 lastTouchPosition;

    [SerializeField] private float swipeThreshold = 50f;
    private bool hasRotated = false;

     private SaveSystemManager saveSystemManager;
    //[SerializeField] private GameObject slideBoutons;

    [SerializeField] private Animator animator;

    private void Start()
    {
        saveSystemManager = SaveSystemManager.Instance;
        ResetAllowedDirections();
        m_currentDir = CameraDirdection.bot;
        eDirectionUpdate?.Invoke(m_currentDir);
    }

    void Update()
    {
        // Debug controls for rotation
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
//            RotateRight();
//        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
//            RotateLeft();

        //if (saveSystemManager)
        //{
        //    if (saveSystemManager.GetGameData().slideMode == SlideMode.Slide)
        //    {
        //        slideBoutons.SetActive(false);
        //        HandleTouchRotation();
        //    }
        //    else
        //    {
        //        slideBoutons.SetActive(true);
        //    }
        //}


//        if (saveSystemManager)
//        {
//            if (saveSystemManager.GetGameData().settings.slideMode == SlideMode.Slide  && slideBoutons.activeSelf)
//            {
//                slideBoutons.SetActive(false);
//                HandleTouchRotation();
//            }
//            else
//            {
//                slideBoutons.SetActive(true);
//            }
//        }
//#endif
    }

    public void UpdateWalls()
    {
        eDirectionUpdate?.Invoke(m_currentDir);
    }

    public CameraDirdection GetDirection()
    {
        return m_currentDir;
    }

    public void SetAllowedRotation(Dictionary<CameraDirdection, bool> value)
    {
        allowedRotations = value;
    }

    public void ResetAllowedDirections()
    {
        allowedRotations = new Dictionary<CameraDirdection, bool>() {
            {CameraDirdection.bot, true },
            {CameraDirdection.right, true },
            {CameraDirdection.top, true },
            {CameraDirdection.left, true },
        };
    }

    public void ForceOrientation(CameraDirdection dir)
    {
        while(m_currentDir != dir)
        {
            m_currentDir = (CameraDirdection)(((int)m_currentDir + 1) % 4);
            FastRotate();
        }
    }

    private void FastRotate()
    {
        transform.rotation*= Quaternion.Euler(0, -90, 0);
        eDirectionUpdate?.Invoke(m_currentDir);
    }

    public void RotateRight()
    {
        if (m_busy || IN_EscapeArrow.m_isBusy) return;
   
        StartCoroutine(CoroutineRotate(transform.rotation, transform.rotation * Quaternion.Euler(0, -90, 0), .5f, 1));
    }
    public void RotateLeft()
    {

        if (m_busy) return;
    
        StartCoroutine(CoroutineRotate(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), .5f, -1));
    }
    IEnumerator CoroutineRotate(Quaternion start, Quaternion end, float duration, int incrementValue)
    {
        int id = (int)(m_currentDir + incrementValue)%4;
        if (id < 0) id = 3;

        float t = 0;
        m_busy = true;

        if (false == allowedRotations[(CameraDirdection)id]){
            animator.SetTrigger("Shake");
            yield break;
        }



        while(t <= duration/2)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, t/ duration);
            yield return null;
        }

        m_currentDir += incrementValue;
        if ((int)m_currentDir < 0) m_currentDir = CameraDirdection.left;
        else if ((int)m_currentDir > 3) m_currentDir = CameraDirdection.bot;
        eDirectionUpdate?.Invoke(m_currentDir);

        while (t <= duration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, t / duration);
            yield return null;
        }

        m_busy = false;
    }

    private void OnEnable()
    {
        SGL_InputManager.Instance.OnStartTouch += HandleStartTouch;
        SGL_InputManager.Instance.OnEndTouch += HandleEndTouch;
    }
    private void OnDisable()
    {
        if (SGL_InputManager.Instance == null) return;
        SGL_InputManager.Instance.OnStartTouch -= HandleStartTouch;
        SGL_InputManager.Instance.OnEndTouch -= HandleEndTouch;
    }

    private void HandleStartTouch(Vector2 pos, float time)
    {
        isDragging = true;
    }

    private void HandleEndTouch(Vector2 pos, float time)
    {
        isDragging = false;
    }

    //private void HandleTouchRotation()
    //{
    //    if (isDragging)
    //    {
    //        Vector2 currentTouchPosition = SGL_InputManager.Instance.GetTouchPosition();
    //        Vector2 delta = currentTouchPosition - lastTouchPosition;

    //        if (!hasRotated && Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && Mathf.Abs(delta.x) > swipeThreshold)
    //        {
    //            if (delta.x > 0)
    //            {
    //                RotateRight();
    //            }
    //            else
    //            {
    //                RotateLeft();
    //            }

    //            hasRotated = true; // empecher de repeter la rotation
    //        }
    //    }
    //    else
    //    {
    //        hasRotated = false; // reinitialise pour le prochain swipe
    //    }
    //}

    public void SetIsBusy(bool isBusy)
    {
        m_busy = isBusy;
    }
}
