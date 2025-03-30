using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampSpawner : MonoBehaviour {
    public GameObject stampPrefab;

    public void SpawnStamp(float delay = 1f) {
        DragDrop stamp = FindObjectOfType<DragDrop>();

        if (stamp != null) {
            Destroy(stamp.gameObject);
        }

        Invoke(nameof(InstantiateStamp), delay);
    }

    private void InstantiateStamp() {
        Instantiate(stampPrefab, transform);
    }
}