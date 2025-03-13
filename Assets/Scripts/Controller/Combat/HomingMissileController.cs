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


        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GetComponentInChildren<ParticleSystem>().Play();
            
            Destroy(gameObject, 20f);
        }

        void OnTriggerEnter(Collider other)
        {
            destroyEffect.PlayParticles();
            Destroy(gameObject,0.2f);
            if (other.TryGetComponent(out IDamageable damageable))
            {
                // other.GetComponent<Rigidbody>().AddForce(-other.transform.forward * 50f, ForceMode.Impulse);
                damageable.TakeDamage(50);  
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
            deviatedPrediction = target.transform.position;
        }

        private void RotateHead()
        {
            var heading =  deviatedPrediction - transform.position;
            var rotation = Quaternion.LookRotation(heading);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, 50f * Time.deltaTime));
        }
    }
}
