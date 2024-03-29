using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //this class should handle:
    //the menu (appears after moving or selecting a tile) :
    //lists options 

    //info (appears when hovering on a tile):
    //display tile info and unit info if a unit is on the tile

    //captain bar

    //building menu
    GameManager Gm;
    [SerializeField]
    private GameObject ActionMenu;
    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.OnStateChange += ChangeActiveUI;
    }
    private void OnDisable()
    {
        GameManager.OnStateChange -= ChangeActiveUI;
    }
    void Awake()
    {
        Gm=FindAnyObjectByType<GameManager>();
        
    }
    private void Start()
    {
        ActionMenu.SetActive(false);
    }
    private void ChangeActiveUI()
    {
       
        switch (Gm.LastState)
        {
            case EGameStates.ActionMenu: { ActionMenu.SetActive(false); break; }
            default: { break; }
        }
        switch (Gm.GameState)
        {
            case EGameStates.ActionMenu: {  ActionMenu.SetActive(true); break; }
            default: { break; }
        }
        
    }
}
