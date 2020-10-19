using System;
using UnityEngine;

public class GridOps : MonoBehaviour
{
    [SerializeField]
    GameMaster gameMaster;
    [SerializeField]
    GridManager gridManager;

    public void TrySettleGroup(Transform groupParent)
    {
        int snapCount = 0, childCount = groupParent.childCount;
        CoordPair[] coordPairs = new CoordPair[groupParent.childCount];

        for (int i = 0; i < childCount; i++)
        {
            int x, y;

            if (gridManager.WorldToGridPosition(groupParent.GetChild(i).position, out x, out y) && gridManager.GetCellValue(x, y) == 0)
            {
                coordPairs[i] = new CoordPair(x, y);
                snapCount++;
            }
        }

        if (snapCount == childCount)
            SettleGroup(groupParent, coordPairs);
    }

    public bool CanBePlaced(Transform groupParent)
    {
        int n;

        if (groupParent == null || (n = groupParent.childCount) == 0)
            return true;

        CoordPair[] coordinatePairs = new CoordPair[n];
        Vector3 pos0 = groupParent.GetChild(0).localPosition;

        for (int i = 0; i < n; i++)
        {
            Vector3 relativePosition = (groupParent.GetChild(i).localPosition - pos0) / gridManager.cellSize;

            coordinatePairs[i] = new CoordPair((int)relativePosition.x, (int)relativePosition.y);
        }

        for (int x = 0; x < gridManager.Width; x++)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                bool placable = true;

                foreach (CoordPair pair in coordinatePairs)
                {
                    if (gridManager.GetCellValue(x + pair.x, y + pair.y) != 0)
                    {
                        placable = false;
                        break;
                    }
                }

                if (placable)
                    return true;
            }
        }

        return false;
    }

    private void SettleGroup(Transform groupParent, CoordPair[] coordPairs)
    {
        Transform[] blocks = DetachReturnChildren(groupParent);

        for (int i = 0; i < blocks.Length; i++)
        {
            // set snap coordinates
            blocks[i].position = gridManager.GetCellPosition(coordPairs[i].x, coordPairs[i].y) + Vector3.forward * 4;

            // upgrade cell
            int value = Convert.ToInt32(blocks[i].name);
            gridManager.UpdateCell(coordPairs[i].x, coordPairs[i].y, blocks[i].gameObject, value);

            // update line sum
            gameMaster.UpdateValues(coordPairs[i], value);
        }

        Destroy(groupParent.gameObject);
        gameMaster.SettleComplete();
    }

    private Transform[] DetachReturnChildren(Transform parent)
    {
        int childCount = parent.childCount;
        Transform[] children = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(0);
            children[i] = child;
            child.SetParent(null);
        }

        return children;
    }
}
