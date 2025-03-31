using System.Collections;
using Model.Stats;
using UnityEngine;

namespace Model.Combat
{
    public class GunScriptableObject : MonoBehaviour
    {
        [Header("Applier")]
        public GunUser gunUser;

        public RobotInGameStats robotInGameStats = null;
        
        [Header("Guns Config")]
        public GunType Type;
        public string name;
        // public GameObject modelPrefab;
        // public Vector3 spawnPosition;
        // public Vector3 spawnRotation;

        public DamageConfigScriptableObject damageConfig;
        public ShootConfigScriptableObject shootConfig;
        public TrailConfigScriptableObject trailConfig;
        public AudioConfigScriptableObject audioConfig;

        public GameObject surfaceEffectPrefab;
    
        private GameObject model;
        private float lastShotTime;
        private ParticleManager shootParticleSystem;
        private UnityEngine.Pool.ObjectPool<TrailRenderer> trailPool;
        private AudioSource shootingAudioSource;

        // public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        // {
            
        //     // lastShotTime = 0; // in editor this will not br properly reset, in build it's fine 
        //     // trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
        
        //     // model = Instantiate(modelPrefab);
        //     // model.transform.SetParent(parent, false);
        //     // model.transform.localPosition = spawnPosition;
        //     // model.transform.localRotation = Quaternion.Euler(spawnRotation);

        //     // shootParticleSystem = model.GetComponentInChildren<ParticleSystem>();
        // }
    
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
            if (shootParticleSystem == null)
            {
                trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
                shootParticleSystem = GetComponentInChildren<ParticleManager>();
                shootingAudioSource = GetComponent<AudioSource>();
            }

            if (Time.time > shootConfig.fireRate + lastShotTime)
            {
                lastShotTime = Time.time;
                shootParticleSystem.PlayParticles();
                audioConfig.PlayShootingClip(shootingAudioSource);
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
                    StartCoroutine(
                        PlayTrail(
                            shootParticleSystem.transform.position,
                            hit.point,
                            hit
                        )
                    );
                }

                else
                {
                    StartCoroutine(
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
                // Debug.Log("Hit: " + hit.collider.name);
                GameObject effect = Instantiate(surfaceEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                effect.gameObject.GetComponent<ParticleManager>().PlayParticles();
                Destroy(effect, 1f);
                if (hit.collider.TryGetComponent(out IDamageable damageable1) && gunUser == GunUser.Player)
                {
                    damageable1.TakeDamage(damageConfig.GetDamage() * (int)robotInGameStats.data.DamageMultiplier);  
                }
                
                else if (hit.collider.TryGetComponent(out IDamageable damageable2) && gunUser == GunUser.AI)
                {
                    damageable2.TakeDamage(damageConfig.GetDamage());  
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

    public enum GunUser
    {
        Player,
        AI
    }
}