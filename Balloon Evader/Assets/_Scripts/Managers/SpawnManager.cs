using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class for spawning balloons where it sets random position on screen
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject balloonPrefab;
    [SerializeField] private Material[] balloonMaterials;
    [SerializeField] private int initialBalloonCount = 2; // how many balloons we want when game starts

    [Space(5)] [Tooltip("Four floats for spawning balloons.\nx/y = left/right, z/w = bottom/up")] [SerializeField]
    private Vector4 minMaxPosition;

    private void Start()
    {
        for (int i = 0; i < initialBalloonCount; i++)
        {
            SpawnBalloon();
        }
    }

    // Setting random position for balloon based on parameters set in inspector
    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minMaxPosition.x, minMaxPosition.y);
        float randomY = Random.Range(minMaxPosition.z, minMaxPosition.w);
        return new Vector3(randomX, randomY, 0);
    }

    //Spawn new balloon on random position and set it's random material
    private void SpawnBalloon()
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

    private void OnEnable()
    {
        EventManager.SpawnEvent.OnSpawnNewBalloon += SpawnBalloon; //subscribe
    }

    private void OnDisable()
    {
        EventManager.SpawnEvent.OnSpawnNewBalloon -= SpawnBalloon; // unsubscribe
    }
}