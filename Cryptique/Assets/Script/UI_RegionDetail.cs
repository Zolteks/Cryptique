using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_RegionDetail : MonoBehaviour, ILocalizedElement
{
    private GameProgressionManager gameProgressionManager;

    [SerializeField] private GameObject regionDetailPrefab;
    [SerializeField] private TextMeshProUGUI pageTitle;

    private LanguageManager languageManager;

    private void Start()
    {
        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }
        if (languageManager == null)
        {
            languageManager = LanguageManager.Instance;
        }
        if(languageManager)
        {
            languageManager.Register(this);
        }
    }

    private void OnDestroy()
    {
        languageManager.Unregister(this);
    }

    public void RefreshLocalized()
    {
        List<RegionData> regionsData = gameProgressionManager.GetRegions();
        //Refresh the buttons text
        foreach (Transform child in this.transform)
        {   
            if(child.GetComponentInChildren<TextMeshProUGUI>())
            {

                child.GetComponentInChildren<TextMeshProUGUI>().text = regionsData[child.GetSiblingIndex()].GetName();
            }
        }
    }

    public void DisplayChapter(string chapter)
    {
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);

        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }

        List<RegionData> regionsData = gameProgressionManager.GetRegions();
        if (regionsData.Count == 0) return;

        pageTitle.text = chapter;

        for (int i = 0; i < regionsData.Count; i++)
        {
            GameObject regionDetail = Instantiate(regionDetailPrefab, this.transform);
            string levelName = regionsData[i].GetName();
            regionDetail.name = levelName;

            regionDetail.GetComponentInChildren<TextMeshProUGUI>().text = levelName;
       
            if (!gameProgressionManager.IsRegionUnlocked(levelName))
            {
                Image hideUnlockedButton = GetComponentInChildByName<Image>(regionDetail.transform, "HideRegion");
                hideUnlockedButton.GameObject().SetActive(true);
                regionDetail.GetComponentInChildren<Button>().interactable = false;
            }

            int totalItems = gameProgressionManager.GetTotalItemsInRegion(levelName);
            int foundItems = gameProgressionManager.GetCollectedItemsInRegion(levelName);
            TextMeshProUGUI collectibleText = GetComponentInChildByName<TextMeshProUGUI>(regionDetail.transform, "CollectibleCount");
            collectibleText.text = foundItems + "/" + totalItems;

            RegionData region = regionsData[i];
            regionDetail.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                SaveAndLoadScene.Excute(region.defaultRegionName);
                gameProgressionManager.SetCurrentRegion(i);
            });
        }
    }

    public void DisplayChapterEnableButton(string chapter)
    {
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);

        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }

        List<RegionData> regionsData = gameProgressionManager.GetRegions();
        if (regionsData.Count == 0) return;

        pageTitle.text = chapter;

        for (int i = 0; i < regionsData.Count; i++)
        {
            GameObject regionDetail = Instantiate(regionDetailPrefab, this.transform);
            string levelName = regionsData[i].GetName();
            regionDetail.name = levelName;

            regionDetail.GetComponentInChildren<TextMeshProUGUI>().text = levelName;

            if (!gameProgressionManager.IsRegionUnlocked(levelName))
            {
                Image hideUnlockedButton = GetComponentInChildByName<Image>(regionDetail.transform, "HideRegion");
                hideUnlockedButton.GameObject().SetActive(true);
            }

            int totalItems = gameProgressionManager.GetTotalItemsInRegion(levelName);
            int foundItems = gameProgressionManager.GetCollectedItemsInRegion(levelName);
            TextMeshProUGUI collectibleText = GetComponentInChildByName<TextMeshProUGUI>(regionDetail.transform, "CollectibleCount");
            collectibleText.text = foundItems + "/" + totalItems;

            regionDetail.GetComponentInChildren<Button>().interactable = false;
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