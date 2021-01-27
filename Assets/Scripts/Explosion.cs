using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine("Dissipation");
    }

    private IEnumerator Dissipation()
    {
        anim.SetTrigger("explode");
        yield return new WaitForSeconds(.85f);
        Destroy(gameObject);
    }
}
