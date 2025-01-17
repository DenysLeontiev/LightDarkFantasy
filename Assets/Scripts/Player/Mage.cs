using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Net.NetworkInformation;
using UnityEditor.U2D.Sprites;
=======
>>>>>>> bcba07212653e8710bd4ff3ca968c1ebe42ec1c4
using UnityEngine;

public class Mage : PlayerBase
{
<<<<<<< HEAD
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject projectilePrefab; 

    public override void Attack()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        SpriteRenderer projectileSpriteRenderer = projectileInstance.GetComponent<SpriteRenderer>();
        MageProjectile mageProjectile = projectileInstance.GetComponent<MageProjectile>();

        bool shouldFlipSpriteOnX = IsFacingRight == false;

        mageProjectile.SetIsMovingRight(IsFacingRight);

        projectileSpriteRenderer.flipX = shouldFlipSpriteOnX;
=======
    public override void Attack()
    {
        Debug.Log("Mage Attack");
>>>>>>> bcba07212653e8710bd4ff3ca968c1ebe42ec1c4
    }
}
