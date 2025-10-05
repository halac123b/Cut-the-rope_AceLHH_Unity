using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "Scriptable Objects/BoxData")]
public class BoxData : ScriptableObject
{
    public int Index;
    public string BoxName;
    public int NumberOfLevels;
    public Sprite BoxBGSprite;
    public float AdjustPosition;
    public int RequireStar;
    
    //UI List Box
    public Sprite BoxSprite;
    public Sprite BGGameplay;
    public Sprite TiledBG;
    public Sprite CharFrogSprites;
}
