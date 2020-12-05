using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : Interactable
{
    public ItemModule imodule = new ItemModule();
    public Player player;
    public int needSoul;
    public float moveSpeed;
    public float dashSpeed;
    public float maxDashEnergy;
    public float maxLineEnergy;

    public override void Interact()
    {
        if(player.haveSouls >= needSoul)
            GiveStat();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        imodule.type = ItemModule.ItemType.Item1;
        imodule.InitItem();
        needSoul = imodule.needSoul;
        moveSpeed = imodule.moveSpeed;
        maxDashEnergy = imodule.maxDashEnergy;
        maxLineEnergy = imodule.maxLineEnergy;
    }

    void GiveStat()
    {
        player.haveSouls -= needSoul;
        player.moveSpeed += moveSpeed;
        player.maxDashEnergy += maxDashEnergy;
        player.maxLineEnergy += maxLineEnergy;
    }
}
