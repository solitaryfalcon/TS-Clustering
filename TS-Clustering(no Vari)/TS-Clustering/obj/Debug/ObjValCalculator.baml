   M S B A M L   `   `   `  ����H  DTS-Clustering, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:TS_ClusteringTS_Clustering  5   	   $    TS_Clustering.ObjValCalculator   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:TS_Clustering     5   	     � Title$  ObjVal Calculator��5   	   $��409.756��6#   $��688.85��64    � 
ResizeMode$ NoResize=�6C   .��5	      � .+�5
   
   �� -   5        $��textBox1��5
   3   $��28��6   $��10,43,131,0q�6   $	��Top=�6C    �� 
IsReadOnly . 6[   �� 5   
   -   5        $��btn_data��5   B   $��Open...��6   $��28��6#   $��	0,43,10,0q�6/   $	��Top=�6R   $��Right=�6�   $��95��6�   �� 5   
   -   5        $��label1��5   e   $��Input Data file:��6   $��28��6+   $
��Left=�67   $��	10,10,0,0q�6R   $	��Top=�6s   $	��127��6�   $��15(�6�   �� 5   
   -   5        $��	btn_start��5   C   $��Start��6   $��28��6!   $��190,0,190,13q�6-   $��Bottom=�6T   6o   �� 5   
   -   5        $��label2��5   u   $��Clustering Result:��6   $��15(�6-   $��31��6;   $
��Left=�6G   $��	10,76,0,0q�6b   $	��Top=�6�   $	��179��6�   �� 5   
   -   5        $��textBox2��5   4   $��28��6   $��10,112,131,0q�6   $	��Top=�6D    . 6\   �� 5   
   -   5        $��
btn_result��5   C   $��Open...��6   $��28��6#   $��
0,112,10,0q�6/   $	��Top=�6U   $��Right=�6�   $��95��6�   � 5   
   $��50��6   $��0,0,0,46q�6   $��Bottom=�6-   .+�5      �� -   5         ��Name$ label3_Copy��5   o   $��	ObjVal =:��6   $��15(�6(   $��31��66   $
��Left=�6B   $��5,10,0,0q�6]   $	��Top=�6�   $��71��6�   �� 5      -	   5         ��Name$ tb_a1��5   P   $��25��6   $
��Left=�6"   $��	76,13,0,0q�6=   $	��Top=�6_   $��30��6w   $��0��6�   6�   �� 5      -
   5        $ tb_sum��5   E   $
��Left=�6   $��
133,13,0,0q�61   $��80��6U    . $��0��6r   $��25��6{   $	��Top=�6�   �� 5      -   5        $ tb_aver��5   Q   $��25��6   $
��Left=�6"   $��
297,13,0,0q�6=   $	��Top=�6b   $��80��6z    . $��0��6�   �� 5      -   5        $ 	tb_result��5   6   $��25��6   $��
568,13,7,0q�6"   $	��Top=�6I    . $��0��6s   �� 5      -   5        $ label3_Copy1��5   i   $��×��6   $��15(�6    $��31��6.   $
��Left=�6:   $��
106,10,0,0q�6U   $	��Top=�6   $��27��6�   �� 5      -   5        $ label3_Copy3��5   k   $	�� + ��6   $��15(�6"   $��31��60   $
��Left=�6<   $��
213,10,0,0q�6W   $	��Top=�6�   $��27��6�   �� 5      -   5        $ label3_Copy5��5   k   $	�� + ��6   $��15(�6"   $��31��60   $
��Left=�6<   $��
377,10,0,0q�6W   $	��Top=�6�   $��27��6�   �� 5      -   5        $ tb_a2��5   Q   $��25��6   $
��Left=�6"   $��
240,13,0,0q�6=   $	��Top=�6`   $��30��6x   $��0��6�   6�   �� 5      -   5        $ tb_a3��5   Q   $��25��6   $
��Left=�6"   $��
404,13,0,0q�6=   $	��Top=�6`   $��30��6x   $��0��6�   6�   �� 5      -   5        $ label3_Copy2��5   i   $��×��6   $��15(�6    $��31��6.   $
��Left=�6:   $��
270,10,0,0q�6U   $	��Top=�6   $��27��6�   �� 5      -   5        $ label3_Copy6��5   i   $��×��6   $��15(�6    $��31��6.   $
��Left=�6:   $��
434,10,0,0q�6U   $	��Top=�6   $��27��6�   �� 5       -   5        $ label3_Copy4��5    k   $	�� = ��6   $��15(�6"   $��31��60   $
��Left=�6<   $��
541,10,0,0q�6W   $	��Top=�6�   $��27��6�   �� 5!      -   5        $ tb_vari��5!   Q   $��25��6   $
��Left=�6"   $��
461,13,0,0q�6=   $	��Top=�6b   $��80��6z    . $��0��6�   5#      �� 5$   
   -   5        $ tb_csv��5$   M   $��28��6   $
��Left=�6   $��
339,76,0,0q�69   $	��Top=�6]   $	��100��6u   $y�Hidden=�6�   $��4��6�   �� 5%   
   -   5        $ 	tb_detail��5%      $��10,145,10,101q�6%    �� TextWrapping$
 Wrap=�6<   $��13.333(�6P    . $ �� HorizontalScrollBarVisibility$
 Auto=�6t   " �� VerticalScrollBarVisibility$
 Auto=�6�   5'      5(      