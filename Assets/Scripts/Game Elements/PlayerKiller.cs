using UnityEngine;
using NPlayer;
using NPlayer.NSkill;

namespace NElements
{
    public class PlayerKiller : MonoBehaviour
    {
        void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                coll.gameObject.GetComponent<PlayerController>().kill();
            }
            else if (coll.gameObject.tag == BullletSkill.COLLISION_TAG)
            {
                Destroy(coll.gameObject);
            }
        }
    }
}
