using UnityEngine;

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
       
        switch (Gm.LastStateOfPlayer)
        {
            case EPlayerStates.InActionsMenu: { ActionMenu.SetActive(false); break; }
            default: { break; }
        }
        switch (Gm.CurrentStateOfPlayer)
        {
            case EPlayerStates.InActionsMenu: {  ActionMenu.SetActive(true); break; }
            default: { break; }
        }
        
    }
}
