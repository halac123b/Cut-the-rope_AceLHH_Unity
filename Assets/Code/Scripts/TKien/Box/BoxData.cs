using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "Scriptable Objects/BoxData")]
public class BoxData : ScriptableObject
{
    public string BoxName;
    public BoxBaseEntity[] ListBoxEntities;
}
