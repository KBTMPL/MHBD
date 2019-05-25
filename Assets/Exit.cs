using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public SceneCounter sceneCounter = null;
    public Highscore highscore = null;
            
    
    [SerializeField]
    private SceneLoader sceneLoader = null;
    
    //TODO

    private AudioSource audioSource = null;
    private bool gameFinished = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !gameFinished)
        {
            //TODO
            gameFinished = true;
            audioSource.Play();
            highscore.time = sceneCounter.timer < highscore.time ? 
                sceneCounter.timer :
                highscore.time;
            
            StartCoroutine(WaitAndPlay());
            
            
        }
    }

    private IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        sceneLoader.LoadScene("Scores");
    }
}
