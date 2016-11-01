using UnityEngine;
using System.Collections;

public class InitBarrier : MonoBehaviour {

    void OnEnable() {
        BallDodge.onDodgeBegan += DisableCollision;
        BallDodge.onDodgeEnd += EnableCollision;
    }

    void OnDisable() {
        BallDodge.onDodgeBegan -= DisableCollision;
        BallDodge.onDodgeEnd -= EnableCollision;
    }

    void DisableCollision() {
        GetComponent<Collider>().enabled = false;
    }

    void EnableCollision() {
        GetComponent<Collider>().enabled = true;
    }
}
