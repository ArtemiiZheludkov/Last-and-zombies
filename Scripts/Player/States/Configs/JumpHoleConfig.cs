using UnityEngine;

namespace LastAndZombies.States
{
    [CreateAssetMenu(fileName = "newState", menuName = "Configs/State/JumpHoleConfig")]
    public class JumpHoleConfig : ScriptableObject
    {
        public float zSpeed;
        public float xSpeed;
        public float limitX;
        public float jumpForce;
    }
}