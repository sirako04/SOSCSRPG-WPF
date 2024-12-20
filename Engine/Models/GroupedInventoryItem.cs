﻿namespace Engine.Models
{
    public class GroupedInventoryItem : Notification
    {
        private GameItem _item;
        private int _quantity;
        public GameItem Item 
        {
            get { return _item; }
            set 
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }
        public int Quantity 
        {
            get { return _quantity; }
            set 
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public GroupedInventoryItem(GameItem item, int quantity = 1) 
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
