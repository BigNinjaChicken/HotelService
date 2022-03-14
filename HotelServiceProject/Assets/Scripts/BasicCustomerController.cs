using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicCustomerController : MonoBehaviour
{
    // Customer Vars
    private NavMeshAgent navMesh;

    // Node Vars
    [SerializeField] private GameObject allQueueNodes;
    private List<QueueNode> queueNodes;
    public int queuePos = 0;
    private bool foundSpot = false; // Found First Spot

    // Interact feilds
    public string[] customerChat;
    public int aimCount;

    // Start is called before the first frame update
    void Start()
    {
        // cache vars
        queueNodes = new List<QueueNode>(allQueueNodes.GetComponentsInChildren<QueueNode>());
        navMesh = gameObject.GetComponent<NavMeshAgent>();

        findQueueSpot();
    }

    // Update is called once per frame
    void Update()
    {
        // if they found the first spot open yet
        if (foundSpot)
        {
            // if not first in line
            if (queuePos != 0)
            {
                // Wait till next person in line leaves
                if (queueNodes[queuePos - 1].nodeTaken == false)
                {
                    queueNodes[queuePos].nodeTaken = false;
                    queueNodes[queuePos - 1].nodeTaken = true;
                    
                    navMesh.SetDestination(queueNodes[queuePos - 1].transform.position);
                    queuePos--;
                }
            }
        } 
        else
        {

        }
    }

    void findQueueSpot()
    {
        for (int i = 0; i < queueNodes.Count; i++)
        {
            QueueNode node = queueNodes[i];
            if (node.nodeTaken == false)
            {
                node.nodeTaken = true;

                queuePos = i;
                foundSpot = true;
                navMesh.SetDestination(node.transform.position);

                return;
            }
        }
    }

    void interactedWith()
    {

    }
}
