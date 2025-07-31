using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    // Initial Variable Values
    enum spawnerType{Fodder, Special, SpecialEvent, MiniBoss, Boss, Count};
    [SerializeField] spawnerType _spawnerType;
    GameManager gameManager;
    float currentTime;
    float spawnCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        Debug.Log("Spawn Cooldown: " + spawnCooldown);
        Debug.Log("Spawner Type: " + _spawnerType);
    }

    // Update is called once per frame
    void Update()
    {
        // Count down to next spawn
        currentTime = 1 * Time.deltaTime;
        spawnCooldown -= currentTime;
        Debug.Log("Time: " + spawnCooldown);
        // Spawn an enemy of matching spawner type if quota not fulfilled
        if (spawnCooldown <= 0)
        {
            // Fodder
            if (_spawnerType == spawnerType.Fodder && gameManager.fodderQuota > 0)
            {
                // Decide whether the Fodder will be replaced by a Random Encounter
                int isRandomEncounter = Random.Range(0, 100);
                // Spawn accordingly
                if (isRandomEncounter <= 4)
                {
                    // Roll a random number to choose the Random Encounter
                    int enemChoice = Random.Range(0, 2);
                    // Spawn accordingly
                    switch (enemChoice)
                    {
                        case 0: // Suicide Pact
                                // Instantiate(RE_Suicide_Pact, transform);
                                Debug.Log("Random Encounter Spawned (Suicide Pact)");
                            break;
                        case 1: // Observer
                                // Instantiate(RE_Observer, transform);
                                Debug.Log("Random Encounter Spawned (Observer)");
                            break;
                    }
                }
                else
                {
                    // Roll a random number to choose the Fodder
                    int enemChoice = Random.Range(0, 2);
                    // Spawn accordingly
                    switch (enemChoice)
                    {
                        case 0: // Prick
                                // Instantiate(F_Prick, transform);
                                Debug.Log("Fodder Spawned (Prick)");
                            break;
                        case 1: // Shotgun
                                // Instantiate(F_Shotgun, transform);
                                Debug.Log("Fodder Spawned (Shotgun)");
                            break;
                    }
                }
                gameManager.fodderQuota -= 1;
            }
            else
            {
                // return;
            }
            // Reset spawnCooldown
            spawnCooldown = Random.Range(2.5f, 5f);
            Debug.Log("Spawn Cooldown:" + spawnCooldown);
        }
    }
}
