using System.Persistence;
using UnityEngine;
using System;


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
        GameStateManager.Instance.SetNextPhase(GameState.MainMenu);
    }

        public void newGame() {
        Instance.currentGameData = new GameData{
            Name = "Demo",
            currentLevelName = "PreparationScene",
            currentLevelIndex = 1,
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
