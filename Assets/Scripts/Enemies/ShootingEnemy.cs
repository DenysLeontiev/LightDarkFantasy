using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float timeBetweenAttacks;

    private Animator entityAnimator;
    private Health entityHealth;

    private float timeSinceLastAttack = 0f;

    private readonly int idleHash = Animator.StringToHash("Idle");
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly int hitHash = Animator.StringToHash("Hit");
    private readonly int dieHash = Animator.StringToHash("Die");

    private void Start()
    {
        timeSinceLastAttack = float.MaxValue;

        entityAnimator = GetComponent<Animator>();
        entityHealth = GetComponent<Health>();

        entityHealth.OnDamageTaken += EntityHealth_OnDamageTaken;
        entityHealth.OnDied += EntityHealth_OnDied;

        entityAnimator.SetBool(idleHash, true);
    }

    private void Update()
    {
        if (entityHealth.IsDead)
            return;

        timeSinceLastAttack += Time.deltaTime;

        if(timeSinceLastAttack > timeBetweenAttacks)
        {
            entityAnimator.SetTrigger(attackHash);
            timeSinceLastAttack = 0f;
        }
    }

    public void AttackAnimationEvent()
    {
        Transform projectileInstance = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity).transform;
        Projectile projectile = projectileInstance.GetComponent<Projectile>();

        float scaleX = projectile.GetIsMovingRight() ? 1f : -1f;
        projectileInstance.localScale = new Vector3(projectileInstance.localScale.x * scaleX, projectileInstance.localScale.y, projectileInstance.localScale.z);

        entityAnimator.SetBool(idleHash, true);
    }

    private void EntityHealth_OnDamageTaken(object sender, System.EventArgs e)
    {
        Debug.Log("EntityHealth_OnDamageTaken");
        entityAnimator.SetTrigger(hitHash);
    }

    private void EntityHealth_OnDied(object sender, System.EventArgs e)
    {
        Debug.Log("Died");
        entityAnimator.SetTrigger(dieHash);
    }
}
