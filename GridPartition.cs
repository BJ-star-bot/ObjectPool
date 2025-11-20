using System.Collections.Generic;
using UnityEngine;

public interface IGridObject
{
    Vector2Int Cell { get; set; }//每个需要分区管理的物体都自己保存自己的分区和分区编号
    int CellIndex { get; set; }
}
public class GridPartition<T> where T : MonoBehaviour, IGridObject
{
    public float cellSize = 20f;
    private Dictionary<Vector2Int, List<T>> grid = new();

    private Vector2Int GetCell(Vector3 pos)//根据三维位置转换到区块
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / cellSize),
            Mathf.FloorToInt(pos.z / cellSize)
        );
    }

    // ✅ O(1) 添加
    public void Add(T obj)
    {
        Vector2Int cell = GetCell(obj.transform.position);
        if (!grid.TryGetValue(cell, out var list))
            grid[cell] = list = new List<T>();

        obj.Cell = cell;
        obj.CellIndex = list.Count;
        list.Add(obj);
    }

    // ✅ O(1) 移除
    public void Remove(T obj)
    {
        if (!grid.TryGetValue(obj.Cell, out var list)) return;
        int lastIndex = list.Count - 1;

        if (obj.CellIndex < 0 || obj.CellIndex > lastIndex) return;

        T last = list[lastIndex];
        list[obj.CellIndex] = last;
        last.CellIndex = obj.CellIndex;//将要删除的物体在列表中的位置让给列表中最后一个物体，复杂度O(1)

        list.RemoveAt(lastIndex);
            obj.CellIndex = -1; 
    }

    // ✅ O(1) 更新（跨格时调用）
    public void UpdateCell(T obj)
    {
        Vector2Int newCell = GetCell(obj.transform.position);
        if (newCell == obj.Cell) return;

        Remove(obj);
        Add(obj);
    }

    // ✅ 获取周围格子对象
    public IEnumerable<T> GetNear(Vector3 center, float radius)//这一段会报错，改为下面的方法
    {
        Vector2Int mid = GetCell(center);
        int cellRadius = Mathf.CeilToInt(radius / cellSize);

        for (int x = -cellRadius; x <= cellRadius; x++)
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                Vector2Int c = mid + new Vector2Int(x, y);
                if (grid.TryGetValue(c, out var list))
                    foreach (var obj in list)
                        yield return obj;
            }
    }

    public List<T> GetNearSnapshot(Vector3 center, float radius, List<T> buffer = null)
{
    Vector2Int mid = GetCell(center);
    int cellRadius = Mathf.CeilToInt(radius / cellSize);

    var result = buffer ?? new List<T>(256);
    result.Clear();

    for (int x = -cellRadius; x <= cellRadius; x++)
    for (int y = -cellRadius; y <= cellRadius; y++)
    {
        Vector2Int c = mid + new Vector2Int(x, y);
        if (grid.TryGetValue(c, out var list))
            result.AddRange(list);   // ← 拷贝元素到快照
    }
    return result;
}
}
