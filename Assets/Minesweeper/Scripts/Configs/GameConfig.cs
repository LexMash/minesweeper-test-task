using UnityEngine;

namespace Minesweeper.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Minesweeper/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Board Config")]
        [Range(9, 30)] public int Rows = 9;
        [Range(9, 24)] public int Columns = 9;
        public int MinesAmount = 10;

#if UNITY_EDITOR
        [Range(0.01f, 0.9f)] public float MaxMinesPercentage = 0.5f;

        private void OnValidate()
        {
            if ((MinesAmount / (Rows * Columns)) > MaxMinesPercentage)
                MinesAmount = (int)(Rows * Columns * MaxMinesPercentage);

            if (MinesAmount <= 0)
                MinesAmount = 1;
        }
#endif 
    }
}
