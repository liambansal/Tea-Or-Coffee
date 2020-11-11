using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
	/// <summary>
	/// The worlds-space direction of the queue.
	/// </summary>
	public Vector3 queueDirection { get; private set; } = Vector3.zero;
	
	public LinkedList<GameObject> queue { get; private set; } = new LinkedList<GameObject>();

	public Transform QueueStart { get { return queueStart; } private set { } }

	/// <summary>
	/// Sets the direction for the queue.
	/// </summary>
	[SerializeField]
	private Vector3 queueDirect = Vector3.back;

	[SerializeField]
	private Transform spawn = null;
	[SerializeField]
	private Transform queueStart = null;

	[SerializeField]
	private GameObject customer = null;

	private void Awake() {
		queueDirection = queueStart.position - (queueStart.position - queueDirect);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			queue.AddLast(Instantiate(customer, spawn.position, Quaternion.identity));
		}
	}
}
