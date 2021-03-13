using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField] private Cell CellPrefab;

    private Cell[,] _cells;
    private Vector2Int _exitPosition;
    private MazeGeneratorCell[,] _dopCells;
    private Vector3 CellSize = new Vector3(10, 0, 10);
    private int _width, _height;
    private Cell _exitCell;
    private Player _player;

    public Door Door { get; private set; }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] cells)
    {
        MazeGeneratorCell current = cells[0, 0];
        current.VisitCell();
        current.SetDistanceFromStart(0);

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.XPosition;
            int y = current.YPosition;

            if (x > 0 && !cells[x - 1, y].Visited) unvisitedNeighbours.Add(cells[x - 1, y]);
            if (y > 0 && !cells[x, y - 1].Visited) unvisitedNeighbours.Add(cells[x, y - 1]);
            if (x < _width - 2 && !cells[x + 1, y].Visited) unvisitedNeighbours.Add(cells[x + 1, y]);
            if (y < _height - 2 && !cells[x, y + 1].Visited) unvisitedNeighbours.Add(cells[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.VisitCell();
                stack.Push(chosen);
                chosen.SetDistanceFromStart(current.DistanceFromStart + 1);
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.XPosition == b.XPosition)
        {
            if (a.YPosition > b.YPosition) a.DisableBottomWall();
            else b.DisableBottomWall();
        }
        else
        {
            if (a.XPosition > b.XPosition) a.DisableLeftWall();
            else b.DisableLeftWall();
        }
    }

    private Vector2Int GetExitPosition()
    {
        MazeGeneratorCell furthest = _dopCells[0, 0];

        for (int x = 0; x < _dopCells.GetLength(0); x++)
        {
            if (_dopCells[x, _height - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = _dopCells[x, _height - 2];
            if (_dopCells[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = _dopCells[x, 0];
        }

        for (int y = 0; y < _dopCells.GetLength(1); y++)
        {
            if (_dopCells[_width - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = _dopCells[_width - 2, y];
            if (_dopCells[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = _dopCells[0, y];
        }

        if (furthest.XPosition == 0)
        {
            furthest.DisableLeftWall();
            furthest.ActivateLeftDoor();
        }
        else if (furthest.YPosition == 0)
        {
            furthest.DisableBottomWall();
            furthest.ActivateBottomDoor();
        }
        else if (furthest.XPosition == _width - 2)
        {
            _dopCells[furthest.XPosition + 1, furthest.YPosition].DisableLeftWall();
            _dopCells[furthest.XPosition + 1, furthest.YPosition].ActivateLeftDoor();
        }
        else if (furthest.YPosition == _height - 2)
        {
            _dopCells[furthest.XPosition, furthest.YPosition + 1].DisableBottomWall();
            _dopCells[furthest.XPosition, furthest.YPosition + 1].ActivateBottomDoor();
        }
        return new Vector2Int(furthest.XPosition, furthest.YPosition);
    }

    public void GenerateNewMaze(int width,int height,Player player)
    {
        _player = player;
        _width = width;
        _height = height;

        _dopCells = new MazeGeneratorCell[_width, _height];

        for (int x = 0; x < _dopCells.GetLength(0); x++)
        {
            for (int y = 0; y < _dopCells.GetLength(1); y++)
            {
                _dopCells[x, y] = new MazeGeneratorCell(x, y);
            }
        }

        for (int x = 0; x < _dopCells.GetLength(0); x++)
        {
            _dopCells[x, _height - 1].DisableLeftWall();
            _dopCells[x, _height - 1].DisableFloor();
        }

        for (int y = 0; y < _dopCells.GetLength(1); y++)
        {
            _dopCells[_width - 1, y].DisableBottomWall();
            _dopCells[_width - 1, y].DisableFloor();
        }

        RemoveWallsWithBacktracker(_dopCells);
        _exitPosition = GetExitPosition();

    }

    public void SpawnMaze()
    {
        _cells = new Cell[_width, _height];

        for (int x = 0; x < _dopCells.GetLength(0); x++)
        {
            for (int y = 0; y < _dopCells.GetLength(1); y++)
            {
                Cell cell = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity, this.transform );

                if (_dopCells[x,y].WallLeftActive == false)
                    cell.DisableLeftWall();
                if (_dopCells[x, y].WallBottomActive == false)
                    cell.DisableBottomWall();
                if (_dopCells[x, y].FloorActive == false)
                    cell.DisableFloor();
                if (_dopCells[x, y].BottomDoorActive)
                {
                    cell.ActivateBottomDoor();
                    Door = cell.ExitDoor;
                }
                if (_dopCells[x, y].LeftDoorActive)
                {
                    cell.ActivateLeftDoor();
                    Door = cell.ExitDoor;
                }

                _cells[x, y] = cell;
                cell.SettingTorchs();
                cell.SpawnDecor();                
            }
        }
        _exitCell = _cells[_exitPosition.x, _exitPosition.y];
    }

    public void SpawnEnemy(Enemy enemy, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomCellXPosition = Random.Range(0, _cells.GetLength(0) - 1);
            int randomCellYPosition = Random.Range(0, _cells.GetLength(1) - 1);

            if (randomCellXPosition == 0 & randomCellYPosition == 0)
            {
                i--;
                break;
            }
            Cell currentCell = _cells[randomCellXPosition, randomCellYPosition];

            if (currentCell.EnemySpawned == false)
            {
                currentCell.SpawnEnemy(enemy, _player);
            }
        }
    }

    public void SpawnChests(Chest prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomCellXPosition = Random.Range(0, _cells.GetLength(0) - 1);
            int randomCellYPosition = Random.Range(0, _cells.GetLength(1) - 1);

            if (randomCellXPosition == 0 & randomCellYPosition == 0)
                break;            

            Cell currentCell = _cells[randomCellXPosition, randomCellYPosition];

            if (currentCell.ChestSpawned == false)
            {
                if (currentCell.TrySpawnChest(prefab, out Chest chest))
                    chest.Opened += ChestOpened;                
            }

        }
    }

    private void ChestOpened(Chest chest)
    {
        chest.Opened -= ChestOpened;
        _player.GetSingTables(chest.CountSingTableReward);
    }
}