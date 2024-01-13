using UnityEngine;

namespace LastAndZombies.States
{
    [CreateAssetMenu(fileName = "newState", menuName = "Configs/State/RunConfig")]
    public class RunConfig : ScriptableObject
    {
        public float zSpeed;
        public float xSpeed;
        public float limitX;
        public float rotationAngle;
        public float rotationSpeed;
    }
}