using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.CatchingGame {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    internal class Player : MonoBehaviour {
        [Tooltip("Speed of the player")]
        [SerializeField] private float speed = 100f;

        private Rigidbody2D rb;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            HandleMovement();
        }

        private void HandleMovement() {
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(Mathf.Clamp(horizontalInput * speed, -speed, speed), rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            
        }
    }
}