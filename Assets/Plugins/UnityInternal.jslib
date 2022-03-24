mergeInto(LibraryManager.library, {

  // Webviewer
  UnityRequest: function (arg) {
    
    //ClickButton(Pointer_stringify(arg))
  },

  // --------------------------------------------

  ViewMap: function (arg) {
    CreateMap(Pointer_stringify(arg))
  },

  InitializeMap: function(arg) {
    InitMap(Pointer_stringify(arg))
  },
  
  OnReadyToPrint: function(arg1, arg2, arg3) {
    OnExcelPrint(
      Pointer_stringify(arg1),
      Pointer_stringify(arg2),
      Pointer_stringify(arg3)
    )
  },

  OnReadyToDrawingPrint: function(arg1, arg2, arg3, arg4, arg5, arg6, arg7,
    arg8, arg9, arg10, arg11, arg12, arg13, arg14) {
    OnDrawingPrint(
      Pointer_stringify(arg1),
      Pointer_stringify(arg2),
      Pointer_stringify(arg3),
      Pointer_stringify(arg4),
      Pointer_stringify(arg5),
      Pointer_stringify(arg6),
      Pointer_stringify(arg7),
      Pointer_stringify(arg8),
      Pointer_stringify(arg9),
      Pointer_stringify(arg10),
      Pointer_stringify(arg11),
      Pointer_stringify(arg12),
      Pointer_stringify(arg13),
      arg14
    )
  }
});