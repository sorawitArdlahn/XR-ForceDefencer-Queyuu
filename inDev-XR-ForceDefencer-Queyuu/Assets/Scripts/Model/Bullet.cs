using System;
using UnityEngine;

namespace Model
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            Destroy(gameObject,lifetime);
        }
    }
}
