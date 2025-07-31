using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    // Initial Variable Values
    enum spawnerType{Fodder, Special, SpecialEvent, MiniBoss, Boss, Count};
    [SerializeField] spawnerType _spawnerType;
    GameManager gameManager;
    int spawnerSetType;
    float currentTime;
    float spawnCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawnerSetType = _spawnerType.ConvertTo<int>();
    }

    // Update is called once per frame
    void Update()
    {
        // Count down to next spawn
        currentTime = 1 * Time.deltaTime;
        spawnCooldown -= currentTime;

        // Spawn an enemy of matching spawner type if quota not fulfilled
        if (spawnCooldown == 0)
        {
            // Fodder
            if (gameManager.fodderQuota > 0 && spawnerSetType == spawnerType.Fodder.ConvertTo<int>())
            {
                Debug.Log("Fodder Spawned");
            }
            else
            {
                return;
            }
            // Reset spawnCooldown
            spawnCooldown = Random.Range(2.5f, 10f);
        }
    }
}
