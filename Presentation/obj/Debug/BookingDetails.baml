   M S B A M L   `   `   `  ����  Presentation!    Presentation.BookingDetails   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  BookingDetails��5   	   $	��681��6    $	��658��6-   .��5      � -   5         �Name$ lblBookingRef��5      ���#95B7F2$��0,0,-8,0q�67   .+�5   
   �� $��	Check In:��6   $
��Left=�6$   $��	26,16,0,0q�6?   $	��Top=�6R   $��Arial Rounded MT Bold)�6j   �� 5   
   $��
Check Out:��6   $
��Left=�6%   $��
267,16,0,0q�6@   $	��Top=�6T   $��Arial Rounded MT Bold)�6l   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ datePickArrival��5      $
��Left=�6.   $��
111,16,0,0q�6I   $	��Top=�6]     5	   
   -   5        $ datePickDepart��5	      $
��Left=�6-   $��
391,16,0,0q�6H   $	��Top=�6\   �� 5
   
   -   5         ��Name$ btnCheckAvail��5
      $��Check Availablity��6(   $
��Left=�6D   $��
516,18,0,0q�6_   $	��Top=�6s   $	��117��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   $��Booking Reference Number:��6   $
��Left=�64   $��
10,111,0,0q�6O   $	��Top=�6c   $��Arial Rounded MT Bold)�6{   �� 5   
   -   5         ��Name$ lblBookingRef1��5      $	��Ref��6(   $
��Left=�66   $��190,111,0,0q�6Q   $	��Top=�6f    J� RenderTransformOrigin$ 0.237,-0.385&�6~   $��Arial Rounded MT Bold)�6�   �� 5   
   $��Customer's Name:��6   $
��Left=�6+   $��
10,152,0,0q�6F   $	��Top=�6Z   $ 0.079,-0.038&�6r   $��Arial Rounded MT Bold)�6�   �� 5   
   $��Customer's Address:��6   $
��Left=�6.   $��
10,194,0,0q�6I   $	��Top=�6]   $��Arial Rounded MT Bold)�6u   �� 5   
   -   5        $ lblCustName��5      $
��Name��6%   $
��Left=�64   $��190,150,0,0q�6O   $	��Top=�6d   $ 1.026,0.385&�6|   $��Arial Rounded MT Bold)�6�   �� 5   
   -   5        $ lblCustAddress��5      $��Address��6(   $
��Left=�6:   $��190,192,0,0q�6U   $	��Top=�6j   $ 0.632,-0.154&�6�   $��Arial Rounded MT Bold)�6�   �� 5   
   -   5         ��Name$ listBoxGuests��5      $
��Left=�6)   $	��138��6D   $��
10,253,0,0q�6Q   $	��Top=�6e   $	��239��6}   
�������6�   �� 5   
   -	   5        $ btnAddGuest��5      $��Add New Guest��6&   $
��Left=�6>   $��
10,396,0,0q�6Y   $	��Top=�6m   $	��239��6�   $ -0.427,0.273&�6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6  �� 5   
   -
   5        $ 
lblNewName��5      $��Name:��6$   $
��Left=�64   $��304,253,0,0q�6O   $	��Top=�6d   $��69��6|   $��Arial Rounded MT Bold)�6�   �� 5   
   -   5        $ 
lblNewPass��5      $��Passport Number:��6$   $
��Left=�6?   $��304,309,0,0q�6Z   $	��Top=�6o   $ -0.105,0.538&�6�   $��Arial Rounded MT Bold)�6�   �� 5   
   -   5        $ 	lblNewAge��5      $
��Age:��6#   $
��Left=�62   $��304,365,0,0q�6M   $	��Top=�6b   $��Arial Rounded MT Bold)�6z   �� 5   
   -   5        $ btnDelGuest��5      $��Delete��6&   $
��Left=�67   $��304,255,0,0q�6R   $	��Top=�6g   $	��116��6   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   -   5        $ btnAmendGuest��5      $
��Edit��6(   $
��Left=�67   $��304,309,0,0q�6R   $	��Top=�6g   $	��116��6   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   -   5        $ btnAmendGuestSave��5      $��Save Changes��6,   $
��Left=�6C   $��304,369,0,0q�6^   $	��Top=�6s   $	��116��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   -   5         ��Name$ txtBoxEditName��5      $
��Left=�6*   $��23��6E   $��441,252,0,0q�6Q    �� TextWrapping$
 Wrap=�6f   $	��Top=�6z   $	��192��6�   �� 5   
   -   5        $ txtBoxEditPassp��5      $
��Left=�6+   $��23��6F   $��441,306,0,0q�6R   $
 Wrap=�6g   $	��Top=�6{   $	��192��6�   �� 5   
   -   5        $ txtBoxEditAge��5      $
��Left=�6)   $��23��6D   $��441,365,0,0q�6P   $
 Wrap=�6e   $	��Top=�6y   $	��192��6�   �� 5   
   -   5        $ btnCnlNewGuest��5      $��Cancel��6)   $
��Left=�6:   $��441,396,0,0q�6U   $	��Top=�6j   $��90��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   -   5        $ btnNewGuestConf��5      $	��Add��6*   $
��Left=�68   $��543,396,0,0q�6S   $	��Top=�6h   $��90��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5   
   -   5        	 ��Name$	 checkBoxEvMeal��5      $��Evening Meal��6+   $
��Left=�6B   $��165,441,0,0q�6]   $	��Top=�6r   $��Arial Rounded MT Bold)�6�   �� 5   
   -   5        $	 checkBoxBreakfast��5      $��	Breakfast��6.   $
��Left=�6B   $��
17,440,0,0q�6]   $	��Top=�6q   $��Arial Rounded MT Bold)�6�   �� 5    
   -   5        $	 checkBoxCarHire��5       $��Car Hire��6,   $
��Left=�6?   $��326,441,0,0q�6Z   $	��Top=�6o   $��Arial Rounded MT Bold)�6�   6�     5!   
   -   5        $ datePickHireStart��5!      $
��Left=�60   $��513,435,0,0q�6K   $	��Top=�6`   $	��120��6x     5"   
   -   5        $ datePickHireEnd��5"      $
��Left=�6.   $��513,475,0,0q�6I   $	��Top=�6^   $	��120��6v   �� 5#   
   -   5        $ lblHireFrom��5#      $��From:��6%   $
��Left=�65   $��441,435,0,0q�6P   $	��Top=�6e   $��Arial Rounded MT Bold)�6}   �� 5$   
   -   5        $ 	lblHireTo��5$      $	��To:��6#   $
��Left=�61   $��441,473,0,0q�6L   $	��Top=�6a   $��Arial Rounded MT Bold)�6y   �� 5%   
   -   5        $ 	lblDriver��5%      $��Driver:��6#   $
��Left=�65   $��441,521,0,0q�6P   $	��Top=�6e   $��Arial Rounded MT Bold)�6}   �� 5&   
   -   5        
 ��Name$
 comBoxDriver��5&      $
��Left=�6)   $��513,521,0,0q�6D   $	��Top=�6Y   $	��120��6q   
�������6}   �� 5'   
   -   5        $
 comBoxChaletId��5'      $
��Left=�6+   $��
83,521,0,0q�6F   $	��Top=�6Z   $��59��6r   
�������6}   �� 5(   
   -   5        $ lblChaletId��5(      $��
Chalet Id:��6%   $
��Left=�6:   $��
10,521,0,0q�6U   $	��Top=�6i   $��Arial Rounded MT Bold)�6�   �� 5)   
   -    5        $ 	btnCancel��5)      $��Delete Booking��6$   $
��Left=�6=   $��
10,597,0,0q�6X   $	��Top=�6l   $	��150��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5*   
   -!   5        $ 
btnInvoice��5*      $��Show Invoice��6%   $
��Left=�6<   $��165,597,0,0q�6W   $	��Top=�6l   $	��150��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5+   
   -"   5        $ btnEdit��5+      $��Edit Booking��6"   $
��Left=�69   $��323,597,0,0q�6T   $	��Top=�6i   $	��150��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   �� 5,   
   -#   5        $ 	btnSaveCh��5,      $��Save Changes��6$   $
��Left=�6;   $��478,597,0,0q�6V   $	��Top=�6k   $	��150��6�   $��Arial Rounded MT Bold)�6�   
����������#407FEE6�   5.      50      