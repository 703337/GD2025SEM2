using TMPro;
using UnityEngine;

public class PlayerControl_Default_Melee : MonoBehaviour
{
    // Initial Variable Values
    [SerializeField] float moveSpeed = 4; // Player movement speed.
    [SerializeField] float sprintMult = 2; // Player movement speed modifier when sprinting.
    [SerializeField] int staminaMax = 100; // Player max stamina.
    int stamina; // Player current stamina.
    bool isSprinting; // Whether or not the player is sprinting.
    float staminaDrainTimer = 0.1f; // Player stamina drain timer.
    float staminaRegenTimer = 0.5f; // Player stamina regen timer.
    [SerializeField] float jumpForce = 10; // Player jump force.
    bool isGrounded = true; // Checks whether the player is grounded or not.
    float jumpCooldown = 0; // Time before the player can jump again.
    [SerializeField] int healthMax = 100; // Player max health.
    int health; // Player current health.
    bool isDead; // Checks whether the player is dead or not.
    float h; // Horizontal rotation for the player's body.
    float v; // Vertical rotation for the player's camera.
    float attackDuration = 0.25f; // Time the player's attack is active for.
    float attackCooldown; // Time before the player can attack again.

    //References
    Rigidbody rb; // The player's body.
    Camera face; // The player's camera.
    [SerializeField] TextMeshProUGUI healthDisplay; // Display for the player's health.
    [SerializeField] TextMeshProUGUI staminaDisplay; // Display for the player's stamina.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the player's face (camera)
        face = GetComponentInChildren<Camera>();
        // Get the player's rigidbody
        rb = GetComponent<Rigidbody>();
        // Lock the cursor by default
        Cursor.lockState = CursorLockMode.Locked;
        // Set current stamina based on max stamina
        stamina = staminaMax;
        // Set current health based on max health
        health = healthMax;
        // Set initial displayed stamina
        staminaDisplay.text = $"Stamina: {stamina}/{staminaMax}";
        // Set initial displayed health
        healthDisplay.text = $"Health: {health}/{healthMax}";
    }

    // Update is called once per frame
    void Update()
    {
        // Unlock the cursor and disable remaining code when dead
        if (isDead == true && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (isDead == true)
        {
            return;
        }

        // Unlock the cursor when pressing escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }


        // Handle Movement
        // Check if the player is sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // Set the direction the player is moving based on their inputs, unless the cursor is unlocked
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Cursor.lockState == CursorLockMode.None)
        {
            direction = new Vector3(0, 0, 0);
        }

        // Move the player based on the direction they are trying to move
        if (isSprinting == true && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && stamina > 0)
        {
            transform.Translate(direction * (moveSpeed * sprintMult) * Time.deltaTime);
            // Drain stamina
            staminaDrainTimer -= Time.deltaTime;
            if (staminaDrainTimer <= 0)
            {
                stamina -= 1;
                staminaDrainTimer = 0.1f;
                staminaDisplay.text = $"Stamina: {stamina}/{staminaMax}";
            }
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            // Regenerate stamina when not trying to sprint
            staminaRegenTimer -= Time.deltaTime;
            if (staminaRegenTimer <= 0 && stamina < staminaMax && isSprinting == false)
            {
                stamina += 1;
                staminaRegenTimer = 0.5f;
                staminaDisplay.text = $"Stamina: {stamina}/{staminaMax}";
            }
        }


        // Make the player jump, unless the cursor is unlocked or jump is on cooldown
        jumpCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && Cursor.lockState == CursorLockMode.Locked)
        {
            if (jumpCooldown <= 0)
            {
                rb.AddForce(0, jumpForce, 0);
                jumpCooldown = 0.5f;
            }
        }


        // CODE BELOW ALTERED FROM A PREVIOUS PROJECT <<<<
        // Handle Camera Movement
        // Get mouse movement
        h += Input.GetAxis("Mouse X");
        v += Input.GetAxis("Mouse Y");

        // Clamp vertical camera movement
        if (v > 66)
        {
            v = 66;
        }
        else if (v < -66)
        {
            v = -66;
        }

        // Rotate the camera/player based on player input (if cursor locked)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            transform.rotation = Quaternion.Euler(0, h, 0);
            face.transform.rotation = Quaternion.Euler(-v, h, 0);
        }
        // CODE ABOVE ALTERED FROM A PREVIOUS PROJECT <<<<

        // Handle attacking
        if (Input.GetMouseButton(0) && attackCooldown <= 0 || attackDuration < 0.25f)
        {
            // Play the attacking animation
            if (attackDuration == 0.25f)
            {
                face.GetComponentInChildren<Animator>().Play("Punching");
            }
            // Enable the attached DamageArea
            face.GetComponentInChildren<BoxCollider>().enabled = true;
            attackDuration -= Time.deltaTime;
            // Once attackDuration ends, disable the attached DamageArea and start attackCooldown
            if (attackDuration <= 0)
            {
                attackDuration = 0.25f;
                attackCooldown = 0.5f;
                face.GetComponentInChildren<BoxCollider>().enabled = false;
            }
        }
        else if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    // Function to cause effects when colliding with certain tagged objects
    private void OnCollisionStay(Collision collision)
    {
        // Allow jumping when colliding with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Function to cause effects when no longer colliding with certain tagged objects
    private void OnCollisionExit(Collision collision)
    {
        // Disable jumping when not colliding with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    // Function to cause effects wehn entering certain tagged objects
    private void OnTriggerEnter(Collider area)
    {
        // Take damage when entering a damage area, and mark isDead as true if health goes below 1
        if (area.gameObject.tag == "DamageArea")
        {
            // Check damage to be dealt and deal it
            int damageAmount = area.gameObject.GetComponent<DamageArea>().GetDamageAmount();
            health -= damageAmount;
            healthDisplay.text = $"Health: {health}/{healthMax}";
            if (health <= 0)
            {
                isDead = true;
                health = 0;
            }
        }
    }

    private void OnTriggerStay(Collider area)
    {
        // Take damage when staying in a damage-over-time area, and mark isDead as true if health goes below 1
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
            if (damageAmount > 0)
            {
                healthDisplay.text = $"Health: {health}/{healthMax}";
            }
            if (health <= 0)
            {
                isDead = true;
                health = 0;
            }
        }
    }
}
