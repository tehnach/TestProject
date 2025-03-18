using UnityEngine;
using UnityEngine.EventSystems;

public class GameArea : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public static GameArea instance;

    Camera mainCamera;

    private float halfWidth;
    private float halfHeight;
    private float leftScreenBorderX;
    private float rightScreenBorderX;

    private Vector3 touchShift;

    public float HalfWidth
    {
        get { return halfWidth; } 
    }

    public float HalfHeight
    {
        get { return halfHeight; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        CalculateScreenBorders();
        CalculateSize();
    }

    private void CalculateScreenBorders()
    {
        rightScreenBorderX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        leftScreenBorderX = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    private void CalculateSize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Bounds bounds = spriteRenderer.bounds;            
            halfWidth = bounds.size.x / 2f;
            halfHeight = bounds.size.y / 2f;
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        touchShift = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(eventData.position) + touchShift;
        SetPositionX(newPos.x);
    }

    private void SetPositionX(float newX)
    {
        float x = newX;

        if (x + halfWidth < rightScreenBorderX)
        {
            x = rightScreenBorderX - halfWidth;
        }
        else if (x - halfWidth > leftScreenBorderX)
        {
            x = leftScreenBorderX + halfWidth;
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

}
