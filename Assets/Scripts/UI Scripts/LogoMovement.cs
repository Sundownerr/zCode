using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMovement : MonoBehaviour {


    Vector3 mousePos, currentPos;

	void Start ()
    {
	}
	

	void Update ()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = Vector3.MoveTowards(mousePos, transform.localPosition, 0) * 2 + new Vector3(0, 200, 0);
    }
}