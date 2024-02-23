using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public event Action<GridTile> TileSelected;

    public int numRows = 6;
    public int numColumns = 6;

    public float padding = 0.1f;

    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private TextMeshProUGUI text;

    private GridTile[] _tiles; //simon says

    private void Awake()
    {
        InitGrid();
    }

    public void InitGrid()
    {
        _tiles = new GridTile[numRows * numColumns]; //simon says
        for (int x = 0; x < numRows; x++)
        {
            for (int y = 0; y < numColumns; y++)
            {
                GridTile tile = Instantiate(tilePrefab, transform);
                Vector2 tilePosition = new Vector2(x + (padding * x), y + (padding * y));
                tile.transform.localPosition = tilePosition;
                tile.name = $"Tile_{x}_{y}";
                tile.gridManager = this;
                tile.gridCoords = new Vector2Int(x, y);
                _tiles[y * numColumns + x] = tile; //simon says
            }
        }
    }

    public void OnTileHoverEnter(GridTile gridTile)
    {
        text.text = gridTile.gridCoords.ToString();
    } 

    public void OnTileHoverExit(GridTile gridTile)
    {
        text.text = "---";
    }

    public void OnTileSelected(GridTile gridTile)
    {
        TileSelected.Invoke(gridTile);
    }

    internal GridTile GetTile(Vector2Int pos) //simon says
    {
        if (pos.x < 0 || pos.x >= numColumns || pos.y < 0 || pos.y >= numRows)
        {
            Debug.LogError($"Invalid coordinate {pos}");
            return null;
        }
        return _tiles[pos.y * numColumns + pos.x];
    }
}
