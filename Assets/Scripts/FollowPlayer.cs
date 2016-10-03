using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Vector3 positionOffset;
    public Vector3 rotation;
    public Transform target;
    
	void Update () {
	    if (target) {
            if (transform.parent != target) {
                transform.parent = target;
            }
            transform.position = target.position + positionOffset;
            transform.localEulerAngles = rotation;
        } else if (transform.parent != null) {
            transform.parent = null;
        }
    }
}
