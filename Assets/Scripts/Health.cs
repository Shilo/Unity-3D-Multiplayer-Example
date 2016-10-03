using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public const int maxHealth = 100;
    [SyncVar(hook = "OnCurrentHealthChanged")]
    public int currentHealth = maxHealth;
    public GameObject healthBarObject;
    public Image healthBarImage;
    public bool destroyOnDeath;
    private NetworkStartPosition[] spawnPoints;

    public override void OnStartLocalPlayer()
    {
        healthBarObject = GameObject.Find("Healthbar");
        healthBarObject.GetComponent<Canvas>().enabled = true;
        
        healthBarImage = healthBarObject.transform.Find("Foreground").GetComponent<Image>();
        OnCurrentHealthChanged(currentHealth);

        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer) return;
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
    }

    void OnCurrentHealthChanged(int value)
    {
        if (healthBarImage)
        {
            healthBarImage.fillAmount = value / 100.0f;
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

            transform.position = spawnPoint;
        }
    }

    void OnDestroy()
    {
        if (healthBarObject)
        {
            healthBarObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
