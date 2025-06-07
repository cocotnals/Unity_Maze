using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public float boostAmount = 0.5f; 

    private void OnTriggerEnter(Collider other)
    {
        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.AddSpeed(boostAmount);

            
            Destroy(gameObject);
        }
    }
}
