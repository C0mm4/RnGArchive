using UnityEngine;
using UnityEngine.UI;

public class DynamicLayoutAdjuster : MonoBehaviour
{
    public GridLayoutGroup parentGrid;
    public GridLayoutGroup[] childGrids;

    void Start()
    {
        AdjustChildLayouts();
    }

    void AdjustChildLayouts()
    {
        foreach (var childGrid in childGrids)
        {
            var rectTransform = childGrid.GetComponent<RectTransform>();
            float preferredHeight = childGrid.cellSize.y * childGrid.constraintCount + childGrid.spacing.y * (childGrid.constraintCount - 1);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
        }
    }
}