using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _wallLeft;
    [SerializeField] private GameObject _wallBottom;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject[] _torchsOnLeftWall;
    [SerializeField] private GameObject[] _torchsOnBottomWall;
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _bottomDoor;
    [SerializeField] private int _chanceSpawnTorch;
    [SerializeField] private int _chanceSpawnDecorObject;
    [SerializeField] private Decor[] _templates;
    [SerializeField] private Transform[] _decorSpawnPoints;
    [SerializeField] private Transform _leftWallChestsSpawnPoints;
    [SerializeField] private Transform _bottomWallChestsSpawnPoints;
    [SerializeField] private Transform _enemySpawnPoint;
   

    public Door ExitDoor { get; private set; }

    public bool LeftWallActive { get; private set; } = true;
    public bool BottomWallActive { get; private set; } = true;
    public bool FloorActive { get; private set; } = true;
    public bool Visited { get; private set; } = false;
    public bool EnemySpawned { get; private set; } = false;
    public bool ChestSpawned { get; private set; } = false;

    private void SetActiveTorch(bool wallActive, GameObject[] torchs)
    {
        if (wallActive)
        {
            for (int i = 0; i < torchs.Length; i++)
            {
                int random = Random.Range(0, 100);

                if (_chanceSpawnTorch > random)
                    torchs[i].SetActive(true);
                else
                    torchs[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < torchs.Length; i++)
                torchs[i].SetActive(false);
        }
    }

    private void SpawnDecorItem(Decor decorTemplate, Transform parent)
    {
        var decorItem = Instantiate(decorTemplate, parent);
        decorTemplate.SetParentCell(transform);

    }

    private Vector3 GetTurnEnemy()
    {
        Vector3 rotation = new Vector3();

        if (LeftWallActive == false)
            rotation.y = -90;
        else if (BottomWallActive == false)
            rotation.y = 180;

        return rotation;
    }

    public void DisableLeftWall()
    {
        _wallLeft.SetActive(false);
        LeftWallActive = false;
    }

    public void DisableBottomWall()
    {
        _wallBottom.SetActive(false);
        BottomWallActive = false;
    }

    public void DisableFloor()
    {
        _floor.SetActive(false);
        FloorActive = false;
    }

    public void SettingTorchs()
    {
        SetActiveTorch(LeftWallActive,_torchsOnLeftWall);
        SetActiveTorch(BottomWallActive, _torchsOnBottomWall);
    }

    public void ActivateLeftDoor()
    {
        _leftDoor.SetActive(true);
        ExitDoor = _leftDoor.GetComponentInChildren<Door>();
    }

    public void ActivateBottomDoor()
    {
        _bottomDoor.SetActive(true);
        ExitDoor = _bottomDoor.GetComponentInChildren<Door>();
    }

    public void SpawnDecor()
    {
        if (FloorActive)
        {
            for (int i = 0; i < _decorSpawnPoints.Length; i++)
            {
                int random = Random.Range(0, 100);
                if (_chanceSpawnDecorObject > random)
                {
                    SpawnDecorItem(_templates[Random.Range(0, _templates.Length)], _decorSpawnPoints[i].transform);
                }
            }
        }
    }

    public void SpawnEnemy(Enemy enemyTemplate, Player player)
    {
        EnemySpawned = true;

        var enemy =Instantiate(enemyTemplate, _enemySpawnPoint);

        enemy.transform.eulerAngles = GetTurnEnemy();

        enemy.Init(player);
    }

    public bool TrySpawnChest(Chest chestPrefab,out Chest chest)
    {
        Transform _currentSpawnPoint = null;

        chest= null;
        if (LeftWallActive)
            _currentSpawnPoint = _leftWallChestsSpawnPoints;
        else if (BottomWallActive)
            _currentSpawnPoint = _bottomWallChestsSpawnPoints;

        if (_currentSpawnPoint != null)
        {
            chest = Instantiate(chestPrefab, _currentSpawnPoint);
        }

            return chest != null;
    }
}