using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    public static class ModuleCodes
    {
        
        public static bool IsWorkQueueProcess()
        {
            bool result = false;

            var mList = EventManager.Instance._ModuleList;

            if (mList.Contains(ModuleCode.WorkQueue) || mList.Contains(ModuleCode.Work_Pinmode))
            {
                result = true;
            }

            return result;
        }

    }
}
