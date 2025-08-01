using UnityEngine;

public class PlayerControl_Default : MonoBehaviour
{
    // Initial Variable Values
    [SerializeField] float moveSpeed = 4; // Player movement speed.
    [SerializeField] float sprintMult = 2; // Player movement speed modifier when sprinting.
    bool isSprinting; // Whether or not the player is sprinting.
    [SerializeField] float jumpForce = 10; // Player jump force.
    bool isGrounded = true; // Checks whether the player is grounded or not.
    [SerializeField] int healthMax = 100; // Player max health;
    int health; // Player current health.
    bool isDead; // Checks whether the player is dead or not.

    //References
    Rigidbody rb; // The player's body.
    Camera face; // The player's face (camera).

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the player's face (camera)
        face = GetComponentInChildren<Camera>();
        // Get the player's rigidbody
        rb = GetComponent<Rigidbody>();
        // Lock the cursor by default
        Cursor.lockState = CursorLockMode.Locked;
        // Set current health based on max health
        health = healthMax;
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

        // Unlock the cursor when pressing escape and disable all controls until locked again
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
            else
            {
                return;
            }
        }

        // CODE BELOW ALTERED FROM A PREVIOUS PROJECT <<<<
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

        // Get the direction the player is facing relative to their inputs
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Move the player based on the direction they are facing
        if (isSprinting == true)
        {
            transform.Translate(direction * (moveSpeed * sprintMult) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        // CODE ABOVE ALTERED FROM A PREVIOUS PROJECT <<<<


        // Make the player jump
        if (Input.GetKey(KeyCode.Space) && isGrounded == true)
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
            Debug.Log($"Damage Taken: {damageAmount}");
            if (health <= 0)
            {
                isDead = true;
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
                Debug.Log($"Damage Taken: {damageAmount}");
            }
            if (health <= 0)
            {
                isDead = true;
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
