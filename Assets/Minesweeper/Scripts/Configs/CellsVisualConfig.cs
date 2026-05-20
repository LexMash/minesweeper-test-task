using Minesweeper.Board;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.Configs
{
    [CreateAssetMenu(fileName = "CellVisualConfig", menuName = "Minesweeper/Cell Visual Config")]
    public class CellVisualConfig : ScriptableObject
    {
        [SerializeField] private CellStateVisualMapping[] states;
        [SerializeField] private CellVisualMinesAmountMapping[] numbers;

        private Dictionary<CellState, Sprite> stateMap;
        private Dictionary<int, Sprite> numberMap;

        public Sprite GetSprite(CellState state)
        {
            if (stateMap == null)
                BuildStateMap();

            return stateMap[state];
        }

        public Sprite GetSprite(int minesAmount)
        {
            if (numberMap == null)
                BuildNumberMap();

            return numberMap[minesAmount];
        }

        private void BuildStateMap()
        {
            stateMap = new Dictionary<CellState, Sprite>(states.Length);

            for (int i = 0; i < states.Length; i++)
            {
                CellStateVisualMapping mapping = states[i];
                stateMap[mapping.State] = mapping.Sprite;
            }
        }

        private void BuildNumberMap()
        {
            numberMap = new Dictionary<int, Sprite>(numbers.Length);

            for (int i = 0; i < numbers.Length; i++)
            {
                CellVisualMinesAmountMapping mapping = numbers[i];
                numberMap[mapping.MinesAmount] = mapping.Sprite;
            }
        }
    }
}
