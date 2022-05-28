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

    public GameObject[] lifes;

    public Player ship;

    private int hits;

    public GameObject laserSound;

    public AudioSource audio;

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
            Destroy(lifes[0].gameObject);
        }
        if (hits == 2)
        {
            Destroy(lifes[1].gameObject);
        }
        if(hits == 3)
        {
            Destroy(lifes[2].gameObject);
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
