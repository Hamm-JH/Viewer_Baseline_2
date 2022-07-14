using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Definition
{
	public static class ExternalAPI
	{
        /// <summary>
        /// 레거시 웹 뷰어 : 지도 On/Off 요청
        /// </summary>
        /// <param name="argument">
        /// Y : 지도 켜기
        /// N : 지도 끄기
        /// </param>
        [DllImport("__Internal")]
        public static extern void ViewMap(string argument);

        /// <summary>
        /// 레거시 웹 뷰어 : 지도 생성 시작 요청
        /// </summary>
        /// <param name="argument">Y : 지도 켜기</param>
        [DllImport("__Internal")]
        public static extern void InitializeMap(string argument);

        /// <summary>
        /// 레거시 관리자 뷰어 : 보고서 엑셀 출력시 필요 이미지 전달
        /// </summary>
        /// <param name="_fName1">Base64 형태로 전달된 이미지 1번 (시설물 전경)</param>
        /// <param name="_fName2">Base64 형태로 전달된 이미지 2번 (그래프 1번)</param>
        /// <param name="_fName3">Base64 형태로 전달된 이미지 3번 (그래프 2번)</param>
        [DllImport("__Internal")]
        public static extern void OnReadyToPrint(string _fName1, string _fName2, string _fName3);

        /// <summary>
        /// 레거시 관리자 뷰어 : 시설물 부재의 상,하,좌,우,전,후 일반 단면, 외곽/치수선 포함 단면 전달
        /// </summary>
        /// <param name="_f1">상부 일반 단면</param>
        /// <param name="_f2">상부 외곽/치수 포함 단면</param>
        /// <param name="_f3">하부 일반 단면</param>
        /// <param name="_f4">하부 외곽/치수 포함 단면</param>
        /// <param name="_f5">좌측 일반 단면</param>
        /// <param name="_f6">좌측 외곽/치수 포함 단면</param>
        /// <param name="_f7">우측 일반 단면</param>
        /// <param name="_f8">우측 외곽/치수 포함 단면</param>
        /// <param name="_f9">전면 일반 단면</param>
        /// <param name="_f10">전면 외곽/치수 포함 단면</param>
        /// <param name="_f11">후면 일반 단면</param>
        /// <param name="_f12">후면 외곽/치수 포함 단면</param>
        /// <param name="_f13">시설물명</param>
        /// <param name="_f14">참일 경우 제목에 손상추가, 거짓일 경우 제목에 보수추가</param>
        [DllImport("__Internal")]
        public static extern void OnReadyToDrawingPrint(string _f1, string _f2, string _f3, string _f4, string _f5, string _f6, string _f7,
            string _f8, string _f9, string _f10, string _f11, string _f12, string _f13, bool _f14);
    }
}
