using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

namespace GameController
{
    public enum GameState
    {
        Singleton,
        MainMenu,
        BattlePreparation,
        InBattle,
        ContinueBattle
    }

    public enum SceneIndexes
    {
        Singleton=0,
        MainMenu=1,
        BattlePreparation=2,
        InBattle=3,
        Victory=4,
        GameCleared=5,
        GameOver=6
    }

    public class GameStateManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static GameStateManager Instance = null;
        GameState currentGameState = GameState.Singleton;
        void Awake()
        {
        //Initialize Singleton

        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public void SetNextPhase(GameState nextState)
        {
            currentGameState = nextState;
            SwitchGameState();
        }

        void SwitchGameState()
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    //TODO : Load MainMenu
                    LoadScene((int)SceneIndexes.MainMenu);
                    //TODO : Unload SingletonScene
                    //TODO : Unload Everything other than MainMenu
                    UnloadScene((int)SceneIndexes.Singleton);
                    UnloadScene((int)SceneIndexes.BattlePreparation);
                    break;
                case GameState.BattlePreparation:
                    LoadScene((int)SceneIndexes.BattlePreparation);
                    UnloadScene((int)SceneIndexes.MainMenu);
                    //TODO : Load PreparationScene
                    //TODO : Unload MainMenuScene
                    break;
                case GameState.InBattle:
                    //TODO : Check if scene is loaded, if not Load InBattleScene
                    LoadScene((int)SceneIndexes.InBattle);
                    //TODO : MAKE A LOADING SCREEN TO WAIT FOR MAP AND SPAWNER TO FINISH INITIALIZING
                    //TODO : Unload PreparationScene
                    //UnloadScene((int)SceneIndexes.BattlePreparation);
                    break;
                case GameState.ContinueBattle:
                    //TODO : Unload VictoryScene
                    ContinueBattleBehavior();
                    break;
            }
        }


        void ContinueBattleBehavior()
        {
            //TODO : MAKE A BLACK SCREEN TRANSITION BEFORE RESETING THE BATTLE
            SetNextPhase(GameState.InBattle);

        }

        /*
        private IEnumerator LoadScene(int sceneNumber) {

        if(!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
            {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;
            }
        //StartCoroutine(CompleteSceneLoad());

        }
        */

        private void LoadScene(int sceneNumber) {

            if(!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                //SceneManager.LoadScene(sceneNumber, LoadSceneMode.Additive);
                SceneManager.LoadScene(sceneNumber);

        }


        private void UnloadScene(int sceneNumber) {

            if(SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                SceneManager.UnloadSceneAsync(sceneNumber);

        }


        private IEnumerator CompleteSceneLoad() {
            yield return new WaitForSeconds(1);
        }
  
    }
}