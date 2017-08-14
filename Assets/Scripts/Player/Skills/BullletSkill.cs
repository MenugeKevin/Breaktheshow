using UnityEngine;
using System.Collections;
using NElements;

namespace NPlayer
{
    namespace NSkill
    {
        public enum eType
        {
            FIRE,
            WATER
        };

        public class BullletSkill : MonoBehaviour
        {
            public static string COLLISION_TAG = "Skills";

            public eType type;
            [SerializeField]            private float lifetime = 20f;

            void Start()
            {
                Invoke("SelfDestroy", lifetime);
            }

            protected void SelfDestroy()
            {
                Destroy(this.gameObject);
            }

       //    void OnCollisionEnter(Collision coll)
       //    {
       //        if (coll.gameObject.tag == TRIGGERABLE_TAG)
       //        {
       //            coll.gameObject.GetComponent<Interactible>().Trigger();
       //            Destroy(this.gameObject);
       //        }
       //    }
        }
    }
}
