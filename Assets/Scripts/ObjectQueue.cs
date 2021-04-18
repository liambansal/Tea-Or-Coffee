using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For creating a world space queue of gameObjects.
/// </summary>
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

	/// <summary>
	/// Adds a gameObject to the queue.
	/// </summary>
	/// <param name="queueObject"> GameObject to add to queue. </param>
	/// <param name="fillBacklog"> Should the backlog be filled with the gameObject if the queue is full? </param>
	/// <returns></returns>
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

	/// <summary>
	/// Sets the queue direction.
	/// </summary>
	private void Start() {
		queueDirection = transform.forward;
	}

	/// <summary>
	/// Refreshes the queue position for each element.
	/// Must update the gameObject's actual position separately.
	/// </summary>
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

			// Try emptying as many backlogged objects as possible.
			for (int i = 0; i < emptyQueuePositions; ++i, --emptyQueuePositions, iterator = queueBacklog.First) {
				if (AddToQueue(queueBacklog.First.Value, true)) {
					queueBacklog.Remove(iterator);
				}
			}
		}
	}
}
