﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player levels data", menuName = "Player levels data")]
public class PlayerLevelsData : ScriptableObject
{
    public List<LevelStar> levelsStars = new List<LevelStar>();
}
