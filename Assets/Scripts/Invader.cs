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

    private RaycastHit2D raycastHit2DUP;
    private RaycastHit2D raycastHit2DLEFT;
    private RaycastHit2D raycastHit2DRIGHT;
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

                chainDestruction();
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
                chainDestruction();
                this.killed.Invoke();
                this.gameObject.SetActive(false);
                audio.Play();
            }           
        }
    }

    public void chainDestruction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2.0f);
        if (this.gameObject.CompareTag(hit.collider.gameObject.tag))
        {
            invadersMatches = GameObject.FindGameObjectsWithTag(hit.collider.gameObject.tag);

            foreach (GameObject invader in invadersMatches)
            {
                if (this.transform.localPosition.x == invader.transform.localPosition.x && ((this.transform.localPosition.y) - (invader.transform.localPosition.y) == -2))
                {
                    invader.SetActive(false);
                    this.killed.Invoke();
                }
                if (this.transform.localPosition.x == invader.transform.localPosition.x && ((this.transform.localPosition.y) - (invader.transform.localPosition.y) == 2))
                {
                    invader.SetActive(false);
                    this.killed.Invoke();
                }

                if (this.transform.localPosition.y == invader.transform.localPosition.y && ((this.transform.localPosition.x) - (invader.transform.localPosition.x) == -2))
                {
                    invader.SetActive(false);
                    this.killed.Invoke();
                }
                if (this.transform.localPosition.y == invader.transform.localPosition.y && ((this.transform.localPosition.x) - (invader.transform.localPosition.x) == 2))
                {
                    invader.SetActive(false);
                    this.killed.Invoke();
                }
            }
        }
    }
}
