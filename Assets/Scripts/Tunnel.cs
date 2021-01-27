/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Tunnel class is for panning through the layers of chunks
public class Tunnel : MonoBehaviour
{

    public GameObject layer, chunk;
    private GameObject [] layers = new GameObject[4];
    private float generate, threshold, chunkHeight;
    public float moveSpeed;
    private bool pause;
    private DynamicAspectRatio dar;

    // Start is called before the first frame update
    void Start()
    {
        dar = FindObjectOfType<DynamicAspectRatio>();
        moveSpeed = GameManager.speed;
        pause = true;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pause)
        {
            MoveThroughLayers();
            DepthGuage.Deeper(moveSpeed);
        }
    }

    //Moves layers so that it looks like elevator is moving down. When layer reaches threshold, destroy layer and generate new one
    private void MoveThroughLayers()
    {
        moveSpeed = GameManager.speed;
        for (int i = 0; i < layers.Length; i++)
        {
            Vector2 current = layers[i].transform.localPosition;
            layers[i].transform.localPosition = new Vector2(current.x, current.y + moveSpeed);

            if(layers[i].transform.localPosition.y >= threshold)
            {
                Destroy(layers[i]);
                layers[i] = Instantiate(layer);
                layers[i].transform.SetParent(transform);
                layers[i].transform.localPosition = new Vector2(0, generate);
            }
        }
    }

    //Destroy all layers and create new ones
    public void NewTunnle()
    {
        DestroyTunnel();
        BuildTunnle();
        TogglePause(false);
    }

    //Pause layer movement
    public void TogglePause(bool state)
    {
        pause = state;
    }

    //Destroy all layers
    private void DestroyTunnel()
    {
        foreach (GameObject l in layers) Destroy(l);
    }

    //Create four layers
    public void BuildTunnle()
    {
        chunkHeight = chunk.GetComponent<Chunk>().GetHeight();
        generate = chunkHeight * 1.5f - chunkHeight * 4;
        threshold = chunkHeight * 1.5f;

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = Instantiate(layer);
            layers[i].transform.SetParent(transform);
            layers[i].transform.localPosition = new Vector2(0, chunkHeight * 1.5f - chunkHeight * i);
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
