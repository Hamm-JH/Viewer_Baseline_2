using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using UnityEngine;
    using View;

    public abstract partial class RootUI : AUI
    {
        [SerializeField] List<AElement> m_elements;

        public List<AElement> Elements { get => m_elements; set => m_elements = value; }
    }
}
