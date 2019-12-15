using UnityEngine;

namespace Project.QueadTree
{
    public interface IQuadTreeBody
    {
        Vector2 Position { get; }
        bool QuadTreeIgnore { get; }
    }
}

