using UnityEngine;

namespace Minesweeper.UI
{
    public class GameplayScreen : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
