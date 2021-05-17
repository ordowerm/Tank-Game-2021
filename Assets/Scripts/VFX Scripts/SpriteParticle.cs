using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteParticle : MonoBehaviour
{
    public SpriteRenderer srenderer;
    public Sprite[] sprites;
    public float lifespan;
    float timer;// = 0;
    public bool randomizeDir;
    public bool randomizeSpawnPosition;
    public float speed;
    public float jitter;
    public bool shrink;

    public Vector3 dir=new Vector3();
    public Vector3 offset=new Vector2();

    // Start is called before the first frame update
    void Awake()
    {
        timer = 0;
        if (sprites.Length > 0)
        {
            srenderer.sprite = sprites[((int)(Random.value * int.MaxValue)) % sprites.Length];
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.value * 360.0f));
        
        if (randomizeDir)
        {
            dir = speed * (new Vector3(-1 + 2*Random.value, -1 + 2*Random.value,0)); //not gonna bother normalizing this
        }
        if (randomizeSpawnPosition)
        {
            offset = jitter * (new Vector3(-1+2*Random.value, -1 + 2*Random.value,0));
            transform.localPosition += offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position += dir*Time.deltaTime;
        if (shrink) { transform.localScale = (lifespan - timer) / lifespan * (new Vector3(1, 1, 1)); }
        if (timer > lifespan) { Destroy(gameObject); }
    }
}
