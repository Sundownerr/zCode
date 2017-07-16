
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTwitch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool Pressed;
    private int count = 0;
    Vector3 startPos;
 

    void Start() {
        startPos = gameObject.transform.localPosition;
    }

    void Update()
    {
        if (Pressed)
        {
            count++;

            if (count < 5)
            {
                gameObject.transform.localPosition += new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
            }

            else
            {
                gameObject.transform.localPosition = startPos;
                count = 0;
            }
        }
        else      
            count = 0;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        gameObject.transform.localPosition = startPos;
    }
}
