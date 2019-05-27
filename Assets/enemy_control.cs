using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEditor;
using UnityEngine;


public class enemy_control :   MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform groundDetection;
    private int dead;
    
    [Range(0,5)]public float healthAmount = 1f;
   [SerializeField] public playercontrolseewhathappends player;
    private bool attack;
    private string name;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private Animator animator;
    public GameObject[] healthbarr1;
    private float healthAmount_after_hit;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioSource audioidle;
    [SerializeField] private AudioClip[] idleSounds;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] deathSounds;
    private int counter;
    
    public float offset;
    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;


    private void Start()
    {
        dead = 0;
        counter = 0;        
        healthAmount_after_hit = healthAmount;
        //playidleSound();

        /*
 audio = GetComponent<AudioSource>();
        audioidle = GetComponent<AudioSource>();*/
        
       // healthbarr1 = GameObject.FindGameObjectsWithTag("HealthBar");// nie może tak być -> kazdy pasek z tym tagiem znika !
       // target = GetComponent<Transform>();
    }
/*    void LateUpdate()
    {
        targetPos = target.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }*/

    private void Update()
    {
        KillPlayerIfCloseEnough();
        PatrolTheArea();
        UpdateLiveOnBar();
        IfZeroHealthThenDie();
        counter++;

        //audio.PlayScheduled(1);
        

    }

    private void PatrolTheArea()
    {
        transform.Translate(UnityEngine.Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    private void UpdateLiveOnBar()
    {
        if (healthBar != null)
        {
            healthBar.SetSize(healthAmount_after_hit / healthAmount);
        }
    }

    private void IfZeroHealthThenDie()
    {
        if (isDead())
        {  
            dead++;
            playDeathSound();
            if (healthBar != null)
            {
                Destroy(healthBar.gameObject); 
            }

            //healthBar.GetComponent<Renderer>().enabled = false;
            if (animator)
            {
                if (dead == 1)
                {   
                    animator.SetBool("dead", true);
                    StartCoroutine(WaitAndPlay());  
                }
                
            }
        }
    }

    private bool isDead()
    {
        return healthAmount_after_hit <= 0.001;
       
    }

    private void KillPlayerIfCloseEnough()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 3)
        {
        
            //animator.SetBool("attack",false);
            // transform.LookAt(player.transform); - nie działą na 2d xD
            /*Vector3 targetPos = player.transform.position;
            Vector3 targetPosFlattened = new Vector3(targetPos.x, targetPos.y, 0);
            transform.LookAt(targetPosFlattened);*/
            
            
            //Debug.Log("attack:" + animator.GetBool("attack"));
            if (animator && dead ==0)
            {
                animator.SetBool("attack", true);
                if (counter % 100 == 11)
                {
                  playAttackSound();  
                }
                
                //StartCoroutine(WaitAndPlay());
            }
        }
        else
        {
        animator.SetBool("attack",false);
        }
               
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        attack = player.isAttacking();
        name = player.getName();
       
        if (name == "Rogue_01" && attack) //col.gameObject.animation.Equals("attack")  )  col.gameObject.name.Equals("Rogue_01")
        {
            healthAmount_after_hit -= 0.2f;
          //  Debug.Log("healthamount_of_frikin  enemy = "+ healthAmount_after_hit);
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        attack = player.isAttacking();
        name = player.getName();
        if (name == "Rogue_01" && attack) //col.gameObject.animation.Equals("attack")  )  col.gameObject.name.Equals("Rogue_01")
        {
            healthAmount_after_hit -= 0.2f;
        }
        
    }
    
    private IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(1f);
        //sceneLoader.LoadScene("Scores");
        Destroy(gameObject);
    }
    private void playAttackSound()
    {
        if (!audio) return;
        audio.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);
        //Debug.Log("play attack sound");
    }
    private void playidleSound()
    {
        if (!audio) return;
        audioidle.Play(0);
        
        //audio.Play(idleSounds);
        //Debug.Log("play attack sound");
    }

    private void playDeathSound()
    {
            if (!audio) return;
            audio.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
            //Debug.Log("play attack sound");
    }
    

}
