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

    // Start is called before the first frame update
<<<<<<< Updated upstream
=======
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
        Gm = FindAnyObjectByType<GameManager>();

    }
>>>>>>> Stashed changes
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
<<<<<<< Updated upstream
=======

        //switch (Gm.LastState)
        //{
        //    case EGameStates.ActionMenu: { ActionMenu.SetActive(false); break; }
        //    default: { break; }
        //}

        if (Gm.LastState == EGameStates.ActionMenu) ActionMenu.SetActive(false);

        if (Gm.GameState == EGameStates.ActionMenu) ActionMenu.SetActive(true);

        //switch (Gm.GameState)
        //{
        //    case EGameStates.ActionMenu: {  ActionMenu.SetActive(true); break; }
        //    default: { break; }
        //}

>>>>>>> Stashed changes
    }
}