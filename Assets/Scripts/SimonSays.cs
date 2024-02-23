using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: We probably won't need this for the finalized version
public class SimonSays : MonoBehaviour
{
    [SerializeField] private bool playing = false;

    [SerializeField] private GridManager _gridManager;
    private List<Vector2Int> _correctPositions = new List<Vector2Int>();
    private int _playerPatternIndex;

    private void OnEnable()
    {
        _gridManager.TileSelected += OnTileSelected;
    }

    private void OnDisable()
    {
        _gridManager.TileSelected -= OnTileSelected;
    }

    private void OnTileSelected(GridTile obj)
    {
        if (playing)
        {
            return;
        }
        if (obj.gridCoords == _correctPositions[_playerPatternIndex])
        {
            Debug.Log("Correct");
            StartCoroutine(Co_FlashTile(obj, Color.green, 0.25f));
            _playerPatternIndex++;
            if (_playerPatternIndex == _correctPositions.Count)
            {
                NextPattern();
            }
        }
        else
        {
            Debug.Log("Incorrect");
            StartCoroutine(Co_FlashTile(obj, Color.red, 0.25f));
            _correctPositions.Clear();
            NextPattern();
        }
    }

    public void Start()
    {
        NextPattern();
    }

    [ContextMenu("Next Pattern")]
    public void NextPattern()
    {
        if (playing)
        {
            return;
        }
        _playerPatternIndex = 0;
        _correctPositions.Add(new Vector2Int(Random.Range(0, _gridManager.numColumns), Random.Range(0, _gridManager.numRows)));
        StartCoroutine(Co_PlayPattern(_correctPositions));
    }

    private IEnumerator Co_PlayPattern(List<Vector2Int> positions)
    {
        yield return new WaitForSeconds(0.5f);
        playing = true;
        foreach(var pos in positions)
        {
            GridTile tile = _gridManager.GetTile(pos);
            yield return StartCoroutine(Co_FlashTile(tile, Color.green, 1f));
            yield return new WaitForSeconds(0.5f);
        }
        playing = false;
    }

    private IEnumerator Co_FlashTile(GridTile tile, Color color, float duration)
    {
        tile.SetColor(color);
        yield return new WaitForSeconds(duration);
        tile.ResetColor();
    }
}
