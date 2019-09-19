using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using Random = UnityEngine.Random;


namespace Devodg.SciFiDesign.UI
{
    public class BackgroundMeshGenerator : MonoBehaviour
    {
        public struct Triangle
        {
            public int a, b, c;
        }


        public int vertices = 20;
        public Color startColor;
        public Color endColor;


        public bool debug = false;

        private Material _material;
        private MeshRenderer _renderer;
        protected void Awake()
        {
            _material = new Material(Shader.Find("Devdog/UI/BackgroundMesh"));
            _renderer = GetOrAddComponent<MeshRenderer>();

            var meshFilter = GetOrAddComponent<MeshFilter>();
            meshFilter.mesh = Generate2DVoronoiMesh(new Rect(0, 0, Screen.width, Screen.height), vertices);

            _renderer.material = _material;

#if UNITY_5_4_OR_NEWER
            Random.InitState(DateTime.Now.Second);
#else
            Random.seed = DateTime.Now.Second;
#endif
        }

        private T GetOrAddComponent<T>() where T: UnityEngine.Component
        {
            var t = GetComponent<T>();
            if (t != null)
            {
                return t;
            }

            return gameObject.AddComponent<T>();
        }

        protected Mesh Generate2DVoronoiMesh(Rect inRect, int amount)
        {
            var mesh = new Mesh();
            var vertices = new List<Vector3>(amount + 4); // + 4 for all edges
            vertices.Add(Vector3.zero);
            vertices.Add(Vector3.up * Screen.height);
            vertices.Add(Vector3.right * Screen.width);
            vertices.Add(new Vector3(1f * Screen.width, 1f * Screen.height, 0f));
            for (int i = 0; i < amount - 4; i++)
            {
                var randomPoint = new Vector3(Random.Range(inRect.x, inRect.x + inRect.width),
                                              Random.Range(inRect.y, inRect.y + inRect.height),
                                              0f);

                vertices.Add(randomPoint);
            }

            vertices = vertices.OrderBy(o => o.x).ThenBy(o => o.y).ToList();
            var arr = vertices.ToArray();

            if (debug)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.SetParent(transform);
                    obj.transform.localPosition = vertices[i];
                    obj.transform.localScale = Vector3.one * 10f;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.layer = gameObject.layer;
                }
            }

            // Connect triangles
            DelaunayTriangulation(arr);


            mesh.vertices = arr;
//            mesh.triangles = triangles;
//            mesh.normals = normals;

            mesh.RecalculateNormals();
            return mesh;
        }

        protected void DelaunayTriangulation(Vector3[] v)
        {
            var leftHalfCount = v.Length/2;
            var leftHalf = v.Take(leftHalfCount).ToArray();
            var rightHalf = v.Skip(leftHalfCount).Take(v.Length - leftHalfCount).ToArray();
            if (leftHalf.Length > 3)
            {
                Debug.Log("Delaunay left : " + leftHalf.Length);
                DelaunayTriangulation(leftHalf);
            }
            else
            {
                Debug.Log("<color=green>Triangulate left</color>: " + leftHalf.Length);
            }

            if (rightHalf.Length > 3)
            {
                Debug.Log("Delaunay right : " + leftHalf.Length);
                DelaunayTriangulation(rightHalf);
            }
            else
            {
                Debug.Log("<color=yellow>Triangulate right</color>: " + rightHalf.Length);
            }

            // Merge left half with right half (LL, LR)
            for (int i = 0; i < leftHalf.Length; i++)
            {
                
            }


        }

        protected int GetClosestVertexIndex(Vector3[] vertices, int fromIndex)
        {
            var curPos = vertices[fromIndex];
            int closestIndex = -1;
            float closest = 9999;
            for (int i = 0; i < vertices.Length; i++)
            {
                var dist = Vector3.Distance(vertices[i], curPos);
                if (dist < closest)
                {
                    closest = dist;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

    }
}