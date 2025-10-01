using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool goRight = true;

    SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(speed * (goRight ? 1f : -1f) * Time.deltaTime, 0f, 0f);

        //temporary ; trace ray debug
        Vector3 origin = transform.position + 0.4f * Vector3.up + Vector3.right * 0.4f * (goRight ? 1f : -1f);
        Vector3 direction = Vector3.right * (goRight ? 1f : -1f);

        Debug.DrawRay(origin, direction * 0.2f, Color.chartreuse);   //(départ, direction *where, couleur,)
        RaycastHit2D sideHit = Physics2D.Raycast(origin, direction, 0.2f);

        
        origin = transform.position + Vector3.right * 0.4f * (goRight ? 1f : -1f);
        direction = Vector3.down;

        Debug.DrawRay(origin, direction * 0.5f, Color.blueViolet);
        RaycastHit2D belowHit = Physics2D.Raycast(origin, direction, 1.1f);


        if (sideHit.collider != null || belowHit.collider == null)
        {
            InverseSpeed();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle")){
            InverseSpeed();
        }
    }

    private void InverseSpeed()
    {
        goRight = !goRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
