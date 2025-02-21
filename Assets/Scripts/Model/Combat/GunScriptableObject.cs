using System.Collections;
using UnityEngine;

namespace Model.Combat
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Our Scriptable Object/Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject
    {
        public GunType Type;
        public string name;
        public GameObject modelPrefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;

        public DamageConfigScriptableObject damageConfig;
        public ShootConfigScriptableObject shootConfig;
        public TrailConfigScriptableObject trailConfig;
    
        private MonoBehaviour activeMonoBehaviour;
        private GameObject model;
        private float lastShotTime;
        private ParticleSystem shootParticleSystem;
        private UnityEngine.Pool.ObjectPool<TrailRenderer> trailPool;

        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            this.activeMonoBehaviour = activeMonoBehaviour;
            lastShotTime = 0; // in editor this will not br properly reset, in build it's fine 
            trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
        
            model = Instantiate(modelPrefab);
            model.transform.SetParent(parent, false);
            model.transform.localPosition = spawnPosition;
            model.transform.localRotation = Quaternion.Euler(spawnRotation);

            shootParticleSystem = model.GetComponentInChildren<ParticleSystem>();
        }
    
        private TrailRenderer CreateTrail()
        {
            GameObject instance = new GameObject("Bullet Trail");
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = trailConfig.color;
            trail.material = trailConfig.trailMaterial;
            trail.widthCurve = trailConfig.widthCurve;
            trail.time = trailConfig.Duration;
            trail.minVertexDistance = trailConfig.MinVertexDistance;

            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
            return trail;
        }
    
        public void Shoot()
        {
            if (Time.time > shootConfig.fireRate + lastShotTime)
            {
                lastShotTime = Time.time;
                shootParticleSystem.Play();
                Vector3 shootDirection = shootParticleSystem.transform.forward +
                                         new Vector3(
                                             Random.Range(
                                                 -shootConfig.Spread.x,
                                                 shootConfig.Spread.x
                                             ),
                                             Random.Range(
                                                 -shootConfig.Spread.y,
                                                 shootConfig.Spread.y
                                             ),
                                             Random.Range(
                                                 -shootConfig.Spread.z,
                                                 shootConfig.Spread.z)
                                         );
                shootDirection.Normalize();

                if (Physics.Raycast(
                        shootParticleSystem.transform.position,
                        shootDirection,
                        out RaycastHit hit,
                        float.MaxValue,
                        shootConfig.hitLayerMask
                    )
                   )
                {
                    activeMonoBehaviour.StartCoroutine(
                        PlayTrail(
                            shootParticleSystem.transform.position,
                            hit.point,
                            hit
                        )
                    );
                }

                else
                {
                    activeMonoBehaviour.StartCoroutine(
                        PlayTrail(
                            shootParticleSystem.transform.position,
                            shootParticleSystem.transform.position + (shootDirection * trailConfig.missDistance),
                            new RaycastHit()
                        )
                    );
                }
            }
        }

        private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
        {
            TrailRenderer instance = trailPool.Get();
            instance.gameObject.SetActive(true);
            instance.transform.position = startPoint;
            yield return null; // avoid position carry-over from last frame if reused
        
            instance.emitting = true;
        
            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;
        
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
                remainingDistance -= trailConfig.SimulationSpeed * Time.deltaTime;
            
                yield return null;
            }
        
            instance.transform.position = endPoint;

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageConfig.GetDamage());
                }
            }
            yield return new WaitForSeconds(trailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            trailPool.Release(instance);
        }
    
    
    }

    public enum GunType
    {
        Kinetic,
        Energy
    }
}