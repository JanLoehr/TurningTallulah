using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TurningTallulah.Database
{
    public class DbService : MonoBehaviour
    {
        private string _dbPath;

        private List<LevelData> _levelData;

        void Start()
        {
            _dbPath = $"{Application.persistentDataPath}/Data/GameData.db";

            if (File.Exists(_dbPath))
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(_dbPath))
                    {
                        _levelData = connection.Table<LevelData>().ToList();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);

                    StartCoroutine(SetupDatabaseAsync());
                }
            }
            else
            {
                StartCoroutine(SetupDatabaseAsync());
            }
        }

        public LevelData GetLevelData(int id)
        {
            return _levelData.FirstOrDefault(l => l.Id == id);
        }

        public void AddLevelData(LevelData newScore)
        {
            if (_levelData.FirstOrDefault(l => l.Id == newScore.Id) == null)
            {
                _levelData.Add(newScore);

                using (SQLiteConnection connection = new SQLiteConnection(_dbPath))
                {
                    connection.Insert(newScore);
                }
            }
            else
            {
                LevelData oldScore = _levelData.First(l => l.Id == newScore.Id);

                if (oldScore.Score < newScore.Score
                    || (oldScore.Score == newScore.Score && oldScore.Stars < newScore.Stars))
                {
                    oldScore.Score = newScore.Score;
                    oldScore.Stars = newScore.Stars;

                    using (SQLiteConnection connection = new SQLiteConnection(_dbPath))
                    {
                        connection.Update(oldScore);
                    }
                }
            }
        }

        private IEnumerator SetupDatabaseAsync()
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/Data");

            //File.Copy($"{Application.streamingAssetsPath}/GameData.db", _dbPath); Not on Android!

            WWW dbFile = new WWW($"{Application.streamingAssetsPath}/GameData.db");

            while (!dbFile.isDone)
            {
                yield return null;
            }

            File.WriteAllBytes(_dbPath, dbFile.bytes);

            _levelData = new List<LevelData>();
        }

        public int GetHighestLevelIndex()
        {
            if (_levelData?.Count > 0)
            {
                return _levelData.OrderBy(l => l.Id).Last().Id;
            }

            return 0;
        }
    }
}
