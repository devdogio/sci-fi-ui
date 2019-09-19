using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.SciFiDesign
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour
    {
        public Vector3 diveTorque;
        public Vector3 rollTorque;

        public float accelerationSpeed = 1f;
        public float maxVelocity = 100f;
        public float maxBoostVelocity = 250f;

        public float maxAngularVelocity = 50f;


        private Rigidbody _rigidbody;
        protected void Awake()
        {
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            _rigidbody.maxAngularVelocity = maxAngularVelocity;
        }

        protected void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _rigidbody.AddRelativeTorque(diveTorque * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                _rigidbody.AddRelativeTorque(-diveTorque * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rigidbody.AddRelativeTorque(rollTorque * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rigidbody.AddRelativeTorque(-rollTorque * Time.deltaTime);
            }

            // Cancel out the centripetal force
            // Forces the ship to go in the direction it's facing ignoring centripetal force
            // (not realistic, but makes ship more controllable).
            var mag = _rigidbody.velocity.magnitude;
            if (mag > 0f)
            {
                var normalizedDir = _rigidbody.velocity / mag;
                var dir = normalizedDir * (mag * 0.95f);
                var fwd = transform.forward * (mag * 0.05f);
                var final = fwd + dir;
                _rigidbody.velocity = final;
            }

            _rigidbody.AddRelativeForce(transform.forward * accelerationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                LimitMaxVelocity(maxBoostVelocity);
            }
            else
            {
                LimitMaxVelocity(maxVelocity);
            }
        }

        // Cancels out the velocity of the object; Not really realistic, but makes the controller easier to handle
        private void LimitMaxVelocity(float max)
        {
            var m = _rigidbody.velocity.magnitude;
            if (m > max)
            {
                var normalizedVelocity = _rigidbody.velocity / m;
                _rigidbody.velocity = normalizedVelocity * max;
            }
        }
    }
}