using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GCIEL.Toolkit
{
    public class SimpleGrab : InteractionObject
    {

        public bool hideControllerModelOnGrab;
        private Rigidbody rb;

        public override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }

        private void AddFixedJointToController(InteractionController controller)
        {
            FixedJoint fx = controller.gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 1500;
            fx.connectedBody = rb;
        }

        private void RemoveFixedJointFromController(InteractionController controller)
        {
            if (controller.gameObject.GetComponent<FixedJoint>())
            {
                FixedJoint fx = controller.gameObject.GetComponent<FixedJoint>();
                fx.connectedBody = null;
                Destroy(fx);
            }
        }

        public override void OnTriggerWasPressed(InteractionController controller)
        {
            base.OnTriggerWasPressed(controller);

            if (hideControllerModelOnGrab)
            {
                controller.HideControllerModel();
            }

            controller.Vibrate(800);
            AddFixedJointToController(controller);
        }

        public override void OnTriggerWasReleased(InteractionController controller)
        {
            base.OnTriggerWasReleased(controller);
       
            if (hideControllerModelOnGrab)
            {
                controller.ShowControllerModel();
            }

            RemoveFixedJointFromController(controller);

            Transform rig = controller.GetComponentInParent<SteamVR_ControllerManager>().gameObject.transform;

            rb.velocity = rig.TransformDirection(controller.velocity);
            rb.angularVelocity = controller.angularVelocity;
        }
    }
}