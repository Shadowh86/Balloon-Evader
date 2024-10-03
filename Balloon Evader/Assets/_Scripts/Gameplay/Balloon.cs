using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private int maxClicks = 6;
    [SerializeField] private float scaleIncrement = 0.2f;
    [SerializeField] private float dragDecrement = 1.5f;
    [SerializeField] private int popValue = 1; // value of destroyed balloon


    private SpawnManager spawnManager;
    private int clickCounter;
    private Rigidbody balloonRigidbody;
    private AudioSource balloonAudioSource;
    private SphereCollider balloonCollider;
    private MeshRenderer balloonMeshRenderer;

    private void Awake()
    {
        balloonRigidbody = GetComponent<Rigidbody>();
        balloonAudioSource = GetComponent<AudioSource>();
        balloonCollider = GetComponent<SphereCollider>();
        balloonMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        InflateBalloon();

        if (IsBalloonFullyInflated())
        {
            PopBalloon();
        }
    }

    private void PopBalloon()
    {
        PopBalloonSound(); // play audio of balloon pop
        EventManager.GameManagerEvent.OnScoreChanged(popValue); // increase score by send value (in this example 1)
        DisableBalloonInteraction();
        RequestNewBalloon();
        Destroy(gameObject, 0.5f); // destroy this game object
    }

    private void RequestNewBalloon()
    {
        EventManager.GameManagerEvent.OnSpawnNewBalloon?.Invoke();
    }

    private void DisableBalloonInteraction()
    {
        balloonCollider.enabled = false; // disable collider so player can't click on object
        balloonMeshRenderer.enabled = false; // disable mesh so player thinks balloon is popped
    }

    private bool IsBalloonFullyInflated()
    {
        return clickCounter >= maxClicks;
    }

    private void InflateBalloon()
    {
        transform.localScale += Vector3.one * scaleIncrement;
        // transform.DOScale(scaleIncrement, 1);
        clickCounter++;
        balloonRigidbody.drag -= dragDecrement;
    }

    private void PopBalloonSound()
    {
        balloonAudioSource.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Limiter"))
        {
            RequestNewBalloon();
            PopBalloonSound();
            //EventManager.PlayerEvent.OnFlyBalloonUpdate(this, 1);
            EventManager.GameManagerEvent.OnFlyBalloonUpdate?.Invoke(1);
            Destroy(gameObject);
        }
    }
}