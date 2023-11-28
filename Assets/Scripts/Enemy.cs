using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyFactory _originFactory;
    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress;
    
    public EnemyFactory OriginFactory 
    {
        get => _originFactory;
        set {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    public void SpawnOn(GameTile tile)
    {
        Debug.Assert(tile.NextTileOnPath != null, "Nowhereto go!", this);
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileTo.ExitPoint;
        _progress = 0;
    }
    public bool GameUpdate()
    {
        _progress += Time.deltaTime;
        while (_progress >= 1f)
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextTileOnPath;
            if (_tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            _positionFrom = _positionTo;
            _positionTo = _tileTo.ExitPoint;
            _progress -= 1f;
        }

        transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        return true;
    }
}
