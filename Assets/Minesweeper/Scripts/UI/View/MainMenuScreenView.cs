using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.UI
{
    public class MainMenuScreenView : MonoBehaviour
    {
        [field: SerializeField] public Button StartBttn { get; private set; }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
