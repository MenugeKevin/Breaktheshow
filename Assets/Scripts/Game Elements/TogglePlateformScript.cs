using UnityEngine;
using System.Collections;
using NPlayer.NSkill;
using System;

namespace NElements
{
    [RequireComponent(typeof(Collider))]
    public class TogglePlateformScript : Interactible
    {
        private Collider _collider;

        [Header ("Preferences")]
        [SerializeField] [Range (0f, 1f)]
        private float transparency = 0.1f;
   //     [SerializeField]        private Color TransparentColor;
        [SerializeField]        private bool ReceiveSkill = true;

        //[Header ("Debug")]
        private bool isTriggered = false;
        private Material[] materials;
        private Color[] colors;
        private Color[] transparent_colors;

        private bool triggerSaveState;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            materials = GetComponent<Renderer>().materials;
            colors = new Color[materials.Length];
            transparent_colors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                Color tmp;

                colors[i] = materials[i].color;
                tmp = colors[i];
                tmp.a = transparency;
                transparent_colors[i] = tmp;
            }
        }

        protected override void Init()
        {
            isTriggered = _collider.isTrigger;
            triggerSaveState = isTriggered;
            handleColor();
        }

        public override void Trigger()
        {
            base.Trigger();
            isTriggered = !isTriggered;
            _collider.isTrigger = isTriggered;
            handleColor();
        }

        protected override void Reset()
        {
           if (isTriggered != triggerSaveState)
                Trigger();
        }

        private void handleColor()
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = isTriggered ? transparent_colors[i] : colors[i];
            }
        }

        private void HandleCollision(GameObject obj)
        {
            if (!ReceiveSkill)
                return;
            if (obj.tag == BullletSkill.COLLISION_TAG)
            {
                Trigger();
                Destroy(obj);
            }
        }

        void OnTriggerEnter(Collider coll)
        {
            HandleCollision(coll.gameObject);
        }

        void OnCollisionEnter(Collision coll)
        {
            HandleCollision(coll.gameObject);
        }
    }
}
