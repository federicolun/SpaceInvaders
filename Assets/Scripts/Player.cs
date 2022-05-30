using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    public float speed = 50.0f;

    private bool _laserActive;

    public Projectile laserPrefab;

    public GameObject lifeCounter;

    public Transform respawnPoint;

    public Player ship;

    public int hits;

    public AudioSource audio;

    public Collider2D collider;

    public Player playerScript;

    private float leftLimit = -14.41f;
    private float rightLimit = 14.33f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y);
    }

    private void Shoot()
    {
        if (!_laserActive)
        {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
            audio.Play();
        }
        
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            hits++;

            Destroy(lifeCounter.transform.GetChild(0).gameObject);
            Destroy(this.gameObject);
            
            Player shipRespawned = Instantiate(ship, respawnPoint.position, Quaternion.identity);
            shipRespawned.transform.GetComponent<Collider2D>().enabled = true;
            shipRespawned.transform.GetComponent<Player>().enabled = true;
            shipRespawned.transform.GetComponent<AudioSource>().enabled = true;
            
        }

        if (hits == 2)
        {
            Destroy(lifeCounter.transform.GetChild(0).gameObject);
            Destroy(this.gameObject);

            Player shipRespawned = Instantiate(ship, respawnPoint.position, Quaternion.identity);
            shipRespawned.transform.GetComponent<Collider2D>().enabled = true;
            shipRespawned.transform.GetComponent<Player>().enabled = true;
            shipRespawned.transform.GetComponent<AudioSource>().enabled = true;
        }

        if(hits == 3)
        {
            Destroy(lifeCounter.transform.GetChild(0).gameObject);
            Destroy(this.gameObject);
            SceneManager.LoadScene(0, LoadSceneMode.Single);           
        }
    }
}
