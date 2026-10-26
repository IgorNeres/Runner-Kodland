using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Rigidbody rb;
    [SerializeField] GameObject[] laneCenters;
    int currentLane = 1;
    float targetX;
    [SerializeField] float laneChangeSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip[] runSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] float stepDelay = 0.3f; 
    private float nextStepTime = 0f;
    [SerializeField] ParticleSystem pickupEffect;
    [SerializeField] ParticleSystem jumpEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = laneCenters[currentLane].transform.position;
        targetX = transform.position.x;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();       
    }

    void Update()
    {
        moveSpeed += Time.deltaTime * 1.1f;

        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
            targetX = laneCenters[currentLane].transform.position.x;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
            targetX = laneCenters[currentLane].transform.position.x;
        }

        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f) 
        {
            if (Time.time >= nextStepTime)
            {
                audioSource.PlayOneShot(runSound[Random.Range(0, runSound.Length)]);
                nextStepTime = Time.time + stepDelay;
            }

            animator.SetBool("jump", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                animator.SetBool("jump", true);
                audioSource.Stop();
                audioSource.PlayOneShot(jumpSound);
                Instantiate(jumpEffect, transform.position, Quaternion.identity);
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        float direction = targetX - rb.position.x;
        rb.linearVelocity = new Vector3(direction * laneChangeSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            moveSpeed = moveSpeed / 1.3f;
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(pickupSound);

            Instantiate(pickupEffect, collision.transform.position, Quaternion.identity);
        }
    }
}