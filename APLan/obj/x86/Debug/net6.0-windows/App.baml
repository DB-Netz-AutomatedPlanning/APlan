   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:APLanAPLan  5      3clr-namespace:APLan.ViewModelsAPLan.ViewModels  5      3clr-namespace:APLan.ConvertersAPLan.Converters  5      9!clr-namespace:APLan.HelperClassesAPLan.HelperClasses  5          	APLan.App   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      %localclr-namespace:APLan     5      5
viewModelsclr-namespace:APLan.ViewModels     5      5
convertersclr-namespace:APLan.Converters     5      ;helperClasses!clr-namespace:APLan.HelperClasses     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5        ?? 	Resources  5
      &    APLan.Converters.XvalueConverter   xValueConverter&    APLan.Converters.YvalueConverter  yValueConverter+   %APLan.Converters.PointsPlotYConverter  pointsYvalueConverter+   %APLan.Converters.PointsPlotXConverter  pointsXvalueConverter4   .APLan.Converters.PointXVisualizationCoordinate  pointXvisualize4   .APLan.Converters.PointYVisualizationCoordinate  pointYvisualize9   3APLan.Converters.XvalueConverterBackSignalsPlanning  XSignalBack9   3APLan.Converters.YvalueConverterBackSignalsPlanning  YSignalBack5	   /APLan.Converters.XvalueConverterSignalsPlanning  xValueConverterSignal5
   /APLan.Converters.YvalueConverterSignalsPlanning 	 yValueConverterSignal+   %APLan.Converters.CoordinatesConverter 
 coordinateConverter6   0APLan.Converters.CoodrinatesSinglePointConverter  SingleCoordinateConverter)   #APLan.Converters.MidCanvasConverter  midCanvasConverter,  &APLan.Converters.MultiBindingConverter  multiBindingConverter.   (APLan.Converters.StringToDoubleConverter  stringToDoubleconverter,   &APLan.ViewModels.ExportWindowViewModel  exportWindowViewModel$   APLan.ViewModels.DrawViewModel  drawViewModel.   (APLan.ViewModels.CanvasToolsTabViewModel  canvasToolsTabViewModel/   )APLan.ViewModels.DrawingLowerTabViewModel  drawingLowerTabViewModel.   (APLan.ViewModels.VisualizedDataViewModel  visualizedDataViewModel*   $APLan.ViewModels.NewProjectViewModel  newProjectViewModel(   "APLan.ViewModels.MainMenuViewModel  mainMenuViewModel/   )APLan.ViewModels.EulynxValidatorViewModel  EulynxValidatorViewModel+   %APLan.ViewModels.PlanningTabViewModel  planTabViewModel5   /APLan.HelperClasses.TemplateSelectorForSelected  selectedTemplateSelector:   4APLan.HelperClasses.TemplateSelectorForLoadedObjects , (selectedTemplateSelectorForLoadedObjects*   $APLan.ViewModels.SymbolsTabViewModel  symbolsTabViewModel  CustomPolylineTempalte ?? DataType Q? 
IsExpanded J? IsHitTestVisible  SymbolTemplate  loadedSignal1   +APLan.HelperClasses.CanvasObjectInformation,   &APLan.HelperClasses.CustomCanvasSignal  Name J? RenderTransformOrigin ?? RelativeSource ?? Path u? Matrix*  $Microsoft.Xaml.Behaviors.Interaction	   Triggers+  %Microsoft.Xaml.Behaviors.EventTrigger
   	EventName*   $Microsoft.Xaml.Behaviors.TriggerBase    Actions2!  ,Microsoft.Xaml.Behaviors.InvokeCommandAction !  Command ?? Source !  CommandParameter ?? ElementName  
loadedText*"   $APLan.HelperClasses.CustomCanvasText " Name??%?  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 ?     &	 ?     &		 ?     &	
 ?     &	 ?     &	 ?     &	 ?     &	 
    &	     &	 0    &	 C    &	 V    &	 i    &	 |    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	     &	     &	 1    7  &	 ?    7    5   
   6?     5   
   6?     5   
   6J     5   
   6J     5   
   6M     5   
   6M     5   
   6N     5   
   6N   	  5   
   6T   
  5   
   6T     5   
   6H     5   
   6Y     5   
   6E     5   
   6K     5   
   6O     5"   
   6+     5%   
   6#     5&   
   6-     5'   
   6.     5*   
   6-     5-   
   6)     5.   
   6'     51   
   6.     54   
   6E     57   
   6W     58   
   6l     5<   
   6)   ?? 5C   
   " F?66   .R?5D      R? $??0q?6   $??Disabled=?6+   $??Disabled=?6a   .@?5E      Q? $??0q?6C   $
 True??6W   ??6   ?? *DataContext.Name+.@?5F      ?? $
??Bold"?6H   ??6   ?? *DataContext.Data+??5G      ?? 5H      .R?5I   "   ? 
?????? $ False??6E   .+?5J   &   ?? $
??Left=?60   $
??Bold"?6F   ??6X   ?? *Key+6o   ?? 5K   &   $??	10,0,10,0q?60   $??Normal"?6F   ??6Z   ?? *Value+6s   5L   #   5M      5O      5P      5Q      5S      ?? 5T   
   " ??6.   5V      ?? 5Y   
   "  5Z      .R?5[        $ signal??6/   $ 0.5,0.5&?5_   /   ??5\   /   ?? *Scale+??5]   /   ??  ?? *self+$ Width???5^   /   ?? *SignalImageSource+z?5f      u? 5g       6&   ?? *RenderTransformMatrix+6R   	 5i        5j      $
 Loaded??6%   . 5k      !   5l   0   ?? 8   $ ObjectLoaded? 5m   0   ?? $ signal??5n      5p      5q      ?? 5u   
   "  6+   .R?5v      "  $
 Text??6-   $	 0,0&?5y   -   ??5w   -   ?? *IncludedText+??5x   -   ?? *IncludedTextSize+.??5z   .   z?5{      u? 5|       6&   ?? *RenderTransformMatrix+6R   .??5}   B   	 5~        5      $
 Loaded??6%   . 5?      !   5?   0   ?? 8   $ ObjectLoaded? 5?   0   ?? $
 Text??5?      .??5?   *   5?      5?      5?      5?      