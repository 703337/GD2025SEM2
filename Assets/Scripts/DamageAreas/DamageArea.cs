using UnityEngine;

public class DamageArea : MonoBehaviour
{
    // Initial variable Values
    [SerializeField] int damageValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable MeshRenderer when playing a full build
        #if UNITY_EDITOR
            // Do nothing
        #else
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        #endif
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
