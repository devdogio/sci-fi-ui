using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class PrimitiveShape : Graphic
    {
        public int elementCount = 6;
        public float edgeThickness = 5f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var r = GetPixelAdjustedRect();

            vh.Clear();

            for (int j = 0; j < elementCount *2; j++)
            {
                float val = ((float)j / elementCount) * Mathf.PI * 2;
                var x = Mathf.Cos(val);
                var y = Mathf.Sin(val);

                vh.AddVert(new Vector3(((x * edgeThickness) + x) * r.x, ((y * edgeThickness) + y) * r.y), color, Vector2.zero);
                vh.AddVert(new Vector3(x * r.x, y * r.y), color, Vector2.zero);

                vh.AddTriangle(j, j + 1, j + 2);
            }
        }
    }
}