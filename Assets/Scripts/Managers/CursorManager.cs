using UnityEngine;
using UnityEngine.Tilemaps;

// Class to manage the cursor
public class CursorManager : MonoBehaviour
{
    UnitManager Um;
    MapManager Mm;
    BuildingManager Bm;
    GameManager Gm;

    // The tile which the cursor is hovering over
    public Vector3Int HoveredOverTile
    {
        get => Mm.Map.WorldToCell(transform.position);
        set => transform.position = value;
    }

    // 
    public Vector3Int SaveTile;

    void Start()
    {
        // Get the unit, map, game and building managers from the hierarchy
        Um = FindAnyObjectByType<UnitManager>();
        Mm = FindAnyObjectByType<MapManager>();
        Gm = FindAnyObjectByType<GameManager>();
        Bm = FindAnyObjectByType<BuildingManager>();
    }

    void Update()
    {
        // Handle input every frame
        if(Gm.CurrentStateOfPlayer == EPlayerStates.Idle || Gm.CurrentStateOfPlayer == EPlayerStates.Selecting) {
            HandleInput();
        }
    }

    // Handles keyboard input
    void HandleInput()
    {
        // Dont handle any input if a unit is moving or attacking
        if (Um.SelectedUnit!= null && Um.SelectedUnit.IsMoving) { return; }

        // X key
        if (Input.GetKeyDown(KeyCode.X))
        {
            XClicked();
        }

        // Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceClicked();
        }

        // Arrow keys
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelector(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelector(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelector(Vector3Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelector(Vector3Int.down);
        }
    }

    // Move the cursor 
     void MoveSelector(Vector3Int offset)
    {
        // Dont let the cursor move out of the highlited tiles
        if (Um.SelectedUnit != null && !Um.SelectedUnit.ValidTiles.ContainsKey(HoveredOverTile + offset))
        {
            return;
        }

        // If a unit is selected, record the path
        if (Um.SelectedUnit != null)
        {
            // Undraw the path if we get back the start point
            if (Um.SelectedUnit.transform.position == HoveredOverTile + offset)
            {
                Um.UndrawPath();
                Um.Path.Clear();
                Um.PathCost = 0;
            }
            else
            {
                int index = Um.Path.IndexOf(HoveredOverTile + offset); // Returns -1 if not found
                if (index < 0)
                {
                    // Add tile to path
                    int cost = Mm.GetTileData(Mm.Map.GetTile<Tile>(HoveredOverTile + offset)).ProvisionsCost;
                    if (Um.PathCost + cost > Um.SelectedUnit.Provisions) { return; }
                    Um.UndrawPath();
                    Um.Path.Add(HoveredOverTile + offset);
                    Um.PathCost += cost;
                }
                else
                {
                    // Remove the arrow loop
                    Um.UndrawPath();
                    Um.Path.RemoveRange(index + 1, Um.Path.Count - index - 1);

                    // Recalculate the new provisions cost
                    Um.PathCost = 0;
                    foreach (Vector3Int pos in Um.Path)
                    {
                        Um.PathCost += Mm.GetTileData(Mm.Map.GetTile<Tile>(pos)).ProvisionsCost;
                    }
                }
            }
            Um.DrawPath();
        }
        HoveredOverTile += offset;
    }

    // Handle X Click
    private void XClicked()
    {
        if(Gm.CurrentStateOfPlayer == EPlayerStates.Selecting) 
        {
            // Cancel selection
            HoveredOverTile = Mm.Map.WorldToCell(Um.SelectedUnit.transform.position);
            Um.DeselectUnit();       
        }
    }

    // Handle Space click
    private void SpaceClicked()
    {
        Unit refUnit = Um.FindUnit(HoveredOverTile);

        // If there is a unit on the hovered tile
        if (refUnit != null)
        {
            // Can't select an another unit when one is selected 
            if (Um.SelectedUnit != null)
            {
                if (Um.SelectedUnit == refUnit) {  StartCoroutine(Um.MoveUnit());  }
                return;
            }

            // Can't select an enemy unit
            if (refUnit.Owner != Gm.PlayerTurn) { return; }

            // Can't select a unit that has already moved
            if (refUnit.HasMoved) { return; }
            SaveTile = HoveredOverTile;
            Um.SelectUnit(refUnit);
        }
        else
        {
            if (Um.SelectedUnit != null)
            {
                // Move towards the selected tile
                StartCoroutine(Um.MoveUnit());
            }
            else
            {
                if (Bm.Buildings.ContainsKey(HoveredOverTile))
                {
                    Bm.SpawnUnit(EUnits.Infantry, Bm.Buildings[HoveredOverTile], Gm.PlayerTurn);
                }
            }
        }
    }
}
