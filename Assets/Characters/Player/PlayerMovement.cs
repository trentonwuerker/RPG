using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]

    public class PlayerMovement : MonoBehaviour
    {

        CameraRaycaster cameraRaycaster;
        Vector3 clickPoint;
        AICharacterControl aiCharacterControl;
        GameObject walkTarget;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        }

        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case 8: //walkable
                    walkTarget.transform.position = raycastHit.point;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;
                case 9: //enemy
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharacterControl.SetTarget(enemy.transform);
                    break;
                default:  //everything else
                    Debug.LogWarning("Invalid Movement");
                    return;
            }
        }
    }

}