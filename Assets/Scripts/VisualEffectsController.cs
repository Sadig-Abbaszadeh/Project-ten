using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsController : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animCurve;
    [SerializeField]
    float animationTime, constantComponent;
    [SerializeField]
    GameObject[] rowLines, columnLines;

    public IEnumerator LineClearAnim(List<int> rows, List<int> columns)
    {
        float startTime = Time.timeSinceLevelLoad;
        float time = 0;

        ActivateDeactivateLines(rows, columns, true);
        
        // animate
        while(time <= animationTime)
        {
            float value = animCurve.Evaluate(time / animationTime);

            foreach (int row in rows)
                rowLines[row].transform.localScale = new Vector3(constantComponent, value, 1);
            foreach (int column in columns)
                columnLines[column].transform.localScale = new Vector3(value, constantComponent, 1);

            yield return null;

            time = Time.timeSinceLevelLoad - startTime;
        }

        ActivateDeactivateLines(rows, columns, false);
    }

    private void ActivateDeactivateLines(List<int> rows, List<int> columns, bool active)
    {
        foreach (int row in rows)
            rowLines[row].SetActive(active);
        foreach (int column in columns)
            columnLines[column].SetActive(active);
    }
}