using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.Collections;
using Fougerite;

namespace LootSpawner
{
    public class Loot : MonoBehaviour
    {
        internal bool LoadupLootEnabled = true;

        void Start()
        {
            StartCoroutine(LoadupLoot());
        }

        // Based on Salva's idea.
        IEnumerator LoadupLoot()
        {
            foreach (var x in LootSpawner.LootPositions.Keys)
            {
                if (LoadupLootEnabled)
                {
                    var obj = Util.GetUtil().FindClosestEntity(x, 1.5f);
                    if (obj != null && obj.Object is LootableObject lootableObject)
                    {
                        Util.GetUtil().DestroyObject(lootableObject.gameObject);
                    }
                    
                    yield return new WaitForSeconds(1);
                    
                    Vector3 tempvector = x;
                    tempvector.y -= 1.6f;
                    World.GetWorld().Spawn(LootSpawner.GetPrefab(LootSpawner.LootPositions[x]), tempvector);
                }
            }
            
            if (LootSpawner.Announce && LoadupLootEnabled)
            {
                Fougerite.Server.GetServer().Broadcast(LootSpawner.orange + LootSpawner.AnnounceMSG);
            }
            
            yield return new WaitForSeconds(LootSpawner.Time * 60);
            StartCoroutine(LoadupLoot());
        }

        public void SpawnLootsMono()
        {
            StartCoroutine(SpawnLootsMonoIE());
        }
		
        IEnumerator SpawnLootsMonoIE()
        {
            foreach (var xx in LootSpawner.LootPositions.Keys)
            {
                var obj = Util.GetUtil().FindClosestEntity(xx, 1.5f);
                if (obj != null && obj.Object is LootableObject lootableObject)
                {
                    Util.GetUtil().DestroyObject(lootableObject.gameObject);
                }
            }
            
            foreach (Vector3 x in LootSpawner.LootPositions.Keys)
            {
                yield return new WaitForSeconds(1);
                Vector3 tempvector = x;
                tempvector.y -= 1.6f;
                World.GetWorld().Spawn(LootSpawner.GetPrefab(LootSpawner.LootPositions[x]), tempvector);
            }
            
            if (LootSpawner.Announce)
            {
                Fougerite.Server.GetServer().Broadcast(LootSpawner.orange + LootSpawner.AnnounceMSG);
            }
        }
    }
}
