using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Minesweeper.UI
{
    public class GameOverScreenView : MonoBehaviour
    {
        [field: SerializeField] public Button RestartBttn { get; private set; }
        [field: SerializeField] public Button MainMenuBttn { get; private set; }

        [SerializeField] private TMP_Text messageLabel;
        [SerializeField] private Image faceImg;

        public void Setup(string message, Sprite image)
        {
            messageLabel.SetText(message);
            faceImg.sprite = image;
        }

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}
