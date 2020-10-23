using System.Collections;
using UnityEngine;

public class GridOps : MonoBehaviour
{
    [SerializeField]
    GameMaster gameMaster;
    [SerializeField]
    GridManager gridManager;

    IEnumerator CheckLineClear;

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
            StartCoroutine(SettleGroup(groupParent, coordPairs));
    }

    public bool CanBePlaced(Transform groupParent)
    {
        int n;

        if (groupParent == null || (n = groupParent.childCount) == 0)
            return false;

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

    private IEnumerator SettleGroup(Transform groupParent, CoordPair[] coordPairs)
    {
        CheckLineClear = gameMaster.CheckLineClear();

        Transform[] blocks = DetachReturnChildren(groupParent);
        Destroy(groupParent.gameObject);

        for (int i = 0; i < blocks.Length; i++)
        {
            // set snap coordinates
            blocks[i].position = gridManager.GetCellPosition(coordPairs[i].x, coordPairs[i].y) + Vector3.forward * 4;

            // upgrade cell
            int value = int.Parse(blocks[i].name);
            gridManager.UpdateCell(coordPairs[i].x, coordPairs[i].y, blocks[i].gameObject, value);

            // update line sum
            gameMaster.UpdateSums(coordPairs[i], value);
        }

        while (CheckLineClear.MoveNext())
            yield return null;

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
