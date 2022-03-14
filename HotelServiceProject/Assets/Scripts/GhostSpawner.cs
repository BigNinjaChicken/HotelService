using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float respawnTime = 10f; // in sec.
    [SerializeField] private float attackDistance = 10f; // in meters
    private bool spawnedIn = false;

    // Update is called once per frame
    void Update()
    {
        if (!spawnedIn)
        {
            spawnedIn = true;
            GameObject ghost = Instantiate(ghostPrefab, gameObject.transform);

            GhostController ghostScript = ghost.GetComponent<GhostController>();
            ghostScript.attackDistance = attackDistance;
        }
    }

    public void died()
    {
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(respawnTime);
        spawnedIn = false;
    }
}
