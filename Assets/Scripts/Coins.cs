using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private static TextMeshProUGUI coinPurse;
    private static int totalCoins;

    // Start is called before the first frame update
    void Start()
    {
        coinPurse = GetComponent<TextMeshProUGUI>();
        coinPurse.text = "Coins: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddSubtractCoins(int num)
    {
        totalCoins += num;
        coinPurse.text = "Coins: " + totalCoins;
    }
}
