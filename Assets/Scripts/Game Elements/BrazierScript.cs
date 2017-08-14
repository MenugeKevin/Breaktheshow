using UnityEngine;
using NPlayer.NSkill;
using System.Collections;
using System;

namespace NElements
{
    public class BrazierScript : Interactible
    {
        [Header ("Brazier parameters")]
        [SerializeField]        GameObject fireParticles = null;
        [SerializeField]        GameObject WaterParticles = null;
        [SerializeField]        bool isOnFire = false;

        private bool fireStateSAve;

        protected override void Init()
        {
            fireStateSAve = isOnFire;
            fireParticles.SetActive(isOnFire);
        }

        public override void Trigger()
        {
            base.Trigger();
            isOnFire = !isOnFire;
            fireParticles.SetActive(isOnFire);
            WaterParticles.SetActive(!isOnFire);
        }

        void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.tag == BullletSkill.COLLISION_TAG)
            {
                eType type = coll.gameObject.GetComponent<BullletSkill>().type;

                if (type == eType.FIRE && !isOnFire)
                {
                    Trigger();
                }
                else if (type == eType.WATER && isOnFire)
                {
                    Trigger();
                }
                Destroy(coll.gameObject);
            }
        }

        protected override void Reset()
        {
            if (isOnFire != fireStateSAve)
            {
                isOnFire = !isOnFire;
                fireParticles.SetActive(isOnFire);
                WaterParticles.SetActive(!isOnFire);
            }
        }
    }
}