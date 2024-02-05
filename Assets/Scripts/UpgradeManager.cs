using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public class UpgradeObject : MonoBehaviour
    {
        public string upgradeName, upgradeEffect;
        public float effectMultiplier;

        public UpgradeObject(string name, string effect, float multiplier)
        {
            this.upgradeName = name;
            this.upgradeEffect = effect;
            this.effectMultiplier = multiplier;
        }
    }
    // Start is called before the first frame update

    UpgradeObject[] commonUpgradeArray =new UpgradeObject[]
    {
        new UpgradeObject("Charge", "Charge", 0.25f),
        new UpgradeObject("Gaze", "Gaze", 0.25f),
    };

    UpgradeObject[] uncommonUpgradeArray =new UpgradeObject[]
    {
        new UpgradeObject("Damage", "Damage", 0.25f),
        new UpgradeObject("Double", "Double", 0.25f),
        new UpgradeObject("Slow", "Slow", 0.25f),
    };

    UpgradeObject[] rareUpgradeArray =new UpgradeObject[]
    {
        new UpgradeObject("Pierce", "ChargePierce", 0.25f),
        new UpgradeObject("Full", "Full", 0.25f),
        new UpgradeObject("EyeFull", "Eyefull", 0.25f),
    };

    public UpgradeObject upgrade1, upgrade2, selectedUpgrade;
    float commonChance, uncommonChance, rareChance;

    int savedIndex, savedIndex2;

    void Start()
    {
        
    }
    
    // Update is called once per frame
    public void RandomizeUpgrade(UpgradeObject slot, int indexToSave)
    {
        int randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum <= rareChance)
        {
            int index = RandomizeSelection(rareUpgradeArray);
            slot = rareUpgradeArray[index];
            indexToSave = index;
        }
        else if (randomNum <= uncommonChance)
        {
            int index = RandomizeSelection(uncommonUpgradeArray);
            slot = uncommonUpgradeArray[index];
            indexToSave = index;
        }
        else
        {
            int index = RandomizeSelection(rareUpgradeArray);
            slot = commonUpgradeArray[RandomizeSelection(commonUpgradeArray)];
            indexToSave = index;
        }
    }
    public int RandomizeSelection(UpgradeObject[] array)
    {
        int randomPick = UnityEngine.Random.Range(0, array.Length);
        return randomPick;
    }

    public void parseUpgrade(UpgradeObject slot)
    {
        switch (slot.upgradeEffect)
        {
            case "Charge":
                break;
            case "Gaze":
                break;
            case "Damage":
                break;
            case "Double":
                break;
            case "Slow":
                break;
            case "Pierce":
                break;
            case "Full":
                break;
            case "EyeFull":
                break;
            default:
                break;
        }
    }
}
