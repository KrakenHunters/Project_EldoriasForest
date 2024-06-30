using System.Collections.Generic;
using UnityEngine;

public class TrackerUIManager : Singleton<TrackerUIManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform pointerUI;
    [SerializeField] private GameObject ArrowIcon;
    [SerializeField] private float minDistance;

    public List<Transform> villages = new List<Transform>();

    [HideInInspector] public bool isFightingWitch = false;
    [HideInInspector] public bool challengeCompleted = false;
    [HideInInspector] public bool isHealthlow = false;

    private bool isClose = false;
    private Transform target;
    private Vector3 center;
    private float angle;

    private void Awake()
    {
        target = this.transform;
        ArrowIcon.SetActive(false);
    }

    private void FixedUpdate() => TrackTarget();

    private void TrackTarget()
    {
        center = player.position;

        float distanceToTarget = Vector3.Distance(center, target.position);
        isClose = distanceToTarget < minDistance;

        bool shouldShowTracker = ShowTracker();
        ArrowIcon.SetActive(shouldShowTracker);

        if (!shouldShowTracker)
            return;

        FindClosestVillage();
        if (target != null)
        {
            CalculateAngle();
            pointerUI.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            ArrowIcon.SetActive(false);
        }
    }

    private void CalculateAngle()
    {
        Vector3 targetDirection = target.position - center;
        targetDirection.y = 0;
        angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
    }

    private void FindClosestVillage()
    {
        if (villages.Count == 0)
            return;

        float closestDistance = float.MaxValue;
        foreach (Transform village in villages)
        {
            float villageDistance = Vector3.Distance(center, village.position);
            if (villageDistance < closestDistance)
            {
                closestDistance = villageDistance;
                target = village;
            }
        }
    }

    private bool ShowTracker()
    {
        return !isFightingWitch && 
               !isClose && CheckCondition();
    }

    private bool CheckCondition()
    {
       if(!GameManager.Instance.pData.tutorialDone)
       {
           return false;
       }  
       return (challengeCompleted || isHealthlow);
    }
}
