using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_RegionDetail : MonoBehaviour
{
    [SerializeField] private GameProgressionManager gameProgressionManager;

    [SerializeField] private GameObject regionDetailPrefab;
    [SerializeField] private TextMeshProUGUI pageTitle;

    private Dictionary<string, List<string>> chapterLevel = new();
    private Dictionary<string, CollectibleProgress> levelCollectibles = new();

    private void Start()
    {
        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.GetInstance();
        }
    }


    public void DisplayChapter(string chapter)
    {
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);

        levelCollectibles.Clear();

        List<string> regions = gameProgressionManager.GetRegions(chapter);

        if (regions.Count == 0) return;

        pageTitle.text = chapter;

        for (int i = 0; i < regions.Count; i++)
        {
            GameObject regionDetail = Instantiate(regionDetailPrefab, this.transform);
            string levelName = regions[i];
            regionDetail.name = levelName;

            regionDetail.GetComponentInChildren<TextMeshProUGUI>().text = levelName;

            levelCollectibles[levelName] = new CollectibleProgress { found = 2, total = 5 };

            if (!gameProgressionManager.IsRegionUnlocked(levelName))
            {
                levelCollectibles[levelName].total = 0;
                Image hideUnlockedButton = GetComponentInChildByName<Image>(regionDetail.transform, "HideRegion");
                hideUnlockedButton.GameObject().SetActive(true);
                regionDetail.GetComponentInChildren<Button>().interactable = false;
            }

            TextMeshProUGUI collectibleText = GetComponentInChildByName<TextMeshProUGUI>(regionDetail.transform, "CollectibleCount");
            collectibleText.text = levelCollectibles[levelName].found + "/" + levelCollectibles[levelName].total;


            regionDetail.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                //Load chapter
                Debug.Log($"{levelName}");
                SaveAndLoadScene.Excute(levelName); 
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
