using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace System.Persistence {
    public class FileDataService : IDataService {
        ISerializer serializer;

        string dataPath;
        string fileExtension;

        public FileDataService(ISerializer serializer) {
            this.serializer = serializer;
            this.dataPath = Application.persistentDataPath;
            this.fileExtension = "json";
        }

        string GetPathToFile(string name) {
            return Path.Combine(dataPath, string.Concat(name, ".", fileExtension));
        }

        public void Save(GameData data, bool overwrite=true) {
            string fileLocation = GetPathToFile(data.Name);
            if (!overwrite && !File.Exists(fileLocation)) {
                throw new IOException($"File '{fileLocation}.{fileExtension}' already exists and could not be overwritten.");
            }
            File.WriteAllText(fileLocation, serializer.Serialize(data));
        }

        public GameData Load(string name) {
            string fileLocation = GetPathToFile(name);
            if (!File.Exists(fileLocation)) {
                throw new ArgumentException($"No persisted GameData with name '{name}' found.");
            }
            return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name) {
            string fileLocation = GetPathToFile(name);
            if (File.Exists(fileLocation)) {
                File.Delete(fileLocation);
            }   
        }

        public void DeleteAll() {
            foreach (string file in Directory.GetFiles(dataPath, $"*{fileExtension}")) {
                File.Delete(file);
            }
        }

        public IEnumerable<string> ListSaves() {
            foreach (string path in Directory.EnumerateFiles(dataPath)) {
                yield return Path.GetFileNameWithoutExtension(path);
            }
        }


    }
}
