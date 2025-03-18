using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Transform bottom;

    [Header("Parameters")]
    [SerializeField] private float movedownSpeed = 1f;

    private Camera mainCamera;
    
    private Vector3 startLocalScale;
    private Vector3 touchShift;
    private bool isMoveDown;
    private bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;
        startLocalScale = transform.localScale;
    }

   
    private void SetSelected(bool enable)
    {
        if (enable)
        {
            transform.localScale = startLocalScale * 0.15f + startLocalScale;
        }
        else
        {
            transform.localScale = startLocalScale;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isMoveDown)
        {
            eventData.pointerDrag = null;
            return;
        }

        SetSelected(true);

        isDragging = true;
        touchShift = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(eventData.position) + touchShift;
        SetPosition(newPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetSelected(false);

        isDragging = false;
        isMoveDown = true;
    }

    private void SetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    private void Update()
    {
        if (!isDragging && isMoveDown)
        {             
            Vector3 pos = transform.position;
            float newPosY = pos.y - (movedownSpeed * Time.deltaTime);
            transform.position = new Vector3(pos.x, newPosY, pos.z);

            if(CheckSurface())
            {
                isMoveDown = false;
            }
        }
    }

    private bool CheckSurface()
    {
        //Debug.Log(LayerMask.NameToLayer("Surface"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(bottom.position, Vector2.zero);

        if(hits!=null && hits.Length > 0)
        {
            for(int i = 0; i<hits.Length; i ++)
            {
                if (hits[i].collider.CompareTag("Surface"))
                {
                    return true;
                }
            }                
        }

        return false;
    }
}
