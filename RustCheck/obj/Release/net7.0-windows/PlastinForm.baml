   M S B A M L   `   `   `  ���� D  @RustCheck, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RustCheck	RustCheck  E'clr-namespace:RustCheck.MVVM.ViewModelsRustCheck.MVVM.ViewModels      RustCheck.PlastinForm   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  #localclr-namespace:RustCheck  8
viewmodels'clr-namespace:RustCheck.MVVM.ViewModels  .���   � ColumnDefinitions  �� -    ��Name$
 Col1��$��6* �$	��400���� -   $
 Col2��$��3 ��� -   $
 Col3��$��* �$	��280��$
��True�� � RowDefinitions �� -    ��Name$
 Row1��.+��� $��1��$��3��$��Stretch=�$��Stretch=��  �� $
��auto ��� $��* �.+��� -    ��Name$ 	InputName��$��26��$	��300��$��0��$��0 20 0 0q� �� TextAlignment$ Center=�$��16(�.��!Введите название�� $
��Auto=�$��1��.���  �� $��* ��� $
��auto �.+�� .+�� $��0��$	��384��.+��� $��20q� �� RadiusX$ 10�� �� RadiusY$ 10��
�������$��3�� �� StrokeDashArray$ 10R�	 � Cursor$
	 Hand��
 �� StrokeDashCap$
 Round=� J� 	AllowDrop$
 True������ *
ImageBrush+ �� Mode$ OneWay=����� *
ImageWidth+���� *ImageHeight+�� $��1�� �� Orientation$ 
Horizontal=�$��Center=�$��0 0 0 20q�.+��� $$��Гамма-коррекция:��$��0 0 10 0q���  �� Orientation$ 
Horizontal=�$	��250�� � Minimum$ 0Q� � Maximum$ 5Q� � Value �� *GammaSliderValueShow+{��� *
ImageExist+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ ValueChanged��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 4  .Microsoft.Xaml.Behaviors.Core.CallMethodAction     
MethodName$ MainImageGammaChange��   TargetObject �� ]� $��2��$
��True��.+��� $)��#Подстройка контура��
�������$	��Top=�$��20q�$��Stretch=�.���� .+�� $��20q�  �� $��* �$	��100���� $��* �$	��100�� �� $��* ��� $��* �.+��� $ Vertical=�$��Center=�$��0��$��0��.+��� $��Верхний край��$
��Wrap=��� -    ��Name$	 Top��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��1��$��0��.+��� $��Нижний край��$
��Wrap=��� -   $ Bottom��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��0��$��1��.+��� $��Левый край��$
��Wrap=��� -   $
 Left��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��1��$��1��.+��� $��Правый край��$
��Wrap=��� -	   $ Right��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� -
    ��Name$ GroupBorder��$��20q�$-��'Групповое управление�� d� 	IsChecked$
 TrueY��� -    ��Name$ 
OutputText��$	��Top=�$	��200��$��26��$��Center=�$��16(�$
��Bold"��� $��Bottom=�$��
20 0 20 20q�.+��� -    ��Name$ SaveCalc��$��Сохранить��$��16(�
�������
���   �
���   �$	��200��$��26��$��0 0 0 20q�$y�	Collapsed=��� -   $ AreaCalc��$!��Расчёт площади��$��16(�
�������
���   �
���   �$	��200��$��26�� �� 	IsDefault$
 True���� $��Bottom=�