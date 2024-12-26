using UnityEngine;

public class DifferenceObject : MonoBehaviour
{
    [SerializeField] Collider2D diffCol;
    [SerializeField] DifferenceObject counterpart;

    [SerializeField] Sprite sprite1 = null, sprite2 = null;

    [SerializeField] SpriteRenderer spriteRenderer;

    DifferenceManager manager;

    void Start()
    {
        if (!spriteRenderer) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if (!manager) { manager = FindObjectOfType<DifferenceManager>(); }
    }

    public void InitializeDifference(float distance)
    {
        // Duplicate the object with the same script and collider.
        DifferenceObject diffObj = Instantiate(gameObject).GetComponent<DifferenceObject>();

        // Move it to the appropriate position.
        diffObj.transform.position = transform.position + new Vector3(distance, 0, 0);

        // Give both this and the counterpart a parent object.
        Transform differenceParent = new GameObject(gameObject.name).transform;
        differenceParent.parent = transform.parent;

        transform.parent = differenceParent;
        diffObj.transform.parent = differenceParent;

        // Set the counterparts to each other.
        diffObj.SetCounterpart(this);
        SetCounterpart(diffObj);

        // Set the sprite counterparts too.
        SetSprite(sprite1);
        diffObj.SetSprite(sprite2);
    }

    void Update()
    {
        // Handle touch input.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
    }

    void OnMouseDown()
    {
        HandleInput(Input.mousePosition);
    }

    void HandleInput(Vector3 inputPosition)
    {
        // Convert input position to world point.
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider == diffCol)
        {
            manager.ResolveDifference(this);

            // Update the counterpart's sprite.
            counterpart.SetSprite(spriteRenderer.sprite);

            // Destroy the collider and this object.
            Destroy(diffCol);
            Destroy(this);
        }
    }

    public void SetCounterpart(DifferenceObject diffObj) { counterpart = diffObj; }
    public void SetSprite(Sprite sprite) { spriteRenderer.sprite = sprite; }

    public DifferenceObject GetCounterpart() { return counterpart; }

    public Collider2D GetCollider() { return diffCol; }
}
