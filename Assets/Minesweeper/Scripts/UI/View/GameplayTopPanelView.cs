using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.UI
{
    public class GameplayTopPanelView : MonoBehaviour
    {
        [field: SerializeField] public Button RestartBttn { get; private set; }
        [field: SerializeField] public Button PauseBttn { get; private set; }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
