using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModule
{
    public int needSoul;
    public float moveSpeed;
    public float dashSpeed;
    public float maxDashEnergy;
    public float maxLineEnergy;

    public enum ItemType
    {
        Item1, Item2, Item3
    }

    public ItemType type;


    public void InitItem()
    {
        switch (type)
        {
            case ItemType.Item1:
                needSoul = 10;
                moveSpeed = 0f;
                dashSpeed = 0f;
                maxDashEnergy = 10f;
                maxLineEnergy = 0f;
                break;
            case ItemType.Item2:
                needSoul = 15;
                moveSpeed = 0f;
                dashSpeed = 0f;
                maxDashEnergy = 0f;
                maxLineEnergy = 1f;
                break;
            case ItemType.Item3:
                needSoul = 5;
                moveSpeed = 1f;
                dashSpeed = 0f;
                maxDashEnergy = 0f;
                maxLineEnergy = 0f;
                break;
        }
    }
}
