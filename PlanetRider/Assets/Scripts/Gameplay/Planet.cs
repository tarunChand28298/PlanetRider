using UnityEngine;

public class Planet : MonoBehaviour
{
    public float rotationSpeed;
    public bool timed = false;

    public ParticleSystem okParticle;
    public ParticleSystem goodParticle;

    void Start()
    {
    }

    public void StartTimer()
    {
        if (timed)
        {
            LeanTween.scale(gameObject, Vector3.one * 0.95f, 0.25f).setLoopPingPong();
            Invoke(nameof(Implode), 5);
        }
    }

    public void PlayParticles(float landAngle)
    {
        if (Mathf.Abs(landAngle) < 15.0f)
        {
            goodParticle.Play();
            AudioManager.instance.Play("goodLand");
        }

        okParticle.Play();
        AudioManager.instance.Play("land");
    }

    public void Implode()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseOutCubic().setOnComplete(() => { Destroy(gameObject); });
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
    }
}
