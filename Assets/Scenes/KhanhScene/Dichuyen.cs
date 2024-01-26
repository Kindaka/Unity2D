using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dichuyen : MonoBehaviour
{
    private int dichuyen;
    private float tocdo = 10;
    private int do_cao = 10;
    private bool isfacingRight = true;
    private bool jump;
    public bool doubleJump = true;
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dichuyen = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dichuyen = 1;
        }
        else { dichuyen = 0; }

        transform.Translate(Vector3.right * tocdo * dichuyen * Time.deltaTime);
        //jump
        if (jump && !Input.GetKey(KeyCode.UpArrow))
        {
            doubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //transform.Translate(Vector3.up * do_cao);
            //rb.AddForce(Vector2.up * do_cao, ForceMode2D.Impulse);
            if (doubleJump || jump)
            {
                rb.velocity = new Vector2(rb.velocity.x, do_cao);
                doubleJump = !doubleJump;
            }

        }
        flip();

        anim.SetFloat("run", Mathf.Abs(dichuyen));

    }
    void flip()
    {
        if (isfacingRight && dichuyen < 0 || !isfacingRight && dichuyen > 0)
        {
            isfacingRight = !isfacingRight;
            Vector3 kich_thuoc = transform.localScale;
            kich_thuoc.x = kich_thuoc.x * -1;
            transform.localScale = kich_thuoc;
        }
    }
    private void OnTriggerEnter2D(Collider2D hitboxkhac)
    {
        if(hitboxkhac.gameObject.tag == "Ground")
        {
            jump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D hitboxkhac)
    {
        if (hitboxkhac.gameObject.tag == "Ground")
        {
            jump = false;
        }
    }
}
