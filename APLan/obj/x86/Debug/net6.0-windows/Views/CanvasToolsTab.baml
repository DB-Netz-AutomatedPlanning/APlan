   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5      /clr-namespace:APLan.CommandsAPLan.Commands  5      3clr-namespace:APLan.ConvertersAPLan.Converters  5           APLan.Views.CanvasToolsTab   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:APLan.Views     5      1commandsclr-namespace:APLan.Commands     5      5
convertersclr-namespace:APLan.Converters     5         canvasToolsTabViewModel  ? DataContext#  [  5
       ? 	Resources 5       ?? 
TargetType ?? Property ?? Value  TextImageProperites  internalBorder??%  '??       &	 ?     '?? ?    &	     ?? 5   
   " ??6   .?5      ??  ? ??$ 30??6&   ?? 5       ? ??$ 30??6'   ?? 5       ? ??$ 5q?6'   ?? 5       ? ??$ Center=?62   ?? 5       ? ??
 ?????6+   5      ?? 5   
   " ??6,   .?5      ??  ? ??$ 30??6&   ?? 5       ? ??$ 30??6'   ?? 5       ? ??$ 5q?6'   ?? 5       ? ??$ Center=?62   5      ?? 5   
   " ??6   .?5      ??  ? ??$ 30??6&   ?? 5       ? ??$ 30??6'   ?? 5       ? ??$ 5q?6'   ?? 5       ?@??TextAlignment$ Center=?6.   ?? 5       ? ??
 ?????6+   5       ?? 5!   
   " ??6   .?5"      ??  ? ??$ Center=?62   ?? 5#       ? ??
 ?????6+   5$      5%      .??5'      ? 
???????$	??Top=?6#   .+?5(   
   ?? -   5         ??Name$ select??5(       ? ToolTip$ Activate selection??5)      $??Center=?5*      ??5+      ?? *SelectBrush+ ?? Mode$ TwoWay=???5,      ?? *SelectButton+??5-      `? 5.        multiBindingConverter `? 	Converter# [ 6   .1?5/      ?? 	 ?? ElementName$	 select??6   ?? 50      $
	 drag??6   ?? 51      $
	 move??6   52      .??54      ?? $!??/Resources/Icons/select.png??6   $
??Fill=?69   55      ?? 56   
   -   5        $
 move??56      $ Move selected items??57      $??Center=?58      ??59      ?? *	MoveBrush+$ TwoWay=???5:      ?? *
MoveButton+??5;      `? 5<      # [ 6   .1?5=      ?? $
	 move??6   ?? 5>      $
	 drag??6   ?? 5?      $	 select??6   5@      .??5B      ?? $??/Resources/Icons/move.png??6   $
??Fill=?67   5C      ?? 5D   
   -   5        $
 drag??5D      $??Center=?5E      $ 
Drag items??5F      ??5G      ?? *	DragBrush+$ TwoWay=???5H      ?? *
DragButton+.??5I      ?? $??/Resources/Icons/drag.png??6   $
??Fill=?67   5J      ?? 5K   
   -   5        
 ??Name$

 Text??5K      #??[ 5L      $??/Resources/Icons/text.png??5M      $
??Fill=?64   $??Center=?5N      $ drag and drop a Text??5O      #   APLan.Commands.MouseBehaviour   MouseDownCommand 5P      ??  ?? Source# [   ?? Path$ MouseDownCommand?5Q      ?? 5R   
   $* $Delete selected items (press delete)??6   $??Center=?5S      .??5T      ?? $!??/Resources/Icons/delete.png??6   $
??Fill=?69   6J   5U      ?? 5V   
   .+?5W      ? $??Center=?6   .+?5Y      ?? -   5         ??Name$ rotateInput??5Y       ?? TextAlignment$ Center=?5Z        drawViewModel#  [ 5[      $	??100??5]      ??5\      ?? *RotateTextBox+5]   $   ?? 5^      -   5        $ rotateSelection??5^      $ Rotate Selection??5_      $??Center=?5`      #  [ 5a      ??5b      ?? *RotateSelectionButton+.??5c      ?? $!??/Resources/Icons/rotate.png??6    $
??Fill=?6E   6V   5d      5e      ?? 5f      -   5         ??Name$ ItemRotateSlider??5f      $	??200??5g      #  [ 5h       ? Minimum$ 0Q?5i   ?    ? Maximum$	 360Q?6?    ? SmallChange$ 1Q?6?    ? Value 6   ?? $	 rotateInput??$
 Text?  stringToDoubleconverter ?? 	Converter# [ $   APLan.Commands.SliderBehaviour   ItemRotateSliderCommand 5j      ?? *RotateItemSlider+5k      ?? 5~      .+?5      ? $??Center=?6   .+?5?      ?? -   5         ??Name$ canvasRotationText??5?       ?? TextAlignment$ Center=?5?      $??Center=?5?      Rotate Canvas6H   ?? 5?      -	   5        $ CanvasRotateText??5?      $	??100??5?      $??Center=?5?      ??5?      ?? $	 canvasRotateSlider??$ Value?$ TwoWay=?.??6f   5?      5?      ?? 5?      -
   5        $ canvasRotateSlider??5?      $	??200??5?      $ 0Q?5?   ?   $	 360Q?6?   $ 1Q?6?    6   ?? $	 CanvasRotateText??$
 Text?# [    SliderCommand 5?      ?? *RotateCanvasSlider+5?      5?      5?      