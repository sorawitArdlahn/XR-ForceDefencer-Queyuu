using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;
using Model.Stats;
using Model.Level;
using Controller.Level;
using System;

namespace Utils.Persistence {
    [Serializable]

    public class GameData {
        [Header("Game Data Structure")]
        public string Name;
        //TODO : Player Record Data
        public PlayerData playerData;
        //TODO : Inventory and Upgrade Data
        public LevelData levelData;
    }


    public interface ISaveable {
        string Id { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable {
        string Id { get; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : MonoBehaviour {
        public GameData gameData;

        public static SaveLoadSystem Instance;

        IDataService dataService;

        void Awake() {
            if (Instance == null) { Instance = this; }     
            else { Destroy(gameObject); }
            
            dataService = new FileDataService(new JsonSerializer());
        }

        //TODO : Rework to fit real Environment, Currently Only Work On One Scene

        /*
        TODO : EXAMPLE OF BINDING METHOD
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "Menu") return;
            
            Bind<Player, PlayerData>(gameData.playerData);
        }
        */

        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            StartCoroutine(HandleDelayedBinding(scene));
        }

        IEnumerator HandleDelayedBinding(Scene scene) {
            yield return new WaitForEndOfFrame();
            
            yield return null;
            yield return null;
            //IF GAMEMANAGER DOES NOT HAVE GAMEDATA DO NOT PERFORM BINDING  
            if (!GameManager.Instance || GameManager.Instance.currentGameData == null) {
                yield break;
            }

            
            if (GameStateManager.Instance.GetCurrentGameState() == GameState.BattlePreparation 
            || GameStateManager.Instance.GetCurrentGameState() == GameState.InBattle
            //|| GameStateManager.Instance.GetCurrentGameState() == GameState.MainMenu
            ) 
            {
            Bind<RobotInGameStats, PlayerData>(GameManager.Instance.currentGameData.playerData);  
            Bind<LevelManagerController, LevelData>(GameManager.Instance.currentGameData.levelData); 
            } 
        }



        //Bind Single Object
        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new() {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null) {
                if (data == null) {
                    data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }

        //Bind Multiple Object/Entity
        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new() {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach(var entity in entities) {
                var data = datas.FirstOrDefault(d=>d.Id == entity.Id);
                if (data == null) {
                    data = new TData { Id = entity.Id};
                    datas.Add(data);
                }
                entity.Bind(data);
            }
            
        } 

        /*

        public void NewGame() {
            
            
            gameData = new GameData{
                Name = "New Game",
                currentLevelName = "Demo",
                currentLevelIndex = 0
            };

            GameManager.Instance.currentGameData = gameData;

            SceneManager.LoadScene(gameData.currentLevelName);
            
        }
        */

        public void SaveGame(GameData data) => dataService.Save(data);

        public GameData LoadGame(string saveName) {
            return dataService.Load(saveName);
        }

        /*

        public void LoadGame(string gameName) {
            gameData = dataService.Load(gameName);

            if (String.IsNullOrWhiteSpace(gameData.currentLevelName)) {
                gameData.currentLevelName = "Demo";
            }

            GameManager.Instance.currentGameData = gameData;

            SceneManager.LoadScene(gameData.currentLevelName);


        }
        */

        public void ReloadGame() => LoadGame(gameData.Name);

        public void DeleteGame(string gameName) => dataService.Delete(gameName);

        public void ClearCache() {
            UnityEngine.Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        




    }
}