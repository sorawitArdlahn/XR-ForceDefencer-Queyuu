using System.Collections.Generic;
using System.Linq;
using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Persistence {
    [System.Serializable]
    public class GameData {
        
        public string Name;
        //TODO : Player Record Data
        public string currentLevelName;

        public int currentLevelIndex;
        public PlayerData playerData;
        //TODO : Inventory and Upgrade Data


    }


    public interface ISaveable {
        SerializableGuid Id { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable {
        SerializableGuid Id { get; set; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem> {
        public GameData gameData;

        IDataService dataService;

        protected override void Awake() {
            base.Awake();
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
            //IF GAMEMANAGER DOES NOT HAVE GAMEDATA DO NOT PERFORM BINDING
            if (!GameManager.Instance || GameManager.Instance.currentGameData == null) return;

            
            if (scene.name == "PreparationScene") Bind<Player, PlayerData>(GameManager.Instance.currentGameData.playerData);   
            //if (scene.name == "BattleScene") Bind<LevelManager, LevelData>(gameData.levelData);   
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