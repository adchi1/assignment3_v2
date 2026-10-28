using UnityEngine;

public class PlayerMovement : MonoBehaviour,IDamagable
{
    public float moveSpeed = 15;
    public float jumpForce = 25;
    public Camera playerCamera;
    private Rigidbody rigidBody;
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float friction = 10;
    [SerializeField] private float gravity = 3;
    private float offset = 1;
    public static PlayerMovement Instance;

    void Awake(){
        Instance = this;
    }
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horzMove = Input.GetAxisRaw("Horizontal");
        float vertMove = Input.GetAxisRaw("Vertical");
        moveDirection = (transform.forward * vertMove + transform.right * horzMove).normalized;
        
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
            rigidBody.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        }
    }

    void FixedUpdate() {
        rigidBody.AddForce(moveDirection.normalized *moveSpeed/10,ForceMode.Impulse);
        rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x*(100-friction)/100,rigidBody.linearVelocity.y-gravity/10,rigidBody.linearVelocity.z*(100-friction)/100);
    }

    private bool IsGrounded(){
        return Physics.Raycast(transform.position,Vector3.down,offset + .15f);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Implement this");
    }
}