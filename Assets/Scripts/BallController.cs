using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class BallController : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedIncrease = 0.3f;
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI computerScore;
    private int hitCounter = 1;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    public AudioClip ballRestartSound; // Add this to reference the restart sound clip
    public readonly float minBallSize = 0.3f;
    public readonly float maxBallSize = 1.0f;
    public float ballSize = 0.5f; // Default size

    void Start()
    {
        // Clamp ball size to make sure it stays within defined limits
        float clampedSize = Mathf.Clamp(ballSize, minBallSize, maxBallSize);
        Vector3 ballSizeV = new Vector3(clampedSize, clampedSize, 1f);

        transform.localScale = ballSizeV; // Set the initial ball size (x = y)


        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Invoke("StartBall", 2f);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter));
    }

    private void StartBall()
    {
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    private void RestartBall()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;

        if (audioSource != null && ballRestartSound != null)
        {
            audioSource.PlayOneShot(ballRestartSound);
        }

        StartCoroutine(RestartBallAnimation());
        Invoke("StartBall", 2.5f);
    }

    private IEnumerator RestartBallAnimation()
    {
        float fadeDuration = 0.5f; // Duration for fading in and out
        float totalDuration = 5f; // Total duration for the effect
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            // Fade out
            Color color = GetComponent<Renderer>().material.color;
            for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
                GetComponent<Renderer>().material.color = color;
                yield return null; // Wait until the next frame
            }

            // Fade in
            for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                GetComponent<Renderer>().material.color = color;
                yield return null; // Wait until the next frame
            }

            elapsedTime += fadeDuration * 2; // Account for both fade out and fade in
        }
    }


    private void PlayerBounce(Transform obj)
    {
        hitCounter++;

        Vector2 ballPostion = transform.position;
        Vector2 playerPosition = obj.position;

        float xDirection;
        float yDirection;

        //we want to flip the direction once it hits the edges
        if (transform.position.x > 0)
            xDirection = -1;
        else
            xDirection = 1;

        yDirection = (ballPostion.y - playerPosition.y) / obj.GetComponent<Collider2D>().bounds.size.y;

        if (yDirection == 0)
        {
            yDirection = 0.25f;
        }

        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.name == "Computer")
            PlayerBounce(other.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x > 0)
        {
            RestartBall();
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
        }
        else if (transform.position.x < 0)
        {
            RestartBall();
            computerScore.text = (int.Parse(computerScore.text) + 1).ToString();
        }
    }
}