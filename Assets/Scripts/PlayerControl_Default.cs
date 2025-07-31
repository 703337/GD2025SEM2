using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerControl_Default : MonoBehaviour
{
    // Initial Variable Values
    Rigidbody rb; // The player's body.
    Camera face; // The player's face (camera).
    [SerializeField] float moveSpeed = 4; // Player movement speed.
    [SerializeField] float sprintMult = 2; // Player movement speed modifier when sprinting.
    bool sprinting; // Whether or not the player is sprinting.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the player's face (camera)
        face = GetComponentInChildren<Camera>();
        // Get the player's rigidbody
        rb = GetComponent<Rigidbody>(); // Currently unused.
        // Lock the cursor by default
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // CODE BELOW ALTERED FROM A PREVIOUS PROJECT <<<<
        // Handle Movement
        // Check if the player is sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

        // Get the direction the player is facing relative to their inputs
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Move the player based on the direction they are facing
        if (sprinting == true)
        {
            transform.Translate(direction * (moveSpeed * sprintMult) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }


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
}
