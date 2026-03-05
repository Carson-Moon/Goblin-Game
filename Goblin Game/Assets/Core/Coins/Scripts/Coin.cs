using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Runtime
    [SerializeField] private Rigidbody rb;

    [Header("Spawn Movement")]
    [SerializeField] private float spawnForce;
    public float SpawnForce => spawnForce;
    [SerializeField] private float spawnTorque;

    private int id;
    public int ID => id;

    [Header("Collect Cooldown")]
    [SerializeField] float collectCooldown;
    private bool canCollect = false;
    private Coroutine collectCoroutine = null;
    

    public void CreateCoin(int _id)
    {
        id = _id;
        Deactivate();
    }

    public void Deactivate()
    {
        if(collectCoroutine != null)
        {
            StopCoroutine(collectCoroutine);
            collectCoroutine = null;
        }

        gameObject.SetActive(false);
        canCollect = false;
    }

    public void Activate(Vector3 _pos, Vector3 _force)
    {
        transform.SetParent(null);

        rb.position = _pos;

        gameObject.SetActive(true);

        rb.AddForce(spawnForce * _force, ForceMode.Impulse);
        rb.AddTorque(spawnTorque * Vector3.right, ForceMode.Impulse);

        collectCoroutine = StartCoroutine(CollectCooldown());
    }

    IEnumerator CollectCooldown()
    {
        yield return new WaitForSeconds(collectCooldown);
        canCollect = true;
    }
}
