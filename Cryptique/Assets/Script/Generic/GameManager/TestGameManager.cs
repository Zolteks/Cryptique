/*using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log(" Test du GameManager");

        // Test : Changer la position du joueur
        GameManager.GetInstance().UpdatePlayerPosition("tile_01", "Tavern");

        // Test : Changer le chapitre
        GameManager.GetInstance().SetChapter(1, "Wendigo");

        // Test : Résoudre un puzzle
        GameManager.GetInstance().SolvePuzzle("puzzle_women_talking");

        GameManager.GetInstance().CollectItem("SecondItemCollected");

        // Vérification
        Debug.Log($" Position : {GameManager.GetInstance().IsPuzzleSolved("puzzle_dice")}");
        Debug.Log($" Collectible récupéré : {GameManager.GetInstance().IsItemCollected("fake_dice")}");
        Debug.Log(" Sauvegarde effectuée !");
    }
}
*/