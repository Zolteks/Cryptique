using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChaptersManager : MonoBehaviour
{
    [SerializeField] private GameProgressionManager gameProgressionManager;

    [SerializeField] private GameObject chapterButtonPrefab;
    [SerializeField] private UI_RegionDetail regionDetail;

    private void Start()
    {

        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.GetInstance();
        }

        List<string> chapters = gameProgressionManager.GetChapters();

        for (int i = 0; i < chapters.Count; i++)
        {
            GameObject chapterButton = Instantiate(chapterButtonPrefab, transform);
            chapterButton.name = chapters[i];

            string roman = ToRoman(i + 1);
            string chapterName = $"Chapter {roman}: {chapters[i]}";
            chapterButton.GetComponentInChildren<TextMeshProUGUI>().text = chapterName;

            string currentChapter = chapters[i]; // capture pour closure
            chapterButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                regionDetail.DisplayChapter(currentChapter);
            });
        }
    }

    public static string ToRoman(int number)
    {
        Dictionary<int, string> romanMap = new()
        {
            {1000, "M"}, {900, "CM"}, {500, "D"}, {400, "CD"},
            {100, "C"}, {90, "XC"}, {50, "L"}, {40, "XL"},
            {10, "X"}, {9, "IX"}, {5, "V"}, {4, "IV"}, {1, "I"}
        };

        string result = "";
        foreach (var kvp in romanMap)
        {
            while (number >= kvp.Key)
            {
                result += kvp.Value;
                number -= kvp.Key;
            }
        }
        return result;
    }
}
