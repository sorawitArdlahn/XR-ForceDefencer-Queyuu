using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Persistence {
    [Serializable] public class GameData {
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
        [SerializeField] public GameData gameData;

        IDataService dataService;

        protected override void Awake() {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }

        //TODO : Rework to fit real Environment, Currently Only Work On One Scene

        /*
        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Bind<Player, PlayerData>(gameData.playerData);
        }
        */



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

        public void NewGame() {
            
            
            gameData = new GameData{
                Name = "New Game",
                currentLevelName = "Demo",
                currentLevelIndex = 0
            };

            SceneManager.LoadScene(gameData.currentLevelName);
            
        }

        public void SaveGame() => dataService.Save(gameData);

        public void LoadGame(string gameName) {
            gameData = dataService.Load(gameName);

            if (String.IsNullOrWhiteSpace(gameData.currentLevelName)) {
                gameData.currentLevelName = "Demo";
            }

            SceneManager.LoadScene(gameData.currentLevelName);


        }

        public void ReloadGame() => LoadGame(gameData.Name);

        public void DeleteGame(string gameName) => dataService.Delete(gameName);




    }
}
