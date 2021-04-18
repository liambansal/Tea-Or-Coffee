using System.Collections.Generic;
using UnityEngine;

public class ObjectQueue : MonoBehaviour {
	/// <summary>
	/// Stores a copy of a gameObject with its queue position.
	/// </summary>
	public Dictionary<GameObject, Vector3> Queue { get; private set; } = new Dictionary<GameObject, Vector3>();

	[SerializeField]
	private int maximumQueuePositions = 1;
	[SerializeField]
	private float queueGapDistance = 1.0f;

	private Vector3 queueDirection = Vector3.zero;
	private Vector3 lastQueuePosition = Vector3.zero;

	/// <summary>
	/// Object's that can't be fitted into the queue yet.
	/// </summary>
	private LinkedList<GameObject> queueBacklog = new LinkedList<GameObject>();

	public bool AddToQueue(GameObject queueObject, bool fillBacklog) {
		if (Queue.Count >= maximumQueuePositions) {
			if (fillBacklog) {
				AddToBacklog(queueObject);
			}

			return false;
		}

		if (Queue.Count > 0) {
			lastQueuePosition = lastQueuePosition + queueDirection * queueGapDistance;
		} else {
			lastQueuePosition = transform.position;
		}

		Queue.Add(queueObject, lastQueuePosition);
		return true;
	}

	public void RemoveFromQueue(GameObject queueObject) {
		if (Queue.ContainsKey(queueObject)) {
			Queue.Remove(queueObject);
			RefreshPositions();
			TryEmptyingBacklog();
		}
	}

	public Vector3 GetPosition(GameObject queueObject) {
		Vector3 position;
		Queue.TryGetValue(queueObject, out position);
		return position;
	}

	private void Start() {
		queueDirection = transform.forward;
	}

	private void RefreshPositions() {
		GameObject[] queueCopy = new GameObject[Queue.Count];
		Queue.Keys.CopyTo(queueCopy, 0);
		int ObjectCount = 0;

		foreach (GameObject element in queueCopy) {
			lastQueuePosition = transform.position + queueDirection * queueGapDistance * ObjectCount++;
			Queue[element] = lastQueuePosition;
		}
	}

	private void AddToBacklog(GameObject extraObject) {
		queueBacklog.AddLast(extraObject);
	}

	private void TryEmptyingBacklog() {
		int emptyQueuePositions = maximumQueuePositions - Queue.Count;

		if (queueBacklog.Count > 0 && emptyQueuePositions > 0) {
			LinkedListNode<GameObject> iterator = queueBacklog.First;

			for (int i = 0; i < emptyQueuePositions; ++i, --emptyQueuePositions, iterator = queueBacklog.First) {
				if (AddToQueue(queueBacklog.First.Value, true)) {
					queueBacklog.Remove(iterator);
				}
			}
		}
	}
}
