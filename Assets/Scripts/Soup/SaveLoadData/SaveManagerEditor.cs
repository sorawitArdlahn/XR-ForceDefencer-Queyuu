using GameController;
using UnityEditor;
using UnityEngine;

namespace System.Persistence.Editor {
    [CustomEditor(typeof(SaveLoadSystem))]
    public class SaveManagerEditor : UnityEditor.Editor {
         public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            SaveLoadSystem saveLoadSystem = (SaveLoadSystem) target;
            string gameName = SaveLoadSystem.Instance.gameData.Name;

            DrawDefaultInspector();

            if (GUILayout.Button("Save Game")) {
                saveLoadSystem.SaveGame(GameManager.Instance.currentGameData);
            }

            if (GUILayout.Button("Load Game")) {
                saveLoadSystem.LoadGame(gameName);
            }

            if (GUILayout.Button("Delete Game")) {
                saveLoadSystem.DeleteGame(gameName);
            }


            
        }
    }

}