using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Tetris
{
    sealed class StorageObject
    {
        public List<int> scores { get; private set; }
        public List<int> classicScores { get; private set; }
        public List<Keys> binds { get; set; }
        public List<bool> settings { get; set; }
        
        private StorageObject() 
        {
            scores = new List<int>();
            classicScores = new List<int>();
            binds = new List<Keys>();
            settings = new List<bool>();
        }
        static StorageObject() { }
        public static StorageObject Instance { get; } = new StorageObject();

        public void Read()
        {
            var dataJSON = File.ReadAllText("data.json");

            var readObject = JsonSerializer.Deserialize<StorageObject>(dataJSON);
            scores = readObject.scores;
            classicScores = readObject.classicScores;
            binds = readObject.binds;
            settings = readObject.settings;
        }

        public void Write()
        {
            var stuffToWrite = JsonSerializer.Serialize(this);
            File.WriteAllText("data.json", stuffToWrite);
        }
    }
}
