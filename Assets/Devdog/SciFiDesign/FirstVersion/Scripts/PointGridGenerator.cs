using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Devdog.General.UI;

namespace Devdog.SciFiDesign
{
    public class PointGridGenerator : MonoBehaviour
    {

        public int rows = 10;
        public int cols = 10;

        public Vector2 spacing = Vector2.one;

        public Material material;

        public float movementEffect = 1.0f;
        public float maxMovementEffect = 0.2f;
        public float effectDegradeSpeed = -0.01f;

        private GameObject _gridObj;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private Vector3 _mousePrevFrame;
        private float _mouseMovementSpeed;

        private void Start()
        {
            _gridObj = new GameObject();
            _gridObj.transform.SetParent(transform);
            UIUtility.ResetTransform(_gridObj.transform);

            _meshFilter = _gridObj.AddComponent<MeshFilter>();
            _meshRenderer = _gridObj.AddComponent<MeshRenderer>();

            _mousePrevFrame = Input.mousePosition;

            GeneratePoints();
        }

        protected void Update()
        {
            var mouseDelta = Input.mousePosition - _mousePrevFrame;
            mouseDelta.x /= Screen.width;
            mouseDelta.y /= Screen.height;
            mouseDelta *= movementEffect;

            _mouseMovementSpeed += mouseDelta.magnitude;
            _mouseMovementSpeed -= effectDegradeSpeed * Time.deltaTime;
            _mouseMovementSpeed = Mathf.Clamp(_mouseMovementSpeed, 0f, maxMovementEffect);

            var pos = Input.mousePosition;
            pos.x /= Screen.width;
            pos.y /= Screen.height;
            
            material.SetFloat("_MouseMovementSpeed", _mouseMovementSpeed);
            material.SetVector("_MousePosition", pos);

            _mousePrevFrame = Input.mousePosition;
        }


        public void GeneratePoints()
        {
            var m = new Mesh();
            var vertices = new List<Vector3>(rows * cols);
            var uvs = new List<Vector2>(rows * cols); // Abusing uvs to store vertex ID
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    vertices.Add(new Vector3(i * spacing.x, j * spacing.y, 0f));
                    uvs.Add(new Vector2(i / (float)rows, j / (float)cols)); // Abusing uvs to store vertex ID
                }
            }

            m.SetVertices(vertices);
            m.SetTriangles(new int[0], 0);
            m.SetIndices(Enumerable.Range(0, rows * cols).ToArray(), MeshTopology.Points, 0);
            m.SetUVs(0, uvs);

            m.RecalculateBounds();
            m.RecalculateNormals();


            _meshFilter.mesh = m;
            _meshRenderer.material = material;
        }
    }
}