using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlobalCustomerManager : MonoBehaviour
{
    [SerializeField] private GameTime inGameTime;

    private int numberOfInteractions = 15;
    private List<bool> hasInteracted = new List<bool>();

    [SerializeField] private GameObject customerPrefab;

    // Room Serivce Feilds
    [SerializeField] private GameObject allServiceNumbers;
    private List<TextMeshPro> serviceText;

    // Start is called before the first frame update
    void Start()
    { 
        serviceText = new List<TextMeshPro>(allServiceNumbers.GetComponentsInChildren<TextMeshPro>());

        for (int i = 0; i < numberOfInteractions; i++)
        {
            hasInteracted.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Customers
        if (inGameTime.currentInGameTime > 3f && !hasInteracted[0])
        {
            hasInteracted[0] = true;
            string[] chat =
                {
                "First robot customer of the day, huh?",
                "Take these papers for me please, make it snappy"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 12f && !hasInteracted[1])
        {
            hasInteracted[1] = true;
            string[] chat =
                {
                "Sorry about my husband, it's been a long spaceship ride",
                "to this hotel in space on the moon",
                "because we are in a hotel",
                "in space",
                "on the moon",
                "so we had to take a spaceship here",
                "in case you didn't know that",
                "just making sure because not everyone knows that",
                "ill be quiet just take my papers"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 30f && !hasInteracted[2])
        {
            hasInteracted[2] = true;
            string[] chat =
                {
                "My robot dad says that robot kids can't talk to strangers",
                "But I don't like him, so here's my papers"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 35f && !hasInteracted[3])
        {
            hasInteracted[3] = true;
            string[] chat =
                {
                "That other family looked terrible to deal with",
                "Hopefully i'm not as bad",
                "I hate all those bad customers, so annoying",
                "If you don't take these papers I will crash my spaceship into the lobby",
                "I'm not joking I have one waiting outside"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 50f && !hasInteracted[4])
        {
            hasInteracted[4] = true;
            string[] chat =
                {
                "Listen here buddy, ya better not tell anyone ya saw me here",
                "If I hear you snitched to the robo cops, ya dead, got it?",
                "Now check these papers like a good little player."
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 60f && !hasInteracted[5])
        {
            hasInteracted[5] = true;
            string[] chat =
                {
                "I love ma wonderful husband",
                "So wonderful and great",
                "He told me to hand ya these papers"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 70f && !hasInteracted[6])
        {
            hasInteracted[6] = true;
            string[] chat =
                {
                "Ayy my robo pop says that I gotta give ya the papers",
                "So here's tha papers"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 90f && !hasInteracted[7])
        {
            hasInteracted[7] = true;
            string[] chat =
                {
                "Was that who I thought it was?",
                "No? Ok",
                "I feel like I'm seeing things here.",
                "Probably because the place was buried on an alien burial ground.",
                "Or at least that's what I heard.",
                "Yeah, it's crazy. Probably false though.",
                "Maybe.",
                "It's a cool story, though.",
                "That's not at all relevant.",
                "Anyways can you take my papers please."
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        if (inGameTime.currentInGameTime > 120f && !hasInteracted[8])
        {
            hasInteracted[8] = true;
            string[] chat =
                {
                "Such a boring day huh",
                "look at us, we have so much in common",
                "in a hotel, on the moon, physical matter",
                "you wanna get coffee sometime?",
                "ERROR ERROR ERROR",
                "I'm sorry, I ruined it, I ruined it.",
                "I ruined the moment",
                "Just take my papers, please just take my papers, don't look at me"
                };
            spawnCustomer(chat, 3); // Text, Number of Aim Trainings
        }

        // Room Serice
        if (inGameTime.currentInGameTime > 2f && !hasInteracted[9])
        {
            hasInteracted[9] = true;
            roomService(10); // Room #
        }

        if (inGameTime.currentInGameTime > 35f && !hasInteracted[10])
        {
            hasInteracted[10] = true;
            roomService(5); // Room #
        }

        if (inGameTime.currentInGameTime > 43f && !hasInteracted[11])
        {
            hasInteracted[11] = true;
            roomService(30); // Room #
        }

        if (inGameTime.currentInGameTime > 79f && !hasInteracted[12])
        {
            hasInteracted[12] = true;
            roomService(13); // Room #
        }

        if (inGameTime.currentInGameTime > 98f && !hasInteracted[13])
        {
            hasInteracted[13] = true;
            roomService(15); // Room #
        }

        if (inGameTime.currentInGameTime > 100f && !hasInteracted[14])
        {
            hasInteracted[14] = true;
            roomService(32); // Room #
        }

    }

    void spawnCustomer(string[] chat, int aimCount)
    {
        GameObject tempCustomer = Instantiate(customerPrefab);
        tempCustomer.SetActive(true);

        BasicCustomerController customerScript = tempCustomer.GetComponent<BasicCustomerController>();
        customerScript.customerChat = chat;
        customerScript.aimCount = aimCount;
    }

    void roomService(int roomNumber)
    {
        for (int i = 0; i < serviceText.Count; i++)
        {
            if (serviceText[i].text == "-")
            {
                serviceText[i].text = "" + roomNumber;

                return;
            }
        }
    }
}
