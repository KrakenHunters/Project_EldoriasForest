using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpellBook : SpellBook
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float range;
    
    protected Vector3 startPos;

    protected override void Awake()
    {
        tier = GameManager.Instance.pdata.baseAttackTier;
        base.Awake();

    }
    protected override void UpgradeTier()
    {
        base.UpgradeTier();
        GameManager.Instance.pdata.baseAttackTier = tier;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Debug.Log("Current Position: " + transform.position);
        Debug.Log("Distance Traveled: " + Vector3.Distance(transform.position, startPos));
        if (Vector3.Distance(transform.position, startPos) > range)
        {
            Destroy(this.gameObject);
        }
    }
}
