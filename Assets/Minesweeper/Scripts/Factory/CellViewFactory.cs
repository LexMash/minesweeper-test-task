using Minesweeper.View;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Minesweeper.Board
{
    public class CellViewFactory : IDisposable
    {
        private readonly ObjectPool<CellView> pool;

        public CellViewFactory(CellView prefab)
        {
            pool = new
                (
                    createFunc: () => GameObject.Instantiate(prefab),
                    actionOnRelease: (instance) => instance.Hide(),
                    actionOnDestroy: (instance) => GameObject.Destroy(instance.gameObject),
                    defaultCapacity: 81
                );
        }

        public CellView Create() => pool.Get();
        public void Release(CellView view) => pool.Release(view);
        public void Dispose() => pool.Dispose();
    }
}
