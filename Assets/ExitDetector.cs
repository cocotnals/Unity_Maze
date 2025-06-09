using UnityEngine;
using TMPro;           

public class ExitDetector : MonoBehaviour
{
  
    public TextMeshProUGUI winTMP;        

    [Header("플레이어 Animator")]
    public Animator playerAnimator;


    public Transform cameraPivot;


    public Camera mainCamera;    


    public float frontDistance = 2.5f; 
    public float frontHeight = 1.5f;  

    private bool hasWon = false;

    private void Start()
    {
        
   
        if (winTMP != null) winTMP.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasWon && other.CompareTag("Player"))
        {
            hasWon = true;
            

          
    
            if (winTMP != null)
            {
                winTMP.text = "Win!";
                winTMP.gameObject.SetActive(true);
            }

          
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("DoVictory");
            }

            
            SwitchCameraToFrontOfPlayer(other.transform);
        }
    }


    private void SwitchCameraToFrontOfPlayer(Transform playerTransform)
    {


    
        mainCamera.transform.SetParent(null, worldPositionStays: true);
    


        Vector3 playerHeadPos = playerTransform.position + Vector3.up * frontHeight;

        Vector3 forwardDir = playerTransform.forward.normalized;


        Vector3 newCameraPos = playerHeadPos - forwardDir * frontDistance;
        mainCamera.transform.position = newCameraPos;


        mainCamera.transform.LookAt(playerHeadPos);


    }
}
