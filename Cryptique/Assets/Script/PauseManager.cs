
using TMPro;
using UnityEngine;

class PauseManager : MonoBehaviour
{
    [SerializeField] private UI_RegionDetail regionDetail;

    private GameProgressionManager gameProgressionManager;

    private void Awake()
    {
        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }
    }

    private void Start()
    {
        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }
    }

    public void active()
    {
        string chapterName = gameProgressionManager.GetCurrentChapter().defaultChapterName;
        regionDetail.DisplayChapterEnableButton(chapterName);
    }
}

