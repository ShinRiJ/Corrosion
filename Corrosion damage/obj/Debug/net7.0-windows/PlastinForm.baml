   M S B A M L   `   `   `  ����K  GCorrosion damage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Corrosion_damageCorrosion_damage  5      "    Corrosion_damage.PlastinForm   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Corrosion_damage     5      .��5	      �   � ColumnDefinitions  5
   
   �� 5      -   5         ��Name$
 Col1��5      $��6* �6-   $	��400��68   �� 5      -   5        $
 Col2��5      $��3 �6-   �� 5      -   5        $
 Col3��5      $��* �6-   $	��280��67   $
��True��6F    � RowDefinitions 5   
   �� 5      -   5         ��Name$
 Row1��5      .+�5   
   �� $��1��6   $��3��6'   $��Stretch=�61   $��Stretch=�6O   6m   � 5   
    5      �� 5      $
��auto �6    �� 5      $��* �6    .+�5      �� -   5         ��Name$ 	InputName��5      $��26��6)   $	��300��65   $��0��6D   $��0 20 0 0q�6Q    �� TextAlignment$ Center=�5      $��16(�6-   .��6;   !Введите название6M   �� 5      $��Hidden=�5      $
��Auto=�5      $��1��5      .��5      � -   5         �Name$ ShowArea��5      $	��300��6)   .+�5       �� -   5         ��Name$ Dropper��5         J� 	AllowDrop$
 True��5!   !   
�������	 �� RadiusX$	 10��5#   !   
 �� RadiusY$
 10��5$   !    �� StrokeDashArray$ 10R�5%   !   $��2��5&   !    �� StrokeDashCap$ Round=�5'   !   
������� � Cursor$
 Hand��5)   !   $��Stretch=�5*   !   $��Stretch=�5+   !   $��40 40 40 40q�5,   !   $��0��5-   !   5.      5/      50      ]� 51   
   $��2��6   $
��True��6$   .+�52      �� $)��#Подстройка контура��6   
�������$	��Top=�6K   $��20q�6`   $��Stretch=�6l   .��53      �� .+�54      � $��20q�6     55      �� 56      $��* �6/   $	��100��69   �� 57      $��* �6/   $	��100��69    59      �� 5:      $��* �6,   �� 5;      $��* �6,   .+�5=      ��  �� Orientation$ Vertical=�6%   $��Center=�6<   $��0��6Y   $��0��6i   .+�5>      �� $��Верхний край��6(   $
��Wrap=�6<   �� 5?      -   5         ��Name$	 Top��5?   %    � Minimum$ 0Q�62    � Maximum$ 15Q�6>    � Value$ 0Q�6K    �� Orientation$ Vertical=�5@   !   $	��150��68   $��Center=�6E   5A      �� 5B      $ Vertical=�6%   $��Center=�6<   $��1��6Y   $��0��6i   .+�5C      �� $��Нижний край��6(   $
��Wrap=�6;   �� 5D      -	   5        $ Bottom��5D   %   $ 0Q�65   $ 15Q�6A   $ 0Q�6N   $ Vertical=�5E   %   $	��150��6<   $��Center=�6I   5F      �� 5G      $ Vertical=�6%   $��Center=�6<   $��0��6Y   $��1��6i   .+�5H      �� $��Левый край��6(   $
��Wrap=�6:   �� 5I      -
   5        $
 Left��5I   %   $ 0Q�63   $ 15Q�6?   $ 0Q�6L   $ Vertical=�5J   !   $	��150��68   $��Center=�6E   5K      �� 5L      $ Vertical=�6%   $��Center=�6<   $��1��6Y   $��1��6i   .+�5M      �� $��Правый край��6(   $
��Wrap=�6;   �� 5N      -   5        $ Right��5N   %   $ 0Q�64   $ 15Q�6@   $ 0Q�6M   $ Vertical=�5O   !   $	��150��68   $��Center=�6E   5P      5Q      �� 5R      -   5         ��Name$ GroupBorder��5R      $��20q�64   $-��'Групповое управление��6@    d� 	IsChecked$
 TrueY�6_   5S      5T      �� 5V      -   5         ��Name$ 
OutputText��5V      $	��Top=�6,   $	��200��6A   $��26��6P   $��Center=�6\   $��16(�6y   $
��Bold"�6�   �� 5W      $��Bottom=�6   $��
20 0 20 20q�61   .+�5X      �� -   5         ��Name$ SaveCalc��5X      $��Сохранить��6+   $��16(�6?   
�������
���   �
���   �$	��200��5Z   %   $��26��64   $��0 0 0 20q�6@   $y�	Collapsed=�6R   �� 5[      -   5        $ AreaCalc��5[      $!��Расчёт площади��6+   $��16(�6D   
�������
���   �
���   �$	��200��5\   L   $��26��6[    �� 	IsDefault$
 True��6g   5]      �� 5^      $��Bottom=�6   5_      5a      5b      