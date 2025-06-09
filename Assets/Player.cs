using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public float moveSpeed = 3f;


    public float rotationSpeed = 360f;

    private Animator anim;
    private Rigidbody rb;
    private float currentRotation; // 회전량 누적

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {

        anim.SetBool("isMove", Input.GetKey(KeyCode.UpArrow));
    }

    void FixedUpdate()
    {
        // 이동 
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }

        //  회전
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) turnInput = -1.5f;
        if (Input.GetKey(KeyCode.RightArrow)) turnInput = 1.5f;

        currentRotation += turnInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(0, currentRotation, 0));
    }
    public void AddSpeed(float value)
    {
        moveSpeed += value;
        
    }
}
