using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StampSpawner : MonoBehaviour {
    public GameObject stampPrefab;

    public void SpawnStamp(float delay = 1f) {
        Invoke(nameof(ResetStamp), delay);
    }

    private void ResetStamp() {
        Stamp stamp = FindObjectOfType<Stamp>();

        if (stamp != null) {
            stamp.ResetStamp();
        }
        else {
            Instantiate(stampPrefab, transform);
        }
    }
}