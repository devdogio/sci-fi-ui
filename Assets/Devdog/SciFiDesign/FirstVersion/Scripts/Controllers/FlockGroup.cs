using System;
using UnityEngine;
using System.Collections;
using Devdog.General;
using Random = UnityEngine.Random;

namespace Devdog.SciFiDesign
{
    public class FlockGroup : MonoBehaviour
    {
        [Required]
        [SerializeField]
        public Transform goals;

        [Required]
        [SerializeField]
        private Transform _units;

        [NonSerialized]
        public FlockUnit[] units = new FlockUnit[0];

        public Transform aimGoal { get; private set; }

        protected void Awake()
        {
            units = _units.GetComponentsInChildren<FlockUnit>(true);
            foreach (var unit in units)
            {
                unit.parent = this;
            }

            StartCoroutine(RandomUpdate());
        }

        private IEnumerator RandomUpdate()
        {
            while (true)
            {
                PickRandomAimPosition();
                yield return new WaitForSeconds(Random.Range(1f, 2f));
            }
        }

        private void PickRandomAimPosition()
        {
            aimGoal = goals.GetChild(Random.Range(0, goals.childCount));
        }
    }
}