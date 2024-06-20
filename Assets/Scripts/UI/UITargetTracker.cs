using DG.Tweening;
using UnityEngine;

public class UITargetTracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform pointerUI;
    [SerializeField] private GameObject ArrowIcon;

    [SerializeField] private float minDistance;

    [SerializeField] private UIPointerEvent TrackEvent;

    private Vector3 center;
    private float distance;
    private float angle;
    private Transform secondTarget;

    private void Awake()
    {
        ArrowIcon.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        
        if (target != null)
        {

            if (newTarget == target)
                return;

            float currentDistance = Vector3.Distance(center, target.position);
            float newDistance = Vector3.Distance(center, newTarget.position);

            if (currentDistance < newDistance)
            {
                secondTarget = newTarget;
                return;
            }
        }
        target = newTarget;
        secondTarget = null;
    }

    public void EndTracking()
    {
      target = null;
        if(secondTarget != null)
        {
           SetTarget(secondTarget);
        }
    }


    private void Update() => TrackCloseTemple();

    private void TrackCloseTemple()
    {
        if (target != null)
        {
            center = player.position;
            CalculateAngle();

            pointerUI.rotation = Quaternion.Euler(0, 0, -angle);


            distance = Vector3.Distance(center, target.position);

            if (distance > minDistance)// && distance < maxDistance) // comment the maxDistance check handle in events
                ArrowIcon.SetActive(true);
            else
                ArrowIcon.SetActive(false);

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

    private void OnEnable()
    {
        TrackEvent.SendTargetPos.AddListener(SetTarget);
        TrackEvent.EndTargetTracking.AddListener(EndTracking);
    }
    private void OnDisable()
    {
        TrackEvent.SendTargetPos.RemoveListener(SetTarget);
        TrackEvent.EndTargetTracking.RemoveListener(EndTracking);
    }

}
