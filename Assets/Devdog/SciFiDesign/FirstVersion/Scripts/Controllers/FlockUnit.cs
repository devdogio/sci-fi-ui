using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEngine.UI;

namespace Devdog.SciFiDesign
{
    public class FlockUnit : MonoBehaviour
    {
        public float speedMin = 50;
        public float speedMax = 100f;

        public float speed { get; private set; }


        public float rotationSpeed = 1f;

        protected Vector3 avgHeading;
        protected Vector3 avgPosition;

        protected float neighbourDistance = 20f;
        protected Quaternion aimRotation;

        public FlockGroup parent { get; set; }

        protected void Start()
        {
            speed = Random.Range(speedMin, speedMax);
            neighbourDistance = neighbourDistance * neighbourDistance; // Sqr it for later use

            StartCoroutine(RandomUpdate());
        }

        private IEnumerator RandomUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
                ApplyRules();
            }
        }

        protected void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, Time.deltaTime * rotationSpeed);
        }

        private void ApplyRules()
        {
            Vector3 center = Vector3.zero;
            Vector3 avoid = Vector3.zero;
            float globalSpeed = speedMin;

            int groupSize = 0;
            foreach (var unit in parent.units)
            {
                if (unit == null || unit == transform)
                {
                    continue;
                }

                var distSqr = Vector3.SqrMagnitude(transform.position - unit.transform.position);
                if (distSqr <= neighbourDistance)
                {
                    center += unit.transform.position;
                    groupSize++;

                    if (distSqr < 4f)
                    {
                        avoid += transform.position - unit.transform.position;
                    }

                    speed += globalSpeed + unit.speed;
                }
            }

            if (groupSize > 0)
            {
                center = center / groupSize + (parent.aimGoal.position - transform.position);
                speed = globalSpeed / groupSize;

                var dir = (center + avoid) - transform.position;
                if (dir != Vector3.zero)
                {
                    aimRotation = Quaternion.LookRotation(dir);
                }
            }
        }
    }
}