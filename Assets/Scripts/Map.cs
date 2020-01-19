using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;

public class Map : MonoBehaviour
{
    public GameObject HexCard;
    public GameObject Boost;
    public GameObject Rocket;
    public GameObject Health;
    public GameObject Mine;
    public GameObject Shield;
    public GameObject Empty;

    private GameObject _currentTile;
    public GameObject CurrentTile
    {
        get => _currentTile;
        set
        {
            value.GetComponent<Tile>().SetIsCurrent(true);
            _currentTile = value;
        }
    }

    private GameObject _previousTile;
    public GameObject PreviousTile
    {
        get => _previousTile;
        set
        {
            value.GetComponent<Tile>().SetIsPrevious(true);
            _previousTile = value;
        }
    }

    private IEnumerable<GameObject> _availableTiles;
    public IEnumerable<GameObject> AvailableTiles
    {
        get => _availableTiles;
        set
        {
            foreach (var tile in value)
            {
                tile.GetComponent<Tile>().SetIsAvailable(true);
            }

            _availableTiles = value;
        }
    }

    private GameObject _selectedTile;
    public GameObject SelectedTile
    {
        get => _selectedTile;
        set
        {
            _selectedTile = value;
        }
    }
    
    public List<GameObject> Tiles;
    
    public float HealthPercent;
    public float BoostPercent;
    public float RocketPercent;
    public float MinePercent;
    public float ShieldPercent;

    public int MapWidth;
    public int MapHeight;

    private const float X_OFFSET = 1.8f;
    private const float Z_OFFSET = 1.55f;

    private float X_ODD_OFFSET => X_OFFSET / 2;
    
    private float HealthRange => 0 + HealthPercent;
    private float BoostRange => HealthRange + BoostPercent;
    private float RocketRange => BoostRange + RocketPercent;
    private float MineRange => RocketRange + MinePercent;
    private float ShieldRange => MineRange + ShieldPercent;
    
    private IEnumerable<GameObject> MovableTiles => GetMovableTiles();
    private IEnumerable<GameObject> BlockedTiles => GetBlockedTiles();
    private IEnumerable<GameObject> PossibleTiles => GetAvailableTiles();

    private int CurrentNorth;
    private int CurrentSouth;
    private int CurrentEast;
    private int CurrentWest;

    private string LastDirectionMoved;

    private Random _random;

    public void Awake()
    {
        _random = new Random();
        for (var x = 0; x < MapWidth; x++)
        {
            for (var y = 0; y < MapHeight; y++)
            {
                var xFloat = x * X_OFFSET;
                if (Math.Abs(y) % 2 == 1)
                    xFloat += X_ODD_OFFSET;
                var clone = Instantiate(
                    HexCard,
                    new Vector3(xFloat, 0, y * Z_OFFSET),
                    Quaternion.identity);
                clone.transform.SetParent(transform);
                var (itemTag, item) = GetCardItem();
                clone.GetComponent<Tile>().SetItem(itemTag, item);
                clone.GetComponent<Tile>().Coordinate = new Coordinate
                {
                    Y = y,
                    X = x
                };
                Tiles.Add(clone);
            }
        }

        CurrentSouth = 0;
        CurrentWest = 0;
        CurrentNorth = MapHeight - 1;
        CurrentEast = MapWidth - 1;
        
        CurrentTile = Tiles.First(tile =>
            tile.GetComponent<Tile>().Coordinate.X == 4
            && tile.GetComponent<Tile>().Coordinate.Y == 4);
        CurrentTile.GetComponent<Tile>().DestroyItem();
        PreviousTile = Tiles.First();
        AvailableTiles = PossibleTiles;
        SelectedTile = PossibleTiles.First(tile =>
            tile.GetComponent<Tile>().Coordinate.X == CurrentTile.GetComponent<Tile>().Coordinate.X + 1
            && tile.GetComponent<Tile>().Coordinate.Y == CurrentTile.GetComponent<Tile>().Coordinate.Y);
    }
 
    public void MoveToTile()
    {
        ResetTiles();
        var currentTile = CurrentTile;
        var selectedTile = SelectedTile;
        GenerateNewLayers(currentTile, selectedTile);

        PreviousTile = currentTile;
        CurrentTile = selectedTile;
        AvailableTiles = PossibleTiles;
        SelectedTile = AvailableTiles.First();
    }

    public void SetLastDirectionMoved(GameObject tile)
    {
        var dir = DirectionToTile(CurrentTile.GetComponent<Tile>(), tile.GetComponent<Tile>());
        LastDirectionMoved = dir;
    }

    public string DirectionToTile(Tile fromTile, Tile toTile)
    {
        if (fromTile.Coordinate.X < toTile.Coordinate.X &&
            fromTile.Coordinate.Y == toTile.Coordinate.Y)
            return "East";

        if (fromTile.Coordinate.Y > toTile.CoordinateY)
            return "South";

        return "North";
    }

    private void GenerateNewLayers(GameObject current, GameObject selected)
    {
        var selectedCoordinate = selected.GetComponent<Tile>().Coordinate;
        var currentCoordinate = current.GetComponent<Tile>().Coordinate;
        if (selectedCoordinate.X > currentCoordinate.X)
        {
            GenerateEastLayer();
            PruneXTiles();
        }

        if (selectedCoordinate.Y > currentCoordinate.Y)
        {
            GenerateNorthLayer();
            PruneYTiles();
        }

        if (selectedCoordinate.Y < currentCoordinate.Y)
        {
            GenerateSouthLayer();
            PruneYTiles();
        }
    }

    private void GenerateNorthLayer()
    {
        CurrentNorth += 1;
        CurrentSouth += 1;
        for (int x = CurrentWest; x <= CurrentEast; x++)
        {
            var xFloat = x * X_OFFSET;
            if (Math.Abs(CurrentNorth) % 2 == 1)
                xFloat += X_ODD_OFFSET;
            var clone = Instantiate(
                HexCard,
                new Vector3(xFloat, 0, CurrentNorth * Z_OFFSET),
                Quaternion.identity);
            clone.transform.SetParent(transform);
            var (itemTag, item) = GetCardItem();
            clone.GetComponent<Tile>().SetItem(itemTag, item);
            clone.GetComponent<Tile>().Coordinate = new Coordinate
            {
                Y = CurrentNorth,
                X = x
            };
            Tiles.Add(clone);
        }
    }
    
    private void GenerateSouthLayer()
    {
        CurrentSouth -= 1;
        CurrentNorth -= 1;
        for (int x = CurrentWest; x <= CurrentEast; x++)
        {
            var xFloat = x * X_OFFSET;
            if (Math.Abs(CurrentSouth) % 2 == 1)
                xFloat += X_ODD_OFFSET;
            var clone = Instantiate(
                HexCard,
                new Vector3(xFloat, 0, CurrentSouth * Z_OFFSET),
                Quaternion.identity);
            clone.transform.SetParent(transform);
            var (itemTag, item) = GetCardItem();
            clone.GetComponent<Tile>().SetItem(itemTag, item);
            clone.GetComponent<Tile>().Coordinate = new Coordinate
            {
                Y = CurrentSouth,
                X = x
            };
            Tiles.Add(clone);
        }
    }

    private void GenerateEastLayer()
    {
        CurrentEast += 1;
        CurrentWest += 1;
        for (int y = CurrentSouth; y <= CurrentNorth; y++)
        {
            var xFloat = CurrentEast * X_OFFSET;
            if (Math.Abs(y) % 2 == 1)
                xFloat += X_ODD_OFFSET;
            var clone = Instantiate(
                HexCard,
                new Vector3(xFloat, 0, y * Z_OFFSET),
                Quaternion.identity);
            clone.transform.SetParent(transform);
            var (itemTag, item) = GetCardItem();
            clone.GetComponent<Tile>().SetItem(itemTag, item);
            clone.GetComponent<Tile>().Coordinate = new Coordinate
            {
                Y = y,
                X = CurrentEast
            };
            Tiles.Add(clone);
        }
    }

    private void PruneXTiles()
    {
        var deadTiles = Tiles.Where(tile => tile.GetComponent<Tile>().Coordinate.X < CurrentWest).ToList();
        Tiles.RemoveAll(tile => tile.GetComponent<Tile>().Coordinate.X < CurrentWest);
        foreach (var deadTile in deadTiles)
        {
            Destroy(deadTile);
        }
    }
    
    private void PruneYTiles()
    {
        var deadTiles = Tiles.Where(tile =>
            tile.GetComponent<Tile>().Coordinate.Y < CurrentSouth
            || tile.GetComponent<Tile>().Coordinate.Y > CurrentNorth).ToList();
        Tiles.RemoveAll(tile =>
            tile.GetComponent<Tile>().Coordinate.Y < CurrentSouth
            || tile.GetComponent<Tile>().Coordinate.Y > CurrentNorth);
        foreach (var deadTile in deadTiles)
        {
            Destroy(deadTile);
        }
    }

    private void ResetTiles()
    {
        CurrentTile.GetComponent<Tile>().Reset();
        PreviousTile.GetComponent<Tile>().Reset();
        PreviousTile.GetComponent<Tile>().SetMaterialInactive();
        SelectedTile.GetComponent<Tile>().Reset();
        foreach (var availableTile in AvailableTiles)
        {
            availableTile.GetComponent<Tile>().Reset();
        }
    }

    public void SetSelected(GameObject tile)
    {
        SelectedTile = tile;
    }

    private IEnumerable<GameObject> GetAvailableTiles()
    {
        var movableTiles = MovableTiles.ToList();
        foreach (var movableTile in MovableTiles)
        {
            foreach (var blockedTile in BlockedTiles)
            {
                if (blockedTile.GetComponent<Tile>().Coordinate.X == movableTile.GetComponent<Tile>().Coordinate.X
                    && blockedTile.GetComponent<Tile>().Coordinate.Y == movableTile.GetComponent<Tile>().Coordinate.Y)
                    movableTiles.RemoveAll(t => 
                        t.GetComponent<Tile>().Coordinate.X == blockedTile.GetComponent<Tile>().Coordinate.X
                        && t.GetComponent<Tile>().Coordinate.Y == blockedTile.GetComponent<Tile>().Coordinate.Y);
            }
        }

        return movableTiles;
    }

    private IEnumerable<GameObject> GetBlockedTiles()
    {
        var currentNorth = NorthTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);
        var currentEast = EastTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);
        var currentSouth = SouthTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);

        if (LastDirectionMoved == "North")
            return MovableTiles.Where(tile =>
                tile.GetComponent<Tile>().Coordinate.X == currentNorth.X && tile.GetComponent<Tile>().Coordinate.Y == currentNorth.Y);

        if (LastDirectionMoved == "East")
            return MovableTiles.Where(tile =>
                tile.GetComponent<Tile>().Coordinate.X == currentEast.X && tile.GetComponent<Tile>().Coordinate.Y == currentEast.Y);

        if (LastDirectionMoved == "South")
            return MovableTiles.Where(tile =>
                tile.GetComponent<Tile>().Coordinate.X == currentSouth.X && tile.GetComponent<Tile>().Coordinate.Y == currentSouth.Y);
        
        return new List<GameObject>();
    }

    private IEnumerable<GameObject> GetMovableTiles()
    {
        var northCoordinate = NorthTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);
        var eastCoordinate = EastTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);
        var southCoordinate = SouthTileCoordinate(CurrentTile.GetComponent<Tile>().Coordinate);
        
        return Tiles.Where(tile =>
            tile.GetComponent<Tile>().Coordinate.X == northCoordinate.X && tile.GetComponent<Tile>().Coordinate.Y == northCoordinate.Y
            || tile.GetComponent<Tile>().Coordinate.X == eastCoordinate.X && tile.GetComponent<Tile>().Coordinate.Y == eastCoordinate.Y
            || tile.GetComponent<Tile>().Coordinate.X == southCoordinate.X && tile.GetComponent<Tile>().Coordinate.Y == southCoordinate.Y)
            .ToList();
    }

    private Coordinate EastTileCoordinate(Coordinate coordinate)
    {
        return new Coordinate
        {
            Y = coordinate.Y,
            X = coordinate.X + 1
        };
    }

    private Coordinate NorthTileCoordinate(Coordinate coordinate)
    {
        return new Coordinate
        {
            Y = coordinate.Y + 1,
            X = Math.Abs(coordinate.Y) % 2 == 1 ? coordinate.X + 1 : coordinate.X
        };
    }

    private Coordinate SouthTileCoordinate(Coordinate coordinate)
    {
        return new Coordinate
        {
            Y = coordinate.Y - 1,
            X = Math.Abs(coordinate.Y) % 2 == 1 ? coordinate.X + 1 : coordinate.X
        };
    }

    private (string, GameObject) GetCardItem()
    {
        var i = _random.NextDouble() * 100;
        if (i < HealthRange)
            return ("Health", Health);
        if (i < BoostRange)
            return ("Boost", Boost);
        if (i < RocketRange)
            return ("Rocket", Rocket);
        if (i < MineRange)
            return ("Mine", Mine);
        if (i < ShieldRange)
            return ("Shield", Shield);

        return ("Empty", Empty);
    }
}
