using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
  [SerializeField] private Vector2Int _boardSize = new Vector2Int(11,11);
  [SerializeField] private GameBoard _board;
  [SerializeField] private GameTileContentFactory _tileContentFactory;
  [SerializeField] private EnemyFactory _enemyFactory;
  [SerializeField, Range(0.1f, 10f)] private float _spawnSpeed = 1f;

    private EnemyCollection _enemies = new EnemyCollection(); 
  
   private float _spawnProgress;
   private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

  private void Awake() 
  {
    _board.Initialize(_boardSize, _tileContentFactory);
    _board.ShowGrid = true;
    }

  private void OnValidate() 
  {
    if(_boardSize.x < 2)
        _boardSize.x = 2;
    if (_boardSize.y < 2)
        _boardSize.y = 2; 
  }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleTouch();
        else if (Input.GetMouseButtonDown(1))
            HandleAlternativeTouch();

        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPaths = !_board.ShowPaths;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _board.ShowGrid = !_board.ShowGrid;
        }

        _spawnProgress += _spawnSpeed * Time.deltaTime;
        while (_spawnProgress >= 1f)
        {
            _spawnProgress -= 1f;
            SpawnEnemy();
        }
        _enemies.GameUpdate();
    }

    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetSpawnPoint(Random.Range(0, _board.SpawnPointCount));
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        _enemies.Add(enemy);
    }

    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
            _board.ToggleDestination(tile);
    }

    private void HandleAlternativeTouch()
    { 
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null) 
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleWall(tile);
            }
            else _board.ToggleSpawnPoint(tile);
        }
            
    }
}
