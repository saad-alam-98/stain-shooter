using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector2 tapPosition;

            if (Input.touchCount > 0)
            {
                tapPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            RaycastHit2D hit = Physics2D.Raycast(tapPosition, Vector2.zero);

            if (hit.collider != null)
            {
               
                Debug.Log($"Tapped on: {hit.collider.gameObject.name}");

                OnTap(hit.collider.gameObject);
            }
        }
    }

    void OnTap(GameObject tappedObject)
    {
        Debug.Log($"You tapped on: {tappedObject.name}");
    }
}
