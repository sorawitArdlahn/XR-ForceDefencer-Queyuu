using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Audio;

namespace GameController
{
    public enum GameState
    {
        Singleton,
        MainMenu,
        BattlePreparation,
        InBattle,
        Tutorial,
        GameCleared,
        GameOver
    }

    public enum SceneIndexes
    {
        Singleton=0,
        MainMenu=1,
        BattlePreparation=2,
        InBattle=3,
        Tutorial=4,
        GameCleared=5,
        GameOver=6
    }

    public class GameStateManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static GameStateManager Instance = null;
        GameState currentGameState = GameState.Singleton;

        [Header("Scene Transition GameObject")]
        [SerializeField] GameObject TransitionScreenObject;
        [NonSerialized] public ScreenTransition TransitionScreen;

        void Awake()
        {
        //Initialize Singleton

        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        TransitionScreenObject = Instantiate(TransitionScreenObject, transform);
        TransitionScreen = TransitionScreenObject.GetComponent<ScreenTransition>();

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
                    AudioManagerController.Instance.StopMusic();         
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.MainMenu));

                    break;
                case GameState.BattlePreparation:
                    AudioManagerController.Instance.StopMusic();
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.BattlePreparation));
                    AudioManagerController.Instance.PlayMusic("PreparationMusic");

                    break;
                case GameState.InBattle:
                    AudioManagerController.Instance.StopMusic();
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.InBattle));
                    break;
                case GameState.Tutorial:
                    AudioManagerController.Instance.StopMusic();
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.Tutorial));
                    AudioManagerController.Instance.PlayMusic("PreparationMusic");
                    break;
                case GameState.GameOver:
                    //StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.GameOver));
                    break;
                case GameState.GameCleared:
                    //StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.GameCleared));
                    break;
            }
        }

        private IEnumerator LoadSceneCoroutine(int sceneNumber) {
            StartCoroutine(TransitionScreen.TransitionScreenFadeOut());
            yield return new WaitForSeconds(TransitionScreen.getTransitionWait());
            
            if (!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded) {
                yield return SceneManager.LoadSceneAsync(sceneNumber);
            }

            if (Instance.GetCurrentGameState() != GameState.InBattle && Instance.GetCurrentGameState() != GameState.GameCleared)
            {
                AudioManagerController.Instance.PlaySFX("TransitionScreenIn");
                yield return TransitionScreen.TransitionScreenFadeIn();
            }
        }
  
    }
}