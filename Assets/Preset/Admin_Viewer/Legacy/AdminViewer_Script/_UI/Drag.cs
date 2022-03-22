using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 데이터 동적 생성을 추가로 두지 않는 인스턴스
    /// </summary>
    public class Call
    {
        #region Instance
        private static Call instance;

        public static Call Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Call();
                }
                return instance;
            }
        }
        #endregion

        public Call()
        {
            Panel_WidthOrHeight = 0;
            Elementindex = 0;
            Cell_WidthOrHeight = 0;
            Spacing_WidthOrHeight = 0;
        }

        /// <summary>
        /// 드래깅을 하고자 하는 패널의 폭 또는 너비
        /// </summary>
        public float Panel_WidthOrHeight { get; set; }

        /// <summary>
        /// 드래깅을 하고자 하는 패널 자식 객체의 수
        /// </summary>
        public int Elementindex { get; set; }

        /// <summary>
        /// 드래깅을 하고자 하는 패널 GridLayoutGroup의 셀 폭 또는 너비
        /// </summary>
        public float Cell_WidthOrHeight { get; set; }

        /// <summary>
        /// 드래깅을 ㅎ하고자 하는 패널 GridLayoutGroup의 간격 폭 또는 너비
        /// </summary>
        public float Spacing_WidthOrHeight { get; set; }

        public void SetDragParameters(DragCall dragCall)
        {
            if(dragCall.direction.Equals(DragDirection.X))
            {
                GridLayoutGroup layoutGroup = dragCall.targetTransform.GetComponent<GridLayoutGroup>();

                Panel_WidthOrHeight = dragCall.targetTransform.rect.width;

                Elementindex = dragCall.targetTransform.childCount;

                Cell_WidthOrHeight = layoutGroup.cellSize.x;
                Spacing_WidthOrHeight = layoutGroup.spacing.x;
            }
            else if(dragCall.direction.Equals(DragDirection.Y))
            {
                GridLayoutGroup layoutGroup = dragCall.targetTransform.GetComponent<GridLayoutGroup>();

                Panel_WidthOrHeight = dragCall.targetTransform.rect.height;

                Elementindex = dragCall.targetTransform.childCount;

                Cell_WidthOrHeight = layoutGroup.cellSize.y;
                Spacing_WidthOrHeight = layoutGroup.spacing.y;
            }
            else if(dragCall.direction.Equals(DragDirection.TemplateY))
            {
                Panel_WidthOrHeight = dragCall.targetTransform.GetComponent<RectTransform>().rect.height;

                Elementindex = 1;

                Cell_WidthOrHeight = Panel_WidthOrHeight;
                Spacing_WidthOrHeight = dragCall.overTextHeight;
            }
        }
    }

    public static class Drag
    {
        

        public static void InitializeScrollBar_barSize(Scrollbar scrollBar, DragCall dragCall)
        {
            Call.Instance.SetDragParameters(dragCall);

            int _elementIndex = Call.Instance.Elementindex;
            float _cellInterval = Call.Instance.Cell_WidthOrHeight;
            float _spacingInterval = Call.Instance.Spacing_WidthOrHeight;

            float panelInterval = Call.Instance.Panel_WidthOrHeight;
            float elementInterval = _cellInterval * _elementIndex + _spacingInterval * (_elementIndex - 1);

            float resultSize = panelInterval / elementInterval;

            if(resultSize > 1f)
            {
                resultSize = 1;
            }

            scrollBar.size = resultSize;
        }

        public static void UpdateScrollBar_barValue(Scrollbar scrollBar, DragCall dragCall)
        {
            Vector3 targetPosition = dragCall.targetTransform.localPosition;

            Call.Instance.SetDragParameters(dragCall);

            float minValue = 0;
            float maxValue = 0;

            if(dragCall.direction.Equals(DragDirection.X))
            {
                if(dragCall.vector.Equals(DragVector.LeftToRight))
                {
                    minValue = maxValue
                        - (Call.Instance.Elementindex * Call.Instance.Cell_WidthOrHeight + (Call.Instance.Elementindex - 1) * Call.Instance.Spacing_WidthOrHeight)
                        + Call.Instance.Panel_WidthOrHeight;

                    float targetX = targetPosition.x;

                    float result = targetX / minValue;

                    if(result >= 1)
                    {
                        result = 1;
                    }

                    scrollBar.value = result;
                }
                else if(dragCall.vector.Equals(DragVector.RightToLeft))
                {

                }
            }
            else if(dragCall.direction.Equals(DragDirection.Y))
            {
                if(dragCall.vector.Equals(DragVector.TopToBottom))
                {
                    maxValue = minValue
                        + Call.Instance.Elementindex * Call.Instance.Cell_WidthOrHeight
                        + (Call.Instance.Elementindex - 1) * Call.Instance.Spacing_WidthOrHeight
                        - Call.Instance.Panel_WidthOrHeight;

                    float targetY = targetPosition.y;

                    float result = targetY / maxValue;

                    if(result >= 1)
                    {
                        result = 1;
                    }

                    scrollBar.value = result;
                }
                else if(dragCall.vector.Equals(DragVector.BottomToTop))
                {

                }
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="dragCall"></param>
        public static void OnControl(DragCall dragCall)
        {
            Vector3 targetPosition = dragCall.targetTransform.localPosition;
            //GridLayoutGroup layoutGroup = dragCall.targetTransform.GetComponent<GridLayoutGroup>();

            Call.Instance.SetDragParameters(dragCall);

            if (dragCall.direction.Equals(DragDirection.X))
            {
                float panelWidth = Call.Instance.Panel_WidthOrHeight;

                int elementIndex = Call.Instance.Elementindex;
                float cellWidth = Call.Instance.Cell_WidthOrHeight;
                float spacingWidth = Call.Instance.Spacing_WidthOrHeight;

                float minValue = 0;
                float maxValue = 0;
                if(dragCall.vector.Equals(DragVector.LeftToRight))
                {
                    minValue = maxValue - (elementIndex * cellWidth + (elementIndex - 1) * spacingWidth) + panelWidth;

                    if(minValue <= maxValue || minValue < 0)
                    {
                        Vector3 targetVector = SetTargetVector(
                            _targetPosition: targetPosition,
                            dragCall: dragCall,
                            _values: Call.Instance,
                            _minValue: minValue,
                            _maxValue: maxValue
                            );

                        if (targetVector.x >= minValue && targetVector.x <= maxValue)
                        {
                            dragCall.targetTransform.localPosition = targetVector;
                        }
                        else if (targetVector.x < minValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                minValue,
                                targetPosition.y,
                                targetPosition.z
                                );
                        }
                        else if (targetVector.x > maxValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                maxValue,
                                targetPosition.y,
                                targetPosition.z
                                );
                        }
                    }

                }
                else if(dragCall.vector.Equals(DragVector.RightToLeft))
                {
                    maxValue = minValue + (elementIndex * cellWidth + (elementIndex - 1) * spacingWidth) - panelWidth;

                    if(maxValue >= minValue || maxValue > 0)
                    {
                        Vector3 targetVector = SetTargetVector(
                            _targetPosition: targetPosition,
                            dragCall: dragCall,
                            _values: Call.Instance,
                            _minValue: minValue,
                            _maxValue: maxValue
                            );

                        if(targetVector.x >= minValue && targetVector.x <= maxValue)
                        {
                            dragCall.targetTransform.localPosition = targetVector;
                        }
                        else if(targetVector.x < minValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                minValue,
                                targetPosition.y,
                                targetPosition.z
                                );
                        }
                        else if(targetVector.x > maxValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                maxValue,
                                targetPosition.y,
                                targetPosition.z
                                );
                        }
                    }
                }
            }
            else if (dragCall.direction.Equals(DragDirection.Y))
            {
                //float velocity = dragCall.moveVelocity;

                float panelHeight = Call.Instance.Panel_WidthOrHeight;      //dragCall.targetTransform.rect.height;

                int elementIndex = Call.Instance.Elementindex;              //dragCall.targetTransform.childCount;
                float cellHeight = Call.Instance.Cell_WidthOrHeight;        //layoutGroup.cellSize.y;
                float spacingHeight = Call.Instance.Spacing_WidthOrHeight;  //layoutGroup.spacing.y;

                float minValue = 0;
                float maxValue = 0;
                if (dragCall.vector.Equals(DragVector.TopToBottom))
                {
                    maxValue = minValue + (elementIndex * cellHeight + (elementIndex - 1) * spacingHeight) - panelHeight;

                    if (maxValue >= minValue || maxValue > 0)
                    {
                        Vector3 targetVector = SetTargetVector(
                            _targetPosition: targetPosition,
                            dragCall: dragCall,
                            _values: Call.Instance,
                            _minValue: minValue,
                            _maxValue: maxValue
                            );

                        // 드래깅 변화량만큼의 y축 값이 드래그 가능한 범위 내부값일 경우
                        if (targetVector.y >= minValue && targetVector.y <= maxValue)
                        {
                            dragCall.targetTransform.localPosition = targetVector;
                        }
                        // 드래깅 y축 값이 최소값보다 낮을 경우
                        else if (targetVector.y < minValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                targetPosition.x,
                                minValue,
                                targetPosition.z
                                );
                        }
                        else if (targetVector.y > maxValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                targetPosition.x,
                                maxValue,
                                targetPosition.z
                                );
                        }

                    }
                }
                else if (dragCall.vector.Equals(DragVector.BottomToTop))
                {
                    //maxValue = minValue + (elementIndex * cellHeight + (elementIndex - 1) * spacingHeight) - panelHeight;

                    //if (maxValue >= minValue || maxValue > 0)
                    //{
                    //    Vector3 targetVector = new Vector3(
                    //        targetPosition.x,
                    //        targetPosition.y + velocity * panelHeight / dragCall.dragResist,
                    //        targetPosition.z
                    //        );

                    //    // 드래깅 변화량만큼의 y축 값이 드래그 가능한 범위 내부값일 경우
                    //    if (targetVector.y >= minValue && targetVector.y <= maxValue)
                    //    {
                    //        dragCall.targetTransform.localPosition = targetVector;
                    //    }
                    //    // 드래깅 y축 값이 최소값보다 낮을 경우
                    //    else if (targetVector.y < minValue)
                    //    {
                    //        dragCall.targetTransform.localPosition = new Vector3(
                    //            targetPosition.x,
                    //            minValue,
                    //            targetPosition.z
                    //            );
                    //    }
                    //    else if (targetVector.y > maxValue)
                    //    {
                    //        dragCall.targetTransform.localPosition = new Vector3(
                    //            targetPosition.x,
                    //            maxValue,
                    //            targetPosition.z
                    //            );
                    //    }

                    //}
                }
            }
            else if(dragCall.direction.Equals(DragDirection.TemplateY))
            {
                float cellHeight = Call.Instance.Cell_WidthOrHeight;
                float spacingHeight = Call.Instance.Spacing_WidthOrHeight;

                float minValue = 0;
                float maxValue = 0;
                if(dragCall.vector.Equals(DragVector.TopToBottom))
                {
                    maxValue = spacingHeight;

                    if (maxValue >= minValue || maxValue > 0)
                    {
                        Vector3 targetVector = SetTargetVector(
                            _targetPosition: targetPosition,
                            dragCall: dragCall,
                            _values: Call.Instance,
                            _minValue: minValue,
                            _maxValue: maxValue
                            );

                        // 드래깅 변화량만큼의 y축 값이 드래그 가능한 범위 내부값일 경우
                        if (targetVector.y >= minValue && targetVector.y <= maxValue)
                        {
                            dragCall.targetTransform.localPosition = targetVector;
                        }
                        // 드래깅 y축 값이 최소값보다 낮을 경우
                        else if (targetVector.y < minValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                targetPosition.x,
                                minValue,
                                targetPosition.z
                                );
                        }
                        else if (targetVector.y > maxValue)
                        {
                            dragCall.targetTransform.localPosition = new Vector3(
                                targetPosition.x,
                                maxValue,
                                targetPosition.z
                                );
                        }

                    }
                }
            }
        }

        private static Vector3 SetTargetVector(Vector3 _targetPosition, DragCall dragCall, Call _values,
            float _minValue, float _maxValue)
        {
            Vector3 targetVector = new Vector3(0, 0, 0);
            //int direction = 0;
            //switch(dragCall.vector)
            //{
            //    case DragVector.TopToBottom:
            //    case DragVector.RightToLeft:
            //        direction = 1;
            //        break;

            //    case DragVector.BottomToTop:
            //    case DragVector.LeftToRight:
            //        direction = -1;
            //        break;
            //}

            if(dragCall.direction.Equals(DragDirection.X))
            {
                switch(dragCall.method)
                {
                    case DragMethod.MouseWheel:
                        if(dragCall.vector.Equals(DragVector.LeftToRight))
                        {
                            targetVector = new Vector3(
                                _targetPosition.x + dragCall.moveVelocity * _values.Panel_WidthOrHeight / dragCall.dragResist,
                                _targetPosition.y,
                                _targetPosition.z
                                );
                        }
                        else if(dragCall.vector.Equals(DragVector.RightToLeft))
                        {
                            targetVector = new Vector3(
                                _targetPosition.x - dragCall.moveVelocity * _values.Panel_WidthOrHeight / dragCall.dragResist,
                                _targetPosition.y,
                                _targetPosition.z
                                );
                        }
                        return targetVector;

                    case DragMethod.ScrollBar:
                        if(dragCall.vector.Equals(DragVector.LeftToRight))
                        {
                            targetVector = new Vector3(
                                _maxValue + dragCall.moveVelocity * _minValue,
                                _targetPosition.y,
                                _targetPosition.z
                                );
                        }
                        else if(dragCall.vector.Equals(DragVector.RightToLeft))
                        {
                            targetVector = new Vector3(
                                _minValue + dragCall.moveVelocity * _maxValue,
                                _targetPosition.y,
                                _targetPosition.z
                                );
                        }
                        return targetVector;
                }
            }
            else if(dragCall.direction.Equals(DragDirection.Y))
            {
                switch(dragCall.method)
                {
                    case DragMethod.MouseWheel:
                        targetVector = new Vector3(
                            _targetPosition.x,
                            _targetPosition.y + dragCall.moveVelocity * _values.Panel_WidthOrHeight / dragCall.dragResist,
                            _targetPosition.z
                            );

                        return targetVector;

                    case DragMethod.ScrollBar:
                        targetVector = new Vector3(
                            _targetPosition.x,
                            _minValue + dragCall.moveVelocity * _maxValue,
                            _targetPosition.z
                            );
                        break;
                }
            }
            else if(dragCall.direction.Equals(DragDirection.TemplateY))
            {
                switch (dragCall.method)
                {
                    case DragMethod.MouseWheel:
                        targetVector = new Vector3(
                            _targetPosition.x,
                            _targetPosition.y + dragCall.moveVelocity * _values.Panel_WidthOrHeight / dragCall.dragResist,
                            _targetPosition.z
                            );

                        return targetVector;

                    case DragMethod.ScrollBar:
                        targetVector = new Vector3(
                            _targetPosition.x,
                            _minValue + dragCall.moveVelocity * _maxValue,
                            _targetPosition.z
                            );
                        break;
                }
            }

            return targetVector;
        }
    }
}
