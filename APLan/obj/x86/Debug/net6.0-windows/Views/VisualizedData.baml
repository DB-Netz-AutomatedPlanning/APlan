   M S B A M L   `   `   `  ????@  <APLan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:APLan.ViewsAPLan.Views  5           APLan.Views.VisualizedData   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:APLan.Views     5        ? 	Resources  5	       ?? 
TargetType ?? Property ?? Value??%V   '??       ?? 5
   
   " ??6   .?5      ??  ? ??$ Center=?62   5      5      .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $??* ?6   ?? 5      $??* ?6    ? RowDefinitions 5   
   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   .+?5   
   ?? $??0??6   $??1??5      $??Knoten??5        d? 	IsChecked$
 TrueY?5!         visualizedDataViewModel ? DataContext# [  5"      ??5#      ?? *Knoten+??5$      ??  ?? RelativeSource ?? *self+6W   ?? 5%   
   $??1??6   $??1??5&      $??Kanten??5'      $
 TrueY?5(      # [  5)      ??5*      ?? *Kanten+??5+      ??  ?? *self+?? 5,   
   $??2??6   $??1??5-      $??LA??5.      $
 TrueY?5/      # [  50      ??51      ?? *
Horizontal+??52      ??  ?? *self+6W   ?? 53   
   $??3??6   $??1??54      $??HO??55      $
 TrueY?56      # [  57      ??58      ?? *
Vertical+??59      ??  ?? *self+6W   ?? 5:   
   $??4??6   $??1??5;      $??UH??5<      $
 TrueY?5=      # [  5>      ??5?      ?? *Cant+??5@      ??  ?? *self+6W   ?? 5A   
   $??5??6   $??1??5B      $??KM??5C      $
 TrueY?5D      # [  5E      ??5F      ?? *	Meilage+??5G      ??  ?? *self+6W   ?? 5L   
   $??1??6   $??0??5M      $??Kanten points??5N      $ FalseY?5O      # [  5P      ??5Q      ?? *KantenPoints+??5R      ??  ?? *self+?? 5S   
   $??2??6   $??0??5T      $??	LA points??5U      $ FalseY?5V      # [  5W      ??5X      ?? *HorizontalPoints+??5Y      ??  ?? *self+6W   ?? 5Z   
   $??3??6   $??0??5[      $??	HO points??5\      $ FalseY?5]      # [  5^      ??5_      ?? *VerticalPoints+??5`      ??  ?? *self+6W   ?? 5a   
   $??4??6   $??0??5b      $??	UH points??5c      $ FalseY?5d      # [  5e      ??5f      ?? *
CantPoints+??5g      ??  ?? *self+6W   ?? 5h   
   $??5??6   $??0??5i      $??	KM points??5j      $ FalseY?5k      # [  5l      ??5m      ?? *MeilagePoints+??5n      ??  ?? *self+6W   5o      5p      