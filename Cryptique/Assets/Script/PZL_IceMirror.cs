using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_IceMirror : MonoBehaviour
{
    [Header("Laser Settings")]
    public GameObject lightBeamPrefab;
    public float fBeamLength = 50f;
    public float fBeamDiameter = 0.1f;
    public string sMirrorTag = "Mirror";
    public string sLightBeamTag = "LightBeam";

    private GameObject currentBeam;
    private bool bIsBeaming;

    [Header("GameObject Interactions")]
    public string sDoorTag = "LockedDoor";
    public PZL_UndergroundLakeDoor NextLockedDoor;

    private void Start()
    {
        bIsBeaming = false;
    }

    void Update()
    {
        if (!bIsBeaming && currentBeam != null)
        {
            Destroy(currentBeam);
            currentBeam = null;
        }
        else if (bIsBeaming)
        {
            ShootReflectedLaser();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(sLightBeamTag) && !bIsBeaming)
        {
            bIsBeaming = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(sLightBeamTag))
        {
            bIsBeaming = false;
        }
    }

    void ShootReflectedLaser()
    {
        if (currentBeam != null) Destroy(currentBeam);

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, fBeamLength))
        {
            currentBeam = CreateBeam(origin, hit.point);

            // Modifié pour mieux gérer les portes
            if (hit.collider.CompareTag("Mirror"))
            {
                var otherMirror = hit.collider.GetComponent<PZL_IceMirror>();
                if (otherMirror != null) otherMirror.ActivateFromBeam();
            }
            else if (hit.collider.gameObject.tag == "LockedDoor")
            {
                NextLockedDoor.PuzzleEnded();
                return;
            }
        }
        else
        {
            currentBeam = CreateBeam(origin, origin + direction * fBeamLength);
        }
    }

    public void ActivateFromBeam()
    {
        if (!bIsBeaming)
        {
            bIsBeaming = true;
            StartCoroutine(ResetActivation());
        }
    }

    IEnumerator ResetActivation()
    {
        yield return new WaitForEndOfFrame();
        if (!IsBeingHitByBeam())
        {
            bIsBeaming = false;
        }
    }

    bool IsBeingHitByBeam()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(sLightBeamTag))
            {
                return true;
            }
        }
        return false;
    }

    GameObject CreateBeam(Vector3 start, Vector3 end)
    {
        GameObject beam = Instantiate(lightBeamPrefab);
        beam.transform.position = (start + end) / 2f;
        beam.transform.rotation = Quaternion.LookRotation(end - start);
        beam.transform.localScale = new Vector3(fBeamDiameter, fBeamDiameter, Vector3.Distance(start, end));
        return beam;
    }

    void OnDisable()
    {
        if (currentBeam != null)
        {
            Destroy(currentBeam);
        }
        bIsBeaming = false;
    }

}