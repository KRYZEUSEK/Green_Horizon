using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.CatchingGame {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    internal class FallingItem : MonoBehaviour {
        private Rigidbody2D rb;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0f, -CatchingGameManager.Instance.itemSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent(out Player player) == false) { 
                CatchingGameManager.Instance.LostItem();
            }

            Destroy(gameObject);
        }
    }
}