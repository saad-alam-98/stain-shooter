using UnityEngine;

public class ClothComponent : MonoBehaviour
{
    public GameObject ContainerClean;
    public GameObject ContainerStain;

    private float moveSpeed = 7f;

    public void Setup()
    {
        transform.localPosition = new Vector2(10, 0);
        ContainerClean.SetActive(false);
        ContainerStain.SetActive(true);
        
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameController.isGameComplete) return;

        transform.localPosition += moveSpeed * Time.deltaTime * Vector3.left;

        if (transform.localPosition.x < -10)
        {
            Dispose();
            return;
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bubble")
        {
            SoundManager.instance.OnHit();
            ContainerClean.SetActive(true);
            ContainerStain.SetActive(false);
        }
    }

    void Dispose()
    {
        gameObject.SetActive(false);
    }
}
