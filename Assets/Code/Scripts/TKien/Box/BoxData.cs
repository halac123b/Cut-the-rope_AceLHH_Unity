using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "Scriptable Objects/BoxData")]
public class BoxData : ScriptableObject
{
    public int Index;
    public string BoxName;
    public int NumberOfLevels;
}
