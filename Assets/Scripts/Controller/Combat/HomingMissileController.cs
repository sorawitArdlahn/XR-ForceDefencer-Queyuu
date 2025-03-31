using System;
using UnityEngine;
using Model;


namespace Controller.Combat
{
    public class HomingMissileController : MonoBehaviour
    {
        private Rigidbody rb;   
        public  Vector3 deviatedPrediction;
        public float speed = 0;

        public ParticleManager destroyEffect;

        public GameObject target;

        [Header("PREDICTION")] 
        [SerializeField] private float _maxDistancePredict = 10;
        [SerializeField] private float _minDistancePredict = 5;
        [SerializeField] private float _maxTimePrediction = 5;


        [Header("DEVIATION")] 
        [SerializeField] private float _deviationAmount = 1;
        [SerializeField] private float _deviationSpeed = 1;

        private Vector3 _standardPrediction;

        [Header("Sound")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip _homingMissileSound;


        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GetComponentInChildren<ParticleSystem>().Play();
            
            Destroy(gameObject, 20f);
        }

        void OnTriggerEnter(Collider other)
        {
            
            destroyEffect.PlayParticles();
            Destroy(gameObject,0.5f);
            if (!other.gameObject.CompareTag("Player") && other.TryGetComponent(out IDamageable damageable))
            {
                audioSource.PlayOneShot(_homingMissileSound);
                // other.GetComponent<Rigidbody>().AddForce(-other.transform.forward * 50f, ForceMode.Impulse);
                damageable.TakeDamage(50);  
            }
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                audioSource.PlayOneShot(_homingMissileSound);
                destroyEffect.PlayParticles();
                Destroy(gameObject,0.5f);
            }
        }

        void FixedUpdate()
        {
            rb.velocity = transform.forward * 50f;
            if (target != null)
            {
                RotateHead();
            }
        }

        public void AssignTarget(GameObject target)
        {
            this.target = target;
            if (target) deviatedPrediction = target.transform.position;
            else deviatedPrediction = transform.forward * 50f;
        }

        private void RotateHead()
        {
            var heading =  deviatedPrediction - transform.position;
            var rotation = Quaternion.LookRotation(heading);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, 50f * Time.deltaTime));
        }
    }
}
