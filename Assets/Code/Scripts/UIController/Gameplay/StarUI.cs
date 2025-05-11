using System;
using TMPro;
using UnityEngine;

public class StarUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _starText;

    private void Start()
    {
        _starText.text = "0";

        // EventDispatcher.Instance.AddEvent(gameObject, currentStarNumber => UpdateStarTextNumber((int)currentStarNumber),
        //     EventDispatcher.UpdateStarNumber);
    }

    public void UpdateStarTextNumber(int star)
    {
        _starText.text = star.ToString();
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}