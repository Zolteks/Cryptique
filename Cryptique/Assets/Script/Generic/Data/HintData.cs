using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "HintData", menuName = "Cryptique/Hint")]
public class HintData : ScriptableObject
{
    private bool isUnlocked = false;

    public string defaultHintText;
    [SerializeField]
    private LocalizedString hintText;

    private float timeToShow = 5f;

    public string GetText()
    {
        return hintText.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultHintText);
    }

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public void SetTimeToShow(float time)
    {
        timeToShow = time;
    }

    public float GetTimeToShow()
    {
        return timeToShow;
    }

    public void StartTimeToShow(MonoBehaviour context)
    {
        context.StartCoroutine(ShowHintCoroutine());
    }

    private IEnumerator ShowHintCoroutine()
    {
        yield return new WaitForSeconds(timeToShow);
        SetUnlocked(true);
        Debug.Log($"Hint unlocked: {GetText()}");
    }


}

