   M S B A M L   `   `   `  ����M  IBacariq.CcmDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Bacariq.CcmDesktopBacariq.CcmDesktop  5   	   #    Bacariq.CcmDesktop.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Bacariq.CcmDesktop     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$   Passerelle Synchronisation��5
   	   $	��500��6,   $
��1000��69    � DataContext 5   	   ��    Locator �� Source# [   �� Path$
 Main���5      �� 5   
    �� ImageSource 6   �� *BcgImg+ � 	Resources 5       # Body_Content_DataGrid_Centering �� 
TargetType*  $System.Windows.Controls.DataGridCell �� Property �� Value	 �� 
TargetType  LvItemStyle
 ��Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value��%  &	       &	 �     �� 5   
   "  68   .�5      ��  � �� 5      �� 5      "	  6&   .R�5      � #��z��6   .+�5      �� $��Center=�6/   5      5      5      5      �� 5   
   " ��6$   .�5      ��  � �� 5       �� 5!      "	 ��6&   .R�5"      �� $
 border��6!   
������  5#        5$   "   CommonStates��. 5%   &     
Normal��62     5&   &   Disabled��62   5'   #     5(   "   SelectionStates��. 5)   &     
Unselected��62     5*   &   Selected��. 5+   *   �� .��5,   .   �� $ border��6K   $0 *(Panel.Background).(SolidColorBrush.Color)�5-   ?   .g�5.   2     $ 0��6F   $ 	LightBlue��6R   5/   /   50   +   51   '     52   &   SelectedUnfocused��. 53   *   �� .��54   .   �� $ border��6K   $0 *(Panel.Background).(SolidColorBrush.Color)�55   ?   .g�56   2     $ 0��6F   $ SkyBlue��6R   57   /   58   +   59   '   5:   #   ._�5<      �� 5=      5>      5@      5A      5B      .��5E      �  � ColumnDefinitions 5F   
   �� 5G      $��2* �6   �� 5H      $��20 �6   �� 5I      $��1* �6    � RowDefinitions 5K   
   �� 5L      $��* �6   .+�5P   
   � -   5         �Name$ ListeDesConsultations��5P   ,   $��0��6   $��0��6   .+�5R      �� $��Stretch=�6   $��Stretch=�63   $��
50,50,5,50q�6O    �� Padding$ 15q�6c    �� CornerRadius$ 5��5S      $��2q�6&   
�������
�������._�5T      &   System.Windows.Controls.DataGrid  -   5          Name$ 	ParentTab��5]      $��1q�5T      $��0q�6/   $��0q�6;   $��Stretch=�6F   $��Stretch=�6b   
������    AlternatingRowBackground �#eeeeee   GridLinesVisibility$ Vertical=�5W         CanUserAddRows .     CanUserDeleteRows .     HeadersVisibility$ Column=�5Z         AutoGenerateColumns .     ColumnHeaderHeight$ 50Q�5\      ��5^      �� *ClientListe+ �� Mode$ TwoWay=�  �� SelectedItem  5_      �� *ClientSelected+$ TwoWay=�!   Columns! 5`      4  .System.Windows.Controls.DataGridTemplateColumn  5b      "   CellTemplate" 5c      �� 5d   "   .R�5e   &   �� # �� Orientation$# 
Horizontal=�61   $��Center=�6J   .+�5f   *   '  !MaterialDesignThemes.Wpf.PackIcon  $��Center=�5g   B   $ J� Opacity$	$ 0.8Q�5h   h   $��0q�5i   B   $��0q�6M   $��20��5j   B   $��20��6N   %   Kind% 5f   B   �� *IcoConnected+��5h   B   �� *ColorConnected+& � ToolTip& 5k   B   �� *TimeConnected+�� 5m   *   $��Center=�5o   1   $(& "Active ou désactive la passerelle��5p   1   $��0q�5q   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5m   1   �� *DataContext.Cmd_ClientLock+' �� ElementName$' 	ParentTab����5n   1   �� .��5s   .     $	$ 0.8Q�5t   g   $��Center=�5u   F   $��0q�5v   F   $��0q�6Q   $��20��5w   F   $��20��6R   % 5s   F   �� *	IcoLock+��5t   F   �� *	ColorLock+5w   R   5x   +   �� 5z   *   $��Center=�5|   1   $%& Télécharger les logs d'erreur��5}   1   $��0q�5~   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5z   1   �� * DataContext.Cmd_ClientDownload+$' 	ParentTab����5{   1   �� .��5�   .     $% Download=�6F   
���   �$	$ 0.8Q�5�   Y   $��Center=�5�   F   $��0q�5�   F   $��0q�6Q   $��20��5�   F   $��20��6R   5�   +   �� 5�   *   $��Center=�5�   1   $+& %Relancer les transactions depuis ....��5�   1   $��0q�5�   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5�   1   �� *DataContext.Cmd_ClientReload+$' 	ParentTab����5�   1   �� .��5�   .     $% 
ClockArrow=�6F   
���   �$	$ 0.8Q�5�   Y   $��Center=�5�   F   $��0q�5�   F   $��0q�6Q   $��20��5�   F   $��20��6R   5�   +   5�   '   5�   #   5�        5�      ,	  &System.Windows.Controls.DataGridColumn( 	  Header$( 
Client Ccm��61   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   $��Right=�6f   .+�5�   *   �� $��Center=�64   ��6O   �� *	ClientCcm+�� 5�   *   $��Center=�5�   1   $��0q�5�   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5�   1   �� *DataContext.Cmd_ClientSauver+$' 	ParentTab����5�   1   �� .��5�   .     $	$ 0.8Q�5�   F   
���   �$��Center=�5�   F   $��0q�5�   F   $��0q�6Q   $��20��5�   F   $��20��6R   % 5�   F   �� *DataContext.IcoSauver+$' 	ParentTab��5�   R   5�   +   5�   '   5�   #   5�        5�      $( Dernier check��61   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5�   *   �� $��Center=�64   ��6O   �� *	LastCheck+) �� StringFormat$) dd-MM-yyyy HH:mm:ss��5�   '   5�   #   5�        5�      $( dernier import OmniPro��61   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5�   *   �� $��Center=�64   ��6O   �� *LastFromOmniPro+$) dd-MM-yyyy HH:mm:ss���� 5�   *   $��Center=�5�   1   $��0q�5�   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5�   1   �� *(&DataContext.Cmd_ClientChangeDteOmniPro+$' 	ParentTab����5�   1   �� .��5�   .     $	$ 0.8Q�5�   F   
���   �$��Center=�5�   F   $��0q�5�   F   $��0q�6Q   $��20��5�   F   $��20��6R   % 5�   F   �� *DataContext.IcoDateRange+$' 	ParentTab��5�   R   5�   +   5�   '   5�   #   5�        5�      $( Dernier export Ccm��62   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5�   *   �� $��Center=�64   ��6O   �� *LastFromCcm+$) dd-MM-yyyy HH:mm:ss���� 5�   *   $��Center=�5�   1   $��0q�5�   1   
������ $��5,0,0,0q�6^   $��0q�6o   ��5�   1   �� *$"DataContext.Cmd_ClientChangeDteCcm+$' 	ParentTab����5�   1   �� .��5�   .     $	$ 0.8Q�5�   F   
���   �$��Center=�5�   F   $��0q�5�   F   $��0q�6Q   $��20��5�   F   $��20��6R   % 5�   F   �� *DataContext.IcoDateRange+$' 	ParentTab��5�   R   5�   +   5�   '   5�   #   5�        5�      $( Ip��61   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5�   *   �� $��Center=�64   ��6O   �� *Ip+5�   '   5�   #   5�        5�      $( Id Serveur OmniPro��61   " 5�      �� 5�   "   .R�5�   &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5�   *   �� $��Center=�64   ��6O   �� *GuidSrvOmniPro+5�   '   5�   #   5�        5�      $( Message��61   " 5      �� 5  "   .R�5  &   �� $# 
Horizontal=�61   $��Center=�6J   .+�5  *   �� $��Center=�64   ��6O   �� *	Message+5  '   5  #   5     5
     5     5     �� 5  
   $��1��6   $��5��6'   $��Stretch=�61   � 5  
   $��0��6   $��2��6   .+�5     �� $��Stretch=�6   $��Stretch=�63   $��
5,50,50,50q�6O   $ 15q�6c   $ 5��5     $��2q�6&   
�������
�������._�5     �� .+�5     �� $��Configuration��6   $��14(�64   $
��Bold"�6B   �� 5     $��Dossier Ccm vers OmniPro��6   �� 5     ��6   �� *UrlCcmToOmniPro+�� 5     $��Center=�5     $��0q�5     
������ $��5,0,0,0q�6J   $��0q�6[   ��5     �� *Cmd_UrlCcmToOmniPro+��5     �� .��5        $	$ 0.8Q�5!  1   
���   �$��Center=�5#  1   $��0q�5$  1   $��0q�6<   $��20��5%  1   $��20��6=   % 5   2   �� *IcoFolderNetworkOutline+5%  =   5&     �� 5(     $��Dossier OmniPro vers Ccm��6   �� 5)     ��6   �� *UrlOmniProToCcm+�� 5+     $��Center=�5-     $��0q�5.     
������ $��5,0,0,0q�6J   $��0q�6[   ��5+     �� *Cmd_UrlOmniProToCcm+��5,     �� .��50       $	$ 0.8Q�51  1   
���   �$��Center=�53  1   $��0q�54  1   $��0q�6<   $��20��55  1   $��20��6=   % 50  2   �� *IcoFolderNetworkOutline+55  =   56     �� 58     $��Dossier download logs��6   �� 59     ��6   �� *	UrlLogs+�� 5;     $��Center=�5<     $��0q�5=     
������ $��5,0,0,0q�6J   $��0q�6[   ��5;     �� *Cmd_UrlLogs+.��5?       $	$ 0.8Q�5@  1   
���   �$��Center=�5B  1   $��0q�5C  1   $��0q�6<   $��20��5D  1   $��20��6=   % 5?  2   �� *IcoFolderNetworkOutline+5D  =   5E     �� 5G     $��Url Api��6   �� 5H     ��6   �� *UrlApi+$ TwoWay=��� 5J     $��Center=�5L     $��0q�5M     
������ $��5,0,0,0q�6J   $��0q�6[   ��5J     �� *
Cmd_Sauver+��5K     �� .��5O       $	$ 0.8Q�5P  1   
���   �$��Center=�5R  1   $��0q�5S  1   $��0q�6<   $��20��5T  1   $��20��6=   % 5O  2   �� *	IcoSauver+5T  =   5U     5X     5Y     5[     5\     5_     