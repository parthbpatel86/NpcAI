using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogController : MonoBehaviour
{
    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _dialogCamera;

    [SerializeField] Transform _NPCTransform;
    [SerializeField] GameObject _dialogUI;


    private Quaternion _npcInitialRot = Quaternion.identity;
    private GameObject _playerObject = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerObject = other.gameObject;
            var playerTransform = other.transform;

            // disable player input
            playerTransform.GetComponent<PlayerInput>().enabled = false;
            var playerPos = playerTransform.position;
            var npcPos = _NPCTransform.position;
            var npcDir = playerPos - npcPos;
            npcDir.y = 0;

            // teleport the player to standing point
            _npcInitialRot = _NPCTransform.rotation;
            _NPCTransform.rotation = Quaternion.LookRotation(npcDir);

            // disable main cam, enable dialog cam
            _mainCamera.SetActive(false);
            _dialogCamera.SetActive(true);

            // enable gpt chat UI
            _dialogUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // disengage
    public void OnDialogExit()
    {
        _dialogUI.SetActive(false);
        _mainCamera.SetActive(true);
        _dialogCamera.SetActive(false);
        _NPCTransform.rotation = _npcInitialRot;
        _playerObject.GetComponent<PlayerInput>().enabled = true;
        _playerObject = null;
    }
}
