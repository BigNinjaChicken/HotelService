using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{

    [SerializeField] private Button quit;

    void Start()
    {
        quit.onClick.AddListener(TaskOnClick);
    }

    //Detect if a click occurs
    public void TaskOnClick()
    {
        Application.Quit();
    }

}
