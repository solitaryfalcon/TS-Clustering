   M S B A M L   `   `   `  ����H  DTS-Clustering, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:TS_ClusteringTS_Clustering  5   	       TS_Clustering.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:TS_Clustering     5   	     � Title$"  TS_Clustering(noVari)1606013��5   	   $	��500��6.   $	��550��6;   $	��450��6G   $	��500��6W   .��5	      � .+�5
   
   �� -   5         ��Name$ tb_log��5
      $��10,315,10,35q�6"    �� TextWrapping$
 Wrap=�68   $�� ��6L   ��.  $ �� HorizontalScrollBarVisibility$
 Auto=�6g   " �� VerticalScrollBarVisibility$
 Auto=�6�   .��6�    J� 	CacheMode 5      &   System.Windows.Media.BitmapCache  5      .��5   !   5      � 5   
   $	��100��6   $��
10,10,10,0q�6   $	��Top=�60   .+�5      �� -   5         ��Name$ label��5      $��Open input file:��6#   $
��Left=�6>   $��10,0,0,0q�6Y   $	��Top=�6k   $	��150��6�   �� 5      -   5        $ tb_inputFilePath��5      $��23��60   $��10,25,105,0q�6<   $
 Wrap=�6Q   $�� ��6e   $	��Top=�6m    �� 
IsReadOnly . 6�   �� 5      -   5        $ label1��5      $��Define output folder��6$   $
��Left=�6C   $��	10,0,0,27q�6^   $��Bottom=�6q   $	��150��6�   �� 5      -   5        $ tb_outputPath��5      $��23��6-   $��
10,0,105,4q�69   $
 Wrap=�6M   $�� ��6a   $��Bottom=�6i    . 6�   �� 5      -   5         ��Name$ btn_inputFile��5      $��Open..��6,   $��0,25,0,0q�6=   $	��Top=�6O   $��Right=�6g   $��85��6�   $��23��6�   �� 5      -   5        $ btn_outputFolder��5      $��	Browser..��6/   $��0,0,0,4q�6C   $��Right=�6T   $��85��6p   $��23��6�   $��Bottom=�6�   5      � 5   
   $��75��6   $��10,110,10,0q�6   $	��Top=�60   .+�5      �� -   5        $ label2��5      $��LBound:��6$   $
��Left=�66   $��	10,10,0,0q�6Q   $	��Top=�6d   $��70��6|   �� 5      -	   5        $ label3��5      $��UBound:��6$   $
��Left=�66   $��
120,10,0,0q�6Q   $	��Top=�6e   $��66��6}   �� 5      -
   5        $ 	tb_lbound��5      $
��Left=�6)   $��23��6D   $��	80,14,0,0q�6P   $
 Wrap=�6c   $��2��6w   $	��Top=�6�   $��40��6�   �� 5      -   5        $ 	tb_ubound��5      $
��Left=�6)   $��23��6D   $��
186,14,0,0q�6P   $
 Wrap=�6d   $�� ��6x   $	��Top=�6�   $��40��6�   �� 5      -   5        $ label4��5      $��ObjVal=��6$   $
��Left=�66   $��	10,0,0,10q�6Q   $��Bottom=�6d   $��70��6   �� 5      -   5        $ tb_a1��5      $
��Left=�6%   $��23��6@   $��80,0,0,8q�6L   $
 Wrap=�6^   $�� ��6r   $��Bottom=�6z   $��40��6�   �� 5      -   5        $ label5��5      $��	× Sum + ��6$   $
��Left=�67   $��
120,0,0,10q�6R   $��Bottom=�6f   $��66��6�   �� 5       -   5        $ label6��5       $��× Average +��6$   $
��Left=�6:   $��
226,0,0,10q�6U   $��Bottom=�6i   $��90��6�   �� 5!      -   5        $ tb_a2��5!      $
��Left=�6%   $��23��6@   $��	186,0,0,8q�6L   $
 Wrap=�6_   $�� ��6s   $��Bottom=�6{   $��40��6�   �� 5"      -   5        $ tb_a3��5"      $
��Left=�6%   $��23��6@   $��	316,0,0,8q�6L   $
 Wrap=�6_   $��0��6s   $��Bottom=�6|   $��40��6�   {�.  6�   �� 5#      -   5        $ label7��5#      $��× Variance ��6$   $
��Left=�6:   $��
356,0,0,10q�6U   $��Bottom=�6i   $��90��6�   5$      � 5%   
   $	��105��6   $��10,185,10,0q�6   $	��Top=�61   .+�5&      �� -   5        $ label8��5&      $��Initialization method:��6$   $
��Left=�6E   $��	10,10,0,0q�6`   $	��Top=�6s   $	��170��6�   	� 5'      -   5        	 	�Name$	 	rb_normal��5'      $��Normal��6-   $��
172,17,0,0q�6>   $	��Top=�6R   
 	� 	GroupName$
 Initialization��6j    d� 	IsChecked$
 TrueY�6�   $
��Left=�6�   $��89��6�   	� 5(      -   5        $	 rb_hierarchical��5(      $��Hierarchical��63   $
��Left=�6J   $��
266,17,0,0q�6e   $	��Top=�6y   $
 Initialization��6�   	� 5)      -   5        $	 rb_RHier��5)      $��RHier��6,   $
��Left=�6<   $��
382,17,0,0q�6W   $	��Top=�6k   $
 Initialization��6�   �� 5*      -   5        $ label9��5*      $��Normalization method:��6$   $
��Left=�6D   $��	10,0,0,35q�6_   $��Bottom=�6r   $	��170��6�   	� 5+      -   5        $	 rb_none��5+      $
��None��6+   $��
172,0,0,40q�6:   $��Bottom=�6N   $
 Normalization��6i   $
 TrueY�6�   $
��Left=�6�   $��89��6�   	� 5,      -   5        $	 	rb_minMax��5,      $��Min-Max��6-   $
��Left=�6?   $��
266,0,0,40q�6Z   $��Bottom=�6n   $
 Normalization��6�   �� 5-      -   5        $ label10��5-      $��Diversification method:��6%   $
��Left=�6G   $��10,0,0,5q�6b   $��Bottom=�6t   $	��170��6�   	� 5.      -   5        $	 
rb_balance��5.      $��Balance��6.   $��
172,0,0,10q�6@   $��Bottom=�6T   $
 Diversification��6o   $
 TrueY�6�   $
��Left=�6�   $��89��6�   	� 5/      -   5        $	 
rb_reRHier��5/      $��reRHier��6.   $
��Left=�6@   $��
266,0,0,10q�6[   $��Bottom=�6o   $
 Diversification��6�   50      � 51   
   -   5         �Name$ pb_main��51      $��15��6'   $��10,295,10,0q�63   $	��Top=�6H   �� 53   
   -   5        $ 	btn_start��53      $��Start��6$   $��	0,0,10,10q�64   $��Right=�6G   $��75��6c   $��19��6�   $��Bottom=�6�   54      55      