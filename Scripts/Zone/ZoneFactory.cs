using System.Collections.Generic;
using UnityEngine;

namespace LastAndZombies
{
    public class ZoneFactory : MonoBehaviour
    {
        public BossZone BossZone { get; private set; }
        
        public Transform PlayerStart;
        [SerializeField] private Transform _spawnStart;
        
        [Header("ZONES SETTINGS")]
        [SerializeField] private int zones;
        [SerializeField] private List<Zone> _deffaultZones = new List<Zone>();
        [SerializeField] private List<Zone> _relaxZones = new List<Zone>();
        [SerializeField] private List<Zone> _transitionZones = new List<Zone>();

        [SerializeField] private BossZone _bossZone;

        public void CreateRoad()
        {
            Vector3 spawn = _spawnStart.position;
            Zone lastZone = null;
            for (int i = 0; i < zones; i++)
            {
                if (i % 2 > 0 && i > 0)
                {
                    Create(_deffaultZones, lastZone, ref spawn);
                    Create(_transitionZones, lastZone, ref spawn);
                    Create(_deffaultZones, lastZone, ref spawn);
                    Create(_relaxZones, lastZone, ref spawn);
                }
                else
                {
                    Create(_deffaultZones, lastZone, ref spawn);
                    Create(_deffaultZones, lastZone, ref spawn);
                    Create(_relaxZones, lastZone, ref spawn);
                }
            }
            
            Create(_transitionZones, lastZone, ref spawn);
            
            BossZone = Instantiate(_bossZone, transform);
            BossZone.transform.position = spawn - BossZone._startZone.position;
        }

        private void Create(List<Zone> zones, Zone lastZone, ref Vector3 spawn)
        {
            int index = Random.Range(0, zones.Count);
            lastZone = Instantiate(zones[index], transform);
            lastZone.transform.position = spawn - lastZone._startZone.position;
            spawn = lastZone._endZone.position;

            if (zones.Count > 1)
                zones.Remove(zones[index]);
        }
    }
}