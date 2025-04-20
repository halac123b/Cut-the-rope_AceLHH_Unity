using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteLevelUI : MonoBehaviour
{
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _backBtn;

    private void Start()
    {
        _backBtn.onClick.AddListener(OnClickBackHome);
    }

    private void OnClickBackHome()
    {
        SceneManager.LoadScene("Home");
    }
}
