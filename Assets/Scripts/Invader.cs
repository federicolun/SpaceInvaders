using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;

    public float animationTime = 1.0f;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    public System.Action killed;

    private float distance = 2.0f;
    public int life = 2;

    private GameObject gameObjectWithSameTag;
    public GameObject[] invadersMatches;

    public AudioSource audio;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        if(_animationFrame >= this.animationSprites.Length)
        {
            _animationFrame = 0;
        }
        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            

            if (this.gameObject.name.Equals("InvaderBlue(Clone)") || this.gameObject.name.Equals("InvaderWhite(Clone)"))
            {

                chainDestruction(this.gameObject);
                this.killed.Invoke();
                this.gameObject.SetActive(false);
                audio.Play();
            }

            else if (this.gameObject.name.Equals("InvaderRed(Clone)") || this.gameObject.name.Equals("InvaderYellow(Clone)"))
            {
                this.life--;
            }

            if(life == 0)
            {
                chainDestruction(this.gameObject);
                this.killed.Invoke();
                this.gameObject.SetActive(false);
                audio.Play();
            }           
        }
    }

    public void chainDestruction(GameObject invaderKilled)
    {
        invadersMatches = GameObject.FindGameObjectsWithTag(this.gameObject.tag);

        foreach (GameObject invader in invadersMatches)
        {
            if (invaderKilled.transform.localPosition.x == invader.transform.localPosition.x && ((invaderKilled.transform.localPosition.y) - (invader.transform.localPosition.y) == -2))
            {
                invader.SetActive(false);
                this.killed.Invoke();
                chainDestruction(invader);
            }
            if (invaderKilled.transform.localPosition.x == invader.transform.localPosition.x && ((invaderKilled.transform.localPosition.y) - (invader.transform.localPosition.y) == 2))
            {
                invader.SetActive(false);
                this.killed.Invoke();
                chainDestruction(invader);
            }

            if (invaderKilled.transform.localPosition.y == invader.transform.localPosition.y && ((invaderKilled.transform.localPosition.x) - (invader.transform.localPosition.x) == -2))
            {
                invader.SetActive(false);
                this.killed.Invoke();
                chainDestruction(invader);
            }
            if (invaderKilled.transform.localPosition.y == invader.transform.localPosition.y && ((invaderKilled.transform.localPosition.x) - (invader.transform.localPosition.x) == 2))
            {
                invader.SetActive(false);
                this.killed.Invoke();
                chainDestruction(invader);
            }
        } 
    }
}
