   M S B A M L   `   `   `  ���� G  CCodeAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CodeAnalysisCodeAnalysis      CodeAnalysis.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:CodeAnalysis       � Title$  Overflow Analyser��$	��700��$	��800��.��� $��0,0,0,-3q�$
��Left=�$	��792�� � RowDefinitions �� $��5* ��� $	��35* ��� $	��44* � � ColumnDefinitions �� $	��15* ��� $	��89* �.+��� -    ��Name$ MainFile��$
��File�� �� 	ClickMode$ Release=�$��0��$��0��$��Center=�$��Center=�$��20��$��
34,12,40,8q�$��40���� -    ��Name$ AnalysisList��$
��Left=�$	��245��$��1��$	��Top=�$	��750��$��2��$��20,0,0,0q� �� View �� .P��� $
��File�� �� Width$	 100�� �� DisplayMemberBinding �� *File+�� $
��Line��$ 70�� �� *Line+�� $��Details��$	 500�� �� *Detail+�� -   	 ��Name$	 FilePresentation��$
��Left=�
 �� 
IsReadOnly
 . $	��300��$	��Top=�$	��750��$��2��$��	20,20,0,0q�$��2��