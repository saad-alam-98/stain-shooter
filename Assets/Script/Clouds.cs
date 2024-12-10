using UnityEngine;

public class Clouds : MonoBehaviour
{
    private float moveSpeed = 5f;

    public void Setup()
    {
        transform.localPosition = new Vector2(-11, Random.Range(1.5f,8.5f));
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.localPosition += moveSpeed * Time.deltaTime * Vector3.right;

        if (transform.localPosition.x > 11)
        {
            Dispose();
            return;
        }
    }

    void Dispose()
    {
        gameObject.SetActive(false);
    }
}
