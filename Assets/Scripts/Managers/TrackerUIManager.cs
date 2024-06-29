using System.Collections.Generic;
using UnityEngine;

public class TrackerUIManager : Singleton<TrackerUIManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform pointerUI;
    [SerializeField] private GameObject ArrowIcon;

    [SerializeField] private float minDistance;

    public List<Transform> villages = new List<Transform>();

    private Transform target;

    [HideInInspector] public bool isFightingWitch = false;
    [HideInInspector] public bool challengeCompleted = false;
    [HideInInspector] public bool isHealthlow = false;
    private bool isClose = false;

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
        if (Vector3.Distance(center, target.position) < minDistance)
            isClose = true;
        else
            isClose = false;

        ArrowIcon.SetActive(ShowTacker());
        if (!ShowTacker())
            return;
        FindClosestVillage();
        if (target != null)
        {
            center = player.position;
            CalculateAngle();
            pointerUI.rotation = Quaternion.Euler(0, 0, -angle);

        }
        else
            ArrowIcon.SetActive(false);

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

        float currentDistance = Vector3.Distance(center, target.position);

        foreach (Transform village in villages)
        {
            float villageDistance = Vector3.Distance(center, village.position);

            if (villageDistance < currentDistance)
            {
                target = village;
            }
        }
    }

    private bool ShowTacker()
    {
        return !isFightingWitch && !isClose && (challengeCompleted || isHealthlow) ;
    }
}
