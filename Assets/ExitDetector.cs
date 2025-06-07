using UnityEngine;
using UnityEngine.UI;     // Legacy UI Text 를 사용할 경우
using TMPro;              // TextMeshProUGUI 를 사용할 경우

public class ExitDetector : MonoBehaviour
{
    [Header("Win 메시지 (UI Text or TextMeshPro)")]
    public Text winUIText;     // Legacy UI Text
    public TextMeshProUGUI winTMP;        // TextMeshProUGUI

    [Header("플레이어 Animator")]
    public Animator playerAnimator;

    [Header("First‐Person 카메라 관련 참조")]
    [Tooltip("첫 번째 인칭용 카메라가 붙어 있는 Head 또는 CameraHolder")]
    public Transform cameraPivot;   // 예: Player/Head 오브젝트의 Transform

    [Tooltip("위 CameraPivot 아래에 붙어 있는 실제 Camera GameObject")]
    public Camera mainCamera;    // 예: Player/Head/MainCamera

    [Header("카메라가 이동할 '전면' 위치를 잡기 위한 높이 오프셋")]
    public float frontDistance = 2.5f;  // 캐릭터 앞쪽으로 얼마나 이동할지
    public float frontHeight = 1.5f;  // 캐릭터 머리 위로 얼마나 올릴지

    private bool hasWon = false;

    private void Start()
    {
        // Win 메시지 숨김
        if (winUIText != null) winUIText.gameObject.SetActive(false);
        if (winTMP != null) winTMP.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasWon && other.CompareTag("Player"))
        {
            hasWon = true;
            Debug.Log("ExitDetector: Win!");

            // 1) Win 텍스트 활성화
            if (winUIText != null)
            {
                winUIText.text = "Win!";
                winUIText.gameObject.SetActive(true);
            }
            if (winTMP != null)
            {
                winTMP.text = "Win!";
                winTMP.gameObject.SetActive(true);
            }

            // 2) 플레이어 Animator의 DoVictory 트리거 발동
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("DoVictory");
            }

            // 3) 카메라를 First‐Person 구조에서 분리하고, 플레이어 전면 위치로 이동
            SwitchCameraToFrontOfPlayer(other.transform);
        }
    }

    /// <summary>
    /// 첫 번째 인칭 카메라를 플레이어 머리에서 분리하고,
    /// 캐릭터 정면 앞쪽으로 이동시킨 뒤, 캐릭터 정면을 바라보게 만드는 함수
    /// </summary>
    private void SwitchCameraToFrontOfPlayer(Transform playerTransform)
    {
        if (cameraPivot == null || mainCamera == null)
        {
            Debug.LogWarning("ExitDetector: 카메라 참조가 할당되지 않았습니다. cameraPivot, mainCamera를 반드시 연결하세요.");
            return;
        }

        // 1) MainCamera를 cameraPivot (Head) 아래에서 분리
        mainCamera.transform.SetParent(null, worldPositionStays: true);
        //    → worldPositionStays: true 로 두면, 기존 월드 좌표는 유지됨

        // 2) 카메라를 플레이어 모델 전면, 약간 위쪽 위치로 이동
        //    playerTransform.position은 캐릭터의 중심(보통 발 위치)에 가깝습니다.
        //    머리 위로 올리고 싶으면 보통 playerTransform.position + Vector3.up * [높이] 를 씁니다.
        Vector3 playerHeadPos = playerTransform.position + Vector3.up * frontHeight;

        // 플레이어가 바라보는 정면 방향(Transform.forward)을 구합니다.
        Vector3 forwardDir = playerTransform.forward.normalized;

        // 카메라를 플레이어 정면(반대 방향)으로 약간 뒤로 빼서 세팅
        Vector3 newCameraPos = playerHeadPos - forwardDir * frontDistance;
        mainCamera.transform.position = newCameraPos;

        // 3) 카메라가 플레이어 쪽(머리 위치) 바라보도록 회전
        mainCamera.transform.LookAt(playerHeadPos);

        // 4) (선택 사항) 기존 First‐Person 카메라 제어 스크립트가 있다면 비활성화
        //    예를 들어, MouseLook 이나 FPSController와 같은 스크립트가 카메라 회전을 제어하고 있다면:
        var fpsLook = mainCamera.GetComponent<MonoBehaviour>(); // 정확한 스크립트 타입으로 교체
        if (fpsLook != null)
        {
            fpsLook.enabled = false;
        }

        // 혹은 CharacterController나 FPSController 스크립트가 카메라를 직접 회전시키는 경우,
        // 그 스크립트를 비활성화해 주세요. (예: mainCamera.GetComponent<FPSController>().enabled = false;)
    }
}
