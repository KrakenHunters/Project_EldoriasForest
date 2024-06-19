using DG.Tweening;
using UnityEngine;

public class UITargetTracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform pointerUI;
    [SerializeField] private GameObject ArrowIcon;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [SerializeField] private Ease easeMode = Ease.Linear;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private UIPointerEvent TrackEvent;

    private Vector3 center;
    private float distance;
    private float angle;

    private void Awake()
    {
        ArrowIcon.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        if(newTarget == target)
            return;
        
        if (target != null)
        {
            float currentDistance = Vector3.Distance(center, target.position);
            float newDistance = Vector3.Distance(center, newTarget.position);

            if (currentDistance < newDistance)
            {
                return;
            }
        }
        target = newTarget;
    }

    public void EndTracking()
    {
        target = null;
    }


    private void Update() => TrackCloseTemple();

    private void TrackCloseTemple()
    {
        if (target != null)
        {
            // SetCenter();
            center = player.position;
            CalculateAngle();

            // Rotate the pointer UI to point towards the target direction
            pointerUI.DORotate(new Vector3(0, 0, -angle), 1f).SetEase(easeMode);


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

        if(distance > maxDistance)
        {
           EndTracking();
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

    //Dont use its crazy $$$$  to use every frame
    /*private void FindCenter()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            center = hit.point;
        }
    }*/
}
