using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Speed Set To Default Value");
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);

        rb.velocity = moveDirection;
    }
}
