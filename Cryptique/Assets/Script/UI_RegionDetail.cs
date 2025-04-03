using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RegionDetail : MonoBehaviour
{
    [SerializeField] private GameObject regionDetailPrefab;
    [SerializeField] private TextMeshProUGUI pageTitle;

    private Dictionary<string, List<string>> chapterLevel = new();
    private Dictionary<string, CollectibleProgress> levelCollectibles = new();

    private void Awake()
    {
        // Simulation des niveaux
        chapterLevel["Wendigo"] = new() { "Level 1", "Level 2", "Level 3", "Level 4" };
        chapterLevel["JeSaisPas"] = new() { "Level 1", "Level 2", "Level 3" };
    }

    public void DisplayChapter(string chapter)
    {
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);

        levelCollectibles.Clear();

        if (!chapterLevel.ContainsKey(chapter)) return;

        pageTitle.text = chapter;

        List<string> levels = chapterLevel[chapter];

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject regionDetail = Instantiate(regionDetailPrefab, this.transform);
            string levelName = levels[i];
            regionDetail.name = levelName;

            regionDetail.GetComponentInChildren<TextMeshProUGUI>().text = levelName;

            levelCollectibles[levelName] = new CollectibleProgress { found = 2, total = 5 };

            TextMeshProUGUI collectibleText = GetComponentInChildByName<TextMeshProUGUI>(regionDetail.transform, "CollectibleCount");
            collectibleText.text = levelCollectibles[levelName].found + "/" + levelCollectibles[levelName].total;


            regionDetail.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Go to {chapter} in {levelName}");
            });
        }
    }


    public static T GetComponentInChildByName<T>(Transform parent, string childName) where T : Component
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true)) // true = inclut inactifs
        {
            if (child.name == childName)
            {
                return child.GetComponent<T>();
            }
        }

        return null;
    }

}

class CollectibleProgress
{
    public int found;
    public int total;
}
