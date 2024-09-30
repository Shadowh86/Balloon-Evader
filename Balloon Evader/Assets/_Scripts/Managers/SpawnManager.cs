using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject balloonPrefab;
    [Tooltip("Four floats for spawning balloons.")]
    [SerializeField] private Vector4 minMaxPosition;
    [SerializeField] private Material[] balloonMaterials;
    [SerializeField] private int initialBalloonCount = 2;

    private void Awake()
    {
        EventManager.PlayerEvent.OnMethodActivate += SpawnBalloon;
    }

    private void OnDestroy()
    {
        EventManager.PlayerEvent.OnMethodActivate -= SpawnBalloon;
    }

    private void Start()
    {
        for (int i = 0; i < initialBalloonCount; i++)
        {
            SpawnBalloon(this, EventArgs.Empty);
        }
    }


    // Setting random position for ballon
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minMaxPosition.x, minMaxPosition.y);
        float randomY = Random.Range(minMaxPosition.z, minMaxPosition.w);
        return new Vector3(randomX, randomY, 0);
    }

    /// if you have certain score spawn another balloon
    /// Find a way to send this method without reference
    /// Event Aggregator is so far best option
    private void SpawnBalloon(object sender, EventArgs e)
    {
        GameObject balloon = Instantiate(balloonPrefab, GetRandomPosition(), Quaternion.identity);
        SetRandomMaterial(balloon);
    }

    private void SetRandomMaterial(GameObject balloon)
    {
        if (balloonMaterials.Length > 0)
        {
            int randomMaterial = Random.Range(0, balloonMaterials.Length);
            balloon.GetComponentInChildren<MeshRenderer>().material = balloonMaterials[randomMaterial];
        }
    }
}