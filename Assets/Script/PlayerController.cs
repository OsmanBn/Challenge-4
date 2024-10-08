using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0, transform.position.y + 2f, 0);
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        powerupIndicator.transform.position = transform.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            hasPowerup = true;
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 repulseDirection = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(repulseDirection * powerupStrength, ForceMode.Impulse);
            Debug.Log("Player collided with " + collision.gameObject + "with powerup set to" + hasPowerup);
        } 
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    
}
