using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement")]
  [SerializeField] float horizontalMoveSpeed = 5f;

  [Header("Hover")]
  [SerializeField] float jumpForce = 40f;

  [Header("Fuel")]
  [SerializeField] float fuelRegenAmount = 5f;
  [SerializeField] float fuelUsageAmount = 20f;

  Rigidbody2D playerRb;

  // movement
  Vector2 playerMove;

  // hover
  bool isEngineOn;

  // fuel
  bool isRegen = false;
  [HideInInspector] public float fuelAmount { get; private set; } = 100f;

  void Awake()
  {
    playerRb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    playerRb.linearVelocity = new Vector2(playerMove.x * horizontalMoveSpeed, playerRb.linearVelocity.y);
  }

  void FixedUpdate()
  {
    switch (isEngineOn)
    {
      case true:
        {
          playerRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
          FuelConsume();
          break;
        }
      case false:
        playerRb.AddForce(new Vector2(0f, 0f), ForceMode2D.Force);
        break;
    }

    if (isRegen)
    {
      FuelRegen();
    }
  }

  public void Move(InputAction.CallbackContext ctx)
  {
    playerMove = ctx.ReadValue<Vector2>();
  }

  public void Hover(InputAction.CallbackContext ctx)
  {
    if (ctx.performed && fuelAmount >= 0 && !isRegen)
    {
      isEngineOn = true;
    }

    if (ctx.canceled)
    {
      isEngineOn = false;
    }
  }

  public bool GetIsRegen()
  {
    return isRegen;
  }

  void FuelRegen()
  {
    fuelAmount += fuelRegenAmount * Time.deltaTime;

    if (fuelAmount >= 100)
    {
      fuelAmount = 100;
      isRegen = false;
    }
  }

  void FuelConsume()
  {
    fuelAmount -= fuelUsageAmount * Time.deltaTime;
    if (fuelAmount <= 0)
    {
      isEngineOn = false;
      isRegen = true;
      fuelAmount = 0;
    }
  }
}
