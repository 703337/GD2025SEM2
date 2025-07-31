using UnityEngine;

public class DamageArea : MonoBehaviour
{
    // Initial variable Values
    [SerializeField] int damageValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to return damage dealt
    public int GetDamageAmount()
    {
        return damageValue;
    }
}
