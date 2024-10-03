using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject balloonPrefab;

    [Tooltip("Four floats for spawning balloons.")] [SerializeField]
    private Vector4 minMaxPosition;

    [SerializeField] private Material[] balloonMaterials;
    [SerializeField] private int initialBalloonCount = 2;
    
    private void Start()
    {
        for (int i = 0; i < initialBalloonCount; i++)
        {
            SpawnBalloon();
        }
    }

    // Setting random position for ballon
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minMaxPosition.x, minMaxPosition.y);
        float randomY = Random.Range(minMaxPosition.z, minMaxPosition.w);
        return new Vector3(randomX, randomY, 0);
    }

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
        EventManager.GameManagerEvent.OnSpawnNewBalloon += SpawnBalloon;
    }

    private void OnDisable()
    {
        EventManager.GameManagerEvent.OnSpawnNewBalloon -= SpawnBalloon;
    }
}