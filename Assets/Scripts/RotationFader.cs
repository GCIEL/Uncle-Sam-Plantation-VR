using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCIEL.Toolkit
{
    [RequireComponent(typeof(CircularDrive))]
    public class RotationFader : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] gameObjects;

        private float[] fadeThresholds;
        private int nextThreshold;
        private CircularDrive circularDrive;

        private void Start()
        {

            circularDrive = GetComponent<CircularDrive>();

            float rotSize = circularDrive.MaxAngle - circularDrive.MinAngle;
            fadeThresholds = new float[gameObjects.Length];
            float step = rotSize / (gameObjects.Length + 1);
            for (int i = 0; i < fadeThresholds.Length; i++)
            {
                fadeThresholds[i] = step * (i + 1);
                gameObjects[i].SetActive(false);
            }
            nextThreshold = 0;

        }

        private void Update()
        {
            for (int i = 0; i < fadeThresholds.Length; i++)
            {
                if (circularDrive.OutAngle >= fadeThresholds[i])
                {
                    gameObjects[i].SetActive(true);
                }
                else
                {
                    gameObjects[i].SetActive(false);
                }
            }
        }
    }
}