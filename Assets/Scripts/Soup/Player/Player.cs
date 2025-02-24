using System;
using System.Persistence;
using UnityEngine;

namespace System {
    public class Player : Robots, IDamagable, IBind<PlayerData>
    {
        // Start is called before the first frame update
        [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        [SerializeField] PlayerData data;

        public void Bind(PlayerData data)
        {
            this.data = data;
            this.data.Id = Id;
            transform.position = data.position;
            transform.rotation = data.rotation;
        }

        void Update()
        {
            data.position = transform.position;
            data.rotation = transform.rotation;
        }

    }

    [Serializable]
    public class PlayerData : ISaveable 
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }

        public Vector3 position;
        public Quaternion rotation;
    }
}
