using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnHit : MonoBehaviour {

	[Range(0,1)] public float spawnChance = 0.1f;
    [Range(0, 1)] public float spawnDistance = 0.5f;

	private HealthController health;
	private int numSpawns;

    [System.Serializable]
	public class Spawnable {
		public string name;
		public float weight;
		public int maxSpawnCount;
	}
	public Spawnable[] spawnables;

	void Awake () {
		health = GetComponent<HealthController>();
	}

	void OnEnable () {
//		health.onHealthChanged += Spawn;
	}

	void OnDisable () {
//		health.onHealthChanged -= Spawn;
	}

void Spawn () {
		float sum = 0;
		foreach (Spawnable spawnable in spawnables) {
			sum += spawnable.weight;
		}

		float rand = Random.Range(0, sum);
		sum = 0;
		foreach (Spawnable spawnable in spawnables) {
			//do some calculations to have number of presents spawned be based on healthChange.
			sum += spawnable.weight;
			if (rand < sum) {
				for (int i = 0; i < Random.Range(1, spawnable.maxSpawnCount); i++) {
					GameObject go = Spawner.Spawn(spawnable.name);
					go.transform.position = transform.position + (Vector3)(Random.insideUnitCircle * spawnDistance);
				}
				break;
			}
		}
	}
}
