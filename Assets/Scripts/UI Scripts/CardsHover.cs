using UnityEngine;

public class CardsHover : MonoBehaviour
{
    public Vector3 startPos, cardScaleVector;

    private bool isDragged = false, isMouseOver = false, canDrag = false;
    private int cardScale, zValue;
    private BoxCollider2D box2d;
    private Rigidbody2D rig;

    void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        cardScaleVector = new Vector3(0.5f, 0.5f, 0f) * 25;
        zValue = 5;
    }

    void Update()
    {
        if (!isDragged)
        {
            bool onStartPos = Vector3.Distance(transform.localPosition, startPos) <= 4;

            if (!onStartPos)
            {
                canDrag = false;
                transform.localPosition = Vector2.Lerp(transform.localPosition, startPos, Time.deltaTime * 6);
            }
            else
            {
                canDrag = true;
            }

            if (!isMouseOver & !canDrag & cardScale > 0)
            {
                cardScale -= 2;
                transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale - cardScaleVector, Time.deltaTime * 9);
                transform.localPosition -= new Vector3(0f, 5f);
            }
        }
    }

    private void OnMouseOver()
    {
        if (cardScale < 13)
        {
            cardScale += 2;
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + cardScaleVector, Time.deltaTime * 9);
            transform.localPosition += new Vector3(0f, 16f);
        }
    }

    private void OnMouseDrag()
    {
        isMouseOver = true;

        Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
        Vector3 lerp = Vector3.Lerp(transform.localPosition, mousePos, Time.deltaTime * 9);
        lerp.Set(lerp.x, lerp.y, -zValue);

        transform.localPosition = lerp;
    }

    private void OnMouseDown()
    {
        isDragged = true;
    }

    private void OnMouseUp()
    {
        isDragged = false;
    }

    private void OnMouseEnter()
    {
        transform.localPosition -= new Vector3(0, 0, zValue);
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        transform.localPosition += new Vector3(0, 0, zValue);
        isMouseOver = false;
    }
}