using UnityEngine;
using UserInterfaceGridLayout;

namespace Minesweeper.View
{
    public class BoardGridView : MonoBehaviour
    {
        [SerializeField] private FlexibleGridLayout grid;
        [field: SerializeField] public RectTransform Root { get; private set; }

        public void SetupGrid(int rows, int col)
        {
            grid.cellSize.x = Root.sizeDelta.y / rows;
            grid.columns = col;
            grid.rows = rows;
        }
    }
}
