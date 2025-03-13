using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header("Scene Transition")]
        [SerializeField] GameObject TransitionScreenObject;
        public ScreenTransition TransitionScreen;

        void Awake()
        {
        //Initialize Singleton

        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        TransitionScreenObject = Instantiate(TransitionScreenObject, transform);
        TransitionScreen = TransitionScreenObject.GetComponent<ScreenTransition>();

        DontDestroyOnLoad(gameObject);
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public void SetNextPhase(GameState nextState)
        {
            currentGameState = nextState;
            StartCoroutine(SwitchGameState());
        }

        IEnumerator SwitchGameState()
        {
            StartCoroutine(TransitionScreen.TransitionScreenFadeOut());
            yield return new WaitForSeconds(TransitionScreen.getTransitionWait());
            

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    //TODO : Load MainMenu
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.MainMenu));

                    UnloadScene((int)SceneIndexes.Singleton);
                    UnloadScene((int)SceneIndexes.BattlePreparation);
                    break;
                case GameState.BattlePreparation:
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.BattlePreparation));

                    UnloadScene((int)SceneIndexes.MainMenu);
                    UnloadScene((int)SceneIndexes.InBattle);
                    break;
                case GameState.InBattle:
                    //TODO : Check if scene is loaded, if not Load InBattleScene
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.InBattle));
                    //TODO : MAKE A LOADING SCREEN TO WAIT FOR MAP AND SPAWNER TO FINISH INITIALIZING
                    //TODO : Unload PreparationScene
                    UnloadScene((int)SceneIndexes.BattlePreparation);
                    break;
                case GameState.ContinueBattle:
                    //TODO : Unload VictoryScene
                    Instance.SetNextPhase(GameState.InBattle);
                    break;
            }
        }

        private IEnumerator LoadSceneCoroutine(int sceneNumber) {
            if(!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded) {
            yield return SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
            }

            if (Instance.GetCurrentGameState() != GameState.InBattle)
            {
                yield return TransitionScreen.TransitionScreenFadeIn();
            }
        }

        private void UnloadScene(int sceneNumber) {

            if(SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                SceneManager.UnloadSceneAsync(sceneNumber);

        }
  
    }
}