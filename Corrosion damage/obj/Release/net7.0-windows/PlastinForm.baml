   M S B A M L   `   `   `  ���� K  GCorrosion damage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Corrosion_damageCorrosion_damage  "    Corrosion_damage.PlastinForm   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Corrosion_damage     .���   � ColumnDefinitions  �� -    ��Name$
 Col1��$��6* �$	��400���� -   $
 Col2��$��3 ��� -   $
 Col3��$��* �$	��280��$
��True�� � RowDefinitions �� -    ��Name$
 Row1��.+��� $��1��$��3��$��Stretch=�$��Stretch=��  �� $
��auto ��� $��* �.+��� -    ��Name$ 	InputName��$��26��$	��300��$��0��$��0 20 0 0q� �� TextAlignment$ Center=�$��16(�.��!Введите название�� $��Hidden=�$
��Auto=�$��1��.��� -    �Name$ ShowArea��$	��300��.+��� -    ��Name$ Dropper�� J� 	AllowDrop$
 True��
�������	 �� RadiusX$	 10��
 �� RadiusY$
 10�� �� StrokeDashArray$ 10R�$��2�� �� StrokeDashCap$ Round=�
������� � Cursor$
 Hand��$��Stretch=�$��Stretch=�$��40 40 40 40q�$��0��]� $��2��$
��True��.+��� $)��#Подстройка контура��
�������$	��Top=�$��20q�$��Stretch=�.���� .+�� $��20q�  �� $��* �$	��100���� $��* �$	��100�� �� $��* ��� $��* �.+���  �� Orientation$ Vertical=�$��Center=�$��0��$��0��.+��� $��Верхний край��$
��Wrap=��� -    ��Name$	 Top�� � Minimum$ 0Q� � Maximum$ 15Q� � Value$ 0Q� �� Orientation$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��1��$��0��.+��� $��Нижний край��$
��Wrap=��� -	   $ Bottom��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��0��$��1��.+��� $��Левый край��$
��Wrap=��� -
   $
 Left��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� $ Vertical=�$��Center=�$��1��$��1��.+��� $��Правый край��$
��Wrap=��� -   $ Right��$ 0Q�$ 15Q�$ 0Q�$ Vertical=�$	��150��$��Center=��� -    ��Name$ GroupBorder��$��20q�$-��'Групповое управление�� d� 	IsChecked$
 TrueY��� -    ��Name$ 
OutputText��$	��Top=�$	��200��$��26��$��Center=�$��16(�$
��Bold"��� $��Bottom=�$��
20 0 20 20q�.+��� -    ��Name$ SaveCalc��$��Сохранить��$��16(�
�������
���   �
���   �$	��200��$��26��$��0 0 0 20q�$y�	Collapsed=��� -   $ AreaCalc��$!��Расчёт площади��$��16(�
�������
���   �
���   �$	��200��$��26�� �� 	IsDefault$
 True���� $��Bottom=�