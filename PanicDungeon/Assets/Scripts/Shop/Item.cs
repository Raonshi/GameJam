using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Player player;
    public int needSoul;
    public float moveSpeed;
    public float dashSpeed;
    public float maxDashEnergy;
    public float maxLineEnergy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        //GiveStat();
    }

    //void GiveStat()
    //{
    //    if (player.isBuy)
    //    {
    //        player.haveSouls -= needSoul;
    //        player.moveSpeed += moveSpeed;
    //        player.maxDashEnergy += maxDashEnergy;
    //        player.maxLineEnergy += maxLineEnergy;
    //    }
    //}
}
