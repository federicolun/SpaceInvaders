using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public Sprite explosionSprite;

    public int diameter = 20;

    private Collider2D collider;

    private SpriteRenderer spriteRenderer;

    private Sprite spriteBarrier;

    private Texture2D explosionTex;

    private float _width;
    private float _height;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        spriteBarrier = spriteRenderer.sprite;

        _width = collider.bounds.size.x * spriteBarrier.pixelsPerUnit;
        _width = collider.bounds.size.y * spriteBarrier.pixelsPerUnit;

        explosionTex = explosionSprite.texture;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.tag.Contains("Laser") || collision.tag.Contains("Missile"))
        {
            return;
        }

        Vector2 hitPoint = collision.transform.position;
        if(CheckPoint(hitPoint, collision))
        {
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.tag.Contains("Laser") || collision.tag.Contains("Missile")) return;
      
        Vector2 hitPoint = collision.transform.position;
        if (CheckPoint(hitPoint, collision)) return;

    }
    
    private bool CheckPoint(Vector2 hitPoint, Collider2D collider)
    {
        RaycastHit2D hit = Physics2D.Raycast(hitPoint, Vector2.up, 5.0f);
        
        if(hit.point != Vector2.zero && IsOpaquePixel(hitPoint))
        {
            Destroy(collider.gameObject);
            return true;
        }
        return false;
    }
    
    private bool IsOpaquePixel(Vector2 hitPoint)
    {
        int radius = diameter / 2;

        Vector2 localPosition = hitPoint - (Vector2)transform.position;
        localPosition += (Vector2)spriteRenderer.sprite.bounds.extents;
        localPosition *= spriteRenderer.sprite.pixelsPerUnit;

        Vector2 pixelUV = Vector2.zero;
        pixelUV.x = localPosition.x;
        pixelUV.y = localPosition.y;

        Texture2D texture = spriteRenderer.sprite.texture;
        _width = texture.width;
        _height = texture.height;

        int centerX = (int)pixelUV.x;
        int centerY = (int)pixelUV.y;

        for(int x = centerX - radius; x < centerX + radius; x++)
        {
            for(int y = centerY - radius; y < centerY + radius; y++)
            {
                if (x < 0 || y < 0 || x >= _width || y >= _height) continue;

                if(texture.GetPixel(x,y).a != 0)
                {
                    MakeAHoleInBarrier(new Vector2(x, y));
                    return true;
                }
            }
        }
        return false;
    }

    private void MakeAHoleInBarrier(Vector2 contactPoint)
    {
        int radius = diameter / 2;
        Texture2D texture = spriteRenderer.sprite.texture;

        Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        newTexture.SetPixels32(texture.GetPixels32());

        int _explosionWidth = explosionTex.width;
        int _explosionHeight = explosionTex.height;

        int a, b = 0;

        for(int y = (int)contactPoint.y - radius; y < (int)contactPoint.y + radius; y++)
        {
            for(int x = (int)contactPoint.x - radius; x < (int)contactPoint.x + radius; x++)
            {
                if (x < 0 || y < 0 || x >= _width || y >= _height) continue;

                a = x - (int)contactPoint.x;
                b = y - (int)contactPoint.y;

                if((a * a) + (b * b) <= (radius * radius))
                {
                    newTexture.SetPixel(x, y, Color.clear);
                }
            }
        }

        newTexture.Apply();

        Rect rect = spriteRenderer.sprite.rect;
        spriteRenderer.sprite = Sprite.Create(newTexture, rect, new Vector2(0.5f, 0.5f));
    }
}
