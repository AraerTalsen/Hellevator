using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FollowerSpawner : MonoBehaviour
{
    public GameObject instance, indicator;
    public static bool pause = true, waiting = true;
    public Image glass, incoming;
    public static int spawnRate = 6, maxWeight = 16, spawnDelay = 10, enemyCap = 2, enemyCount = 0;
    private float mod;
    private int blockSize;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        DynamicAspectRatio dar = FindObjectOfType<DynamicAspectRatio>();
        mod = dar.GetModifier();
        blockSize = BlockManager.blockSize;
        anim = indicator.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!waiting && !pause && enemyCount <= enemyCap) StartCoroutine("CallNewSpawn");
    }

    private IEnumerator Spawn()
    {
        Vector2 spawnPoint = RandomVector2();
        FadeInIndicator(spawnPoint);
        yield return new WaitForSeconds(1.75f);
        anim.SetTrigger("out");
        yield return new WaitForSeconds(1.25f);
        GameObject g = Instantiate(instance, spawnPoint, Quaternion.identity);
        g.transform.localScale *= blockSize * mod;
        waiting = false;
        enemyCount++;
    }

    private void FadeInIndicator(Vector2 spawnPos)
    {
        //int side = spawnPos.x > 0 ? -1 : 1;
        Vector2 flip = spawnPos.x > 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
        indicator.transform.localScale = flip;

        //indicator.transform.localPosition = spawnPos.x < 0 ? worldLeftPos : worldRightPos;
        //float buffer = side * (DynamicAspectRatio.worldWidth * .05f );
        //spawnPos.x = -side * DynamicAspectRatio.worldWidth * .5f + buffer;
        float glassScale = glass.rectTransform.localScale.x;
        glass.rectTransform.position = spawnPos.x < 0 ? new Vector2(spawnPos.x + 2 + glassScale, spawnPos.y) : new Vector2(spawnPos.x - 2 - glassScale, spawnPos.y);
        anim.SetTrigger("in");
    }

    private Vector2 RandomVector2()
    {
        Vector2 widthRange = Layer.widthBorders;
        Vector2 heightRange = Layer.heightBorders;

        float xPos = Random.Range(0, 2) == 0 ? widthRange.x - 2 : widthRange.y + 2;
        float yPos = Random.Range(heightRange.x + 1, heightRange.y);

        return new Vector2(xPos, yPos);
    }

    private IEnumerator CallNewSpawn()
    {
        waiting = true;
        float waitTime = Random.Range(1, spawnDelay);
        yield return new WaitForSeconds(waitTime);

        float chance = Random.Range(1, maxWeight);

        if (!pause && chance <= spawnRate) StartCoroutine("Spawn");
        else waiting = false;

        StopCoroutine("CallNewSpawn");
    }
}
