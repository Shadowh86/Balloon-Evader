using UnityEngine;


/// <summary>
/// Class that is on Balloon.prefab where it checks what is happening to it.
/// @Tomislav Marković
/// </summary>
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

    // Unity method that is triggered when player presses object with collider
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
        RequestNewBalloon(); // send event to request to spawn new balloon
        Destroy(gameObject, 0.5f); // destroy this game object
    }

    
    private void RequestNewBalloon()
    {
        EventManager.SpawnEvent.OnSpawnNewBalloon?.Invoke();
    }

    private void DisableBalloonInteraction()
    {
        balloonCollider.enabled = false; // disable collider so player can't click on object
        balloonMeshRenderer.enabled = false; // disable mesh so player thinks balloon is popped
    }

    // check if clicks are equal or greater than max clicks
    private bool IsBalloonFullyInflated()
    {
        return clickCounter >= maxClicks;
    }

    
    private void InflateBalloon()
    {
        transform.localScale += Vector3.one * scaleIncrement; // increase size of object by 1 * scaleIncrement 
        clickCounter++; // counter to count how many clicks has player pressed
        balloonRigidbody.drag -= dragDecrement; // reducing drag so balloon can fly faster as it is getting larger
    }

    private void PopBalloonSound()
    {
        balloonAudioSource.Play();
    }

    // if balloon enters limiter collier it destroys balloon game object and requests new balloon
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