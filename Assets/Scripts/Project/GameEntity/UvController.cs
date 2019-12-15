using UnityEngine;

namespace Project.GameEntity
{
    public class UvController : MonoBehaviour
    {
        public Mesh Mesh;

        public int x;
        public int y;
        public int width;
        public int height;
        public int textureWith;
        public int textureHeight;

        private Vector3[] _vertices;
        private Vector2[] _uv;
        private int[] _triangles;

        private void Awake()
        {
            Mesh = GetComponent<MeshFilter>().mesh;

            _vertices = new Vector3[4];
            _uv = new Vector2[4];
            _triangles = new int[6];

            _vertices[0] = new Vector3(0, 1);
            _vertices[1] = new Vector3(1, 1);
            _vertices[2] = new Vector3(0, 0);
            _vertices[3] = new Vector3(1, 0);

            _uv[0] = new Vector2(0, 1);
            _uv[1] = new Vector2(1, 1);
            _uv[2] = new Vector2(0, 0);
            _uv[3] = new Vector2(1, 0);

            _triangles[0] = 0;
            _triangles[1] = 1;
            _triangles[2] = 2;
            _triangles[3] = 2;
            _triangles[4] = 1;
            _triangles[5] = 3;
        }

        private void Update()
        {
            var uvrEctangleFromPixels = GetUvRectangleFromPixels(x, y, width, height, textureWith, textureHeight);

            ApplyUvToUvArray(uvrEctangleFromPixels);
        }


        private Vector2 ConvertPixelsToUVCoodinates(int x, int y, int textureWith, int textureHeight)
        {
            return new Vector2((float) x / textureWith, (float) y / textureHeight);
        }

        private Vector2[] GetUvRectangleFromPixels(int x, int y, int width, int height,
            int textureWidth, int textureHeight)
        {
            /* 0, 1
             * 1, 1
             * 0, 0
             * 1, 0
             */
            return new[]
            {
                ConvertPixelsToUVCoodinates(x, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoodinates(x + width, y + height, textureWidth, textureHeight),
                ConvertPixelsToUVCoodinates(x, y, textureWidth, textureHeight),
                ConvertPixelsToUVCoodinates(x + width, y, textureWidth, textureHeight),
            };
        }

        private void ApplyUvToUvArray(Vector2[] uv)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;
            mesh.uv = uv;
            mesh.triangles = _triangles;
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}