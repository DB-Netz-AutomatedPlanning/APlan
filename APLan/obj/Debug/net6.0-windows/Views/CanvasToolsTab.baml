   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5      /clr-namespace:APLan.CommandsAPLan.Commands  5      3clr-namespace:APLan.ConvertersAPLan.Converters  5           APLan.Views.CanvasToolsTab   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:APLan.Views     5      1commandsclr-namespace:APLan.Commands     5      5
convertersclr-namespace:APLan.Converters     5         canvasToolsTabViewModel  ? DataContext#  [  5
       ? 	Resources 5        TextBoxBaseControlTemplate ?? 
TargetType ??Name ?? CornerRadius ??Name ?? 
TargetType  viewbox1 ?? Property ?? Value  viewbox2  TextImageProperites  internalBorder??%?  &	       &	 ?     &	 )    '?? ?    &	 }    '?? 4    &	 !    ?? 5   
   " ??6=   .R?5      ?? $ Bd??5      #??z??5      
???   ?$??1q?5      $ 5??6%   ._?5      ?? $ PART_ContentHost??6   5      5      ?? 5   
   " (?6   .?5      ??  ? ??$ 75??6&   63   ?? 5       ? ??$ 75??6'   64   5      ?? 5   
   " (?6   .?5      ??  ? ??$	 200??6&   64   ?? 5       ? ??$ 75??6'   64   5      ?? 5   
   " ??6   .?5      ??  ? ??$ 30??6&   ?? 5       ? ??$ 30??6'   ?? 5       ? ??$ 5q?6'   ?? 5        ? ??$ Center=?62   ?? 5!       ? ??
 ?????6+   5"      ?? 5#   
   " ??6,   .?5$      ??  ? ??$ 30??6&   ?? 5%       ? ??$ 30??6'   ?? 5&       ? ??$ 5q?6'   ?? 5'       ? ??$ Center=?62   5(      ?? 5)   
   " ??6   .?5*      ??  ? ??$ 30??6&   ?? 5+       ? ??$ 30??6'   ?? 5,       ? ??$ 5q?6'   ?? 5-       ?@??TextAlignment$ Center=?6.   ?? 5.       ? ??$ Center=?69   5/      ?? 50   
   " ??6   .?51      ??  ? ??$ Center=?62   ?? 52       ? ??
 ?????6+   53      54      .??56      ? 
???????$	??Top=?6#   .+?57   
   (? #??[ 6   .??58      ?? -   5        	 ??Name$	 select??58      
 ? ToolTip$
 Activate Multi selection??59      $??Center=?5:      ??5;      ?? *SelectBrush+ ?? Mode$ TwoWay=???5<      ?? *SelectButton+??5=      `? 5>        multiBindingConverter `? 	Converter# [ 6#   .1?5?      ??  ?? ElementName$ select??6"   ?? 5@      $
 drag??6"   ?? 5A      $
 move??6"   5B      .??5D      ?? $!??/Resources/Icons/select.png??6   $
??Fill=?6=   5E      5F      (? 5G   
   #??[ 6   .??5H      ?? -   5        $
	 move??5H      $
 Move selected items??5I      $??Center=?5J      ??5K      ?? *	MoveBrush+$ TwoWay=???5L      ?? *
MoveButton+??5M      `? 5N      # [ 6#   .1?5O      ?? $
 move??6"   ?? 5P      $
 drag??6"   ?? 5Q      $ select??6"   5R      .??5T      ?? $??/Resources/Icons/move.png??6   $
??Fill=?6;   5U      5V      (? 5W   
   #??[ 6   .??5X   
   ?? -   5        $
	 drag??5X      $??Center=?5Y      $
 
Drag items??5Z      ??5[      ?? *	DragBrush+$ TwoWay=???5\      ?? *
DragButton+.??5]      ?? $??/Resources/Icons/drag.png??6   $
??Fill=?67   5^      5_      (? 5`   
   #??[ 6   .??5a      ?? -   5         ??Name$
 Text??5a      #??[ 5b      $??/Resources/Icons/text.png??5c      $
??Fill=?64   $??Center=?5d      $
 drag and drop a Text??5e      #   APLan.Commands.MouseBehaviour   MouseDownCommand 5f      ??  ?? Source# [   ?? Path$ MouseDownCommand?5g      5h      (? 5i   
   #??[ 6   .??5j      ?? .+?5k      ? $??Center=?6   .+?5m      ?? -   5         ??Name$ rotateInput??5m      #??[ 5n       ?? TextAlignment$ Center=?5o        drawViewModel#  [ 5p      $	??100??5r      ??5q      ?? *RotateTextBox+5r   $   ?? 5s      -   5        $	 rotateSelection??5s      $
 Rotate Selection??5t      $??Center=?5u      #  [ 5v      ??5w      ?? *RotateSelectionButton+.??5x      ?? $!??/Resources/Icons/rotate.png??6    $
??Fill=?6E   6V   5y      5z      ?? 5{      -   5         ??Name$ ItemRotateSlider??5{      $	??200??5|      #  [ 5}       ? Minimum$ 0Q?5~   ?    ? Maximum$	 360Q?6?    ? SmallChange$ 1Q?6?    ? Value 6   ?? $ rotateInput??$
 Text?  stringToDoubleconverter ?? 	Converter# [ $   APLan.Commands.SliderBehaviour   ItemRotateSliderCommand 5      ?? *RotateItemSlider+5?      5?      (? 5?   
   #??[ 6   .??5?      ?? .+?5?      ? $??Center=?6   .+?5?      ?? -   5         ??Name$ canvasRotationText??5?      $
??Bold"?5?       	 
themeColor#??[	 5?       ?? TextAlignment$ Center=?5?      $??Center=?5?      Rotate Canvas6H   ?? 5?      -	   5        $ CanvasRotateText??5?      #??[ 5?      $	??100??5?      $??Center=?5?      ??5?      ?? $ canvasRotateSlider??$ Value?$ TwoWay=?.??6f   5?      5?      ?? 5?      -
   5        $ canvasRotateSlider??5?      $	??200??5?      $ 0Q?5?      $	 360Q?6#   $ 1Q?61    5?      ?? $ CanvasRotateText??$
 Text?# [    ItemScaleSliderCommand 5?      ?? # [ $ RotateCanvasSlider?5?      5?      5?      5?      