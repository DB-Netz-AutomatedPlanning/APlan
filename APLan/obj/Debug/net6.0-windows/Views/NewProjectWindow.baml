   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5   	   "    APLan.Views.NewProjectWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:APLan.Views     5   	   -   5          ?Name$  newProjectWindow??5	   	      newProjectViewModel ? DataContext# [  5   	    ? Title$ New Project??5
   	   $	??400??5   	   $	??500??5   	    ? Icon$ /Resources/Icons/new.png??5   	    ? 
ResizeMode$ NoResize=?5   	    ? 	Resources 5        TextBoxBaseControlTemplate ?? 
TargetType ??Name ?? CornerRadius	 ??Name
 ?? 
TargetType ?? Property ?? Value ?? 	Resources??%?  &	       '?? ?     7  '?? ?    '?? J    ?? 5   
   " ??6=   .R?5      ?? $ Bd??5      #??z??5      
???   ?$??1q?5      $ 5??6%   ._?5      ?? $	 PART_ContentHost??6   5      5      ?? 5   
   "
 ??6   .?5      ??  ? ??$ Center=?69   6J   ?? 5       ? ??
 ?????6:   ?? 5       ? ??$ 2q?6'   63   ?? 5       ? ??8   6_   -   5        5      ?? 5    
   "
 ??6   .?5!      ??  ? ??$ 2q?6'   63    5"      ???? 5#      '??       "
 ??6   .?5$      ??  ?@??CornerRadius$ 5??65   6A   5%      5&      5'      ?? 5)   
   "
 ??6   .?5*      ??  ? ??$ Center=?69   6J   ?? 5+       ? ??$ 2q?6'   63   5,      5.      .??50      ? -   5         ?Name$ 	container??50      $??20q?6    ? RowDefinitions 52   
   ?? 53      $??* ?6   ?? 54      $??* ?6   ?? 55      $??* ?6   ?? 56      $??* ?6   ?? 57      $??* ?6   ?? 58      $??* ?6   ?? 59      $??* ?6   ?? 5:      $??* ?6   ?? 5;      $??* ?6   ?? 5<      $??* ?6    ? ColumnDefinitions 5>   
   ?? 5?      $??* ?6   ?? 5@      $??2* ?6   .+?5D   
   ?? $
??Left=?6   $??Center=?6/   $??0??6J   $??0??6W   $??Project Name??6g   ?? 5E   
   $
??Left=?6   $??Center=?6/   $??1??6J   $??0??6W   $??	Directory??6g   ?? 5F   
   $
??Left=?6   $??Center=?6/   $??2??6J   $??0??6W   $??Country Code??6g   ?? 5G   
   $
??Left=?6   $??Center=?6/   $??3??6J   $??0??6W   $??File Format??6g   ?? 5H   
   -   5         ??Name$ 	jsonFiles??5H   y   $
??Left=?6   $??Center=?6/   $??4??6J   $??0??6W   $??
json files??6g   6y   ?? 5I   
   -   5        $ mdbfile??5I   w   $
??Left=?6   $??Center=?6/   $??5??6J   $??0??6W   $??mdb file??6g   6w   ?? 5J   
   -   5        $ 	euxmlfile??5J   y   $
??Left=?6   $??Center=?6/   $??6??6J   $??0??6W   $??
euxml file??6g   6y   ?? 5K   
   -   5        $ 	ppxmlfile??5K   y   $
??Left=?6   $??Center=?6/   $??7??6J   $??0??6W   $??
ppxml file??6g   6y   ?? 5L   
   -   5        $ dxfFile??5L   w   $
??Left=?6   $??Center=?6/   $??8??6J   $??0??6W   $??dxf file??6g   6w   ?? 5N   
   -	   5         ??Name$ projectNameBox??5P      #??[ 5O      $??30??5Q      $??0??5R      $??1??5S      
?????????5V      ?? *HelperHintProjectName+??5W      ?? *ProjectName+ ?? Mode$ TwoWay=?? 5X   
   $??30??6   $??1??6   $??1??6)    5Y      ?? 5Z      $??3* ?6#   ?? 5[      $??* ?6#   .+?5]      ?? -
   5        $ directortPathBox??5]      $??0??5^      $??Stretch=?5`      $??Stretch=?5a      
?????????5c      ?? *HelperHintProjectPath+??5d      ?? *ProjectPath+?? 5e      $??1??6   $??Stretch=?5f      $??Stretch=?5g      $??Add Path??5h      ??5i      ?? *	AddPath+5k      ?? 5m   
   -   5         ??Name$ 	countCode??5m      $??30??6&   $??2??62   $??1??6?    ?? SelectedIndex$ 0??6O    ?? SelectedItem 6a   ?? *	Country+$ TwoWay=?.@?5n      ?? $??de??6   ?? 5o      $??fr??6   5p      ?? 5r   
   -   5        $ fileType??5r      $??30??6%   $??3??61   $??1??6>   $ 0??6N    6`   ?? *Format+$ TwoWay=?.@?5s      ?? $??.json??6   ?? 5t      $
??.mdb??6   ?? 5u      $??.euxml??6   ?? 5v      $
??.dxf??6   ?? 5w      $??.ppxml??6   ${?False??6.   5x      ? 5z   
   -   5        $ n1??5z      $??30??6   $??4??6'   $??1??64    5{      ?? 5|      $??3* ?6#   ?? 5}      $??* ?6#   .+?5      ?? -   5        $ jsonFilesBox??5      $??0??5?      $??Stretch=?5?      $??Stretch=?5?      
?????????5?      ?? *	JsonFiles+?? 5?      -   5         ??Name$ ButtonEntwurfselement_KM??5?      $??1??5?      $??Stretch=?5?      $??Stretch=?5?      $??Browse??5?      ??5?      ?? *
BrowseJson+??5?      ??  ?? ElementName$ createProject??5?      ? 5?   
   -   5        $ n2??5?      $??30??6   $??5??6(   $??1??65    5?      ?? 5?      $??3* ?6#   ?? 5?      $??* ?6#   .+?5?      ?? -   5        $ 
mdbFileBox??5?      $??0??5?      $??Stretch=?5?      $??Stretch=?5?      
?????????5?      ?? *MDB+?? 5?      -   5        $ 	Buttonmdb??5?      $??1??5?      $??Stretch=?5?      $??Stretch=?5?      $??Browse??5?      ??5?      ?? *	BrowseMDB+??5?      ?? $ createProject??6R   5?      ? 5?   
   -   5        $ n3??5?      $??30??6   $??6??6(   $??1??65    5?      ?? 5?      $??3* ?6#   ?? 5?      $??* ?6#   .+?5?      ?? -   5        $ euxmlFileBox??5?      $??0??5?      $??Stretch=?5?      $??Stretch=?5?      
?????????5?      ?? *EUXML+?? 5?      -   5        $ Buttoneuxml??5?      $??1??5?      $??Stretch=?5?      $??Stretch=?5?      $??Browse??5?      ??5?      ?? *BrowseEuxml+??5?      ?? $ createProject??6R   5?      ? 5?   
   -   5        $ n4??5?      $??30??6   $??7??6(   $??1??65    5?      ?? 5?      $??3* ?6#   ?? 5?      $??* ?6#   .+?5?      ?? -   5        $ ppxmlFileBox??5?      $??0??5?      $??Stretch=?5?      $??Stretch=?5?      ??5?      ?? *PPXML+?? 5?      -   5        $ ppxml??5?      $??1??5?      $??Stretch=?5?      $??Stretch=?5?      $??Browse??5?      ??5?      ?? *BrowsePpxml+??5?      ?? $ createProject??6R   5?      ? 5?   
   -   5        $ n5??5?      $??30??6   $??8??6(   $??1??65    5?      ?? 5?      $??3* ?6#   ?? 5?      $??* ?6#   .+?5?      ?? -   5        $ 
dxfFileBox??5?      $??0??5?      $??Stretch=?5?      $??Stretch=?5?      ??5?      ?? *DXF+?? 5?      -   5        $	 dxf??5?      $??1??5?      $??Stretch=?5?      $??Stretch=?5?      $??Browse??5?      ??5?      ?? *	BrowseDxf+??5?      ?? $ createProject??6R   5?      ? 5?   
   $??30??6   $??9??6   $??0??6)   $??2??69    5?      ?? 5?      $??* ?6#   ?? 5?      $??* ?6#   .+?5?      ?? $??Cancel??6   $??0??5?      $??	50,0,50,0q?5?      $??Stretch=?5?      $??Stretch=?5?      ??5?      ?? *Cancel+??5?      ??  ?? RelativeSource ??  ?? Mode$ FindAncestor=? ?? AncestorType" ?5?   V   ?? 5?      -   5        $ createProject??5?      $??Create??5?      $??1??5?      $??	50,0,50,0q?5?      $??Stretch=?5?      $??Stretch=?5?      ${?False??5?        
themeColor#??[ 5?      $
??Bold"?5?      ???	#FF707070??5?      ?? *Create+??5?      `? 5?        multiBindingConverter `? 	Converter# [ 6#   .1?5?      ?? $ fileType??6"    ?? Path$ SelectedItem?69   ?? 5?      $ newProjectWindow??6"   5?      5?      5?      5?      5?      