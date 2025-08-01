using UnityEngine;

public class DamageOverTimeArea : MonoBehaviour
{
    // Initial variable Values
    enum damageOverTimeType {Fire, Acid, Laser, InstaK};
    [SerializeField] damageOverTimeType _damageOverTimeType;
    int damageValue;
    float currentTime;
    float damageCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable MeshRenderer when playing a full build
        #if UNITY_EDITOR
            // Do nothing
        #else
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        #endif
        // Set damageCooldown and damageValue based on _damageOverTimeType
        switch (_damageOverTimeType)
        {
            case damageOverTimeType.Fire:
                damageValue = 5;
                damageCooldown = 1;
                break;
            case damageOverTimeType.Acid:
                damageValue = 1;
                damageCooldown = 0.25f;
                break;
            case damageOverTimeType.Laser:
                damageValue = 2;
                damageCooldown = 0.10f;
                break;
            case damageOverTimeType.InstaK:
                damageValue = 999;
                damageCooldown = 0f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to return damage dealt
    public int GetDamageAmount()
    {
        currentTime = 1 * Time.deltaTime;
        damageCooldown -= currentTime;
        // Deal damage only if damageCooldown is 0
        if (damageCooldown <= 0)
        {
            // Reset damageCooldown based on _damageOverTimeType
            switch (_damageOverTimeType)
            {
                case damageOverTimeType.Fire:
                    damageCooldown = 1;
                    break;
                case damageOverTimeType.Acid:
                    damageCooldown = 0.25f;
                    break;
                case damageOverTimeType.Laser:
                    damageCooldown = 0.10f;
                    break;
                case damageOverTimeType.InstaK:
                    damageCooldown = 0f;
                    break;
            }
            return damageValue;
        }
        else
        {
            return 0;
        }
    }
}
