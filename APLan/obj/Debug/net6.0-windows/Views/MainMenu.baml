   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5      3clr-namespace:APLan.ViewModelsAPLan.ViewModels  5          APLan.Views.MainMenu   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:APLan.Views     5      4	viewModelclr-namespace:APLan.ViewModels     5      
???????   mainMenuViewModel  ? DataContext#  [  5
       ? 	Resources 5       ?? 
TargetType ?? Property ?? Value??%T   'm?       ?? 5   
   " m?6   .?5      ??  ? ??
 ?????6;   5      5      .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $??5* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   .+?5   
   o? $??Stretch=?6   
???????.@?5      m? $??_File??6   .@?5      m? $??_New project??6    m? Command 5      ??  ?? Source# [   ?? Path$ 
NewProject?	 m? CommandParameter	 5      ?? 
 ?? RelativeSource
 ??  ?? Mode$ FindAncestor=? ?? AncestorType" ? m? Icon 5      ?? 5       $??/Resources/Icons/new.png??6    5"      m? 5#      $??_Open??6    5$      ??   newProjectViewModel# [ $
 Open? 5%      ?? 5&      $??/Resources/Icons/open.png??6    5(      m? 5/      $??_Save??6   {?50      ?? # [ $ SaveButtonActive? 51      ?? *Save+ 52      ?? 53      $??/Resources/Icons/save.png??6    55      m? 56      $??_Save as??6   {?57      ?? # [ $ SaveAsButtonActive? 58      ?? *SaveAs+ 59      ?? 5:      $"??/Resources/Icons/save_as.png??6    5<      m? 5D      $??_Exit??6    5E      ?? *ExitProgram+	 5F      ?? 
 ?? $ FindAncestor=?" ? 5J      ?? 5K      $??/Resources/Icons/exit.png??6    5M      5N      m? 5O      $??_Edit??6   .@?5d      m? $??_Canvas??6   .@?5e      m? -   5        $??
BlackTheme??5e       m? 	IsChecked$ False??5f      5g   9   5h      5i      m? 5j      $??_View??6   .@?5l      m? -   5         m?Name$ visualizedDataItem??5l      $??_Visualized Data??5m      5n      m? 5o      -   5        $ canvasContentItem??5o      $??_Canvas Content??5p      5q      m? 5r      -   5        $??_Planning/Canvas tabs??5r      $
 True??5s      5t      m? 5u      -   5        $??_Symbols/Selection tabs??5u      $
 True??5v      5w      m? 5y      -   5        $??
SignalItem??5y      $??_Planned Signals info??5z      5{      5~      m? 5      $??_Tools??6   .@?5?      m? $??_EULYNX Validator??6    5?      ?? *EulynxValidator+ 5?      ?? 5?      $#??/Resources/Icons/validate.png??6    6I   5?      5?      m? 5?      $??_Help??6   .@?5?      m? $??
_About WPF??6    5?      ?? *
AboutWPF+ 5?      ?? 5?      $??/Resources/Icons/info.png??6    6E   5?      5?      5?      ?? 5?   
   $??1??6   $??Center=?5?      $
??Bold"?5?      ??5?      ?? # [ $ CurrentProjectNameBind?5?      ? 5?   
   $??2??6   
???????.+?5?      ? $??Center=?5?      $
??Left=?5?      y?5?      ??   globalLoading# [ $ LoadingVisibility?.+?5?      ?? -   5        $??myImage??5?      $&?? /Resources/Icons/eulynx-logo.png??6'   z?5?      ?? 5?       ?? Angle 6*   ?? # [ $ LoadingIconAngle? ?? CenterX 5?   *   ??  ?? ElementName$ myImage??$ ActualWidth?  HalfLengthConverter ?? 	Converter# [  ?? CenterY 5?   *   ?? $ myImage??$ ActualHeight?# [ 5?      5?      ?? 5?      $??Center=?6   ??5?      ?? # [ $ LoadingReport?5?      5?      5?      5?      