using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
  [SerializeField] float enemySpeed = 5f;
  [SerializeField] float enemyDamage = 1f;
  [SerializeField] float enemyHealth = 15f;
  [SerializeField] ParticleSystem hitFX;

  Transform enemyTarget;
  Rigidbody2D enemyRb;

  CameraShake cameraShake;
  AudioPlayer audioPlayer;

  void Awake()
  {
    cameraShake = Camera.main.GetComponent<CameraShake>();
    enemyRb = GetComponent<Rigidbody2D>();
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    enemyTarget = player.transform;
    audioPlayer = FindObjectOfType<AudioPlayer>();
  }
  void Start()
  {
    if (transform.position.x > enemyTarget.position.x)
    {
      Flip();
    }
  }

  void FixedUpdate()
  {
    MoveCharacter();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      other.GetComponent<Health>().TakeDamage(enemyDamage);
      ShakeCamera();
      Destroy(gameObject);
    }
    else if (other.CompareTag("Bullet"))
    {
      enemyHealth -= other.GetComponent<Bullet>().bulletDamage;
      PlayHitFX();
      audioPlayer.PlayHittingClip();
      if (enemyHealth <= 0)
      {
        PlayHitFX();
        Destroy(gameObject);
      }
    }
  }

  void MoveCharacter()
  {
    enemyRb.linearVelocity = new Vector2(enemySpeed * Time.fixedDeltaTime, enemyRb.linearVelocity.y);
  }

  void Flip()
  {
    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    enemySpeed *= -1;
  }

  void ShakeCamera()
  {
    if (cameraShake != null)
    {
      cameraShake.Play();
    }
  }

  void PlayHitFX()
  {
    if (hitFX != null)
    {
      ParticleSystem fx = Instantiate(hitFX, transform.position, Quaternion.identity, transform);
      Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
    }
  }
}
