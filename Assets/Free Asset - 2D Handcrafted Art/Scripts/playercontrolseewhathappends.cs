﻿using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class playercontrolseewhathappends : PhysicsObject {

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        [SerializeField] private GameObject graphic;
        [SerializeField] private bool jumping;
        [SerializeField] private AudioSource audio;
        [SerializeField] private AudioClip[] stepSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private string name;
        private float timeElapsed;
        private float delayBeforeScene = 1f;
    
        public bool attack;
        // Use this for initialization

        /*void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();
    }*/
    

        protected override void ComputeVelocity()
        {
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis ("Horizontal");

            if (Input.GetButtonDown ("Jump") && grounded) {
                velocity.y = jumpTakeOffSpeed;
                Jump();

            } else if (Input.GetButtonUp ("Jump")) 
            {
                if (velocity.y > 0) {    //no double jumping
                    velocity.y = velocity.y * 0.5f;
                }
            }
        
            FlipCharacterDependOnDirection(move);

            if (animator)
            {
                animator.SetBool ("grounded", grounded);
                animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
                if (dead == true)
                {
                    animator.SetBool("dead", true);
                    timeElapsed += Time.deltaTime;
                    
                    if(timeElapsed >= delayBeforeScene)
                        UnityEngine.SceneManagement.SceneManager.LoadScene("game_over");


                }
                   
            }

            targetVelocity = move * maxSpeed;
        
        }

        protected override void Attack()
        {
            attack = false || Input.GetButtonDown("Fire1");

            if (animator){
                animator.SetBool("attack",attack);
            }
        }

        public bool isAttacking()
        {
            if (animator){
                return animator.GetBool("attack");
            }
            else
            {
                return false;
            }
            
            
            /*if (attack == true)
                return true;
            else
                return false;*/
        }

        public string getName()
        {
            return name;
        }

        private void FlipCharacterDependOnDirection(Vector2 move)
        {
            if (graphic)
            {
                //if(Input.GetKey(KeyCode.RightArrow))
                if (move.x > 0.01f)
                {
                    graphic.transform.localScale = new Vector3(0.1f, transform.localScale.y, transform.localScale.z);


                    if (graphic.transform.localScale.x == -0.1f)
                    {
                        //Debug.Log("very bad word");
                        graphic.transform.localScale = new Vector3(0.1f, transform.localScale.y, transform.localScale.z);
                    }
                }
                else if (move.x < -0.01f)
                {
                    if (graphic.transform.localScale.x == 0.1f)
                    {
                        graphic.transform.localScale = new Vector3(-0.1f, transform.localScale.y, transform.localScale.z);
                    }
                }
            }
        }

        void Footstep()
        {
            if (!audio) return;
            audio.PlayOneShot(stepSounds[Random.Range(0,stepSounds.Length)]);
            Debug.Log("play Footstep");

        }

        private void Jump()
        {
            if (!audio) return;
            audio.PlayOneShot(jumpSounds[Random.Range(0,jumpSounds.Length)]);
            Debug.Log("play jumpSounds");

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name.Equals("Enemy"))
            {
                healthAmount -= 0.1f;
                Debug.Log("masz mniej życia!");
            
            }
        }
    
    }
}



   