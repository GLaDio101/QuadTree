using System.Collections.Generic;
using UnityEngine;

namespace Project.QueadTree
{
    public partial class QuadTree
    {
        private class QuadTreePool
        {
            ///// Fields /////

            private static Queue<QuadTree> _pool;
            private static readonly int _maxPoolCount = 1024;
            private static readonly int _defaultMaxBodiesPerNode = 6;
            private static readonly int _defaultMaxLevel = 6;

            ///// Methods /////

            public static QuadTree GetQuadTree(Rect bounds, QuadTree parent)
            {
                if (_pool == null) Init();
                QuadTree tree = null;
                if (_pool != null && _pool.Count > 0)
                {
                    tree = _pool.Dequeue();
                    tree._bounds = bounds;
                    tree._parent = parent;
                    tree._maxLevel = parent._maxLevel;
                    tree._maxBodiesPerNode = parent._maxBodiesPerNode;
                    tree._curLevel = parent._curLevel + 1;
                }
                else tree = new QuadTree(bounds, parent);

                return tree;
            }

            public static void PoolQuadTree(QuadTree tree)
            {
                if (tree == null) return;
                tree.Clear();
                if (_pool.Count > _maxPoolCount) return;
                _pool.Enqueue(tree);
            }

            private static void Init()
            {
                _pool = new Queue<QuadTree>();
                for (int i = 0; i < _maxPoolCount; i++)
                {
                    _pool.Enqueue(new QuadTree(Rect.zero, _defaultMaxBodiesPerNode, _defaultMaxLevel));
                }
            }
        }
    }
}