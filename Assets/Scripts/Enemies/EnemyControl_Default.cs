using TMPro;
using UnityEngine;

public class EnemyControl_Default : MonoBehaviour
{
    // Initial Variable Values
    [SerializeField] float moveSpeed = 4; // Enemy movement speed.
    [SerializeField] float sprintMult = 2; // Enemy movement speed modifier when sprinting.
    [SerializeField] int staminaMax = 100; // Enemy max stamina.
    int stamina; // Enemy current stamina.
    bool isSprinting; // Whether or not the enemy is sprinting.
    float staminaDrainTimer = 0.1f; // Enemy stamina drain timer.
    float staminaRegenTimer = 0.5f; // Enemy stamina regen timer.
    [SerializeField] float jumpForce = 10; // Enemy jump force.
    bool isGrounded = true; // Checks whether the enemy is grounded or not.
    [SerializeField] int healthMax = 100; // Enemy max health.
    int health; // Enemy current health.
    bool isDead; // Checks whether the enemy is dead or not.
    float h; // Horizontal rotation for the enemy's body.
    float v; // Vertical rotation for the enemy's camera.
    float attackDuration = 0.25f; // Time the enemy's attack is active for.
    float attackCooldown; // Time before the enemy can attack again.

    //References
    Rigidbody rb; // The enemy's body.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the enemy's rigidbody
        rb = GetComponent<Rigidbody>();
        // Set current stamina based on max stamina
        stamina = staminaMax;
        // Set current health based on max health
        health = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        // No movement behaviour right now.
    }

    // Function to jump
    void Jump()
    {
        rb.AddForce(0, jumpForce, 0);
    }

    // Function to cause effects when colliding with certain tagged objects
    private void OnCollisionEnter(Collision collision)
    {
        // Allow jumping when colliding with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log($"isGrounded: {isGrounded}");
        }
    }

    // Function to cause effects wehn entering certain tagged objects
    private void OnTriggerEnter(Collider area)
    {
        // Take damage when entering a damage area, and mark isDead as true and Destroy() if health goes below 1
        if (area.gameObject.tag == "DamageArea")
        {
            // Check damage to be dealt and deal it
            int damageAmount = area.gameObject.GetComponent<DamageArea>().GetDamageAmount();
            health -= damageAmount;
            if (health <= 0)
            {
                isDead = true;
                health = 0;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider area)
    {
        // Take damage when staying in a damage-over-time area, and mark isDead as true and Destroy() if health goes below 1
        if (area.gameObject.tag == "DamageOverTimeArea")
        {
            // Don't take damage if isDead is true
            if (isDead == true)
            {
                return;
            }
            // Check damage to be dealt and deal it
            int damageAmount = area.gameObject.GetComponent<DamageOverTimeArea>().GetDamageAmount();
            health -= damageAmount;
            if (health <= 0)
            {
                isDead = true;
                health = 0;
                Destroy(gameObject);
            }
        }
    }

    // Function to cause effects when no longer colliding with certain tagged objects
    private void OnCollisionExit(Collision collision)
    {
        // Disable jumping when not colliding with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            Debug.Log($"isGrounded: {isGrounded}");
        }
    }
}
