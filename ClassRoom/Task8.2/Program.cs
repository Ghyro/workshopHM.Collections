using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task8._2
{
    public class OrderItem
    {
        public readonly int PartNumber;
        public readonly string Description;
        public readonly double UnitPrice;
        private int _quantity = 0;

        public OrderItem(int partNumber, string description, int quantity, double unitPrice)
        {
            this.PartNumber = partNumber;
            this.Description = description;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Quantity cannot be negative.");
                }

                _quantity = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0,9} {1,6} {2,-12} at {3,8:#,###.00} = {4,10:###,###.00}",
                PartNumber, _quantity, Description, UnitPrice, UnitPrice * _quantity);
        }
    }

    public class Order
    {
        private readonly OrderItemCollection _orderItems;

        public OrderItemReadOnlyCollection OrderItems
        {
            get
            {
                return new OrderItemReadOnlyCollection(_orderItems);
            }
        }

        public Order()
        {
            _orderItems = new OrderItemCollection();
            _orderItems.OrderItemAdded += AddItem;
            _orderItems.OrderItemRemoved += RemoveItem;
        }

        public void AddRange(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems is null)
            {
                throw new ArgumentNullException(nameof(orderItems));
            }

            AddItems(orderItems);
        }

        public void AddItems(IEnumerable<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                _orderItems.AddItem(item);
            }
        }

        public void RemoveItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new ArgumentNullException(nameof(orderItem));
            }

            _orderItems.RemoveItem(orderItem);
        }

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new ArgumentNullException(nameof(orderItem));
            }

            _orderItems.AddItem(orderItem);
        }

        private void AddItem(object sender, ItemCollectionEventArgs e)
        {
            Console.WriteLine($"The item {e.OrderItem.PartNumber} has been added.");
        }

        private void RemoveItem(object sender, ItemCollectionEventArgs e)
        {
            Console.WriteLine($"The item {e.OrderItem.PartNumber} has been removed.");
        }
    }

    public class OrderItemCollection : Collection<OrderItem>
    {
        public event EventHandler<ItemCollectionEventArgs> OrderItemAdded = delegate { };

        public event EventHandler<ItemCollectionEventArgs> OrderItemRemoved = delegate { };

        public void AddItem(OrderItem orderItem)
        {
            Add(orderItem);

            OnItemAdded(new ItemCollectionEventArgs(orderItem));
        }

        public void RemoveItem(OrderItem orderItem)
        {
            Remove(orderItem);

            OnItemRemoved(new ItemCollectionEventArgs(orderItem));
        }

        protected virtual void OnItemAdded(ItemCollectionEventArgs eventArgs)
        {
            OrderItemAdded.Invoke(this, eventArgs);
        }

        protected virtual void OnItemRemoved(ItemCollectionEventArgs eventArgs)
        {
            OrderItemRemoved.Invoke(this, eventArgs);
        }
    }

    public class ItemCollectionEventArgs : EventArgs
    {
        public OrderItem OrderItem { get; }

        public ItemCollectionEventArgs(OrderItem orderItem)
        {
            OrderItem = orderItem;
        }
    }

    public class OrderItemReadOnlyCollection : ReadOnlyCollection<OrderItem>
    {
        public OrderItemReadOnlyCollection(IList<OrderItem> orderItems) : base(orderItems)
        {
            // empty
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var order = new Order();
            var orderItems = new OrderItemCollection
            {
                new OrderItem(110072674, "Widget", 400, 45.17),
                new OrderItem(110072675, "Sprocket", 27, 5.3),
                new OrderItem(101030411, "Motor", 10, 237.5),
                new OrderItem(110072684, "Gear", 175, 5.17)
            };

            order.AddRange(orderItems);

            var item = new OrderItem(110072674, "Widget", 400, 45.17);

            order.AddItem(item);

            order.RemoveItem(item);

            Console.ReadLine();
        }
    }
}
