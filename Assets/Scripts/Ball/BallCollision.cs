using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        // Check if the collided object is on the "Enemy" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Destroy(gameObject);
            EnemyManager.Instance.HitByBall();
        }
        SoundManager.PlaySound(SoundManager.Sound.BallHit);
    }

}
