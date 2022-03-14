using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostController : MonoBehaviour
{
    public float attackDistance = 0f;
    public StationaryPlayerController playerScript;
    [SerializeField] private float movementSpeed = 2f;

    private void Start()
    {
        playerScript = FindObjectOfType<StationaryPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, playerScript.transform.position) < attackDistance)
        {
            Vector3 targetDirection = playerScript.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, targetDirection.y, targetDirection.z));
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * movementSpeed);

            gameObject.transform.transform.position += gameObject.transform.transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    public void die()
    {
        GhostSpawner spawner = GetComponentInParent<GhostSpawner>();
        spawner.died();

        Destroy(gameObject);
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //The SceneManager loads your new Scene as a single Scene (not overlapping). This is Single mode.
            SceneManager.LoadScene("YouDied", LoadSceneMode.Single);
        }
    }

    
}
