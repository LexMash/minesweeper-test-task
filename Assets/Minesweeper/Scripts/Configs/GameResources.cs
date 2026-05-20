using Minesweeper.View;
using UnityEngine;

namespace Minesweeper.Configs
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "Minesweeper/Game Resources")]
    public class GameResources : ScriptableObject
    {
        public Sprite WinImage;
        public Sprite LoseImage;

        public string WinMessage;
        public string LoseMessage;

        public CellView CellPrefab;
    }
}
