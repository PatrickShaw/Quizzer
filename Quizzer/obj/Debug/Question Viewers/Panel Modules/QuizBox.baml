   M S B A M L   `   `   `  ����  Quizzer    Quizzer.QuizBox   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          d�Name$  ctrBox��5      $��64��5      $	��352��6    � DataContext 6'   ��  �� RelativeSource �� *Self+��5      �� 5	   
    �� 
TargetType" d�6   .�5
      ��  �� Property � �� �� Value �	#007474CD6+   �� 5       � �� �	#0074CD746+   �� 5       � ��$ 0q�6(   �� 5       � ��$ 0q�60   �� 5       � �� 5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FFA3AEB9��6'   	 � Offset$	 0Q�69   � 5      $ 	#FF8399A9��6'   $	 0.375Q�69   � 5      $ 	#FF718597��6'   $	 0.375Q�69   � 5      $ 	#FF617584��6'   $	 1Q�69   5      5      �� 5       � �� 5      �� 5      
 �� 
TargetType"
 d�6&   .R�5      � ��6   Z� '  !System.Windows.VisualStateManager   VisualStateGroups 5      %  System.Windows.VisualStateGroup  5   "   CommonStates��   States. 5   &      System.Windows.VisualState  
Normal��   
Storyboard. 5   *   �� .��5    .   ��  g� Duration$ 0:0:0.75D�6=    �� 
TargetName$ BackgroundGradient��6Q    �� TargetProperty$. ((Rectangle.Fill).(SolidColorBrush.Color)�6|   	 �� To$ 	#007474CDX�6�   5!   +   5"   '     5#   &   	MouseOver��. 5$   *   �� .��5%   .   W� $ 0D�6>   $ BackgroundAnimation��6K   $ Opacity�6w   	 W� To$ 1X�6�   �� 5&   .   $ 0:0:0D�6=   $ BackgroundGradient��6N   $. ((Rectangle.Fill).(SolidColorBrush.Color)�6y   $ 	#447474CDX�6�   5'   +   5(   '     5)   &   Pressed��. 5*   *   �� .��5+   .   �� $ 0D�6=   $ 
Background��6J   $1 +(Border.Background).(SolidColorBrush.Color)�6m   $ 	#006DBDD1X�6�   W� 5,   .   $ 0D�6>   $ BackgroundAnimation��6K   $ Opacity�6w   $ 1X�6�   �� 5-   .   $ 	0:0:0:0.5D�6=   $ BackgroundGradient��6R   $. ((Rectangle.Fill).(SolidColorBrush.Color)�6}   $ 	#557474CDX�6�   5.   +   5/   '     50   &   Disabled��. 51   *   �� .��52   .   W� $ 0D�6>   $ DisabledVisualElement��6K   $ Opacity�6y   $	 .55X�6�   53   +   54   '   55   #     56   "   CheckStates��. 57   &     Checked��. 58   *   �� .��59   .   �� $ 0:0:0.75D�6=   $ 
Background��6Q   $1 +(Border.Background).(SolidColorBrush.Color)�6t    �� From$ 	#669292CDX�6�   $ 	#558484CDX�6�   5;   +   5<   '     5=   &   	Unchecked��62   5>   #     5?   "   FocusStates��. 5@   &     Focused��. 5A   *   �� .��5B   .   W� $ 0D�6>   $ FocusVisualElement��6K   $ Opacity�6v   $ 1X�6�   5C   +   5D   '     5E   &   	Unfocused��62   5F   #   .+�5H      ��  ��Name$ 
Background��6%   
������ #��z��6S   #��z��6�   ._�5I   "   � #��z��6'   $��1q�6R   .+�5J   &   �� $ BackgroundAnimation��6:    J� Opacity$ 0Q�6-   
������ 6W   �� 5K   &    ��Name$ BackgroundGradient��60   ��5L   *   �� 5M   .    �� Color$ 	#007474CD��6>   5O   '   5P   #   5Q      �� 5R       ��Name$ contentPresenter��5S      #��z��5T      #��z��5U      $��Stretch=�5V      $��Stretch=�5W      #��z��5X      �� 5Y      $ DisabledVisualElement��6(    �� RadiusX$ 3��6G    �� RadiusY$ 3��6S   ���	#FFFFFFFF$ 0Q�6p    J� IsHitTestVisible .  6|   �� 5Z      $ FocusVisualElement��6(   $ 2��6D   $ 2��6P   $��1q�6\   ���	#FF5075C6$��1��6z   $ 0Q�6�    .  6�   5[      5\      5^      5_      .��5a      � -   5         �Name$	 grd��5a      $��Stretch=�6   $��Stretch=�66    6R   ��  �� ElementName$ ctrBox����6~   Z�  � ContextMenu 5c   
   �� 5d      .@�5e      m� -   5         m�Name$ mnuDoQuestion��5e      $��Do Question��6K   6`   m� 5f      $��Add Question��6   .@�5g      m� -   5        $ mnuAddMultiChoice��5g      $��Multiple Choice��6S   m� 5h      -   5        $ mnuAddWordedQuestion��5h      $��Worded Question��6V   5i      m� 5j      -   5        $ mnuEditQuestion��5j      $��Edit Question��6M   6d   m� 5k      -   5        $ mnuDeleteQuestion��5k      $��Delete Question��6O   5l        � ColumnDefinitions  5n   
   �� 5o      $
��131* �6   �� 5p      $	��81* �6   �� 5q      $	��68* �6   �� 5r      $	��73* �6   ! � RowDefinitions! 5t   
   �� 5u      $	��45* �6   �� 5v      $	��19* �6   .+�5y   
   �� -   5        " ��Name$" lblQuestion��5y      $��18(�6^   $
��Bold"�6l   $��
45,10,10,0q�6~   $
��Wrap=�6�   $	��Top=�6�   $��25��6�   $��4��6�   
���������6C   �� *
Question+6�   �� 5{   
   -	   5        $" lblAnsweringFormat��5{      $��13(�6I   $
��Bold"�6W   $
��Wrap=�6i   $��	TextBlock��6}   $	��Top=�6�   
�������$��19��6�   $��1��6�   $��
10,0,4.5,0q�6�   5}      �� 5~   
   -
   5        $" 
lblSubject��5~      $��13(�6A   $
��Bold"�6O   $
��Wrap=�6a   $��	TextBlock��6u   $	��Top=�6�   
�������$��19��6�   $��1��6�   $��5.5,0,10.5,0q�6�   $��1��6�   �� 5   
   -   5        $" 
lblCorrect��5      $��13(�6B   $
��Bold"�6P   $
��Wrap=�6b   $��	TextBlock��6v   $	��Top=�6�   
�������$��19��6�   $��1��6�   $��	11,0,10,0q�6�   $��3��6�   5�      A� 5�   
   -   5        # A�Name$# rctMark��5�      ���	#FFF4F4F5$
��Left=�6M   $��35��6h   $��5,5,0,0q�6t   
���   �$	��Top=�6�   $��35��6�   �� 5�   
   -   5        $" lblPercentChance��5�      $��13(�6G   $
��Bold"�6U   $
��Wrap=�6g   
�������$��1��6�   $��2��6�   $��	5.5,0,0,0q�6�   5�      5�      