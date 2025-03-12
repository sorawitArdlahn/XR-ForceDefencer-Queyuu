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
        [SerializeField] GameObject TransitionScreen;
        public float transitionWait = 0.5f;
        private Image TransitionScreenImage;

        void Awake()
        {
        //Initialize Singleton

        if (Instance == null) { Instance = this; }     
        else { Destroy(gameObject); }

        TransitionScreen = Instantiate(TransitionScreen, transform);
        TransitionScreenImage = TransitionScreen.GetComponentInChildren<Image>();
        TransitionScreen.SetActive(false);

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
            StartCoroutine(TransitionScreenFadeOut());
            yield return new WaitForSeconds(transitionWait);
            

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
            yield return TransitionScreenFadeIn();
        }

        private void UnloadScene(int sceneNumber) {

            if(SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                SceneManager.UnloadSceneAsync(sceneNumber);

        }


        //Transition Screen
        public IEnumerator TransitionScreenFadeIn() {
            Color startColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            1);

            Color endColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            0);

            yield return StartCoroutine(TransitionScreenFade(startColor, endColor, transitionWait));

            TransitionScreen.SetActive(false);
        }

        public IEnumerator TransitionScreenFadeOut() {
            TransitionScreen.SetActive(true);

            Color startColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            0);

            Color endColor = new Color(TransitionScreenImage.color.r,
            TransitionScreenImage.color.g,
            TransitionScreenImage.color.b,
            1);

            yield return StartCoroutine(TransitionScreenFade(startColor, endColor, transitionWait));
        }

        private IEnumerator TransitionScreenFade(Color start, Color end, float duration) {
            float elapsedTime = 0.0f;
            float elapsedPercentage = 0.0f;

            while (elapsedPercentage < 1.0f) {

                elapsedPercentage = elapsedTime / duration;
                TransitionScreenImage.color = Color.Lerp(start, end, elapsedPercentage);
                yield return null;

                elapsedTime += Time.deltaTime;
            }

        }
  
    }
}