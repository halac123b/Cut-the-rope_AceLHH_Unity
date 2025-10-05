using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public ScrollLevelData ScrollLevelData;
    public BaseEntity[] ListEntities;
}

[Serializable]
public class ScrollLevelData 
{
    public bool IsScrollLevel;
    public int ScrollAmount;
}