using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
	/// <summary>
	/// The worlds-space direction of the queue.
	/// </summary>
	public Vector3 queueDirection { get; private set; } = Vector3.zero;
	
	public static LinkedList<GameObject> Queue { get; private set; } = new LinkedList<GameObject>();

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

	/// <summary>
	/// Removes customers from the queue if they have been served and re-positions the customers.
	/// </summary>
	protected void UpdateQueue() {
		LinkedListNode<GameObject> queueNode = Queue.First;

		// Removes served customers from the queue.
		for (int iterator = 0; iterator < Queue.Count; ++iterator) {
			if (!queueNode.Value.GetComponent<Customer>().OpenOrder) {
				Queue.Remove(queueNode);
				// Restart the queue search.
				queueNode = Queue.First;
				iterator = -1;
				continue;
			}

			if (queueNode != Queue.Last) {
				queueNode = queueNode.Next;
			}
		}

		queueNode = Queue.First;

		// Re-positions the remaining customers.
		for (int iterator = 0; iterator < Queue.Count; ++iterator) {
			queueNode.Value.gameObject.GetComponent<Customer>().SetQueuePosition();

			if (queueNode != Queue.Last) {
				queueNode = queueNode.Next;
			}
		}
	}

	private void Awake() {
		queueDirection = queueStart.position - (queueStart.position - queueDirect);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			Queue.AddLast(Instantiate(customer, spawn.position, Quaternion.identity));
		}
	}
}
