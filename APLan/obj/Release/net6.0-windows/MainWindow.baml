   M S B A M L   `   `   `  ???? @  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:APLanAPLan  )clr-namespace:APLan.ViewsAPLan.Views  3clr-namespace:APLan.ViewModelsAPLan.ViewModels  3clr-namespace:APLan.ConvertersAPLan.Converters      APLan.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:APLan     +viewsclr-namespace:APLan.Views     5
viewModelsclr-namespace:APLan.ViewModels     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  /
Convertersclr-namespace:APLan.Converters    ? WindowState$  	Maximized=? ? Icon$&  /Resources/Icons/eulynx-logo.png?? ? 
ResizeMode$ 	CanResize=? ? Title$ 
MainWindow?? ? DataContext ??  ?? ResourceKey$ drawViewModel??*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ KeyDown??*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  	   PassEventArgsToCommand$
	 True??
   Command
 ?? *KeyDownForMainWindow+.???  ? RowDefinitions ?? -    ??Name$ R0??$??30 ??? -   $ R1??$	??130 ??? -   $ R2??$	??20* ??? -   $ R3??$??5 ??? -   $ R4??$
??0.5* ? ? ColumnDefinitions ?? -    ??Name$ C0??$??* ??? -   $ C1??$??5* ??? -   $ C2??$??0.01* ??? -	   $ C3??$??* ?.+?   APLan.Views.MainMenu  $??0??$??0??$??4?? K? ZIndex$ 2??   APLan.Views.Welcome  $??1??$??4??$??4??$ 3??   APLan.Views.Tabs  $??1??$??0??$??2??
???????$ 2??   APLan.Views.RightTabs  $??3??$??1??$??5??
???????$ 2???? $??2??$??1??$??3??$??Stretch=?$??Stretch=?   
themeColor#??[  $ 2???? -
    ??Name$ 
baseCanvas??$??2??$??0??$??2??$??2??.+?	   APLan.Views.pdfDetail	  -    	 Name$ pdfDetailsView??
???????$ 2?? ?? Right$ 0?? ?? Bottom$ 0??????  ?? ElementName$ 
baseCanvas?? ?? Path$ ActualWidth?  PdfDetailSizeConverter ?? 	Converter# [ ???? $ 
baseCanvas??$ ActualHeight?# [ y???   planTabViewModel ?? Source# [ $ PdfDetailViewerVisibility?
   APLan.Views.Draw
  -    
 Name$ 
drawingTab??$ 1??$
??-500q?  
backGround#???  ???? *ActualWidth+ ?? Mode$ OneWay=? ?? RelativeSource ?? *FindAncestor+ ?? AncestorType" ?  DrawUserControlSize# [ ???? *ActualHeight+$ OneWay=? ?? *FindAncestor+" ?# [ z???  ?? Angle ??   drawViewModel# [ $ CanvasRotation? ?? CenterX ?? *ActualWidth+  HalfLengthConverter# [ $ OneWay=? ?? *FindAncestor+" 
  ?? CenterY ?? *ActualHeight+# [ $ OneWay=? ?? *FindAncestor+" 
 #   APLan.Views.DrawingUpperLayer  $??1??$??2??$ 2???? $??3??$??2??#??[  $??Stretch=?$ 2???? $??4??$??2??
???????$ 2??.+?!   APLan.Views.DrawingLowerTab  $	??Top=?"   APLan.Views.SignalObjectInfo      APLan.Views.VisualizedData  -     Name$ visualizedData??$??0??$??2??$ -1??   APLan.Views.CanvasContent  -      Name$  CanvasContent??$??0??$??2??$ -1??