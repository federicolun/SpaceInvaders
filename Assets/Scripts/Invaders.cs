using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;

    public Projectile missilePrefab;

    private bool _laserActive;

    public int rows = 4;

    public int columns = 10;

    public float paddingUnits = 2.0f;

    private Vector3 _direction = Vector2.right;

    public AnimationCurve speed;

    private int missileAttackRate = 2;

    public int amountKilled { get; private set; }

    public int totalInvaders => this.rows * this.columns;

    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    public int amountAlived => this.totalInvaders - this.amountKilled;

    List<Transform> invadersCanShoot = new List<Transform>();

    private int rowShooting = -3;

    private int invadersDead;

    public int score;

    public Text textScore;

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = paddingUnits * (this.columns - 1);
            float height = paddingUnits * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + row * paddingUnits, 0.0f);

            for(int col = 0; col < this.columns; col++)
            {
                int randomPrefab = UnityEngine.Random.Range(0, prefabs.Length);

                Invader invader = Instantiate(this.prefabs[randomPrefab], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * paddingUnits;
                invader.transform.localPosition = position;
            }

        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), this.missileAttackRate, this.missileAttackRate);
    }


    private void Update()
    {
        Debug.Log("total de vivooos : " + amountAlived);

        if(amountAlived != 0)
        {
            score = totalInvaders - amountAlived;
            textScore.text = "Score : " + score * 10;
        }

        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if(_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
            }
            else if(_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
        }
        WinCondition();
    }

    private void Shoot()
    {
        if (!_laserActive)
        {
            invadersCanShoot.Clear();

            foreach (Transform invader in this.transform)
            {               
                if(invader.localPosition.y == rowShooting && invader.gameObject.activeSelf)
                {
                    invadersCanShoot.Add(invader);

                    var random = new System.Random();
                    int invaderShooter = random.Next(invadersCanShoot.Count);

                    if (UnityEngine.Random.value < 1.0f / (float)invaderShooter)
                    {
                        Projectile projectile = Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                        projectile.destroyed += LaserDestroyed;
                        _laserActive = true;
                        Debug.Log("Disparo pium pium");
                    }
                }
                
            }
            if(invadersCanShoot.Count == 0)
            {
                rowShooting += 2;
            }
            Debug.Log("invadeeers: " + invadersCanShoot.Count);
            Debug.Log("rowShooting: " + rowShooting);
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void InvaderKilled()
    {
        this.amountKilled++;
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void WinCondition()
    {
        if(amountAlived == 0)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

}
