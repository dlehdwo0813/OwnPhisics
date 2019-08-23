using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public List<ObjectBasic> pooledObj;
    public ObjectBasic objectToPool;
    public int amount;


	// Use this for initialization
	public void Start () {
        if (objectToPool)
        {
            PoolingObject();
        }
        else
        {
            Debug.LogError("objectToPool is empty");
        }


	}


    public void PoolingObject()
    {
        for(int i = 0; i < amount; i++)
        {
            AddObject();
        }
    }

    ObjectBasic AddObject()
    {
        ObjectBasic obb = Instantiate<ObjectBasic>(objectToPool);
        obb.gameObject.SetActive(false);
        pooledObj.Add(obb);
        return obb;
    }

    public ObjectBasic GetInActivatedObject()
    {
        ObjectBasic obj;

        for (int i = 0; i < pooledObj.Count; i++)
        {
            obj = pooledObj[i];
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        obj = AddObject();
        return obj;
    }

}
