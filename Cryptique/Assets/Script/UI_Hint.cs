using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hint : MonoBehaviour, ILocalizedElement
{
    [SerializeField] private GameObject hintPuzzlePrefab;
    [SerializeField] private GameObject hintPrefab;
    private GameProgressionManager gameProgressionManager;

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
        if (languageManager)
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
        List<PuzzleData> puzzles = gameProgressionManager.GetCurrentsPuzzles();
        //Refresh the buttons text
        foreach (Transform child in this.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>())
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = puzzles[child.GetSiblingIndex()].GetPuzzleID();

                //Refresh the hints text
                Transform hintsContainer = child.Find("HintsContainer");
                if (hintsContainer != null)
                {
                    foreach (Transform hint in hintsContainer)
                    {
                        TextMeshProUGUI text = hint.GetComponentInChildren<TextMeshProUGUI>();
                        if (text != null)
                        {
                            int index = hint.GetSiblingIndex();
                            text.text = puzzles[child.GetSiblingIndex()].GetHints()[index].GetText();
                        }
                    }
                }
            }
        }
    }


    public void DisplayHints()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }

        List<PuzzleData> puzzles = gameProgressionManager.GetCurrentsPuzzles();

        Debug.Log("DisplayHints() called. Number of puzzles: " + puzzles.Count);

        foreach (PuzzleData puzzle in puzzles)
        {
            if (puzzle.GetHints() == null || puzzle.GetHints().Count == 0)
                continue;

            GameObject hintPuzzleGO = Instantiate(hintPuzzlePrefab, transform);
            //Afficher le nom du puzzle
            TextMeshProUGUI puzzleNameText = hintPuzzleGO.GetComponentInChildren<TextMeshProUGUI>();
            if(puzzleNameText != null ) 
            {
                puzzleNameText.text = puzzle.GetPuzzleID();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found in hintPuzzlePrefab.");
            }

            // Récupère le container des hints dans le prefab
            Transform hintsContainer = hintPuzzleGO.transform.Find("HintsContainer");
            if (hintsContainer == null)
            {
                Debug.LogWarning("HintsContainer non trouvé dans le prefab.");
                continue;
            }
            hintsContainer.gameObject.SetActive(false); // Masqué au départ

            // Remplit les hints
            foreach (HintData hint in puzzle.GetHints())
            {
                GameObject hintGO = Instantiate(hintPrefab, hintsContainer);

                if (!hint.IsUnlocked())
                {
                    Image hideImage = GetComponentInChildByName<Image>(hintGO.transform, "HideHint");
                    if (hideImage != null)
                    {
                        hideImage.gameObject.SetActive(true);
                    }
                }

                foreach (Transform child in hintGO.transform)
                {
                    Debug.Log(child.name);
                }

                TextMeshProUGUI text = hintGO.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = hint.GetText();
                    Debug.Log(text.text);
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI component not found in hintPrefab.");
                }
            }

            // Ajout du listener sur le bouton
            Button toggleButton = hintPuzzleGO.GetComponentInChildren<Button>();
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(() =>
                {
                    StartCoroutine(AnimateToggleAndRebuild(hintsContainer.gameObject, (RectTransform)transform));
                });
            }
        }
    }


    private IEnumerator AnimateToggleAndRebuild(GameObject container, RectTransform layoutRoot)
    {
        bool expanding = !container.activeSelf;
        container.SetActive(true);

        Vector3 startScale = expanding ? new Vector3(1, 0, 1) : Vector3.one;
        Vector3 endScale = expanding ? Vector3.one : new Vector3(1, 0, 1);

        float duration = 0.25f;
        float elapsed = 0f;

        RectTransform rt = container.GetComponent<RectTransform>();
        rt.localScale = startScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rt.localScale = Vector3.Lerp(startScale, endScale, t);

            // Force le recalcul du layout pendant l’animation
            LayoutRebuilder.MarkLayoutForRebuild(layoutRoot);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        rt.localScale = endScale;

        if (!expanding)
            container.SetActive(false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
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

