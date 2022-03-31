using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indicator
{
    public interface IIndicator
    {
        IndicatorType Type { get; set; }
    }
}
