using UnityEngine;
using System.Collections.Generic;

namespace NElements
{
    public abstract class Interactible : MonoBehaviour
    {
        [SerializeField]        protected List<Interactible> TriggeredElements = null;

        void Start()
        {
            FindObjectOfType<NPlayer.PlayerController>().DeathEvent.AddListener(Reset);
            Init();
        }

        protected abstract void Init();

        public virtual void Trigger()
        {
            foreach (Interactible i in TriggeredElements)
                i.Trigger();
        }

        protected abstract void Reset();
    }
}
