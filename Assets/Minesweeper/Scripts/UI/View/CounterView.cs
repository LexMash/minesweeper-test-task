using TMPro;
using UnityEngine;

namespace Minesweeper.View
{
    public class CounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text counterLabel;

        public void UpdateCounter(float value)
        {
            counterLabel.text = value.ToString();
        }
    }
}
