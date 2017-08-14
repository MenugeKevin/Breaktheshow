using UnityEngine;
using System.Collections;
using NPlayer;

namespace NElements
{
    public class RingThroughScript : Interactible
    {
        private bool activated = false;

        protected override void Init()
        {

        }

        protected override void Reset()
        {
            activated = false;
        }

        public override void Trigger()
        {
            base.Trigger();
        }

        void OnTriggerEnter(Collider coll)
        {
            if (activated)
                return;
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                Trigger();
                activated = true;
            }
        }
    }
}
