using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PortalTrackerArrow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private RectTransform pointerUI;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [SerializeField] private float angle;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Ease easeMode = Ease.Linear;
    [SerializeField] private LayerMask groundLayer;

    Vector3 center;
    float distance;

    private void Awake()
    {
        pointerUI.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        SetCenter();
        CalculateAngle();

        // Rotate the pointer UI to point towards the target direction
        pointerUI.DORotate(new Vector3(0, 0, -angle), rotateSpeed).SetEase(easeMode);

      
      distance = Vector3.Distance(center, target.position);
        if (distance > maxDistance)
            pointerUI.gameObject.SetActive(true);
        else if (distance < minDistance)
            pointerUI.gameObject.SetActive(false);
        else
            pointerUI.gameObject.SetActive(true);
    }

    private void CalculateAngle()
    {
        Vector3 targetDirection = target.position - center;
        targetDirection.y = 0;
        angle = Vector3.SignedAngle(Vector3.forward, targetDirection, Vector3.up);
    }

    private void SetCenter()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            center = hit.point;
        }
    }
}
