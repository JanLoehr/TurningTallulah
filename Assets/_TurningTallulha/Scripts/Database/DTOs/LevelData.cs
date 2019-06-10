using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurningTallulah.Database
{
    public class LevelData
    {
        [SQLite4Unity3d.PrimaryKey]
        public int Id { get; set; }
        public int Score { get; set; }
        public int Stars { get; set; }
        public int Cluster { get; set; }
    } 
}
