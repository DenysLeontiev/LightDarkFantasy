using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DartPlace : MonoBehaviour
{
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform dartSpawnPoint;
    [SerializeField] private float timeBetweenAttacks = 2f;

    private float timeSinceLastAttack = 0f;

    private void Start()
    {
        timeSinceLastAttack = float.MaxValue;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if(timeSinceLastAttack > timeBetweenAttacks)
        {
            Transform projectileInstance = Instantiate(dartPrefab, dartSpawnPoint.position, Quaternion.identity).transform;
            projectileInstance.parent = transform;

            timeSinceLastAttack = 0f;
        }
    }
}
