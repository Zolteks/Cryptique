using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PZL_IceDoor : MonoBehaviour
{
    [Header("Settings")]
    public string lightBeamTag = "LightBeam";
    public float disableDelay = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(lightBeamTag))
        {
            StartCoroutine(CoroutineDisableWithDelay());
        }
    }

    private IEnumerator CoroutineDisableWithDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}