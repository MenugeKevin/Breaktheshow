using UnityEngine;
using System.Collections;
using NPlayer;

namespace NElements
{
    public class StayOnPlatform : MonoBehaviour
    {
        private Transform _transform;

        void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        void OnCTriggerEnter(Collider coll)
        {
            Debug.Log("TRIGGERED IN"); /////////////////
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                coll.gameObject.transform.SetParent(_transform);
            }
        }

        void OnTriggerExit(Collider coll)
        {
            Debug.Log("TRIGGERED OUT"); /////////////////
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                coll.gameObject.transform.SetParent(null);
            }
        }
    }
}
