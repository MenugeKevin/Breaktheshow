using UnityEngine;
using System.Collections;
using NPlayer;
using NCore;

namespace NElements
{
    public class EndOfLevel : MonoBehaviour
    {
        void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                coll.gameObject.GetComponent<PlayerController>().reset();
                App.changeGameState(GameManager.eGameState.EndOfTheLevel);
            }
        }
    }
}
