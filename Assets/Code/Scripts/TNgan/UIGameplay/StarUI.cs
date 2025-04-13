using System;
using TMPro;
using UnityEngine;

public class StarUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _starText;

    private void Start()
    {
        _starText.text = "0";
    }

    public void UpdateStarTextNumber(int star)
    {
        _starText.text = star.ToString();
    }
}
