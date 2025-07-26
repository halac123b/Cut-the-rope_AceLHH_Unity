using System;
using UnityEngine;
using UnityEngine.UI;

public class StarCollectActive : MonoBehaviour
{
    [SerializeField] private GameObject _starActive;

    public void SetActiveStar(bool active)
    {
        _starActive.SetActive(active);
    }
}
