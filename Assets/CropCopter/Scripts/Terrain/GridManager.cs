using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private List<List<(Vector2, GameObject)>> grid = new List<List<(Vector2, GameObject)>>();
    public int boxSize = 5;
    
    private int length;
    private int height;
    
    void Start()
    {
        length = (int) GetComponent<BoxCollider>().size.x;
        height = (int) GetComponent<BoxCollider>().size.z;
        
        for (int i = 0; i < length; i+=boxSize)
        {
            List<(Vector2, GameObject)> row = new List<(Vector2, GameObject)>();
            
            for (int j = 0; j < height; j+=boxSize)
            {
                row.Add((new Vector2(i - length /2, j - height/2), null));
            }
            
            grid.Add(row);
        }
    }

    public (Vector2, GameObject) GetClosestBox(Vector2 pos)
    {
        if((pos.x < -length / 2f || pos.x > length / 2f || pos.y < -height / 2f || pos.y > height / 2f)) {
            return (new Vector2(-1, -1), null);
        }
        
        (Vector2, GameObject) closest = (new Vector2(-1, -1), null);
        float minDistance = float.MaxValue;

        foreach (var row in grid)
        {
            foreach (var box in row)
            {
                float distance = Vector2.Distance(box.Item1, pos);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = (box.Item1, box.Item2);
                }
            }
            
        }

        return closest;
    }

    public void SetBox(Vector2 pos, GameObject plant) {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j].Item1 == pos)
                {
                    grid[i][j] = (pos, plant);
                }
            }
        }
    }

    public List<List<(Vector2, GameObject)>> GetGrid()
    {
        return grid;
    }
}