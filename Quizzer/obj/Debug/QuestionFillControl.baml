   M S B A M L   `   `   `  ����B  >Quizzer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:QuizzerQuizzer  5   	   !    Quizzer.QuestionFillControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:Quizzer     5   	   $	��300��5      $
��auto��6D   $
��auto��6Q   .��5      �   � ColumnDefinitions  5	   
   �� 5
      $
��auto �6   �� 5      $	��41* �6   �� 5      $	��73* �6    � RowDefinitions 5   
   �� 5      $��78 �6   �� 5      $��50 �6   �� 5      $	��96* �6   �� 5      $��35 �6   .+�5   
   �� 
�������
���   �$��2��64   $��3��6H   $��4��6\   $��
0,0,-1.5,0q�6m   6�   �� 5   
   $!��Leave empty if unlimited...��6   $��23��65   $��26.5,47,8.5,0q�6A    �� TextWrapping$
 Wrap=�6X   $	��Top=�6l   $��2��6�   $��1��6�   �� 5   
   $��32��6   $��24(�6   $
��Bold"�6,   $��10,10,8.5,0q�6>   $
 Wrap=�6S   $��Add a question title...��6g   $	��Top=�6�   $��3��6�   �� 5   
   $
��Left=�6   $��	10,48,0,0q�6/   $
��Wrap=�6B   $��Time Limit:��6V   $	��Top=�6i   $��22��6�   $��58��6�   �� 5   
   $��Remove��6   $
��Left=�6"   $��	23,5,0,10q�6=   $��3��6P   $��76��6]   $��1��6h   $��2��6x   �� 5   
   -   5         ��Name$ btnAddQuestion��5      $��Add Question��6)   $��3��6A   $��	10,5,0,10q�6k   $
��Left=�6~   $��76��6�   $��2��6�   � 5   
   
����ح�$��1��6&   $��3��63   $��0,0,0,0q�6G     5      �� 5      �� 5      .+�5       	� -   5         	�Name$ 
rdbSection��5       $��Section��6.   $
��Left=�6@   $��	10,31,0,0q�6[   $	��Top=�6n   $��1��6�   6�   	� 5!      -   5        $ rdbQuestion��5!      $��Question��62   $
��Left=�6E   $��	10,31,0,0q�6`   $	��Top=�6s   6�   �� 5"      $
��Left=�6   $��	10,10,0,0q�63   $
��Wrap=�6F   $&�� Is this a section or a question?��6Z   $	��Top=�6�   $��2��6�   $	��280��6�   5#      �� 5$   
   -   5         ��Name$ stkSubQuestions��5$      $��2��6/   $��2��6<   $��1��6P   $y�	Collapsed=�6`   �� 5%   
   -   5        $ stkQuestionElements��5%      $��2��62   $��2��6?   $��1��6S   $y�Visible=�6c   5&      5'      