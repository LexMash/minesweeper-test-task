using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minesweeper.View
{
    public class CellView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image mainImg;

        private int index;

        public event Action<MouseClickType, int> OnClick;

        public void Setup(Sprite main, int index)
        {
            this.index = index;
            mainImg.sprite = main;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public void UpdateImage(Sprite sprite)
        {
            mainImg.sprite = sprite;
        }

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            MouseClickType click = eventData.button == PointerEventData.InputButton.Left 
                ? MouseClickType.Left 
                : MouseClickType.Right;

            OnClick?.Invoke(click, index);
        }
    }
}