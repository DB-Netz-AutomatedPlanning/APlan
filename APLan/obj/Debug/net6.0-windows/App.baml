   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:APLan.ConvertersAPLan.Converters  5      9!clr-namespace:APLan.HelperClassesAPLan.HelperClasses  5      3clr-namespace:APLan.ViewModelsAPLan.ViewModels  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5          	APLan.App   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      /
convertersclr-namespace:APLan.Converters  5      5helperClasses!clr-namespace:APLan.HelperClasses  5      /
viewModelsclr-namespace:APLan.ViewModels  5      .s&clr-namespace:System;assembly=mscorlib  5        ?? 	Resources  5
       	  arrow  
backGround  
themeColor  planningActivate!   APLan.HelperClasses.Loading  globalLoading ?? 
TargetType ?? Property ?? Value&    APLan.Converters.XvalueConverter  xValueConverter&    APLan.Converters.YvalueConverter  yValueConverter5   /APLan.Converters.XvalueConverterSignalsPlanning  xValueConverterSignal5   /APLan.Converters.YvalueConverterSignalsPlanning  yValueConverterSignal+   %APLan.Converters.CoordinatesConverter 	 coordinateConverter6   0APLan.Converters.CoodrinatesSinglePointConverter 
 SingleCoordinateConverter,  &APLan.Converters.MultiBindingConverter  multiBindingConverter.	   (APLan.Converters.StringToDoubleConverter  stringToDoubleconverter*
  $APLan.Converters.HalfLengthConverter  HalfLengthConverter*  $APLan.Converters.DrawUserControlSize  DrawUserControlSize,   &APLan.Converters.LoadingAngleConverter  LoadingAngleConverter%   APLan.Converters.TitleConverter  TitleConverter-   'APLan.Converters.PdfDetailSizeConverter  PdfDetailSizeConverter$   APLan.ViewModels.BaseViewModel  baseViewModel,   &APLan.ViewModels.ExportWindowViewModel  exportWindowViewModel$   APLan.ViewModels.DrawViewModel  drawViewModel.   (APLan.ViewModels.CanvasToolsTabViewModel  canvasToolsTabViewModel.   (APLan.ViewModels.VisualizedDataViewModel  visualizedDataViewModel*   $APLan.ViewModels.NewProjectViewModel  newProjectViewModel(   "APLan.ViewModels.MainMenuViewModel  mainMenuViewModel/   )APLan.ViewModels.EulynxValidatorViewModel  EulynxValidatorViewModel+   %APLan.ViewModels.PlanningTabViewModel  planTabViewModel5   /APLan.HelperClasses.TemplateSelectorForSelected  selectedTemplateSelector:   4APLan.HelperClasses.TemplateSelectorForLoadedObjects , (selectedTemplateSelectorForLoadedObjects*   $APLan.ViewModels.SymbolsTabViewModel  symbolsTabViewModel  CustomPolylineTempalte ?? DataType ?? Padding ?? Path ?? 	Converter&   System.Windows.Controls.DataGrid   AutoGenerateColumns	   AlternatingRowBackground
   
IsReadOnly   Columns0  *System.Windows.Controls.DataGridTextColumn,  &System.Windows.Controls.DataGridColumn   Header1  +System.Windows.Controls.DataGridBoundColumn   Binding  SymbolTemplate   loadedSignal1   +APLan.HelperClasses.CanvasObjectInformation,    &APLan.HelperClasses.CustomCanvasSignal   Name J? RenderTransformOrigin ?? RelativeSource u? Matrix*!  $Microsoft.Xaml.Behaviors.Interaction !  Triggers+"  %Microsoft.Xaml.Behaviors.EventTrigger "  	EventName*#  $Microsoft.Xaml.Behaviors.TriggerBase #  Actions2$  ,Microsoft.Xaml.Behaviors.InvokeCommandAction $  Command ?? Source $  CommandParameter ?? ElementName ! 
loadedText*%   $APLan.HelperClasses.CustomCanvasText % Name " CanvaContentTemplate$&   APLan.HelperClasses.CustomItem??%  &	        &	      &	 :     &	 Y     &	 u     '?? ?     'n? ?     &	      &	 3    &	 F    &	 Y    &		 l    &	
     &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	     &	     &	 *    &	 =    &	 P    &	 c    &	 v    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    &	 ?    7  7  &	 y    &	  ?    7  &	! N    7  &	" ?	    7  ??5   
   	Arrow??6&   ??5   
   	white??64   ??5   
   #cd7ef0??66   ??5   
   True??63     5   
   68   ?? 5   
   " ??6   .?5      ??  ? ??
 ?????6+   5      ?? 5   
   " n?6   .?5      ??  ? ?? ?#cc54f46+   5        5   
   6?     5   
   6?     5   
   6T     5   
   6T     5   
   6H     5    
   6Y     5#   
   6K   	  5$   
   6O   
  5&   
   6G     5'   
   6G     5)   
   6K     5*   
   6=     5+   
   6M     5.   
   6#     51   
   6+     54   
   6#     55   
   6-     58   
   6-     5;   
   6)     5<   
   6'     5?   
   6.     5B   
   6E     5E   
   6W     5F   
   6l     5J   
   6)   ?? 5O   
   " F?67   .R?5P      ?? 8??  $??1q?6?   $ 3q?6S   ._?5Q      ?? .+?5R      ?? ??5S      ?? 5T      $ DataContext?6"   8  65   5V      R? 5W      $??0q?6   $??Disabled=?5X      $??Disabled=?5Y      .@?5[      Q? $??Eulynx attributes??6#   .@?5\        $ False??5]   #   
	 ?????$

 True??5_   #   ??5`   #   ?? *DataContext.Data+ 5a   "     5b   &   $ Property??69    6M   ?? $	 Key?  5c   "   $ Value??65    6F   ?? $ Value?5e      5f      Q? 5g      $??Source files attributes??6#   .@?5h        $ False??5i   #   
	 ?????$

 True??5k   #   ??5l   #   ?? *DataContext.ExtraInfo+ 5m        5n   "   $ Property??65    6I   ?? $	 Key?  5o   "   $ Value??65    6F   ?? $ Value?5q      5r      5s      5t      5u      5v      ?? 5x   
   " ??6.   5z      ?? 5   
   "  5?      .R?5?         $ signal??6/   $ 0.5,0.5&?5?   /   ??5?   /   ?? *Scale+??5?   /   ??  ?? *self+$ Width???5?   /   ?? *SignalImageSource+z?5?      u? 5?       6&   ?? *RenderTransformMatrix+6R    5?      "  5?      $ Loaded??6%   . 5?      $   5?   0   ?? 8   $ ObjectLoaded? 5?   0   ?? $ signal??5?      5?      5?      ?? 5?   
   "  6+   .R?5?      %  $
 Text??6-   $	 0,0&?5?   -   ??5?   -   ?? *IncludedText+??5?   -   ?? *IncludedTextSize+.??5?   .   z?5?      u? 5?       6&   ?? *RenderTransformMatrix+6R   .??5?   B    5?      "  5?      $ Loaded??6%   . 5?      $   5?   0   ?? 8   $ ObjectLoaded? 5?   0   ?? $
 Text??5?      .??5?   *   5?      5?      ?? 5?   
   " & 64   .R?5?      ?? $??myText??6   $	??100??6'   $??Stretch=?63   ??6Q   ?? $ myText??$ DataContext?8   5?      5?      5?      5?      