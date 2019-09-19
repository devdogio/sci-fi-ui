using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using UnityEngine.UI;

namespace Devdog.SciFiDesign
{
    [RequireComponent(typeof(AudioSource))]
    public class UnitShooter : MonoBehaviour
    {
        [Required]
        public GameObject laserPrefab;
        public float laserSpeed = 10f;

        public AudioClipInfo audioClip;

        [Required]
        public Transform leftGun;
        [Required]
        public Transform rightGun;

        private AudioSource _audioSource;
        private GameObjectPool _laserPool;
        private WaitForSeconds _waitForSeconds;


        protected void Start()
        {
            _audioSource = gameObject.GetOrAddComponent<AudioSource>();
            _waitForSeconds = new WaitForSeconds(5f);
            _laserPool = new GameObjectPool(laserPrefab, 16);

            StartCoroutine(RandomUpdate());
        }

        private IEnumerator RandomUpdate()
        {
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }

        protected void Update()
        {
            foreach (var inst in _laserPool.activeObjectsList)
            {
                if (inst == null)
                {
                    continue;
                }

                inst.transform.Translate(Vector3.forward * Time.deltaTime * laserSpeed);
            }
        }

        private void Shoot()
        {
            var laser1 = _laserPool.Get();
            var laser2 = _laserPool.Get();

            laser1.transform.position = leftGun.position;
            laser1.transform.rotation = leftGun.rotation;

            laser2.transform.position = rightGun.position;
            laser2.transform.rotation = rightGun.rotation;

            // Little optimization to avoid emitting audio when it can't be heard anyway
            if (Vector3.Distance(transform.position, Camera.main.transform.position) < _audioSource.maxDistance)
            {
                _audioSource.Play(audioClip);
            }

            StartCoroutine(DestroyAfter(laser1));
            StartCoroutine(DestroyAfter(laser2));
        }

        private IEnumerator DestroyAfter(GameObject laser)
        {
            yield return _waitForSeconds;
            _laserPool.Destroy(laser);
        }
    }
}