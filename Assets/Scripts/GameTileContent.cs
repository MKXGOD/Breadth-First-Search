using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField] GameTileContentType type;
    public GameTileContentType Type => type;

    private GameTileContentFactory originFactory;

    public GameTileContentFactory OriginFactory 
    { 
        get => originFactory;
        set 
        { 
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void Recycle()
    {
        originFactory.Reclaim(this);
    }
}
public enum GameTileContentType
{
    Empty, Destination, Wall
}
