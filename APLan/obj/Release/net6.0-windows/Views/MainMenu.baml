   M S B A M L   `   `   `  ???? @  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  3clr-namespace:APLan.ViewModelsAPLan.ViewModels      APLan.Views.MainMenu   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     +localclr-namespace:APLan.Views     4	viewModelclr-namespace:APLan.ViewModels     
???????   mainMenuViewModel  ? DataContext#  [   ? 	Resources  ?? 
TargetType ?? Property ?? Value??%/   'm?       ?? " m?.???  ? ??
 ?????.???  ? ColumnDefinitions ?? $??5* ??? $??* ??? $??* ?.+?o? $??Stretch=?
???????.@?m? $??_File??.@?m? $??_New project?? m? Command ??  ?? Source# [   ?? Path$ 
NewProject?	 m? CommandParameter	 ?? 
 ?? RelativeSource
 ??  ?? Mode$ FindAncestor=? ?? AncestorType" ? m? Icon ?? $??/Resources/Icons/new.png??m? $??_Open?? ??   newProjectViewModel# [ $
 Open? ?? $??/Resources/Icons/open.png??m? $??_Save??{??? # [ $ SaveButtonActive? ?? *Save+ ?? $??/Resources/Icons/save.png??m? $??_Save as??{??? # [ $ SaveAsButtonActive? ?? *SaveAs+ ?? $"??/Resources/Icons/save_as.png??m? $??_Exit?? ?? *ExitProgram+	 ?? 
 ?? $ FindAncestor=?" ? ?? $??/Resources/Icons/exit.png??m? $??_Edit??.@?m? $??_Canvas??.@?m? -   $??
BlackTheme?? m? 	IsChecked$ False??m? $??_View??.@?m? -    m?Name$ visualizedDataItem??$??_Visualized Data??m? -   $ canvasContentItem??$??_Canvas Content??m? -   $??_Planning/Canvas tabs??$
 True??m? -   $??_Symbols/Selection tabs??$
 True??m? -   $??
SignalItem??$??_Planned Signals info??m? $??_Tools??.@?m? $??_EULYNX Validator?? ?? *EulynxValidator+ ?? $#??/Resources/Icons/validate.png??m? $??_Help??.@?m? $??
_About WPF?? ?? *
AboutWPF+ ?? $??/Resources/Icons/info.png???? $??1??$??Center=?$
??Bold"????? # [ $ CurrentProjectNameBind?? $??2??
???????.+?? $??Center=?$
??Left=?y???   globalLoading# [ $ LoadingVisibility?.+??? -   $??myImage??$&?? /Resources/Icons/eulynx-logo.png??z???  ?? Angle ?? # [ $ LoadingIconAngle? ?? CenterX ??  ?? ElementName$ myImage??$ ActualWidth?  HalfLengthConverter ?? 	Converter# [  ?? CenterY ?? $ myImage??$ ActualHeight?# [ ?? $??Center=????? # [ $ LoadingReport?