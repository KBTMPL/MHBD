using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_fallow_character : MonoBehaviour
   
{ 
    public float speed;
    private Transform target;
    public string tag;

    public float stoppingDistance;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("enemy_2").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
    }
}
