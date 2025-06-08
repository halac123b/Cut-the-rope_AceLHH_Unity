using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private SeasonData _season1st;
    [SerializeField] private SeasonData _season2nd;
    [SerializeField] private SeasonData _season3rd;

    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private Transform _gridLayoutGroup;
    [SerializeField] private LevelSelection _levelSelection;
    [SerializeField] private TextMeshProUGUI _numberStar;
    
    [SerializeField] private Button _btnBack;
    [SerializeField] private SeasonSelection _seasonSelection;

    private void OnEnable()
    {
        _numberStar.text = UserProfile.Instance.GetAllStars().ToString();
        LoadBox(UserProfile.Instance.SeasonIndex);
    }

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, boxData =>
        {
            LoadLevel((BoxData)boxData);
        }, EventDispatcher.LoadLevelUI);
        
        // EventDispatcher.Instance.AddEvent(gameObject, seasonIndx =>
        // {
        //     LoadBox((int)seasonIndx);
        // }, EventDispatcher.LoadBoxUI);
        
        _btnBack.onClick.AddListener(() => OnBackButtonClicked());
    }

    public void LoadBox(int seasonIndex)
    {
        BoxData[] boxlList = null;
        switch (seasonIndex)
        {
            case 0:
                boxlList = _season1st.BoxList;
                break;
            case 1:
                boxlList = _season2nd.BoxList;
                break;
            case 2:
                boxlList = _season3rd.BoxList;
                break;
        }
        
        for (int i = 0; i < boxlList.Length; i++)
        {
            GameObject boxGameObject = Instantiate(_boxPrefab, _gridLayoutGroup);
            boxGameObject.GetComponent<BoxUIComponent>().MyBoxData = boxlList[i];
        }
    }

    private void LoadLevel(BoxData boxData)
    {
        _levelSelection.gameObject.SetActive(true);
        _levelSelection.LoadLevel(boxData);
        gameObject.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        if (_gridLayoutGroup.childCount != 0)
        {
            foreach (Transform child in _gridLayoutGroup)
            {
                Destroy(child.gameObject);
            }
        }
        gameObject.SetActive(false);
        _seasonSelection.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
}