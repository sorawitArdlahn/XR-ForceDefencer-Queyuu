using System.Persistence;
using UnityEngine;


namespace GameController {
public class GameManager : MonoBehaviour
{
    //GameState GameState;
    public static GameManager Instance = null;

    public GameData currentGameData;

    //LEVEL MANAGER SECTION
    
    void Awake()
    {
        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }
        
        DontDestroyOnLoad(gameObject);
    }

    public void newGame() {
        Instance.currentGameData = new GameData{
            Name = "Demo",
            currentLevelName = "PreparationScene",
            currentLevelIndex = 1
        };

    }

    public void LoadGame() {
        currentGameData = SaveLoadSystem.Instance.LoadGame("Demo");
    }

    public void SaveGame() {
        if(currentGameData != null) SaveLoadSystem.Instance.SaveGame(currentGameData);
    }

    // Start is called before the first frame update

    //TODO : This is only Placeholder for button commands
    public void NewGameButton() {
        newGame();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("New Game Button Pressed.");

        //DEBUGGING
    }

    public void LoadGameButton() {
        Instance.LoadGame();
        GameStateManager.Instance.SetNextPhase(GameState.BattlePreparation);
        Debug.Log("Save Loaded.");
    }

    public void SaveGameButton() {
        Instance.SaveGame();
        Debug.Log("Game Saved.");
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
    }

    void Update() {
        Debug.Log("Hello.");
    }

    void OnReset() {
        currentGameData = null;
    }

}
}
