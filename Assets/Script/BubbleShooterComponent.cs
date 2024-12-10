using UnityEngine;
using DG.Tweening;

public class BubbleShooterComponent : MonoBehaviour
{
    public float shootSpeed = 10.0f;
    public float rotationSpeed = 50.0f;
    private bool rotatingForward = true;
    private float currentAngle = 0.0f;

    public BubbleComponent presetBubble;
    public Transform shooter;
    public Transform shootPoint;
    public Transform container;

    private ObjectPooler<BubbleComponent> mPooler;
    private ObjectPooler<BubbleComponent> Pooler
    {
        get
        {
            if (mPooler == null)
            {
                mPooler = new ObjectPooler<BubbleComponent>(presetBubble, container, 20);
            }
            return mPooler;
        }
    }

    private float shootCooldown = 0.85f;
    private float rotationCooldown = 0.75f;
    private float nextShootTime = 0f;
    private bool isPaused = false;

    private void Start()
    {
        Pooler.ClearAllActiveObjects();
    }

    void Update()
    {
        if (!GameController.isGameRunning || GameController.isGameComplete)
            return;


        if (!isPaused)
        {
            float targetAngle = rotatingForward ? -40 : 40;
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            shooter.rotation = Quaternion.Euler(0, 0, currentAngle);

            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                rotatingForward = !rotatingForward;
            }
        }

        //if (IsTapDetected() && Time.time >= nextShootTime)
        //{
        //    ShootBall();
        //    PauseRotationForCooldown();
        //    PlayShootEffect();
        //    nextShootTime = Time.time + shootCooldown;
        //}

        if (IsTapDetected() && Time.time >= nextShootTime)
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
                ShootBall();
                PauseRotationForCooldown();
                PlayShootEffect();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    void ShootBall()
    {
        if (presetBubble == null || shootPoint == null)
        {
            return;
        }

        var bubble = Pooler.GetPooledObject();
        bubble.Setup(
            shootPoint.position,
            () =>
            {
                Pooler.ReturnToPool(bubble);
            },
            shootPoint.up * shootSpeed
        );
    }

    void PauseRotationForCooldown()
    {
        isPaused = true;
        Invoke(nameof(ResumeRotation), rotationCooldown);
    }

    void ResumeRotation()
    {
        isPaused = false;
    }

    void PlayShootEffect()
    {
        SoundManager.instance.OnShoot();

        shooter.DOShakeRotation(
            duration: 0.75f,
            strength: 5f,
            vibrato: 6,
            randomness: 30,
            fadeOut: true
        );

        shooter.DOPunchScale(
            punch: Vector3.one * 0.1f,
            duration: 0.65f,
            vibrato: 6,
            elasticity: 0.7f
        );
    }

    bool IsTapDetected()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) return true;
        return false;
    }
}
