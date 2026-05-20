using UnityEngine;
using UserInterfaceGridLayout;

namespace Minesweeper.View
{
    public class BoardGridView : MonoBehaviour
    {
        [SerializeField] private FlexibleGridLayout grid;
        [field: SerializeField] public Transform Root { get; private set; }

        public void SetupGrid(int width, int height)
        {  
            grid.columns = width;
            grid.rows = height;
        }

        public void EnableGrid(bool enable) => grid.enabled = enable;
    }
}
