using TMPro;
using UnityEngine;

public class PlayerControl_Default : MonoBehaviour
{
    // Initial Variable Values
    [SerializeField] float moveSpeed = 4; // Player movement speed.
    [SerializeField] float sprintMult = 2; // Player movement speed modifier when sprinting.
    [SerializeField] int staminaMax = 100; // Player max stamina.
    int stamina; // Player current stamina.
    bool isSprinting; // Whether or not the player is sprinting.
    float staminaDrainTimer = 0.1f; // Player stamina drain modifier.
    float staminaRegenTimer = 0.5f; // Player stamina regen modifier.
    [SerializeField] float jumpForce = 10; // Player jump force.
    bool isGrounded = true; // Checks whether the player is grounded or not.
    [SerializeField] int healthMax = 100; // Player max health.
    int health; // Player current health.
    bool isDead; // Checks whether the player is dead or not.

    //References
    Rigidbody rb; // The player's body.
    Camera face; // The player's face (camera).
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


        // Make the player jump, unless the cursor is unlocked
        if (Input.GetKey(KeyCode.Space) && isGrounded == true && Cursor.lockState == CursorLockMode.Locked)
        {
            Jump();
        }


        // CODE BELOW ALTERED FROM A PREVIOUS PROJECT <<<<
        // Handle Camera Movement
        // Get mouse movement
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        // Rotate the camera/player based on player input (if cursor locked)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            transform.Rotate(0, h, 0);
            face.transform.Rotate(-v, 0, 0); // <- Find a way to clamp the camera's vertical movement <- !!!
        }
        // CODE ABOVE ALTERED FROM A PREVIOUS PROJECT <<<<
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
