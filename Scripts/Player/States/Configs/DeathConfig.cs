using UnityEngine;

namespace LastAndZombies.States
{
    [CreateAssetMenu(fileName = "newState", menuName = "Configs/State/DeathConfig")]
    public class DeathConfig : ScriptableObject
    {
        public float backForce;
    }
}