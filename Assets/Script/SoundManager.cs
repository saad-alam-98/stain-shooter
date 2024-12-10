using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource BubbleHit;
    public AudioSource GameWon;
    public AudioSource BgSource;
    public AudioSource Washing;
    public AudioSource Shoot;

    private void Awake()
    {
        if (instance == null)
        {
            BgSource.Play();
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(instance);
        }
    }

    public void OnHit()
    {
        BubbleHit.Play();
    }

    public void OnGameWon()
    {
        GameWon.Play();
    }

    public void OnWashing()
    {
        Washing.Play();
    }
    
    public void OnShoot()
    {
        Shoot.Play();
    }
}
