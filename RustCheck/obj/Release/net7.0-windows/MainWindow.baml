   M S B A M L   `   `   `  ���� D  @RustCheck, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RustCheck	RustCheck  E'clr-namespace:RustCheck.MVVM.ViewModelsRustCheck.MVVM.ViewModels      RustCheck.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  #localclr-namespace:RustCheck  8
viewmodels'clr-namespace:RustCheck.MVVM.ViewModels    � Title$  
MainWindow��$	��864��$
��1536�� � WindowStartupLocation$ CenterScreen=� � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       '�� #     '�� F     '�� ~     �� " ��.���  � ��$ 14(��� " ��.���  � ��$ 14(��� " ��.���  � ��$ 40����  � ��$ 5q��� " ��.���  � ��$ 12(�.���  � ColumnDefinitions �� $��* � � RowDefinitions �� $��25 ��� $��* �.+�o� $��0��$��Center=�.@�m� $��Открыть��m� $��Сохранить��m� $��Закрыть��*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger  	   	EventName$	 Click��*  $Microsoft.Xaml.Behaviors.TriggerBase
   Actions.
 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command �� *CloseCommand+�� -    ��Name$ MainTab��$��1��.@��� $+��%Выделение пластинок��.���  �� $��* �$	��683���� $��3 ��� $��0.25* �$	��336�� �� $��* �.+��� $
��Auto=�.���  �� $��* ��� $
��auto �.+�(�  (� Stretch$ Uniform=�$
��1064��$��0��.��� $��0��$	��384��.+��� $��20q� �� RadiusX$ 10�� �� RadiusY$ 10��
�������$��3�� �� StrokeDashArray$ 10R� � Cursor$
 Hand�� �� StrokeDashCap$ Round=� J� 	AllowDrop$
 True������ *
ImageBrush+ �� Mode$ OneWay=����� *
ImageWidth+���� *ImageHeight+   $
	 Drop��.
 4  .Microsoft.Xaml.Behaviors.Core.CallMethodAction     
MethodName$ 	LoadImage��   TargetObject ��   $	 PreviewMouseDown��.
    �� *LoadClickCommand+�� $��Center=�$��Center=�$��0��y��� *
LogoMarker+ �� UpdateSourceTrigger$ PropertyChanged=�.+��� $��35��$��35��$��35��$��35��$��Center=�$��Center=�.+�F� 
�������$��1�� �� StrokeStartLineCap$ Round=� �� StrokeEndLineCap$ Round=� �� StrokeLineJoin$ Round=� �� Stretch$
 Fill=�$��30��$��35����� .��B� $����M13.5 3H12H8C6.34315 3 5 4.34315 5                                                      6V18C5 19.6569 6.34315 21 8 21H12M13.5 3L19                                                     8.625M13.5 3V7.625C13.5 8.17728 13.9477 8.625                                                     14.5 8.625H19M19 8.625V11.8125C�B� $6��0M17.5 21L17.5 15M17.5 15L20 17.5M17.5 15L15 17.5C��� $!��Загрузка файла��$��12(�
��������� $��1�� �� Orientation$ 
Horizontal=�$��Center=�$��0 0 0 20q�.+��� $$��Гамма-коррекция:��$��0 0 10 0q���  �� Orientation$ 
Horizontal=�$	��250�� � Minimum$ 0Q� � Maximum$ 5Q�  � Value  �� *GammaSliderValueShow+{��� *
ImageExist+   $	 ValueChanged��.
   $ MainImageGammaChange�� �� �� $��1��$��Stretch=�$��Stretch=�$��3���� $#��Поиск пластинок��$��16(�$��2��$	��555��$��	20 0 20 0q�
�������.���� $
|�True��
������ $��Center=�{��� *
ImageExist+.+��� $��Center=�$��0 20 0 0q�$ 0Q�$ 5Q�$ Vertical=�$	��400��  �� *GammaSliderValueSearch+   $	 ValueChanged��.
    �� *FindPlastinCommand+�� $��50��$	��100��$��Рассчитать��$��16(�
�������
���   �
���   �$��
20 20 20 0q�! �� 	IsDefault$
! True��   $	 Click��.
   $ CompileNewPlastinsForm�� �� 