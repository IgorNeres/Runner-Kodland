
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] GameObject loseMenu;  
    AudioSource audioSource;
    [SerializeField] AudioClip loseSound;
    [SerializeField] ParticleSystem loseEffect;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(loseEffect, transform.position, Quaternion.identity);
            audioSource.Stop();
            audioSource.PlayOneShot(loseSound);
            Time.timeScale = 0f;
            loseMenu.SetActive(true);
        }    
    }
}