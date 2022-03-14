using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    [SerializeField] private Button retry;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        retry.onClick.AddListener(TaskOnClick);
    }

    //Detect if a click occurs
    public void TaskOnClick()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
