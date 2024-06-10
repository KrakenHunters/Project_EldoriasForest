using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaMachine_SpecialSpell : SpecialSpellBook
{
    private float limitUp;

    private float startPosY;

    private SphereCollider radiusCollider;

    private List<Collider> enemiesInRange = new List<Collider>();

    private bool isFiring;

    [SerializeField]
    private float interval;

    [SerializeField]
    private SpellBook lightningBaseSpell;

    protected override void CastSpell(int tier)
    {

        radiusCollider = GetComponent<SphereCollider>();
        radiusCollider.radius = spellData.currentTierData.radius;
        limitUp = 1f;
        startPosY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update();
        if (transform.position.y < limitUp && timer < duration)
        {
            transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
        }
        else if (timer >= duration)
        {

            transform.Translate(Vector3.down * projectileSpeed * Time.deltaTime);
            if (transform.position.y <= startPosY)
            {
                Destroy(gameObject);
            }
        }
        //Stop when reaching upper limit
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an enemy
        if (other.GetComponent<CharacterClass>() && charAttacker != other.gameObject)
        {
            enemiesInRange.Add(other); // Add the enemy to the list
            if (!isFiring)
            {
                StartCoroutine(FireProjectiles());
            }
        }
    }
    IEnumerator FireProjectiles()
    {
        isFiring = true;
        while (enemiesInRange.Count > 0) // Continue firing as long as there are enemies in range
        {
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy != null) // Check if the enemy still exists
                {
                    Vector3 direction = (enemy.transform.position - transform.position).normalized; // Calculate the direction towards the enemy

                    SpellBook lightning = Instantiate(lightningBaseSpell, transform.position, transform.rotation); // Instantiate the projectile
                    lightning.tier = this.tier;
                    lightning.Shoot(direction, this.gameObject);

                }
            }
            yield return new WaitForSeconds(interval); // Wait for the specified interval
        }
        isFiring = false;
    }
    void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other); // Remove the enemy from the list
            if (enemiesInRange.Count == 0)
            {
                StopCoroutine(FireProjectiles());
                isFiring = false;
            }
        }
    }
}
