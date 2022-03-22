using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public enum Type
    {
        Main,
        JSON,
        Object,
        DimView,
    }

    public enum RequestCode
    {
        JSON_ObjectCall,        // GLTF 객체 호출
        JSON_IssueCall,         // 손상/보수 정보 호출

        Object_Initialize,

        DimView_3DCall,
        DimView_2DCall,
    }
}
