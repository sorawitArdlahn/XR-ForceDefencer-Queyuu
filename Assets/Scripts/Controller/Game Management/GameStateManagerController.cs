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

        [NonSerialized] public TransitionScreenVR TransitionScreen;

        void Awake()
        {
        //Initialize Singleton

        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

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
                     
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.MainMenu));
                    AudioManagerController.Instance.PlayMusic("PreparationMusic");

                    break;
                case GameState.BattlePreparation:

                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.BattlePreparation));
                    AudioManagerController.Instance.PlayMusic("PreparationMusic");

                    break;
                case GameState.InBattle:
                    AudioManagerController.Instance.StopMusic();
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.InBattle));
                    break;
                case GameState.Tutorial:
                    StartCoroutine(LoadSceneCoroutine((int)SceneIndexes.Tutorial));
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
            if (TransitionScreen != null) {
                StartCoroutine(TransitionScreen.TransitionScreenFadeOut());
                yield return new WaitForSeconds(TransitionScreen.getTransitionWait());
            }
            
            if (!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded) {
                //yield return SceneManager.LoadSceneAsync(sceneNumber);
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
                asyncLoad.allowSceneActivation = false; // Prevent the scene from activating immediately
                while (!asyncLoad.isDone) {
                    if (asyncLoad.progress >= 0.9f) {
                        asyncLoad.allowSceneActivation = true; // Activate the scene when loading is complete
                    }
                    yield return null; // Wait for the next frame
                }
            }

            TransitionScreen = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<TransitionScreenVR>();

            if (Instance.GetCurrentGameState() != GameState.InBattle && Instance.GetCurrentGameState() != GameState.GameCleared)
            {
                yield return TransitionScreen.TransitionScreenFadeIn();
                AudioManagerController.Instance.PlaySFX("TransitionScreenIn");
            }
        }
  
    }
}