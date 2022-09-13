using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public List<GameObject> fruits = new List<GameObject>();
	public GameObject bombPrefab;
	public float minSpawnDelay = .25f;
	public float maxSpawnDelay = 1f;

	public float minSpawnAngle = -25f;
	public float maxSpawnAngle = 25f;

	public float minSpawnForce = 18f;
	public float maxSpawnForce = 22f;

	public float timeAlive = 5f;
	[Range(0f, 1f)]
	public float bombSpawnChance = 0.05f;

    private BoxCollider _spawnArea = null;
	private Coroutine _spawnCoroutine = null;

	private void Awake()
	{
        _spawnArea = GetComponent<BoxCollider>();
	}

	private void OnEnable()
	{
		_spawnCoroutine = StartCoroutine(Spawn());
	}

	private void OnDisable()
	{
		StopCoroutine(_spawnCoroutine);
	}

	private IEnumerator Spawn()
	{
		yield return new WaitForSeconds(3f);

		while (enabled)
		{
			GameObject prefab = fruits[Random.Range(0, fruits.Count)];

			if (Random.value < bombSpawnChance)
			{
				prefab = bombPrefab;
			}

			Vector3 position = Vector3.zero;
			position.x = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
			position.y = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
			position.z = Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);

			Quaternion rotation = Quaternion.Euler(0f, 0f,
				Random.Range(minSpawnAngle, maxSpawnAngle));
			GameObject instance = Instantiate(prefab, position, rotation);
			Destroy(instance, timeAlive);
			float force = Random.Range(minSpawnForce, maxSpawnForce);
			instance.GetComponent<Rigidbody>().AddForce(instance.transform.up * force, ForceMode.Impulse);
			yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
		}
        yield return null;
	}
}
