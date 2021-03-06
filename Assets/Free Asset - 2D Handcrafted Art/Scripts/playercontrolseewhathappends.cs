﻿using System.Collections;

using UnityEngine;
using Random = UnityEngine.Random;
using Scripts;
namespace Scripts
{
   
    [RequireComponent(typeof(Animator))]
    public class playercontrolseewhathappends : PhysicsObject {

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        [SerializeField] private GameObject graphic;
        
        public Highscore highscore;
        public SceneCounter sceneCounter = null;

        
        [SerializeField] private bool jumping;
        [SerializeField] private AudioSource audio;
        [SerializeField] private AudioClip[] stepSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] gameOverSounds;
        [SerializeField] private AudioClip[] perkSounds;
        [SerializeField] private AudioClip[] winSounds;
        [SerializeField] private AudioClip[] coinSounds;
        [SerializeField] private Animator animator;
        [SerializeField] private string name;
        
        private AnimationClip attack_clip;
        public bool attack;
        private bool canAttack = true;
        
        


        protected override void NewUpdate()
        {
            if (dead1 == 1)
            {
                PlayGameOverSound();
            }
            IfDeadThenGameOver();
            IfGameIsFinished_UpdateHighscore();
            
        }

        private void IfGameIsFinished_UpdateHighscore()
        {
            if (gamefinished)
            {
                highscore.time = sceneCounter.timer < highscore.time ? sceneCounter.timer : highscore.time;
                
            }
        }


        protected override void ComputeVelocity()
        {
            Vector2 move = Vector2.zero;
            //GameObject.FindGameObjectsWithTag("player");

            move.x = Input.GetAxis ("Horizontal");

            if (Input.GetButtonDown ("Jump") && grounded) {
                velocity.y = jumpTakeOffSpeed;
                playJumpSound();

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
                
                   
            }
            targetVelocity = move * maxSpeed;
        
        }

        private void IfDeadThenGameOver()
        {
            if (dead == true)
            {
                animator.SetBool("dead", true);
                StartCoroutine(WaitForDeadAnimation());
            }
        }

        protected override void Attack()
        {
            attack = false || Input.GetButtonDown("Fire1");

            if (animator){
                animator.SetBool("attack",attack);
                
                StartCoroutine(WaitForAnimation());

                attack_clip = GetAnimationClip("Rogue_attack_01"); //to i ta funkcja i zmiennaasdfghjkl
               

            }

            if (attack)
            {
                playAttackSound(); 
            }
            
        }
        
        public AnimationClip GetAnimationClip(string name) {
            if (!animator) return null; // no animator
 
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
                if (clip.name == name) {
                    return clip;
                }
            }
            return null; // no clip by that name
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
            //Debug.Log("play Footstep");

        }

        private void playJumpSound()
        {
            if (!audio) return;
            audio.PlayOneShot(jumpSounds[Random.Range(0,jumpSounds.Length)]);
            //Debug.Log("play jumpSounds");

        }
        private void playAttackSound()
        {
            if (!audio) return;
            audio.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);
            //Debug.Log("play attack sound");
        }
        private void PlayGameOverSound()
        {
            if (!audio) return;
            audio.PlayOneShot(gameOverSounds[Random.Range(0, gameOverSounds.Length)]);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
           // if (col.gameObject.name.Equals("Enemy"))
            //if (GameObject.FindGameObjectWithTag("enemy") )
            if (col.tag.Equals("enemy") )
            {  
                healthAmount -= 0.15f;
               // Debug.Log("masz mniej życia!");
            
            }

            if (col.tag.Equals("healthPotion"))
            {
                
                PlayPerkSound();

                if (healthAmount <= 0.6f)
                {
                  healthAmount += 0.4f;  
                }
                else
                {
                    healthAmount = 1;
                }
                
                Destroy(col.gameObject);
            }

            if (col.tag.Equals("coin"))
            {
                PlayCoinSound();
                coin_amount++;
                Destroy(col.gameObject);
            }

            if (col.tag.Equals("portal"))
            {
                gamefinished = true;
                PlayWinSound();
                StartCoroutine(WaitforGameFinishSound());
            }
            
            
        }

        private void PlayWinSound()
        {
            if (!audio) return;
            audio.PlayOneShot(winSounds[Random.Range(0, winSounds.Length)]);
        }

        private void PlayCoinSound()
        {
            if (!audio) return;
            audio.PlayOneShot(coinSounds[Random.Range(0, coinSounds.Length)]);
        }

        private  void PlayPerkSound()
        {
            if (!audio) return;
            audio.PlayOneShot(perkSounds[Random.Range(0, perkSounds.Length)]);
         }

        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(1f);
            //sceneLoader.LoadScene("Scores");
            //Destroy(gameObject);
        }

        private IEnumerator WaitForDeadAnimation()
        {
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("game_over");
        }
        private IEnumerator WaitforGameFinishSound()
        {
            yield return new WaitForSeconds(6f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("game_over");
        }
        

    }

}



   