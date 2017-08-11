using UnityEngine;

public class CardsHover : MonoBehaviour
{
    public Vector3 startPos;

    private Vector3 clickPos, cardScaleVector, mousePos, screenParams;
    private bool isDragged = false, isMouseOver = false, canDrag = false;
    private int cardScale, zValue;

    private void Start()
    {
        cardScaleVector = new Vector3(0.5f, 0.5f, 0f) * 25;
        screenParams = new Vector3(Screen.width / 2, Screen.height / 2);
        zValue = 5;
    }

    private void Update()
    {
        mousePos = Input.mousePosition - screenParams;

        if (!isDragged & !isMouseOver)
        {
            bool onStartPos = Vector3.Distance(transform.localPosition, startPos) <= 4;

            if (!onStartPos)
            {               
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 6);
                canDrag = false;
            } else
            {
                canDrag = true;
            }         

            if (!isMouseOver & cardScale > 0)
            {
                Vector3 downwardVector = new Vector3(0f, 5f, 0f);
                transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale - cardScaleVector, Time.deltaTime * 9);
                transform.localPosition -= downwardVector;

                cardScale -= 2;
            }
        }
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
        
        if (cardScale < 14)
        {
            Vector3 upwardVector = new Vector3(0f, 16f, 0f);            
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + cardScaleVector, Time.deltaTime * 9);
            transform.localPosition += upwardVector;

            cardScale += 2;
        }
    }

    private void OnMouseDrag()
    {
        isMouseOver = true;
     
        Vector3 lerp = Vector3.Lerp(transform.localPosition, mousePos, Time.deltaTime * 9);
        lerp.Set(lerp.x, lerp.y, transform.localPosition.z - zValue);

        transform.localPosition = lerp;
    }

    private void OnMouseDown()
    {
        clickPos = mousePos;       
      
        isDragged = true;
    }

    private void OnMouseUp()
    {
        isDragged = false;
    }

    private void OnMouseEnter()
    {    
        isMouseOver = true;
        transform.localPosition -= new Vector3(0, 0, transform.position.z + zValue);
    }

    private void OnMouseExit()
    {
        //transform.localPosition += new Vector3(0, 0, zValue);
        isMouseOver = false;
    }
}