using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRocket : MonoBehaviour
{
    public float speed;
    Vector3 launchDirection;
    public GameObject rocketParticles;

    public event Action<Vector3, float> PlayerLanded;
    public event Action PlayerOutOfBounds;

    private Transform targetPlanet;
    public Transform TargetPlanet
    {
        get { return targetPlanet; }
        set 
        { 
            transform.parent = value;
            targetPlanet = value;
        }
    }

    private void OnBecameInvisible()
    {
        PlayerOutOfBounds?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Land(collision.gameObject);
    }

    void Start()
    {
        launchDirection = transform.up;
    }

    void Update()
    {
        if(TargetPlanet == null)
        {
            transform.Translate(launchDirection * Time.deltaTime * speed);
            return;
        }
    }

    public void OnLaunchInputRecieved(InputAction.CallbackContext inputContext)
    {
        if (inputContext.phase == InputActionPhase.Canceled)
        {
            Launch();
        }
    }

    void Land(GameObject planet)
    {
        TargetPlanet = planet.transform;

        float correctionAngle1 = Vector3.SignedAngle( transform.up, (transform.position - targetPlanet.position), Vector3.forward);
        float correctionAngle2 = Vector3.SignedAngle(-transform.up, (transform.position - targetPlanet.position), Vector3.forward);

        float correctionAngle = Mathf.Abs(correctionAngle1) > Mathf.Abs(correctionAngle2) ? correctionAngle2 : correctionAngle1;
        launchDirection = Mathf.Abs(correctionAngle1) > Mathf.Abs(correctionAngle2) ? -Vector3.up : Vector3.up;

        Quaternion targetRotation = Quaternion.AngleAxis(correctionAngle, Vector3.forward) * transform.rotation;
        LeanTween.rotate(gameObject, targetRotation.eulerAngles, 0.05f);

        PlayerLanded?.Invoke(targetPlanet.position, correctionAngle);
        targetPlanet.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        targetPlanet.GetComponent<Planet>().StartTimer();
        targetPlanet.GetComponent<Planet>().PlayParticles(correctionAngle);

        rocketParticles.GetComponent<ParticleSystem>().enableEmission = false;
    }

    void Launch()
    {
        if(TargetPlanet != null)
        {
            targetPlanet.gameObject.GetComponent<Planet>().Implode();
            TargetPlanet = null;

            rocketParticles.transform.LookAt(-launchDirection);
            rocketParticles.GetComponent<ParticleSystem>().enableEmission = true;
            AudioManager.instance.Play("launch");
        }
    }
}
