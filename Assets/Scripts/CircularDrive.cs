using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCIEL.Toolkit
{
    public class CircularDrive : InteractionObject
    {
        private Vector3 worldPlaneNormal = new Vector3(1, 0, 0); // Could make the axis of rotation more generic
        private Vector3 localPlaneNormal = new Vector3(1, 0, 0);

        private Quaternion startQuaternion;
        [SerializeField] private float minAngle;
        [SerializeField] private float maxAngle;
        [SerializeField] private float startAngle;
        private float outAngle;

        private Vector3 lastHandProjected;
        private float minMaxAngularThreshold = 1.0f;

        public float OutAngle
        {
            get
            {
                return outAngle;
            }
        }

        public float MinAngle
        {
            get
            {
                return minAngle;
            }
        }

        public float MaxAngle
        {
            get
            {
                return maxAngle;
            }
        }

        private void Start()
        {
            if (transform.parent)
            {
                worldPlaneNormal = transform.parent.localToWorldMatrix.MultiplyVector(worldPlaneNormal).normalized;
            }

            // Put this GameObject into its starting rotation
            startQuaternion = transform.rotation * Quaternion.AngleAxis(startAngle, Vector3.up);
            transform.localRotation = startQuaternion;
            outAngle = startAngle;
        }

        private void UpdateGameObject()
        {
            transform.localRotation = startQuaternion * Quaternion.AngleAxis(outAngle, Vector3.up);
        }

        //--- From SteamVR's CircularDrive 
        private Vector3 ComputeToTransformProjected(Transform xForm)
        {
            Vector3 toTransform = (xForm.position - transform.position).normalized;
            Vector3 toTransformProjected = new Vector3(0.0f, 0.0f, 0.0f);

            // Need a non-zero distance from the hand to the center of the CircularDrive
            if (toTransform.sqrMagnitude > 0.0f)
            {
                toTransformProjected = Vector3.ProjectOnPlane(toTransform, worldPlaneNormal).normalized;
            }
            else
            {
                Debug.LogFormat("The collider needs to be a minimum distance away from the CircularDrive GameObject {0}", gameObject.ToString());
                Debug.Assert(false, string.Format("The collider needs to be a minimum distance away from the CircularDrive GameObject {0}", gameObject.ToString()));
            }

            return toTransformProjected;
        }

        //-------------------------------------------------
        // Computes the angle to rotate the game object based on the change in the transform
        //-------------------------------------------------
        private void ComputeAngle(GameObject hand)
        {
            Vector3 toHandProjected = ComputeToTransformProjected(hand.transform);

            if (!toHandProjected.Equals(lastHandProjected))
            {
                float absAngleDelta = Vector3.Angle(lastHandProjected, toHandProjected);

                Vector3 cross = Vector3.Cross(lastHandProjected, toHandProjected).normalized;
                float dot = Vector3.Dot(worldPlaneNormal, cross);

                float signedAngleDelta = absAngleDelta;

                if (dot < 0.0f)
                {
                    signedAngleDelta = -signedAngleDelta;
                }

                outAngle = Mathf.Clamp(outAngle + signedAngleDelta, minAngle, maxAngle);
                lastHandProjected = toHandProjected;

                // Future work: could very easily add controller rumble for when the angle reaches different thresholds
            }

            lastHandProjected = toHandProjected;
        }

        private void Update()
        {
            if (currentController)
            {
                ComputeAngle(currentController.ControllerModel);
                UpdateGameObject();
            }
        }
    }
}