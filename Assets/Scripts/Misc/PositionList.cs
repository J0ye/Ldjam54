using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionList
{
    private List<Vector3> positions = new List<Vector3>();
    private int margin;
    private Vector2 xRange;
    private Vector2 zRange;

    private void FillList()
    {
        for(float i = xRange.x; i <= xRange.y; i += margin)
        {
            for(float j = zRange.x; j <= zRange.y; j +=margin)
            {
                positions.Add(new Vector3(i, j, 0));
            }
        }
    }

    private void FillList(Vector3 area)
    {
        for (float i = xRange.x; i <= xRange.y; i += margin)
        {
            for (float j = zRange.x; j <= zRange.y; j += margin)
            {
                positions.Add(area + new Vector3(i, j, 0));
            }
        }
    }

    public PositionList(int newMargin, Vector2 newXRange, Vector2 newZRange)
    {
        margin = newMargin;
        xRange = newXRange;
        zRange = newZRange;

        FillList();
    }

    public PositionList(int newMargin, Vector3 area, Vector2 newXRange, Vector2 newZRange)
    {
        margin = newMargin;
        xRange = newXRange;
        zRange = newZRange;

        FillList(area);
    }

    public List<Vector3> GetCopyPositionList()
    {
        return positions;
    }

    public Vector3 GetRandomPosition()
    {
        if(positions.Count <= 0)
        {
            FillList();
        }
        int rand = UnityEngine.Random.Range(0, positions.Count -1);
        Vector3 pos = positions[rand];
        positions.RemoveAt(rand);

        return pos;        
    }

    public int GetPositionCount()
    {
        return positions.Count;
    }

    public void RemovePositionsCloseTo(Vector3 position, int rows)
    {
        positions.RemoveAll(pos => Vector3.Distance(pos, position) < margin * rows);
    }
}
