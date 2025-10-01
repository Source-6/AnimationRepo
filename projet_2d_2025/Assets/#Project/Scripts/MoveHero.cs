using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MoveHero : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField]private float jumpForce = 300f;
    [SerializeField] private float speed;
    private InputAction xAxis;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private bool isJumping = false;
    private bool isCrouching = false;



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        xAxis = actions.FindActionMap("Luigi").FindAction("XAxis");
    }

    private void OnEnable()
    {
        actions.FindActionMap("Luigi").Enable();
        actions.FindActionMap("Luigi").FindAction("Jump").performed += OnJump;   //listener  (lie)
        actions.FindActionMap("Luigi").FindAction("Crouch").performed += OnCrouch;

    }

    private void OnDisable()
    {
        actions.FindActionMap("Luigi").Disable();
        actions.FindActionMap("Luigi").FindAction("Jump").performed -= OnJump;  //(délie)
        actions.FindActionMap("Luigi").FindAction("Crouch").performed -= OnCrouch;

    }


    void Update()
    {
        MoveX();
        Vector3 origin = transform.position +0.01f* Vector3.down;
        Vector3 direction = Vector3.down;
        RaycastHit2D downHit = Physics2D.Raycast(origin, direction, 0.5f);
        Debug.DrawRay(origin, direction *0.5f, Color.aquamarine);
        if (isJumping)
        {
            if (downHit.collider != null && rigidbody2D.linearVelocityY < 0f )                          //(rigidbody2D.linearVelocityY < 0f)     //changer avec raycasting (vers le bas, verifier à 0.2f) ->
            {
                isJumping = false;
                animator.SetBool("on jump", false);
            }
        }
        if ((!Keyboard.current.downArrowKey.isPressed) && isCrouching)
        {
            animator.SetBool("crouch", false);
            isCrouching = false;
        }
    }

    private void OnJump(InputAction.CallbackContext callbackContext)
    {
        isJumping = true;
        animator.SetBool("on jump", isJumping);
        rigidbody2D.AddForce(Vector2.up * jumpForce);
        Debug.Log("jump");
    }

    private void OnCrouch(InputAction.CallbackContext callbackContext)
    {
        animator.SetBool("crouch", true);
        // boxCollider2D.size = new Vector2(boxCollider2D.size.x, boxCollider2D.size.y / 2);
        isCrouching = true;
    }

    private void MoveX()
    {
        spriteRenderer.flipX = xAxis.ReadValue<float>() < 0;
        animator.SetFloat("speed", MathF.Abs(xAxis.ReadValue<float>()));            //renvoit valeur absolue
        transform.Translate(xAxis.ReadValue<float>() * speed * Time.deltaTime, 0f, 0f);
    }
}
