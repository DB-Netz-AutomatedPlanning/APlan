   M S B A M L   `   `   `  ����@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:APLanAPLan  5   	   )clr-namespace:APLan.ViewsAPLan.Views  5   	   3clr-namespace:APLan.ViewModelsAPLan.ViewModels  5   	   3clr-namespace:APLan.ConvertersAPLan.Converters  5
   	       APLan.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:APLan     5   	   +viewsclr-namespace:APLan.Views     5   	   5
viewModelsclr-namespace:APLan.ViewModels     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5	   	   /
Convertersclr-namespace:APLan.Converters  5
   	     � WindowState$  	Maximized=�5   	    � Icon$&  /Resources/Icons/eulynx-logo.png��5   	    � 
ResizeMode$ 	CanResize=�5   	    � Title$ 
MainWindow��5   	    � DataContext 5      �� 5   
    �� ResourceKey$ drawViewModel��6   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5   
      	EventName$ KeyDown��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  	   PassEventArgsToCommand$
	 True��5   $   
   Command
 5   $   �� *KeyDownForMainWindow+5   $   5      .��5      �  � RowDefinitions 5   
   �� 5      -   5         ��Name$ R0��5      $��30 �6(   �� 5       -   5        $ R1��5       $	��130 �6(   �� 5!      -   5        $ R2��5!      $	��20* �6(   �� 5"      -   5        $ R3��5"      $��5 �6(   �� 5#      -   5        $ R4��5#      $
��0.5* �6(    � ColumnDefinitions 5%   
   �� 5&      -   5         ��Name$ C0��5&      $��* �6+   �� 5'      -   5        $ C1��5'      $��5* �6+   �� 5(      -   5        $ C2��5(      $��0.01* �6+   �� 5)      -	   5        $ C3��5)      $��* �6+   .+�5+   
      APLan.Views.MainMenu  $��0��6   $��0��6)   $��4��66    K� ZIndex$ 2��5,         APLan.Views.Welcome  5.   
   $��1��6   $��4��5/      $��4��50      $ 3��51      6+      APLan.Views.Tabs  52   
   $��1��6   $��0��53      $��2��54      
�������$ 2��56         APLan.Views.RightTabs  57   
   $��3��6   $��1��58      $��5��59      
�������$ 2��5;      �� 5<   
   $��2��6   $��1��5=      $��3��5>      $��Stretch=�5?      $��Stretch=�5@         
themeColor#��[  5A      $ 2��5B      �� 5C   
   -
   5         ��Name$ 
baseCanvas��5C      $��2��5D      $��0��5E      $��2��5F      $��2��5G      .+�5I      "	   APLan.Views.APlanCadToolView	  -   5         	 Name$ aplanCadToolView��5I   %   
�������$ 2��5N       �� Right$ 0��6+   
 �� Top$ 0��6<   ��5K      ��  �� ElementName$ 
baseCanvas�� �� Path$ ActualWidth� # AplanCadToolViewerSizeConverter �� 	Converter# [ ��5L      �� $ 
baseCanvas��$ ActualHeight�# [ y�5M      ��   aplanCADViewerViewModel �� Source# [ $  AplanCadToolViewVisibility�5N   <   
   APLan.Views.pdfDetail
  5O      -   5         
 Name$ pdfDetailsView��5O      
�������$ 2��5T      $ 0��6+    �� Bottom$ 0��6<   ��5Q      �� $ 
baseCanvas��$ ActualWidth�  PdfDetailSizeConverter# [ ��5R      �� $ 
baseCanvas��$ ActualHeight�# [ y�5S      ��   planTabViewModel# [ $ PdfDetailViewerVisibility�5T   <      APLan.Views.Draw  5U      -   5          Name$ 
drawingTab��5U      $ 1��5V      $
��-500q�5W        
backGround#���  5X      ��5Y      �� *ActualWidth+ �� Mode$ OneWay=� �� RelativeSource �� *FindAncestor+ �� AncestorType" �  DrawUserControlSize# [ ��5Z      �� *ActualHeight+$ OneWay=� �� *FindAncestor+" �# [ z�5[      �� 5\       �� Angle 6&   ��   drawViewModel# [ $ CanvasRotation� �� CenterX 5]   &   �� *ActualWidth+  HalfLengthConverter# [ $ OneWay=� �� *FindAncestor+"    �� CenterY  5^   &   �� *ActualHeight+# [ $ OneWay=� �� *FindAncestor+"  5`      5a      #   APLan.Views.DrawingUpperLayer  5b   
   $��1��6"   $��2��62   $ 2��6?   �� 5c   
   $��3��6   $��2��5d      #��[  5e      $��Stretch=�5f      $ 2��5g      �� 5h   
   $��4��6   $��2��5i      
�������$ 2��5k      .+�5l      !   APLan.Views.DrawingLowerTab  $	��Top=�6$   "   APLan.Views.SignalObjectInfo  5m      5n          APLan.Views.VisualizedData  5o   
   -   5        !  Name$! visualizedData��5p      $��0��5q      $��2��5r      $ -1��5s         APLan.Views.CanvasContent  5t   
   -   5        "  Name$" CanvasContent��5u      $��0��5v      $��2��5w      $ -1��5x          APLan.Views.AplanCADViewer  5y   
   -   5        #  Name$# AplanCADViewer��5z      $��0��5{      $��2��5|      $ -1��5}      5~      5      