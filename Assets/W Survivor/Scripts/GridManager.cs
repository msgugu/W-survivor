using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public HashSet<Gem> Gems { get; } = new HashSet<Gem>();
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

public class GridManager : MonoBehaviour
{   
    [SerializeField]
    private UnityEngine.Grid _grid;
    
    private Dictionary<Vector3Int, Cell> _cells;

    public int cellSizeX;
    public int cellSizeY;
    
    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _cells = new Dictionary<Vector3Int, Cell>();
        
        cellSizeX = (int)_grid.cellSize.x;
        cellSizeY = (int)_grid.cellSize.y;
    }
    
    public Grid grid
    {
        get { return _grid; }
    }
    
    public Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;
        
        if (_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell();
            _cells.Add(cellPos, cell);
        }

        return cell;
    }

    public Cell GetCell(Vector2 cellPos) => GetCell(new Vector3Int((int)cellPos.x, (int)cellPos.y, 0));

    public void AddToCell(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Objects.Add(go);
    }

    public void AddToCell<T>(T component) where T : Component
    {
        Vector3Int cellPos = _grid.WorldToCell(component.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        if (component is Gem)
        {
            cell.Gems.Add(component as Gem);
        }
        else
        {
            Debug.Log("AddToCell Type is wrong." + typeof(T).Name);
        }
    }

    public void RemoveFromCell(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Objects.Remove(go);
    }

    public void RemoveFromCell<T>(T component) where T : Component
    {
        Vector3Int cellPos = _grid.WorldToCell(component.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;
        
        if (component is Gem)
        {
            cell.Gems.Remove(component as Gem);
        }
        else
        {
            Debug.Log("RemoveFromCell Type is wrong." + typeof(T).Name);
        }
    }
    
    // get the nearest cell in a particular direction
    // 0-> right, 1->left, 2->down, 3->up

    public Vector3Int GetNearCellPos(Vector3Int pos, int direction, int distance) =>
        GetNearCellPos(pos.x, pos.y, direction, distance);
    
    public Vector3Int GetNearCellPos(int x, int y, int direction, int distance)
    {
        switch (direction)
        {
            case 0:
                return new Vector3Int(x + (cellSizeX * distance), y, 0);
                break;
            case 1:
                return new Vector3Int(x - (cellSizeX * distance), y, 0);
                break;
            case 2:
                return new Vector3Int(x, y + (cellSizeY * distance), 0);
                break;
            case 3:
                return new Vector3Int(x, y - (cellSizeY * distance), 0);
                break;
            default:
                return Vector3Int.one;
                break;
        }
    }

    public List<GameObject> GatherObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();
        
        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, +range));
        
        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int temp = new Vector3Int(x, y, 0);
                if (_cells.ContainsKey(temp) == false)
                    continue;
                
                objects.AddRange(_cells[temp].Objects);
            }
        }

        return objects;
    }

    public int[] GetRange(Vector3 pos, float range)
    {
        int[] roi = new int[4];
        
        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, +range));
        
        roi[0] = left.x;
        roi[1] = right.x;
        roi[2] = bottom.y;
        roi[3] = top.y;

        return roi;
    }

    public List<Gem> GatherGems(Vector3 pos, float range)
    {
        List<Gem> gems = new List<Gem>();
        int[] roi = GetRange(pos, range);

        for (int x = roi[0]; x <= roi[1]; x++)
        {
            for (int y = roi[2]; y <= roi[3]; y++)
            {
                Vector3Int temp = new Vector3Int(x, y, 0);
                if (_cells.ContainsKey(temp) == false)
                    continue;
                
                gems.AddRange(_cells[temp].Gems);
            }
        }

        return gems;
    }
}
