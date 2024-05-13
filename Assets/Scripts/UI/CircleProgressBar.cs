using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CircleProgressBar : MonoBehaviour
{
    [SerializeField] private float indicatorTimer = 1f;
    [SerializeField] private float maxIndicatorTimer = 1f;

    [SerializeField] private Image indicatorImageUI;

    [SerializeField] private KeyCode selectedKey = KeyCode.Mouse0;

    [SerializeField] private UnityEvent onIndicator = null;

    private bool shouldUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKey(selectedKey))
        {
            shouldUpdate = false;
       indicatorTimer -= Time.deltaTime;
            indicatorImageUI.enabled = true;
            indicatorImageUI.fillAmount = indicatorTimer;

            if (indicatorTimer <= 0)
            {
               
                indicatorTimer = maxIndicatorTimer;
                indicatorImageUI.fillAmount = maxIndicatorTimer;
                indicatorImageUI.enabled = false;
                onIndicator.Invoke();
            }
        }   
        else
        {
            if(shouldUpdate)
            {
                indicatorTimer += Time.deltaTime;
                indicatorImageUI.fillAmount = indicatorTimer;
                if(indicatorTimer >= maxIndicatorTimer)
                {
                    indicatorTimer = maxIndicatorTimer;
                    indicatorImageUI.fillAmount = maxIndicatorTimer;
                    indicatorImageUI.enabled = false;
                    shouldUpdate = false;
                }
            }
        }

      if(Input.GetKeyUp(selectedKey))
        {
            shouldUpdate = true;
        }
    }
}
