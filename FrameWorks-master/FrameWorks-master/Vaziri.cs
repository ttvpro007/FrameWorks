using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goomba : MonoBehaviour
{
    public float speed;
    private float offset;
    private bool move= false;
    Animator anim;
    public bool dead;
    public mario _mario;
    public AudioSource squish_sound;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        _mario = GameObject.Find("mario").GetComponent<mario>();
        rb = GetComponent<Rigidbody2D>();
        speed = 0f;
        anim = GetComponent<Animator>();
        anim.SetBool("dead", false);
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        offset = transform.position.x - _mario.transform.position.x;

        if(offset < 2 && move == false)
        {
            move = true;
            speed = -0.35f;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (dead == true)
        {
            speed = 0f;
            
            StartCoroutine(gombaDie());
            IEnumerator gombaDie()
            {
                yield return new WaitForSeconds(0.4f);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (_mario.transform.position.y > transform.position.y)
            {
                Debug.Log("mario kill goomba");
                squish_sound.Play();
                anim.SetBool("dead", true);
                dead = true;
            }
            else if(_mario.star != true)
            {
                Debug.Log("goomba kill mario");
                if (_mario.fire == true)
                {
                    _mario.fire = false;
                    _mario.isbig = true;
                }
                if (_mario.isbig == false)
                {
                    _mario.isDead = true;
                }
                if (_mario.isbig == true)
                {
                    _mario.isbig = false;
                }
            }
            if (_mario.star == true)
            {
                Destroy(this.gameObject);
            }
        }
        if (other.gameObject.tag == "fireball")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "pipe")
        {
            Debug.Log("goomba change direction");
            speed = -speed;
        }
    }
}
