   M S B A M L   `   `   `  ����F  BMid_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Mid_Project.Views.GroupMid_Project.Views.Group  5      *    $Mid_Project.Views.Group.ManageGroups   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:Mid_Project.Views.Group     5        � 	Resources  5       �� 
TargetType �� Property �� Value��%S  '��       '�� (    �� 5   
   " ��6   .�5      ��  � ��$	 175��6&   �� 5       � ��$ 75��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ Arial)�6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 20(�6)   �� 5       � ��
 ��� �6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 auto��6'   �� 5       � ��$ Arial)�6+   �� 5       � ��$ 20(�6)   �� 5       � ��$ WrapWithOverflow=�6-   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   5      5      .��5       � $��Stretch=�6   $��Stretch=�6'    � RowDefinitions 5!   
   �� 5"      $
��.15* �6   �� 5#      $
��.60* �6   �� 5$      $
��.25* �6   .+�5'   
   �� $5��/Select Any Row to perform the button operations��6   &   System.Windows.Controls.DataGrid  5(   
   -   5          Name$ dgTableData��5(       $��1��6   $��10q�65   $��3q�6A      
IsReadOnly . 6U   �� 5+   
    �� Orientation$ 
Horizontal=�6   $��3��6.   $��Center=�6;   $��Center=�6V   .+�5,      �� -   5         ��Name$ btnUpdatePrj��5,      $��Update Project��6+   �� 5-      -   5        $ btnAddEvaluation��5-      $��Add Evaluation��6/   ���#20B1AA6H   �� 5.      -   5        $ btnUpdateStu��5.      $��Update Students��6+   5/      50      51      