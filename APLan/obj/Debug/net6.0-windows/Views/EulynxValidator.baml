   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5   	   3clr-namespace:APLan.ViewModelsAPLan.ViewModels  5   	   !    APLan.Views.EulynxValidator   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:APLan.Views     5   	   5
viewModelsclr-namespace:APLan.ViewModels     5   	   -   5           EulynxValidatorViewModel  ? DataContext#  [  5	   	    ? 
ResizeMode$ NoResize=?5
   	    ? Icon$# /Resources/Icons/validate.png??5   	    ? Title$ Eulynx Validator??5   	   $	??450??6"   $	??800??6/    ? 	Resources 5       ?? 
TargetType 	 input ?? Property ?? Value ?? 	Resources  TextBoxBaseControlTemplate	 ?? 
TargetType
 ??Name ?? CornerRadius ??Name??%g  &	       '?? ?     &	 ?    ?? 5   
   " ??6   .?5      ??  ? ??$ 30??6'   ?? 5       ? ??$ 20q?6'   ?? 5       ? ??$ Center=?69   -   5        5      ?? 5   
   " ??6   .?5      ??  ? ??$ 30??6'   64   ?? 5       ? ??$ 20q?6'   64    5      ???? 5      '??       " ??6   .?5      ??  ?@??CornerRadius$ 5??65   6A   5      5      5      ?? 5   
   "	 ??6=   .R?5       ?? $
 Bd??5!      #??z??5       
???   ?$??1q?5"      $ 5??6%   ._?5#      ?? $ PART_ContentHost??6   5$      5%      5&      .??5'      ?  ? ColumnDefinitions 5(   
   ?? 5)      $??* ?6   ?? 5*      $??4* ?6   .+?5,   
   ?? $??0??6   $	??Top=?6%   $??10q?6=   .+?5-      ?? $??50??6   $??50??6   $??/Resources/Icons/info.png??6+   6P   ?? 5.      $
??Wrap=?6   $????EULYNX Validator is intended to validate against version 1.0 and version 1.1 validation schemas (XSD) found at https://www.eulynx.eu/index.php/dataprep. Validation is done through auto-detection of the provided EUXML file version, and apply the corresponding schema.??51   "   52      53      ? 54   
   $??1??6    55      ?? 56      $??3* ?6#   ?? 57      $??* ?6#    ? RowDefinitions 59      ?? 5:      $??0* ?6    ?? 5;      $??1* ?6    ?? 5<      $??1* ?6    ?? 5=      $??2* ?6    ?? 5>      $??1* ?6    .+?5E      ?? -   5         ??Name$ fileBox??5E      #??[ 5F      #??[ 5G      $??0??5H      $??1??6*   ??5I      ?? *XML+ ?? Mode$ TwoWay=?5J      ?? 5K      -   5        $ 	outputBox??5K      #??[ 5L      #??[ 5M      $??0??5N      $??2??6+   ??5P      ?? *Path+$ TwoWay=??? 5S      $??	File Path??6   $??20q?6)   $??1??65   $??1??6E   ??5U      ??  ?? Path$ FilePath??? 5V      $??Output Path??6   $??20q?6+   $??1??67   $??2??6G   ??5W      ?? $ 
OutputPath??? 5X      $??0??5Y      $??2??5Z      $??3??5[      $??	20,0,20,0q?5\        
themeColor#??[ 5]      .@?5^      ?? $??XSD validation??6   #??[ 62   .??5_      ?? $
??Auto=?5`      $
??Auto=?5a      .??5b      ??  ?? 
IsReadOnly$
 True??6:   $	??Top=?6L   
?????????6"   ?? *Report+6z   5c      5d      ?? 5e      $??Rules validation??6   #??[ 64   .??5f      ?? $
??Auto=?5g      $
??Auto=?5h      .??5i      ?? $
 True??6@   $	??Top=?6R   
?????? ??6"   ?? *Report_rules+6?   5j      5k      5m      ? 5o      $??2??5p      $??4??5q      $??2??5r       5s      ?? 5t      $??* ?6'   ?? 5u      $??* ?6'   ?? 5v      $??* ?6'   .+?5x      ?? $??0??6   $	??150??5y      $??Cancel??5z      ??5{      ?? *Cancel+??5|      ??  ?? RelativeSource ??  ?? Mode$ FindAncestor=? ?? AncestorType" ??? 5      $??1??6   $??Center=?5?      $??Center=?5?      y?5?      ?? *LoadingVisibility+.+?5?      ?? $&?? /Resources/Icons/eulynx-logo.png??6   $??30??6F   $??30??6R   z?5?      ?? 5?       ?? CenterX$ 15Q?5?   .    ?? CenterY$ 15Q?5?   .    ?? Angle 5?   .   ?? *LoadingIconAngle+5?   .   5?      ?? 5?      ??6    ?? *LoadingReport+6A   5?      ?? 5?      -   5         ??Name$ validate??5?      $??2??5?      $	??150??5?      $
??Bold"?5?      #??[ 5?      $??Validate??5?      ${?False??5?      ??5?      ?? *
Validate+5?      5?      5?      5?      5?      