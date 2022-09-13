using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
	public float velocityThreshold = 0.01f;
	public float sliceForce = 5f;

	private Camera _mainCamera;
	private bool _slicing;
	private Collider _collider;
	private TrailRenderer _trail;

	public Vector3 Direction { get; private set; }

	private void Awake()
	{
		_collider = GetComponent<Collider>();
		_trail = GetComponentInChildren<TrailRenderer>();
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			StartSlicing();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			StopSlicing();
		} 
		else if (_slicing)
		{
			ContinueSlicing();
		}
	}

	private void StartSlicing()
	{
		Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
		newPosition.z = 0f;
		transform.position = newPosition;
		_slicing = true;
		_trail.Clear();
	}

	private void StopSlicing()
	{
		_slicing = false;
		_collider.enabled = false;
	}

	private void ContinueSlicing()
	{
		Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
		newPosition.z = 0f;
		Direction = newPosition - transform.position;
		float speed = Direction.magnitude / Time.deltaTime;
		_collider.enabled = speed > velocityThreshold;
		transform.position = newPosition;
	}

	private void OnDisable()
	{
		StopSlicing();
	}
}
