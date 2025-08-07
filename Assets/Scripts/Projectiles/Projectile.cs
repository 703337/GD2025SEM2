using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Initial Variable Values
    [SerializeField] int projectileSpeed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
        if (transform.position.magnitude >= 100 )
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
