using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObjectManager : MonoBehaviourPun, IPunPrefabPool
{
    public static ObjectManager Instance;
    public List<GameObject> photonObjects;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Object.Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        foreach (var obj in photonObjects)
        {
            pool.ResourceCache.Add(obj.name, obj);
        }
    }

    public void Destroy(GameObject gameObject)
    {
        Debug.Log("Destroy : " + gameObject.name);
        Object.Destroy(gameObject);
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        Debug.Log("Instantiate : " + prefabId);
        // GameObject obj = Instantiate(bullet, position, rotation);
        return null;
    }
}
