using UnityEngine;
using System.Collections;
using NPlayer;
using System;

namespace NElements
{
    public class SkillEnabler : Interactible
    {
        [Header("Paramters")]
        [SerializeField]        private eSkill skill;
        [SerializeField]        private bool obtain = true;

        private ParticleSystem particles;
        private MeshRenderer mesh;

        void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
            particles = GetComponentInChildren<ParticleSystem>();
        }

        protected override void Init()
        {

        }

        protected override void Reset()
        {
            mesh.enabled = true;
            particles.gameObject.SetActive(true);
        }

        public override void Trigger()
        {
            base.Trigger();
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == PlayerController.COLLISION_TAG)
            {
                coll.gameObject.GetComponent<PlayerController>().enableSkill(skill, obtain);
                mesh.enabled = false;
                particles.gameObject.SetActive(false);
            }
        }
    }
}
