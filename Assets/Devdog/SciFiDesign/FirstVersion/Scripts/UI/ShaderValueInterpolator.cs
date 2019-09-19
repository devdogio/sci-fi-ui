using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Devdog.General.UI;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Devdog.SciFiDesign.UI
{
    public class ShaderValueInterpolator : MonoBehaviour
    {
        [System.Serializable]
        public class GraphicRow
        {
            public Graphic graphic;
            public float waitTime;
            public float intensityFactor = 1f;
        }

        public string shaderValue = "";

        public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 0f, 0f));
        public float interpSpeed = 1f;

        public Material material;
        public bool createMaterialCopy;
        public GraphicRow[] graphics = new GraphicRow[0];

        public bool setMaterialToNull = true;

        private int _shaderValueID;
        private int _shaderNormalizedTimeID;
        private UIWindow _window;


        public UnityEvent onAnimate = new UnityEvent();


        protected void Awake()
        {
            _shaderValueID = Shader.PropertyToID(shaderValue);
            _shaderNormalizedTimeID = Shader.PropertyToID("_NormalizedTime");
            if (createMaterialCopy)
            {
                material = new Material(material);
                material.name += "(copy)";
            }

            _window = GetComponent<UIWindow>();
            if (_window == null)
            {
                _window = GetComponentInParent<UIWindow>();
            }

            if (_window != null)
            {
                _window.OnHide += WindowOnOnHide;
            }
        }

        private void WindowOnOnHide()
        {
            StopAllCoroutines();
            if (setMaterialToNull)
            {
                foreach (var row in graphics)
                {
                    if (row.graphic == null)
                        continue;

                    row.graphic.material = null;
                }
            }
        }

        protected void Start()
        {
            DoAnimations();
        }

        public void DoAnimations()
        {
            if (isActiveAndEnabled)
            {
                StartCoroutine(_DoAnimations());
            }
        }

        private IEnumerator _DoAnimations()
        {
            for (int i = 0; i < graphics.Length; i++)
            {
                StartCoroutine(Adder(graphics[i]));

                // Avoid hickups
                if (i % 20 == 0)
                    yield return null;
            }

            onAnimate.Invoke();
        }

        private IEnumerator Adder(GraphicRow row)
        {
            if (row.graphic != null && row.graphic.gameObject.activeInHierarchy)
            {
                if (row.waitTime > 0f)
                {
                    row.graphic.material = new Material(material);
                    yield return new WaitForSeconds(row.waitTime);
                }
                else
                {
                    row.graphic.material = material;
                }

                StartCoroutine(Interpolator(row));
            }
        }

        private IEnumerator Interpolator(GraphicRow row)
        {
            float maxTime = curve.keys[curve.length - 1].time + row.waitTime;
            float valPrevious = 0f;
            float time = 0f;
            while (time < maxTime)
            {
                time += Time.deltaTime * interpSpeed;
                time = Mathf.Min(maxTime, time);

                var val = curve.Evaluate(time) * row.intensityFactor;
                if (Mathf.Approximately(val, valPrevious) == false)
                {
                    row.graphic.materialForRendering.SetFloat(_shaderValueID, val);
                    row.graphic.materialForRendering.SetFloat(_shaderNormalizedTimeID, time / maxTime * row.graphic.color.a);
                }

                valPrevious = val;

                yield return null;
            }

            if (setMaterialToNull)
            {
                row.graphic.material = null;
            }
        }
    }
}