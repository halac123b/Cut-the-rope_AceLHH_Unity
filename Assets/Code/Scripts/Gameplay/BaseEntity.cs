using System;
using UnityEngine;

[Serializable]
public class BaseEntity
{
    public int Id;
    public LevelSceneLoader.ObjectCategory Category;
    public Vector3 Position;
    public string ExpandProperty;
}