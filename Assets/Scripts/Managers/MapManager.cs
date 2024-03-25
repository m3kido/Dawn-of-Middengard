using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap Map;
    public Tilemap HighlightMap;
    public Tilemap Bordermap;
    public Tilemap ArrowMap;

    [SerializeField] private List<TileData> _tileDatas;
    [SerializeField] private AnimatedTile _highlightedTile;
    [SerializeField] private RuleTile _borderedTile;
    [SerializeField] private Tile[] _arrowTiles;

    private Dictionary<Tile, TileData> _dataFromTile = new();
    
    // Get tile datas of every tile type from the inspector
    private void Awake()
    {
        
        foreach (var tileData in _tileDatas)
        {
            foreach(var tile in tileData.Tiles)
            {
                _dataFromTile.Add(tile, tileData);
            }
        }     
    }

    // Get data of given tile
    public TileData GetTileData(Tile tile)
    {
        return _dataFromTile[tile];
    }

    // Get data of given grid position - Overloading GetTileData()
    public TileData GetTileData(Vector3Int Pos)
    {
        var tile = Map.GetTile<Tile>(Pos);
        if (tile == null) { return null; }
        return _dataFromTile[tile];

    }

    // Highlight the given grid position
    public void HighlightTile(Vector3Int pos)
    {
        HighlightMap.SetTile(pos, _highlightedTile);
        Bordermap.SetTile(pos, _borderedTile);
        Bordermap.SetColor(pos, Color.yellow);
    }

    // Unhighlight the given grid position
    public void UnHighlightTile(Vector3Int pos)
    {
        HighlightMap.SetTile(pos, null);
        Bordermap.SetTile(pos, null);
      
    }

    // Select the adequate arrow sprite based on the next tile and the previous one
    public void DrawArrow(Vector3Int prev, Vector3Int curr, Vector3Int next)
    {
        EArrowDirections arrow = EArrowDirections.None;

        Vector2 distancePrev = new(curr.x - prev.x, curr.y - prev.y);
        Vector2 distanceNext = new(next.x - curr.x, next.y - curr.y);
        Vector2 distance = distancePrev != distanceNext ? distanceNext + distancePrev : distanceNext;

        if(distance == new Vector2(0, 0))
        {
            arrow = EArrowDirections.None ;
        }
        else if (distance == new Vector2(1, 0) && next == curr )
        {
            arrow = EArrowDirections.Right;
        }
        else if (distance == new Vector2(-1, 0) && next == curr)
        {
            arrow = EArrowDirections.Left;
        }
        else if (distance == new Vector2(0, 1) && next == curr)
        {
            arrow = EArrowDirections.Up;
        }
        else if (distance == new Vector2(0, -1) && next == curr)
        {
            arrow = EArrowDirections.Down;
        }
        else if (distance == new Vector2(1, 0) || distance == new Vector2(-1, 0))
        {
            arrow = EArrowDirections.Horizontal;
        }
        else if (distance == new Vector2(0, 1) || distance == new Vector2(0, -1))
        {
            arrow = EArrowDirections.Vertical;
        }
        else if (distance == new Vector2(1, 1))
        {
            if (curr.y != prev.y)
            {
                arrow = EArrowDirections.TopRight;
            }
            else
            {
                arrow = EArrowDirections.BottomLeft;
            }
        }
        else if (distance == new Vector2(-1, 1))
        {
            
            if (curr.y != prev.y)
            {
                arrow = EArrowDirections.TopLeft;
            }
            else
            {
                arrow = EArrowDirections.BottomRight;
            }

        }
        else if (distance == new Vector2(1, -1))
        {
           
            if (curr.y != prev.y)
            {
                arrow = EArrowDirections.BottomRight;
            }
            else
            {
                arrow = EArrowDirections.TopLeft;
            }

        }
        else if (distance == new Vector2(-1, -1))
        {
            
            if (curr.y != prev.y)
            {
                arrow = EArrowDirections.BottomLeft;
            }
            else
            {
                arrow = EArrowDirections.TopRight;
            }
        }
        ArrowMap.SetTile(curr, _arrowTiles[(int)arrow]);
    }
}
