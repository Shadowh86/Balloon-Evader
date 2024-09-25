using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private int maxClicks = 6;
    [SerializeField] private float scaleIncrement = 0.2f;
    [SerializeField] private float dragDecrement = 1.5f;


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
        balloonAudioSource.Play(); // play audio of balloon pop
        EventManager.PlayerEvent.OnScoreChanged(this, 1); // increase score by 1
        DisableBalloonInteraction();
        RequestNewBalloon();
        Destroy(gameObject, 0.5f); // destroy this game object
    }

    private void RequestNewBalloon()
    {
        EventManager.PlayerEvent.OnMethodActivate?.Invoke(this,
            EventArgs.Empty); // spawn another balloon
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
        clickCounter++;
        balloonRigidbody.drag -= dragDecrement;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Limiter"))
        {
            Destroy(gameObject);
        }
    }
}