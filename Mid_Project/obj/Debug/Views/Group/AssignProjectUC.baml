   M S B A M L   `   `   `  ����F  BMid_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Mid_Project.Views.GroupMid_Project.Views.Group  5      -    'Mid_Project.Views.Group.AssignProjectUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:Mid_Project.Views.Group     5        � 	Resources  5
       �� 
TargetType �� Property �� Value��%�  '��       '�� {    �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 40��6'   �� 5       � ��$ 20q�6'   �� 5       � ��$ Center=�69   �� 5       � ��$ Center=�6;   �� 5       � ��
 �   �6+   �� 5       � �� �#20B1AA6+   �� 5       � ��$ 20(�6)   �� 5       � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 25(�6)   �� 5       � ��$ Book Antiqua)�6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$	 200��6&   �� 5       � ��$ 75,10,10,10q�6'   �� 5       � ��$ 55��6'   5       5!      .��5#      �  � RowDefinitions 5$   
   �� 5%      $	��.2* �6   �� 5&      $
��.55* �6   �� 5'      $
��.35* �6   .+�5*   
   �� -   5         ��Name$ lblTable��5*      $��	Projects:��6"   $��0��66   &   System.Windows.Controls.DataGrid  5+   
   -   5          Name$ 
dgProjects��5+      $��10q�6'   $��3q�63   $��1��6G      
IsReadOnly . 6T   �� 5-   
   $��2��6    �� Orientation$ Vertical=�6"   .+�5/      �� $ 
Horizontal=�6   $��Center=�62   .+�50      �� -   5        $ 
lblcurrent��50      $��Current:��6,   �� 51      -   5        $ lbldata��51      $	��350��6)   $��0q�65   
�������6@   �� 52      -   5        	 ��Name$	 	btnDelete��52      $��Delete��6,   
���  ��6=   53      �� 55      $ 
Horizontal=�6   $��Center=�62   .+�56      �� -   5        $ lbldate��56      $��Assignment Date:��6)   (  "System.Windows.Controls.DatePicker  57      -   5        
  Name$
 txtdate��57      $	��250��6.   $��45��6:    � 
InputScope$
 Date��6F   $��Arial)�6X   $��28(�58      $��Center=�6#   �� 59      -   5        $	 btnEnter��59      $��Update��6+   
����� �6<   5:      5;      5=      5>      