using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MazeDifficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private Transform Parent;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;
    [SerializeField] private MazeDifficulty _difficulty = MazeDifficulty.Medium;
    [SerializeField] private GameObject destinationMarker;
    [SerializeField] private GameObject playerPrefab;

    private MazeCell[,] _mazeGrid;
    private MazeCell _playerStartCell;

    void Start()
    {
        if (MazeDifficulty.Easy == _difficulty)
        {
            _mazeWidth = 4;
            _mazeDepth = 7;
        }
        else if (MazeDifficulty.Medium == _difficulty)
        {
            _mazeWidth = 5;
            _mazeDepth = 8;

        }
        else if (MazeDifficulty.Hard == _difficulty)
        {
            _mazeWidth = 8;
            _mazeDepth = 12;
        }

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, Parent);
            }
        }
        

        
        if (_difficulty == MazeDifficulty.Easy)
            GenerateSimplifiedMaze();
        else
            GenerateMaze(null, _mazeGrid[0, 0]);

        Parent.rotation = Quaternion.Euler(90, 0, 0);

        if (MazeDifficulty.Easy == _difficulty)
        {
            Parent.localScale = new Vector3(1.3f, 1, 1.2f);
            Parent.transform.position = new Vector3(-1.95f, 4, 0f);
        }
        else if (MazeDifficulty.Medium == _difficulty)
        {
            Parent.transform.position = new Vector3(-2f, 4, 0f);
        }
        else if (MazeDifficulty.Hard == _difficulty)
        {
            Parent.localScale = new Vector3(0.63f, 0.63f, 0.63f);
            Parent.transform.position = new Vector3(-2.22f, 4.3f, 0f);
        }

        if (_difficulty == MazeDifficulty.Hard)
            AddExtraDeadEnds();

        PlacePlayerAndDestination();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();

        if (_difficulty == MazeDifficulty.Easy)
        {
            return unvisitedCells.OrderBy(_ => Random.value).FirstOrDefault();
        }
        else if (_difficulty == MazeDifficulty.Hard)
        {
            return unvisitedCells.OrderBy(_ => Random.value).Skip(Random.Range(0, unvisitedCells.Count)).FirstOrDefault();
        }

        return unvisitedCells.OrderBy(_ => Random.value).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited)
            yield return _mazeGrid[x + 1, z];

        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited)
            yield return _mazeGrid[x - 1, z];

        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited)
            yield return _mazeGrid[x, z + 1];

        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited)
            yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
            return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private void ClearWallBetween(MazeCell a, MazeCell b)
    {
        if (a.transform.position.x < b.transform.position.x)
        {
            a.ClearRightWall();
            b.ClearLeftWall();
        }
        else if (a.transform.position.x > b.transform.position.x)
        {
            a.ClearLeftWall();
            b.ClearRightWall();
        }
        else if (a.transform.position.z < b.transform.position.z)
        {
            a.ClearFrontWall();
            b.ClearBackWall();
        }
        else if (a.transform.position.z > b.transform.position.z)
        {
            a.ClearBackWall();
            b.ClearFrontWall();
        }
    }

    private void GenerateSimplifiedMaze()
    {
        MazeCell startCell = _mazeGrid[0, 0];
        GenerateMaze(null, startCell);

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                MazeCell cell = _mazeGrid[x, z];
                if (!cell.IsVisited && Random.value < 0.7f)
                {
                    _mazeGrid[x, z].gameObject.SetActive(false);
                }
            }
        }
    }

    private void AddExtraDeadEnds()
    {
        for (int i = 0; i < 1000; i++)
        {
            int x = Random.Range(1, _mazeWidth - 1);
            int z = Random.Range(1, _mazeDepth - 1);

            MazeCell cell = _mazeGrid[x, z];
            if (cell.IsVisited == false)
            {
                List<Vector2Int> directions = new List<Vector2Int>
                {
                    new Vector2Int(1, 0), new Vector2Int(-1, 0),
                    new Vector2Int(0, 1), new Vector2Int(0, -1)
                };

                directions = directions.OrderBy(d => Random.value).ToList();

                foreach (var dir in directions)
                {
                    int nx = x + dir.x;
                    int nz = z + dir.y;

                    if (nx >= 0 && nx < _mazeWidth && nz >= 0 && nz < _mazeDepth)
                    {
                        MazeCell neighbor = _mazeGrid[nx, nz];
                        if (neighbor.IsVisited)
                        {
                            ClearWallBetween(cell, neighbor);
                            cell.Visit();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void PlacePlayerAndDestination()
    {
        List<Vector2Int> corners = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, _mazeDepth - 1),
            new Vector2Int(_mazeWidth - 1, 0),
            new Vector2Int(_mazeWidth - 1, _mazeDepth - 1)
        };

        List<MazeCell> validCorners = corners
            .Select(corner => _mazeGrid[corner.x, corner.y])
            .Where(cell => cell.IsVisited)
            .ToList();

        if (validCorners.Count == 0) return;

        _playerStartCell = validCorners[Random.Range(0, validCorners.Count)];

        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, _playerStartCell.transform.position , Quaternion.identity);
        }

        MazeCell farthest = null;
        float maxDistance = 0f;

        foreach (var cell in _mazeGrid)
        {
            if (cell.IsVisited && cell != _playerStartCell)
            {
                float dist = Vector3.Distance(cell.transform.position, _playerStartCell.transform.position);
                if (dist > maxDistance)
                {
                    maxDistance = dist;
                    farthest = cell;
                }
            }
        }

        if (farthest != null && destinationMarker != null)
        {
            Instantiate(destinationMarker, farthest.transform.position, Quaternion.identity);
        }
    }
} 