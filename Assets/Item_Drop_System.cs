using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Drop_System : MonoBehaviour
{
    // Ben Soars
    public List<GameObject> itemDrops = new List<GameObject>(); // list of all the objects to drop
    // public List<float> dropPercentage = new List<float>(); // the drop percantage of each item

    public int dropRate = 35; // the regular drop rate
    public int ammoDropRate = 15; // the ammo drop rate
    

    public void dropItem(Transform spawnPos)
    {
        int randomNumber = Random.RandomRange(0, 101); // generate a number between 0 and 100

        if (randomNumber < dropRate)
        {
            if (randomNumber < ammoDropRate) // if the number is below the ammo drop rate number
            {
                randomNumber = Random.RandomRange(1, 6); // a 1 in 5 chance
                if (randomNumber == 3) // spawn large ammo crate drop
                {
                    Instantiate(itemDrops[1], spawnPos.position, Quaternion.identity);
                }
                else // else spawn regular ammo crate
                {
                    Instantiate(itemDrops[0], spawnPos.position, Quaternion.identity);
                }
            }  else // spawn a health kit instead
            {
                Instantiate(itemDrops[2], spawnPos.position, Quaternion.identity);
            } 
        } 
    }
    
}
