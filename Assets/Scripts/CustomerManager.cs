using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
	// World space.
	public Vector3 queueDirection { get; private set; } = Vector3.zero;
	
	public LinkedList<GameObject> queue { get; private set; } = new LinkedList<GameObject>();

	public Transform QueueStart { get { return queueStart; } private set { } }

	[SerializeField]
	private Transform spawn = null;
	[SerializeField]
	public Transform queueStart = null;


	[SerializeField]
	private GameObject customer = null;

	private void Awake() {
		queueDirection = queueStart.position - (queueStart.position - Vector3.left);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			queue.AddLast(Instantiate(customer, spawn.position, Quaternion.identity));
		}
	}

	//protected void AddToQueue(Customer customer) {
	//	Queue.Add(customer);	
	//}

	//protected void RemoveFromQueue(GameObject customer) {
	//	if (queue.Contains(customer)) {
	//		queue.Remove(customer);
	//		return;
	//	}
	//}
}
