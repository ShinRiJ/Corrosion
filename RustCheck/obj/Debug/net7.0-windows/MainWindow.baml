   M S B A M L   `   `   `  ����D  @RustCheck, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RustCheck	RustCheck  5   	   E'clr-namespace:RustCheck.MVVM.ViewModelsRustCheck.MVVM.ViewModels  5	   	       RustCheck.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   #localclr-namespace:RustCheck  5   	   8
viewmodels'clr-namespace:RustCheck.MVVM.ViewModels  5	   	     � Title$  
MainWindow��5   	   $	��864��6   $
��1536��6)    � WindowStartupLocation$ CenterScreen=�66    � 	Resources 5       �� 
TargetType �� Property �� Value��%�  '��       '�� H     '�� �     '�� !    �� 5   
   " ��6   .�5      ��  � ��$ 14(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 14(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$	 100��6&   �� 5       � ��$ 50��6'   �� 5       � ��$ 14(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 12(�6)   5      5      .��5      �  � ColumnDefinitions 5    
   �� 5!      $��* �6    � RowDefinitions 5#   
   �� 5$      $��25 �6   �� 5%      $��* �6   .+�5(   
   o� $��0��6   $��Center=�6   .@�5)      m� $��Открыть��6   m� 5*      $��Сохранить��6   m� 5+      $��Закрыть��6   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5,      +  %Microsoft.Xaml.Behaviors.EventTrigger  5-      	   	EventName$	 Click��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase
   Actions.
 5.      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *CloseCommand+5/      51      52      �� 54   
   -   5         ��Name$ MainTab��54   "   $��1��6   .@�55      �� $+��%Выделение пластинок��6   .��56      �  57      �� 58      $��* �6+   $	��683��65   �� 59      $��3 �6+   �� 5:      $��0.25* �6+   $	��336��69    5=      �� 5>      $��* �6(   .+�5A      �� $
��Auto=�6#   .��5B      �  5C      �� 5D   "   $��* �60   �� 5E   "   $
��auto �60   .+�5H      (�  (� Stretch$ Uniform=�6&   $
��1064��68   $��0��6H   .��5I   "   � $��0��6'   $	��384��67   .+�5J   &   �� $��20q�60    �� RadiusX$ 10��6<    �� RadiusY$ 10��6I   
�������$��3��5K   =    �� StrokeDashArray$ 10R�6Q    � Cursor$
 Hand��6f    �� StrokeDashCap$ Round=�6t    J� 	AllowDrop$
 True��5L   -   ��5J   V   �� *
ImageBrush+ �� Mode$ OneWay=���5L   >   �� *
ImageWidth+��6[   �� *ImageHeight+ 5M   *     5N   .   $
	 Drop��6=   .
 5O   2   4  .Microsoft.Xaml.Behaviors.Core.CallMethodAction     
MethodName$ 	LoadImage��6E      TargetObject 6\   �� 5P   /     5Q   .   $	 PreviewMouseDown��6=   .
 5R   2      6H   �� *LoadClickCommand+5S   /   5U   '   �� 5V   &   $��Center=�61   $��Center=�5W   )   $��0��5X   )   y�5Y   )   �� *
LogoMarker+ �� UpdateSourceTrigger$ PropertyChanged=�.+�5Z   *   �� $��35��61   $��35��5[   )   $��35��5\   )   $��35��5]   )   $��Center=�5^   )   $��Center=�5_   )   .+�5`   .   F� 
�������$��1��5a   )    �� StrokeStartLineCap$ Round=�5b   )    �� StrokeEndLineCap$ Round=�5c   )    �� StrokeLineJoin$ Round=�5d   )    �� Stretch$
 Fill=�5e   )   $��30��5f   )   $��35��5g   )   ��5h   2   � 5i   6   .��5j   :   B� $����M13.5 3H12H8C6.34315 3 5 4.34315 5                                                      6V18C5 19.6569 6.34315 21 8 21H12M13.5 3L19                                                     8.625M13.5 3V7.625C13.5 8.17728 13.9477 8.625                                                     14.5 8.625H19M19 8.625V11.8125C�5k   5   B� 5o   :   $6��0M17.5 21L17.5 15M17.5 15L20 17.5M17.5 15L15 17.5C�5p   5   5q   7   5s   /   5t   +   �� 5u   *   $!��Загрузка файла��64   $��12(�6J   
�������6X   5v   '   5w   #   5x      �� 5z      $��1��6)    �� Orientation$ 
Horizontal=�66   $��Center=�6O   $��0 0 0 20q�6l   .+�5{   "   �� $$��Гамма-коррекция:��6,   $��0 0 10 0q�6D   �� 5|   "    �� Orientation$ 
Horizontal=�6)   $	��250��6B    � Minimum$ 0Q�6N    � Maximum$ 5Q�6Z     � Value  6f   �� *GammaSliderValueShow+{�5}   )   �� *
ImageExist+ 5~   &     5   *   $	 ValueChanged��69   .
 5�   .     $ MainImageGammaChange��6A    6c   �� 5�   +   5�   #   5�      5�      5�      �� 5�      $��1��6#   $��Stretch=�63   $��Stretch=�6Q   $��3��6m   �� 5�      $#��Поиск пластинок��6   $��16(�68   $��2��6F   $	��555��6V   $��	20 0 20 0q�6c   
�������.��5�      �� $
|�True��6F   
������ $��Center=�6p   {�6%   �� *
ImageExist+.+�5�      �� $��Center=�6%   $��0 20 0 0q�6B   $ 0Q�6T   $ 5Q�6`   $ Vertical=�5�   %   $	��400��6<     5�   l   �� *GammaSliderValueSearch+ 5�   "     5�   &   $	 ValueChanged��65   .
 5�   *      6@   �� *FindPlastinCommand+5�   '   5�      �� 5�      $��Рассчитать��6%   
�������
���   �
���   �$��
20 20 20 0q�5�   L   ! �� 	IsDefault$
! True��6`    5�   "     5�   &   $	 Click��65   .
 5�   *     $ CompileNewPlastinsForm��6=    6a   �� 5�   '   5�      5�      5�      5�      5�      5�      5�      5�      