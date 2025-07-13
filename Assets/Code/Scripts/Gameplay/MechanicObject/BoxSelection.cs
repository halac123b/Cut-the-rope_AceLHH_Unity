using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEditor.DeviceSimulation.TouchPhase;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private SeasonData _season1st;
    [SerializeField] private SeasonData _season2nd;
    [SerializeField] private SeasonData _season3rd;

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private Transform _gridLayoutGroup;
    [SerializeField] private LevelSelection _levelSelection;
    [SerializeField] private TextMeshProUGUI _numberStar;

    [SerializeField] private Button _btnBack;
    [SerializeField] private SeasonSelection _seasonSelection;

    private float[] _pos;
    private BoxData[] _boxlList;
    private float _scrollPos;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable(); //Support touch khi enable - New input system not support 

        _numberStar.text = UserProfile.Instance.GetAllStars().ToString();
        
        if (_gridLayoutGroup.childCount != 0)
        {
            foreach (Transform child in _gridLayoutGroup)
            {
                Destroy(child.gameObject);
            }
        }

        LoadBox(); 
    }

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, boxData => { LoadLevel((BoxData)boxData); },
            EventDispatcher.LoadLevelUI);

        _btnBack.onClick.AddListener(() => OnBackButtonClicked());
    }

    private void Update()
    {
        if (_boxlList.Length <= 0) return;
        if (_boxlList != null)
        {
            _pos = new float[_gridLayoutGroup.childCount];
        }

        float distance = 1f / (_pos.Length - 1f); //Khoảng cách giữa các box

        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = distance * i; //Tính vị trí của các box dựa trên khoảng cách 
        }

        if (Touch.activeTouches.Count > 0)
        {
            var firstTouch = Touch.activeTouches[0];
            
            if (firstTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                _scrollPos = _scrollbar.value; //Lấy gía trị scrollbar mỗi khi move
            }
        }
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))  //So giá trị scrollbar hiện tại với khoảng cách từ box và 1/2 khoảng trống với box kề cạnh
                {
                    _scrollbar.value = Mathf.Lerp(_scrollbar.value, _pos[i], 0.1f); //Lerp scrollbar về vị trí box gần nhất 
                }
            }
        }
        
        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
            {
                _gridLayoutGroup.GetChild(i).localScale =
                    Vector2.Lerp(_gridLayoutGroup.GetChild(i).localScale, new Vector2(1f, 1f), Time.deltaTime * 5f); //Đưa scale box gần nhất về 1
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i) //Nếu không phải là box gần nhất 
                    {
                        _gridLayoutGroup.GetChild(j).localScale = Vector2.Lerp(_gridLayoutGroup.GetChild(j).localScale,
                            new Vector2(0.8f, 0.8f), 0.1f); //Scale các box về 0.8
                    }
                }
            }
        }
    }

    private void LoadBox()
    {
        switch (UserProfile.Instance.SeasonIndex)
        {
            case 0:
                _boxlList = _season1st.BoxList;
                break;
            case 1:
                _boxlList = _season2nd.BoxList;
                break;
            case 2:
                _boxlList = _season3rd.BoxList;
                break;
        }

        for (int i = 0; i < _boxlList.Length; i++)
        {
            GameObject boxGameObject = Instantiate(_boxPrefab, _gridLayoutGroup);
            boxGameObject.GetComponent<BoxUIComponent>().MyBoxData = _boxlList[i];
            
            BoxUIComponent boxUIComponent = boxGameObject.GetComponent<BoxUIComponent>();
            boxUIComponent.SetUIBoxComponent();
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

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
}