using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    // Managers will be needed
    private CursorManager _cm;
    private GameManager _gm;
    private UnitManager _um;
    private BuildingManager _bm;

    [SerializeField] private Sprite _cursor;
    [SerializeField] private GameObject _options;
    private List<GameObject> _optionsList;
    private int _selectedOption;

    [SerializeField] private GameObject _waitOption;
    // public GameObject FireOption;
    // public GameObject CaptureOption;

    private GameObject _waitOptionInstance;
    private GameObject _waitOptionInstance2;
    // public GameObject FireOption;
    // public GameObject CaptureOption;

    private void Awake()
    {
        _cm = FindAnyObjectByType<CursorManager>();
        _um = FindAnyObjectByType<UnitManager>();
        _gm = FindAnyObjectByType<GameManager>();
        _bm = FindAnyObjectByType<BuildingManager>();
        _optionsList = new List<GameObject>();

        _waitOptionInstance = Instantiate(_waitOption, _options.transform);
        _waitOptionInstance.SetActive(false);
        _waitOptionInstance2 = Instantiate(_waitOption, _options.transform);
        _waitOptionInstance2.SetActive(false);
    }

    private void OnEnable()
    {
        if (_bm.BuildingFromPosition == null) { return; }
        CalculateOptions();
    }

    private void OnDisable()
    {
        if(_optionsList.Count == 0) { return; }
        _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        foreach (GameObject option in _optionsList) { option.SetActive(false); }
        _optionsList.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _gm.CurrentStateOfPlayer = EPlayerStates.Selecting;
            _um.SelectedUnit.transform.position = _cm.SaveTile;
            if (_um.Path.Count != 0)
            {
                _cm.HoveredOverTile = _um.Path.Last();
            }
            _um.SelectUnit(_um.SelectedUnit);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

            if (_optionsList[_selectedOption].name.Contains("Wait"))
            {
                _um.EndMove();
                _gm.CurrentStateOfPlayer = EPlayerStates.Idle;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            _selectedOption = (_selectedOption - 1 + _optionsList.Count) % _optionsList.Count;
            _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            _selectedOption = (_selectedOption + 1 ) % _optionsList.Count;
            _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
    }

    private void CalculateOptions()
    {

        // CheckFire();
        CheckCapture();
        _waitOptionInstance2.SetActive(true);
        _optionsList.Add(_waitOptionInstance2);

        if (_optionsList.Count > 0)
        {
            _selectedOption = 0;
            _optionsList[_selectedOption].transform.GetChild(0).GetComponent<Image>().color = Color.white;

        }
    }

    /* private void CheckFire()
      {
          if (Um.SelectedUnit.CanAttack())
          {
              OptionsList.Add(FireOption);
          }
      } */

    private void CheckCapture()
    {
        if (_bm.BuildingFromPosition == null) { return; }
        var building = _bm.BuildingFromPosition.ContainsKey(_cm.HoveredOverTile) ? _bm.BuildingFromPosition[_cm.HoveredOverTile] : null;
        if (building != null && building.Owner != _gm.PlayerTurn)
        {
            // OptionsList.Add(Instantiate(CaptureOption, Options.transform));
        }
        else
        {
            _waitOptionInstance.SetActive(true);
            _optionsList.Add(_waitOptionInstance);
        }
    }
}
