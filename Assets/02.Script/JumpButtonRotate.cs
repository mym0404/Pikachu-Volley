using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButtonRotate : MonoBehaviour {
    private Transform tr;
    public static float rotateSpeed = 1.0f;
	void Start () {
        tr = transform;

	}
	
	void Update () {
      //  tr.Rotate(0 , 0 , rotateSpeed);
	}
}
