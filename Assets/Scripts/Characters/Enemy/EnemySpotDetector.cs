using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpotDetector : MonoBehaviour
{
    [HideInInspector]
    public List<AISpot> spotList;


    // Start is called before the first frame update
    public void GetEnemySpotsList()
    {
        spotList = new List<AISpot>();
        spotList = GridManager.Instance.enemySpots[GetComponent<Enemy>().tier];
    }

}
