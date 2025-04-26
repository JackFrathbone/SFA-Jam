using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpriteRandomizer : MonoBehaviour
{
    public List<Sprite> sprites;

    public SpriteRenderer spriteRenderer;


    public bool randomizeOnStart = true;


    void Start()
    {
        if (randomizeOnStart)
        {
            RandomizeSpriteFunction();
        }

    }


    public void RandomizeSpriteFunction()
    {
        int randomIndex = Random.Range(0, sprites.Count);


        spriteRenderer.sprite = sprites[randomIndex];
    }
}
