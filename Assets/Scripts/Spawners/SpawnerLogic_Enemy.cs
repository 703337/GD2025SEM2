using UnityEngine;

public class SpawnerLogic_Enemy : MonoBehaviour
{
    // Initial Variable Values
    enum spawnerType{Fodder, Special, SpecialEvent, MiniBoss, Boss};
    [SerializeField] spawnerType _spawnerType;
    float spawnCooldown;

    // References
    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        Debug.Log($"Spawner Type: {_spawnerType}"); 
        // Check _spawnerType and set initial spawnCooldown
        switch (_spawnerType)
        {
            // Fodder
            case spawnerType.Fodder:
                spawnCooldown = spawnCooldown = Random.Range(2.5f, 5f);
                break;
            // Mini-Boss
            case spawnerType.MiniBoss:
                spawnCooldown = spawnCooldown = Random.Range(30f, 45f);
                break;
            // Boss
            case spawnerType.Boss:
                spawnCooldown = spawnCooldown = Random.Range(45f, 60f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Count down to next spawn
        spawnCooldown -= Time.deltaTime;
        // Spawn an enemy of matching spawner type if quota not fulfilled
        if (spawnCooldown <= 0)
        {
            // Check _spawnerType
            switch (_spawnerType)
            {
                // Fodder
                case spawnerType.Fodder:
                    // Only spawn an enemy if the quota is not met
                    if (gameManager.fodderQuota > 0)
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
                        // decrease the quota by one
                        gameManager.fodderQuota -= 1;
                        // Reset spawnCooldown
                        spawnCooldown = Random.Range(2.5f, 5f);
                    }
                    break;
                // Mini-Boss
                case spawnerType.MiniBoss:
                    // Only spawn an enemy if the quota is not met
                    if (gameManager.miniBossQuota > 0)
                    {
                        // Roll a random number to choose the Mini-Boss
                        int enemChoice = Random.Range(0, 2);
                        // Spawn accordingly
                        switch (enemChoice)
                        {
                            case 0: // Stab
                                // Instantiate(MB_Stab, transform);
                                Debug.Log("Mini-Boss Spawned (Stab)");
                                break;
                            case 1: // Shotgun_Slug
                                // Instantiate(MB_Shotgun_Slug, transform);
                                Debug.Log("Mini-Boss Spawned (Shotgun_Slug)");
                                break;
                        }
                        // decrease the quota by one
                        gameManager.miniBossQuota -= 1;
                        // Reset spawnCooldown
                        spawnCooldown = Random.Range(30f, 45f);
                    }
                    break;
                // Boss
                case spawnerType.Boss:
                    // Only spawn an enemy if the quota is not met
                    if (gameManager.bossQuota > 0)
                    {
                        // Roll a random number to choose the Mini-Boss
                        int enemChoice = Random.Range(0, 2);
                        // Spawn accordingly
                        switch (enemChoice)
                        {
                            case 0: // Cloaksaw
                                // Instantiate(B_Cloaksaw, transform);
                                Debug.Log("Boss Spawned (Cloaksaw)");
                                break;
                            case 1: // Juggernaut
                                // Instantiate(B_Juggernaut, transform);
                                Debug.Log("Boss Spawned (Juggernaut)");
                                break;
                        }
                        // decrease the quota by one
                        gameManager.bossQuota -= 1;
                        // Reset spawnCooldown
                        spawnCooldown = Random.Range(45f, 60f);
                    }
                    break;
            }
            // Ensure spawnCooldown still resets even when the quota is met
            if (spawnCooldown <= 0)
            {
                // Check _spawnerType
                switch (_spawnerType)
                {
                    // Fodder
                    case spawnerType.Fodder:
                        spawnCooldown = spawnCooldown = Random.Range(2.5f, 5f);
                        break;
                    // Mini-Boss
                    case spawnerType.MiniBoss:
                        spawnCooldown = spawnCooldown = Random.Range(30f, 45f);
                        break;
                    // Boss
                    case spawnerType.Boss:
                        spawnCooldown = spawnCooldown = Random.Range(45f, 60f);
                        break;
                }
            }
        }
    }
}
