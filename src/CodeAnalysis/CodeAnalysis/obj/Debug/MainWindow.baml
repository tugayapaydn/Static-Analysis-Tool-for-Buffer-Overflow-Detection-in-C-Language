   M S B A M L   `   `   `  ����G  CCodeAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CodeAnalysisCodeAnalysis  5   	       CodeAnalysis.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:CodeAnalysis     5   	     � Title$  Overflow Analyser��5   	   $	��700��6#   $	��800��60   .��5	      � $��0,0,0,-3q�6   $
��Left=�6   $	��792��68    � RowDefinitions 5
   
   �� 5      $��5* �6   �� 5      $	��35* �6   �� 5      $	��44* �6    � ColumnDefinitions 5   
   �� 5      $	��15* �6   �� 5      $	��89* �6   .+�5   
   �� -   5         ��Name$ MainFile��5      $
��File��6#    �� 	ClickMode$ Release=�62   $��0��6]   $��0��6m   $��Center=�6z   $��Center=�6�   $��20��6�   $��
34,12,40,8q�6�   $��40��6�   6�   �� 5   
   -   5         ��Name$ AnalysisList��5      $
��Left=�6)   $	��245��6D   $��1��6Q   $	��Top=�6^   $	��750��6v   $��2��6�   $��20,0,0,0q�6�    �� View 5      �� 5      .P�5      �� $
��File��6%    �� Width$	 100��63    �� DisplayMemberBinding 6?   �� *File+�� 5      $
��Line��6%   $ 70��63    6>   �� *Line+�� 5      $��Details��6%   $	 500��66    6B   �� *Detail+5      5      �� 5   
   -   5        	 ��Name$	 FilePresentation��5      $
��Left=�6,   
 �� 
IsReadOnly
 . $	��300��6Y   $	��Top=�6f   $	��750��6~   $��2��6�   $��	20,20,0,0q�6�   $��2��6�   5      5       