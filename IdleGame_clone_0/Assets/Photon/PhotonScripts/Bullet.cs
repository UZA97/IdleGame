using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] float bulletSpeed = 10.0f;
    void Update()
    {
        if (this.gameObject.activeSelf.Equals(true))
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Wall"))
        {
            PhotonNetwork.PrefabPool.Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            PhotonNetwork.PrefabPool.Destroy(this.gameObject);
        }
    }
    public void FireBullet()
    {
        gameObject.SetActive(true);
    }
}
