using UnityEngine;
using EL.Core.Player;

namespace EL.Core.PlayerAcessPointer
{
    public class PlayerAccessPoint : MonoBehaviour
    {
        private static PlayerAccessPoint instance;
        public static PlayerAccessPoint Instance => instance;

        [SerializeField] string PlayerTag;

        public enum SendRequest
        {
            DoMovement,
            DoInteraction,
            DoHandMovement,
            PopUpObjectID
        }
        public SendRequest sendRequest = SendRequest.DoMovement;

        // Player Vontroller Scirpts
        private Player.Player player;
        private PlayerCameraController playerCameraController;
        private PlayerInventory playerInventory;
        private Motion motion;
        private HeadBOB headBOB;
        private PlayerUIManager playerUI;
        private Interactor interactor;

        private void Awake() 
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }
        public static T FindComponentInRootOrChildren<T>(GameObject rootObject) where T : Component
        {
            T component = rootObject.GetComponent<T>();

            if (component != null)
            {
                return component;
            }

            foreach (Transform child in rootObject.transform)
            {
                component = FindComponentInRootOrChildren<T>(child.gameObject);

                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        private void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag(PlayerTag);
            player = FindComponentInRootOrChildren<Player.Player>(playerObj);
            playerInventory = FindComponentInRootOrChildren<PlayerInventory>(playerObj);
            playerCameraController = FindComponentInRootOrChildren<PlayerCameraController>(playerObj);
            motion = FindComponentInRootOrChildren<Motion>(playerObj);
            headBOB = FindComponentInRootOrChildren<HeadBOB>(playerObj);
            playerUI = FindComponentInRootOrChildren<PlayerUIManager>(playerObj);
            interactor = FindComponentInRootOrChildren<Interactor>(playerObj);
        }

        #region Return Component

        public Player.Player GetPlayerComponent() { return player; }
        public PlayerInventory GetPlayerInventoryComponent() { return playerInventory; }
        public PlayerCameraController GetPlayerCameraControllerComponent() { return playerCameraController; }
        public Motion GetMotionComponent() { return motion; }
        public HeadBOB GetHeadBOBComponent() { return headBOB; }
        public PlayerUIManager GetPlayerUIComponent() { return playerUI; }
        public Interactor GetPlayerInteractor() { return interactor; }

        #endregion

        public void UpdatePlayerAction()
        {
            switch (sendRequest)
            {
                case SendRequest.DoMovement:
                    DoMovement();
                    break;
                case SendRequest.DoInteraction:
                    DoInteraction();
                    break;
                case SendRequest.PopUpObjectID:
                    //playerUI.PopUpObjectID();
                    break;
                case SendRequest.DoHandMovement:
                    DoHandMovement();
                    break;
                default:
                    DoMovement();
                    break;
            }
        }

        private void DoMovement()
        {
            GetPlayerComponent().SetPlayerMovementActive(true);
            GetPlayerCameraControllerComponent().SetPlayerCamActive(true);
            GetMotionComponent().SetPlayerMotionActive(true);
            GetHeadBOBComponent().SetHeadBobActivation(true);
            GetPlayerInteractor().IneractLight.enabled = false;
        }
        private void DoInteraction()
        {
            GetPlayerComponent().SetPlayerMovementActive(false);
            GetPlayerCameraControllerComponent().SetPlayerCamActive(false);
            GetMotionComponent().SetPlayerMotionActive(false);
            GetHeadBOBComponent().SetHeadBobActivation(false);
            GetPlayerInteractor().IneractLight.enabled = true;
        }
        private void DoHandMovement()
        {
            GetPlayerCameraControllerComponent().SetPlayerCamActive(false);
        }
    }

}