using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
	public int score = 1;
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody _fruitRigidBody;
	private Collider _fruitcollider;
	private ParticleSystem _juiceParticle;
	private GameManager _gameManager;

	private void Awake()
	{
		_fruitcollider = GetComponent<Collider>();
		_fruitRigidBody = GetComponent<Rigidbody>();
		_juiceParticle = GetComponentInChildren<ParticleSystem>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Blade blade = other.GetComponent<Blade>();
			_juiceParticle.Play();
			GameManager.Instance.IncreaseScore(score);
			Slice(blade.Direction, blade.transform.position, blade.sliceForce);
		}
	}

	private void Slice(Vector3 direction, Vector3 position, float force)
	{
		whole.SetActive(false);
		sliced.SetActive(true);
		_fruitcollider.enabled = false;
		
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

		Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
		
		foreach (var slice in slices)
		{
			slice.velocity = _fruitRigidBody.velocity;
			slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
		}
	}
}
