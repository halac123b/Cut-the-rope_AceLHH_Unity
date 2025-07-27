using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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
    [SerializeField] private Image _frogMask;

    [SerializeField] private RectTransform viewport;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    private float[] _pos;
    private BoxData[] _boxlList;
    private float _scrollPos;
    private BoxUIComponent _boxUI;
    private List<BoxUIComponent> _boxUIList = new();
    private bool _isTouchEnd;
    private Vector2 _lastMousePos;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable(); //Support touch khi enable - New input system not support 

        _numberStar.text = UserProfile.Instance.GetAllStars().ToString();

        _frogMask.transform.SetParent(transform);
        _frogMask.transform.SetSiblingIndex(1);

        if (_gridLayoutGroup.childCount != 0)
        {
            foreach (Transform child in _gridLayoutGroup)
            {
                Destroy(child.gameObject);
            }
        }

        ClearBox();
        LoadBox();
    }

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, boxData => { LoadLevel((BoxData)boxData); },
            EventDispatcher.LoadLevelUI);

        _btnBack.onClick.AddListener(() => OnBackButtonClicked());
        UpdatePadding();
    }

    private void Update()
    {
        if (_boxlList != null)
        {
            if (_boxlList.Length <= 0) return;
            _pos = new float[_gridLayoutGroup.childCount];
        }

        float distance;
        if (_pos.Length == 1)
        {
            _pos[0] = 0.5f;
            distance = 1f;
        }
        else
        {
            distance = 1f / (_pos.Length - 1f);
            for (int i = 0; i < _pos.Length; i++)
            {
                _pos[i] = distance * i;
            }
        }

        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = distance * i; //Tính vị trí của các box dựa trên khoảng cách 
        }

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 currentMousePos = Pointer.current.position.ReadValue();
            
            if ((currentMousePos - _lastMousePos).sqrMagnitude > 1f)
            {
                _isTouchEnd = true;
                _scrollPos = _scrollbar.value; //Lấy gía trị scrollbar mỗi khi move
                for (int i = 0; i < _pos.Length; i++)
                {
                    _boxUIList[i].IsPlayAnimation = false;
                }
            }
            
            _lastMousePos = currentMousePos;
        }
        else
        {
            _isTouchEnd = false;
            //So giá trị scrollbar hiện tại với khoảng cách từ box và 1/2 khoảng trống với box kề cạnh
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
                {
                    _scrollbar.value =
                        Mathf.Lerp(_scrollbar.value, _pos[i], 0.1f); //Lerp scrollbar về vị trí box gần nhất 
                }
            }
        }
        for (int i = 0; i < _pos.Length; i++)
        {
            bool isFocusedBox;

            if (_pos.Length == 1)
            {
                // Luôn true nếu chỉ có 1 box
                isFocusedBox = true;
            }
            else
            {
                isFocusedBox = _scrollPos < _pos[i] + (distance / 2f) && _scrollPos > _pos[i] - (distance / 2f);
            }
            
            if (isFocusedBox)
            {
                _frogMask.transform.SetParent(_boxUIList[i].FrogMask.transform, false);
                _frogMask.transform.SetAsFirstSibling();
                _frogMask.transform.localScale = Vector3.one;
                if (!_isTouchEnd)
                {
                    _boxUIList[i].PlayBoxAnimation();
                }
            }
        }
    }

    private void UpdatePadding()
    {
        if (viewport == null || horizontalLayoutGroup == null) return;

        float viewportWidth = viewport.rect.width;
        int halfViewport = Mathf.RoundToInt(viewportWidth / 2f - 300f);

        horizontalLayoutGroup.padding.left = halfViewport;
        horizontalLayoutGroup.padding.right = halfViewport;

        LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.GetComponent<RectTransform>());
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

            _boxUI = boxGameObject.GetComponent<BoxUIComponent>();
            _boxUIList.Add(_boxUI);
            _boxUI.SetUIBoxComponent();
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
        _frogMask.transform.SetParent(transform);
        _frogMask.transform.SetSiblingIndex(1);

        if (_gridLayoutGroup.childCount != 0)
        {
            foreach (Transform child in _gridLayoutGroup)
            {
                Destroy(child.gameObject);
            }
        }

        ClearBox();

        gameObject.SetActive(false);
        _seasonSelection.gameObject.SetActive(true);
    }

    private void ClearBox()
    {
        if (_boxUIList.Count > 0)
        {
            _boxUIList.Clear();
        }
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