using System;
using UnityEngine;
using System.Collections;

public class BubbleComponent : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public float Force = 30;
    private Action returnCallback;
    bool isDestroy;

    public SpriteRenderer spriteComponent;
    public Sprite defaultSprite;

    public void Setup(
        Vector2 position,
        Action returnCallback,
        Vector2 direction
    )
    {
        CurrentTime = 0;
        isDestroy = false;
        spriteComponent.sprite = defaultSprite;
        this.gameObject.SetActive(true);
        animator.SetTrigger("Scale");
        rb.velocity = direction;
        this.returnCallback = returnCallback;
        this.transform.position = position;
    }

    public float DestroyTime = 0.75f;
    private float CurrentTime = 0;
    void Update()
    {
        if (isDestroy)
        {
            return;
        }

        if (CurrentTime > DestroyTime)
        {
            isDestroy = true;
            StartCoroutine(Dispose());
        }
        else
        {
            CurrentTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Cloth")
        {
            GameController.instance.AddScore();
            StartCoroutine(Dispose());
        }
    }

    IEnumerator Dispose()
    {
        animator.SetTrigger("Blast");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        returnCallback.Invoke();
    }
}
