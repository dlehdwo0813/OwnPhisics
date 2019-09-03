using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    private static EventManager _instance = null;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    void Init()
    {

    }

    public delegate void GameEvent(Transform transform, float timer = 0);
    private static Event  _GameEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddGameEvent(Transform transform, float timer = 0)
    {

    }
}
