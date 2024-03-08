using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    //this game object will handle the map 
    
    
    public Tilemap map;
    public Tilemap highlight;
    [SerializeField]
    private Tile[] ArrowTiles;
    [SerializeField]
    private List<TileData> TileDatas;
   

    enum EArrowDirection
    {
        None=0,
        Up=1,
        Down=2,
        Left=3,
        Right=4,
        Horizontal=5,
        Vertical=6,
        TopRight=7,
        TopLeft=8,
        BottomLeft=9,
        BottomRight=10,

    }
   
    private Dictionary<Tile, TileData> _dataFromTile;
    
    private void Awake()
    {
        _dataFromTile = new Dictionary<Tile, TileData>();
        
        foreach (var TileData in TileDatas)
        {
            foreach(var tile in TileData.tiles)
            {
                _dataFromTile.Add(tile, TileData);
            }
        }
        
        
    }
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TileData GetTileData(Tile tile)
    {
        return _dataFromTile[tile];
    }
    public TileData GetTileData(Vector3Int tile)
    {
        return _dataFromTile[map.GetTile<Tile>(tile)];
    }

    //calculates the sprite for a tile based on the next tile and previous tile
    public void DrawArrow(Vector3Int prev, Vector3Int curr, Vector3Int next)
    {
        EArrowDirection Arrow=EArrowDirection.None;

        Vector2 DistancePrev = new Vector2(curr.x - prev.x, curr.y - prev.y);
        Vector2 DistanceNext = new Vector2(next.x- curr.x,next.y- curr.y);
        Vector2 Distance= DistancePrev != DistanceNext ? DistanceNext + DistancePrev: DistanceNext ;

        if(Distance == new Vector2(0, 0))
        {
            Arrow= EArrowDirection.None ;
        }
        else if (Distance == new Vector2(1, 0) && next==curr )
        {
            Arrow = EArrowDirection.Right;
        }
        else if (Distance == new Vector2(-1, 0) && next == curr)
        {
            Arrow = EArrowDirection.Left;
        }
        else if (Distance == new Vector2(0, 1) && next == curr)
        {
            Arrow = EArrowDirection.Up;
        }
        else if (Distance == new Vector2(0, -1) && next == curr)
        {
            Arrow = EArrowDirection.Down;
        }
        else if (Distance == new Vector2(1, 0) || Distance == new Vector2(-1, 0))
        {
            Arrow = EArrowDirection.Horizontal;
        }
        else if (Distance == new Vector2(0, 1) || Distance == new Vector2(0, -1))
        {
            Arrow = EArrowDirection.Vertical;
        }
        else if (Distance == new Vector2(1, 1) )
        {
            if (curr.y != prev.y)
            {
                Arrow = EArrowDirection.TopRight;
            }
            else
            {
                Arrow = EArrowDirection.BottomLeft;
            }
           
        }
        else if (Distance == new Vector2(-1, 1))
        {
            
            if (curr.y != prev.y)
            {
                Arrow = EArrowDirection.TopLeft;
            }
            else
            {
                Arrow = EArrowDirection.BottomRight;
            }

        }
        else if (Distance == new Vector2(1, -1))
        {
           
            if (curr.y != prev.y)
            {
                Arrow = EArrowDirection.BottomRight;
            }
            else
            {
                Arrow = EArrowDirection.TopLeft;
            }

        }
        else if (Distance == new Vector2(-1, -1))
        {
            
            if (curr.y != prev.y)
            {
                Arrow = EArrowDirection.BottomLeft;
            }
            else
            {
                Arrow = EArrowDirection.TopRight;
            }

        }
      
        
        highlight.SetTile(curr, ArrowTiles[(int)Arrow]);
        
        
    }


}
