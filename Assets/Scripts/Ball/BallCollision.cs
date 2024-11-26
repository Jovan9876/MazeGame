using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        // Check if the collided object is on the "Enemy" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            GameManager.Instance.AddScore();
            Destroy(gameObject);
            EnemyManager.Instance.HitByBall();
        }
        SoundManager.PlaySound(SoundManager.Sound.BallHit);
    }

}
