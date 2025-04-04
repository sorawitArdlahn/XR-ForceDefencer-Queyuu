using Utils.Persistence;
using UnityEngine;
using System.Collections;


namespace GameController {
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("Game Data Detail")]
    public GameData currentGameData;

    //LEVEL MANAGER SECTION
    
    void Awake()
    {
        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(DelayedSetNextPhase());
    }

    private IEnumerator DelayedSetNextPhase()
    {
        yield return new WaitForSeconds(3f); // Wait for 4 seconds
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
    }


    public void newGame() {
        Instance.currentGameData = new GameData{
            Name = "Demo",
            playerData = new Model.Stats.PlayerData(), //TODO : Important.
            levelData = new Model.Level.LevelData() 
        };

    }

    //This Game only have 1 save slot.
    public void LoadGame() {
        currentGameData = SaveLoadSystem.Instance.LoadGame("Demo");
    }

    public void SaveGame() {
        if(currentGameData != null) SaveLoadSystem.Instance.SaveGame(currentGameData);
    }

    public void DeleteGame() {
        SaveLoadSystem.Instance.DeleteGame(currentGameData.Name);
    }

    void OnReset() {
        currentGameData = null;
    }

    //TODO : This is only Placeholder for button commands

}
}
