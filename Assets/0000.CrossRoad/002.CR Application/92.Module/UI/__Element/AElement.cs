using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using View;

    public abstract partial class AElement : AUI
    {
        [SerializeField] AUI m_rootUI;

        public AUI RootUI { get => m_rootUI; set => m_rootUI = value; }
    }
}
