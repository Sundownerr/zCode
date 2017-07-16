
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonTwitch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    private bool Pressed, MouseOver;
    private int posCount;
    Vector3 startPos, startScale, newScale, mousePos;
    private double scaleCount;
       
    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
      
    }

    void Update()
    {     
        if (Pressed | MouseOver)
        {           
            posCount++;
            scaleCount++;
           
            if(scaleCount < 10)
            {
                scaleCount+= 0.01;
                newScale = transform.localScale + new Vector3(0.01f, 0.01f);
            }

            if (posCount < 5)
            {
                transform.localPosition += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));               
            }

            else
            {
                transform.localPosition = startPos;
                transform.localScale = startScale;
                posCount = 0;
            }

            transform.localScale = newScale;
        }
        else
        {
            posCount = 0;

            if (scaleCount > 0 & transform.localScale != startScale)
            {

                scaleCount -= 0.01;
                transform.localScale -= new Vector3(0.01f, 0.01f);
            }           
        }          
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        MouseOver = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        MouseOver = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        transform.localPosition = startPos;
    }
}
