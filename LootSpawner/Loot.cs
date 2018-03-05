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
        void Start()
        {
            StartCoroutine(LoadupLoot());
        }
        
        // Based on Salva's idea.
        IEnumerator LoadupLoot()
        {
            yield return new WaitForSeconds(120);

            foreach (var x in LootSpawner.LootPositions.Keys)
            {
                var obj = Util.GetUtil().FindClosestEntity(LootSpawner.LootPositions[x], 1.5f);
                if (obj != null && obj.Object is LootableObject)
                {
                    Util.GetUtil().DestroyObject(((LootableObject) obj.Object).gameObject);
                }
                yield return new WaitForSeconds(1);
                World.GetWorld().Spawn(LootSpawner.GetPrefab(x), LootSpawner.LootPositions[x]);
            }
            yield return new WaitForSeconds(LootSpawner.Time * 60); // 20mins * 60 = 1800
            StartCoroutine(LoadupLoot());
        }
    }
}