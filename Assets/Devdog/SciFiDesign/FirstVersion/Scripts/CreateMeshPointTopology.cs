using System.Collections.Generic;
using System.Linq;
using Devdog.General;
using UnityEngine;

namespace Devdog.SciFiDesign
{
    public class CreateMeshPointTopology : MonoBehaviour
    {

        [Required]
        public GameObject toCopy;

        public MeshTopology meshTopology = MeshTopology.Lines;


        private GameObject _copy;

        private void Awake()
        {
            _copy = Instantiate<GameObject>(toCopy);
            
            var filter = _copy.GetComponent<MeshFilter>();

            const int amount = 50000;
            var mesh = new Mesh();
            var vertices = new List<Vector3>(amount);
            var indices = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                vertices.Add(Random.insideUnitSphere);
                indices[i] = i;
            }

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, meshTopology, 0);

            //            mesh.SetVertices(_meshFilter.sharedMesh.vertices.ToList());
            //            mesh.SetTriangles(_meshFilter.sharedMesh.GetTriangles(0), 0);
            //            mesh.SetIndices(_meshFilter.sharedMesh.GetIndices(0), meshTopology, 0);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            filter.mesh = mesh;

            _copy.transform.SetParent(toCopy.transform.parent);
            _copy.transform.position = toCopy.transform.position;
            _copy.transform.rotation = toCopy.transform.rotation;
            _copy.transform.localScale = toCopy.transform.localScale;
        }
        
    }
}