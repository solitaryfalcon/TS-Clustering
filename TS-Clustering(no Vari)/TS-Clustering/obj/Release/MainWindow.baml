   M S B A M L   `   `   `  ���� H  DTS-Clustering, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:TS_ClusteringTS_Clustering      TS_Clustering.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:TS_Clustering       � Title$  TS_Clustering160513��$	��450��$	��550��$	��400��$	��500��.��� .+��� -    ��Name$ tb_log��$��10,275,10,35q� �� TextWrapping$
 Wrap=�$�� ����.  $ �� HorizontalScrollBarVisibility$
 Auto=�" �� VerticalScrollBarVisibility$
 Auto=�.�� J� 	CacheMode &   System.Windows.Media.BitmapCache  .��� $	��100��$��
10,10,10,0q�$	��Top=�.+��� -    ��Name$ label��$��Open input file:��$
��Left=�$��10,0,0,0q�$	��Top=�$	��150���� -   $ tb_inputFilePath��$��23��$��10,25,105,0q�$
 Wrap=�$�� ��$	��Top=� �� 
IsReadOnly . �� -   $ label1��$��Define output folder��$
��Left=�$��	10,0,0,27q�$��Bottom=�$	��150���� -   $ tb_outputPath��$��23��$��
10,0,105,4q�$
 Wrap=�$�� ��$��Bottom=� . �� -    ��Name$ btn_inputFile��$��Open..��$��0,25,0,0q�$	��Top=�$��Right=�$��85��$��23���� -   $ btn_outputFolder��$��	Browser..��$��0,0,0,4q�$��Right=�$��85��$��23��$��Bottom=�� $��75��$��10,110,10,0q�$	��Top=�.+��� -   $ label2��$��LBound:��$
��Left=�$��	10,10,0,0q�$	��Top=�$��70���� -	   $ label3��$��UBound:��$
��Left=�$��
120,10,0,0q�$	��Top=�$��66���� -
   $ 	tb_lbound��$
��Left=�$��23��$��	80,14,0,0q�$
 Wrap=�$��2��$	��Top=�$��40���� -   $ 	tb_ubound��$
��Left=�$��23��$��
186,14,0,0q�$
 Wrap=�$�� ��$	��Top=�$��40���� -   $ label4��$��ObjVal=��$
��Left=�$��	10,0,0,10q�$��Bottom=�$��70���� -   $ tb_a1��$
��Left=�$��23��$��80,0,0,8q�$
 Wrap=�$�� ��$��Bottom=�$��40���� -   $ label5��$��	× Sum + ��$
��Left=�$��
120,0,0,10q�$��Bottom=�$��66���� -   $ label6��$��× Average +��$
��Left=�$��
226,0,0,10q�$��Bottom=�$��90���� -   $ tb_a2��$
��Left=�$��23��$��	186,0,0,8q�$
 Wrap=�$�� ��$��Bottom=�$��40���� -   $ tb_a3��$
��Left=�$��23��$��	316,0,0,8q�$
 Wrap=�$��0��$��Bottom=�$��40���� -   $ label7��$��× Variance ��$
��Left=�$��
356,0,0,10q�$��Bottom=�$��90��� $��65��$��10,185,10,0q�$	��Top=�.+��� -   $ label8��$��Initialization method:��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��170��	� -   	 	�Name$	 	rb_normal��$��Normal��$��
172,17,0,0q�$	��Top=�
 	� 	GroupName$
 Initialization�� d� 	IsChecked$
 TrueY�$
��Left=�$��89��	� -   $	 rb_hierarchical��$��Hierarchical��$
��Left=�$��
266,17,0,0q�$	��Top=�$
 Initialization���� -   $ label9��$��Normalization method:��$
��Left=�$��10,0,0,5q�$��Bottom=�$	��170��	� -   $	 rb_none��$
��None��$��
172,0,0,10q�$��Bottom=�$
 Normalization��$
 TrueY�$
��Left=�$��89��	� -   $	 	rb_minMax��$��Min-Max��$
��Left=�$��
266,0,0,10q�$��Bottom=�$
 Normalization��� -    �Name$ pb_main��$��15��$��10,255,10,0q�$	��Top=��� -   $ 	btn_start��$��Start��$��	0,0,10,10q�$��Right=�$��75��$��19��$��Bottom=�