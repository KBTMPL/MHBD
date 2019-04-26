using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEditor;
using UnityEngine;

public class enemy_control :   MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform groundDetection;
    private float healthAmount = 1f;
   [SerializeField] public playercontrolseewhathappends player;
    private bool attack;
    private string name;
    [SerializeField] protected HealthBar healthBar;


    private void Start()
    {
        healthAmount = 1f;
        healthBar.SetSize(1f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right*speed*Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0,-180,0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0,0,0);
                movingRight = true;
            }
        }
        
        healthBar.SetSize(healthAmount);
        
        if (healthAmount <= 0)
        {
            Destroy (gameObject);
        }
            
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        attack = player.isAttacking();
        name = player.getName();
        Debug.Log(name +"   "+ attack +" "+ healthAmount);
        if (name == "Rogue_01" && attack) //col.gameObject.animation.Equals("attack")  )  col.gameObject.name.Equals("Rogue_01")
        {
            healthAmount -= 0.2f;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        attack = player.isAttacking();
        name = player.getName();
        Debug.Log(name +"   "+ attack +" "+ healthAmount);
        if (name == "Rogue_01" && attack) //col.gameObject.animation.Equals("attack")  )  col.gameObject.name.Equals("Rogue_01")
        {
            healthAmount -= 0.2f;
        }
        
    }
}
