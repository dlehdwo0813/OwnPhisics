using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    public bool isButtonDown;

    public void LoadScene(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
    }

    public void CanvasChange(string from, string to)
    {
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();


        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].gameObject.name == from)
                canvases[i].gameObject.SetActive(false);

            if (canvases[i].gameObject.name == to)
                canvases[i].gameObject.SetActive(true);
        }


    }

    public void CanvasEnable(string canvas)
    {
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();


        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].gameObject.name == canvas)
            {
                canvases[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void CanvasDisable(string canvas)
    {
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();


        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].gameObject.name == canvas)
            {
                canvases[i].gameObject.SetActive(false);
                break;
            }
        }
    }


    public void GM_Log()
    {
        Debug.Log("GM_Log");
    }
}
