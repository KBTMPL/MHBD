using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);
    protected bool dead;

    public static float healthAmount;


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    [SerializeField] protected HealthBar healthBar;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start ()
    {
        healthAmount = 1f;
        healthBar.SetSize(1f);
        
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
        
    }
    
    void Update () 
    {
       
        targetVelocity = Vector2.zero;
                ComputeVelocity (); 
                Attack();
                computeHealth();
                healthBar.SetSize(healthAmount);
        
        
    }

    protected virtual void computeHealth()
    {
        if (healthAmount < 0.3f)
        {
            Debug.Log("masz mało życia");
           // healthBar.SetColor(Color.white);
        }
        if (healthAmount <= 0)
        {
            dead = true;        // zrób tak aby była animacja śmierci i ekran game over - powrót do menu
            
            
           // Destroy(gameObject);
        }
        
    }

    protected virtual void ComputeVelocity()
    {
    
    }
    protected virtual void Attack()
    {
    
    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement (move, false);

        move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance) 
        {
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                Vector2 currentNormal = hitBufferList [i].normal;
                if (currentNormal.y > minGroundNormalY) 
                {
                    grounded = true;
                    if (yMovement) 
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot (velocity, currentNormal);
                if (projection < 0) 
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList [i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

}