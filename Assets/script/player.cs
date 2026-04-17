using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour

   
{
    public float speed;
    private Rigidbody2D rb;
    private Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector3(move * speed, rb.linearVelocity.y);

        anim.SetFloat("speed", Mathf.Abs(move));


        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);



    }

   
}
