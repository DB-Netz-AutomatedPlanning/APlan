   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5   	   1    +APLan.Views.ExportConfirmationAndValidation   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:APLan.Views     5   	   -   5        $??window??5   	     ? Icon$"  /Resources/Icons/success.png??5	   	    ? 
ResizeMode$ NoResize=?5
   	    ? Title$ Export Sucessfull??5   	   $	??450??6#   $	??800??60    ? 	Resources 5       ?? 
TargetType ?? Property ?? Value ?? 	Resources??%+  '??       ?? 5   
   " ??6   .?5      ??  ? ??$	 100??6&   64   ?? 5       ? ??$ 2q?6'   63   ?? 5       ? ??$ Center=?64   6E    5      ???? 5      '??       " ??6   .?5      ??  ?@??CornerRadius$ 5??65   6A   5      5      5      5      .??5      ? $??20q?6    ? RowDefinitions 5   
   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $	??10* ?6   ?? 5      $??* ?6   .+?5   
   ?? $??0??6   ??6   ??    exportWindowViewModel	 ?? Source#	 [  
 ?? Path$
 Successfull?6s   ? 5    
   $??1??6   .+?5!      ?? $??
Station : ??6   6+   ?? 5"      ??6   ??   newProjectViewModel#	 [ $
 ProjectName?5$   (   5%      ?? 5&   
   $??2??6   .??5'      ??  ?? 
IsReadOnly$
 True??6   ??6(   ?? #	 [  $
 EuxmlResult? ?? IsAsync$ False?? ?? FallbackValue$ loadingEuxml...??.??6?   5(      5)      ? 5*   
   $??3??6    ? ColumnDefinitions 5+      ?? 5,      $??* ?6#   ?? 5-      $??* ?6#   ?? 5.      $??* ?6#   .+?50      ?? $??0??6   ??51      ?? #	 [  $
 Ok?.??6`   Cancel??52      `? 53        multiBindingConverter `? 	Converter# [ 6#   .1?54      ??  ?? ElementName$ window??6"   55      57      ?? 58      $??2??6   $??Validate XML??5:      ??59      ?? #	 [  $
 ValidateXML???5;      `? 5<      # [ 6#   .1?5=      ?? $ window??6"   5>      5@      ? 5A      $??1??6   $??Center=?6#   .+?5B      ? y?6   ?? #	 [  $
 LoadingVisibility?.+?5C      ?? $&?? /Resources/Icons/eulynx-logo.png??6    J? RenderTransformOrigin$ 0.5,0.5&?6F   z?5D      ?? 5E       ?? Angle 6.   ?? #	 [  $
 LoadingIconAngle?5G      ?? 5H      ??6    ?? #	 [  $
 LoadingReport?6u   5I      5J      5K      5L      5M      