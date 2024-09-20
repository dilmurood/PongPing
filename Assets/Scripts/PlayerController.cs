using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public bool isPlayer;
    public bool isTwoPlayerMode; // Enable/disable two-player mode
    public GameObject circle;
    private Rigidbody2D rb;
    private Vector2 playerMovement;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isTwoPlayerMode)
        {
            if (isPlayer)
            {
                float verticalInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
                playerMovement = new Vector2(0, verticalInput);
            }
            else
            {
                float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
                playerMovement = new Vector2(0, verticalInput);
            }
        }
        else
        {
            if (isPlayer)
            {
                float verticalInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
                playerMovement = new Vector2(0, verticalInput);
            }
            else
            {
                ComputerMove();
            }
        }
    }

    public void ComputerMove()
    {
        if (circle.transform.position.y > transform.position.y + 0.5f)
            playerMovement = new Vector2(0, 1);
        else if (circle.transform.position.y < transform.position.y - 0.5f)
            playerMovement = new Vector2(0, -1);
        else
            playerMovement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = playerMovement * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Play the sound when the ball hits the paddle
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
