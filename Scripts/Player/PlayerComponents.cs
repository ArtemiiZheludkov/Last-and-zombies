using System;
using UnityEngine;

namespace LastAndZombies
{
    [Serializable]
    public class PlayerComponents
    {
        [field: SerializeField] public BoxCollider BoxCollider { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        
        [field: SerializeField] public PlayerBagUI Bag { get; private set; }
        [field: SerializeField] public PlayerShooter Shooter { get; private set; }
        
        [HideInInspector] public HashAnimation Animations = new HashAnimation();
        
    }
}