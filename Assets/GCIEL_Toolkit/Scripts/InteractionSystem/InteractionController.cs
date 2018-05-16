using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GCIEL.Toolkit
{
    public class InteractionController : MonoBehaviour
    {

        public Transform snapColliderOrigin; // 1
        public GameObject ControllerModel; // 2

        [HideInInspector]
        public Vector3 velocity; // 3
        [HideInInspector]
        public Vector3 angularVelocity; // 4

        private InteractionObject objectBeingInteractedWith; // 5

        private SteamVR_TrackedObject trackedObj;
        public SteamVR_TrackedObject TrackedObj
        {
            get { return trackedObj; }
        }


        public InteractionObject InteractionObject // 2
        {
            get { return objectBeingInteractedWith; }
        }

        public SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int)trackedObj.index); }
        }

        void Awake() // 3
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        private void CheckForInteractionObject()
        {
            Collider[] overlappedColliders = Physics.OverlapSphere(snapColliderOrigin.position, snapColliderOrigin.lossyScale.x / 2f); // 1

            foreach (Collider overlappedCollider in overlappedColliders) // 2
            {
                if (overlappedCollider.CompareTag("InteractionObject") && overlappedCollider.GetComponent<InteractionObject>().IsFree()) // 3
                {
                    objectBeingInteractedWith = overlappedCollider.GetComponent<InteractionObject>(); // 4
                    objectBeingInteractedWith.OnTriggerWasPressed(this); // 5
                    return; // 6
                }
            }
        }

        void Update()
        {
            if (Controller.GetHairTriggerDown()) // 1
            {
                CheckForInteractionObject();
            }

            if (Controller.GetHairTrigger()) // 2
            {
                if (objectBeingInteractedWith)
                {
                    objectBeingInteractedWith.OnTriggerIsBeingPressed(this);
                }
            }

            if (Controller.GetHairTriggerUp()) // 3
            {
                if (objectBeingInteractedWith)
                {
                    objectBeingInteractedWith.OnTriggerWasReleased(this);
                    objectBeingInteractedWith = null;
                }
            }
        }

        private void UpdateVelocity()
        {
            velocity = Controller.velocity;
            angularVelocity = Controller.angularVelocity;
        }

        void FixedUpdate()
        {
            UpdateVelocity();
        }

        public void HideControllerModel()
        {
            ControllerModel.SetActive(false);
        }

        public void ShowControllerModel()
        {
            ControllerModel.SetActive(true);
        }

        public void Vibrate(ushort strength) // 1
        {
            Controller.TriggerHapticPulse(strength);
        }

        public void SwitchInteractionObjectTo(InteractionObject interactionObject) // 2
        {
            objectBeingInteractedWith = interactionObject; // 3
            objectBeingInteractedWith.OnTriggerWasPressed(this); // 4
        }
    }
}