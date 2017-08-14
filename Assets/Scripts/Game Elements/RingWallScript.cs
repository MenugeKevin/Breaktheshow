using UnityEngine;
using System.Collections;

namespace NElements
{
    public class RingWallScript : Interactible
    {
        [Header ("Elements")]
        [SerializeField]        private GameObject wall = null;
        [SerializeField]        private GameObject trigger = null;

        [Header ("Paramaters")]
        [SerializeField]        private bool isOpen = false;
        [SerializeField]        private Color triggeredColor;

        private Color basicColor;
        private Material mat;

        private bool isOpenSaveState;

        void Awake()
        {
            isOpenSaveState = isOpen;
            mat = GetComponent<Renderer>().materials[1];
            basicColor = mat.color;
        }

        protected override void Init()
        {
            wall.SetActive(!isOpen);
            trigger.SetActive(isOpen);
            mat.color = isOpen ? triggeredColor : basicColor;
        }

        protected override void Reset()
        {
            isOpen = isOpenSaveState;
            Init();
        }

        public override void Trigger()
        {
            base.Trigger();
            isOpen = true;
            wall.SetActive(!isOpen);
            trigger.SetActive(isOpen);
            mat.color = isOpen ? triggeredColor : basicColor;
        }
    }
}
