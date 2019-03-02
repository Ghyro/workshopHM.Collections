using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    private readonly Dictionary<int, OrderItem> _orderItems = new Dictionary<int, OrderItem>();

    public IDictionary<int, OrderItem> OrderItemsDictionary
    {
        get
        {
            return _orderItems;
        }
    }

    public void AddRange(IEnumerable<OrderItem> orderItems)
    {
        foreach (var item in orderItems)
        {
            _orderItems.Add(item.PartNumber, item);
        }
    }

    public OrderItem FindByPartNumber(int partNumber)
    {
        OrderItem resultItem;
        if (OrderItemsDictionary.TryGetValue(partNumber, out resultItem))
        {
            return resultItem;
        }

        return null;
    }

    public IList<OrderItem> FindByDescription(string description)
    {
        return OrderItemsDictionary.Values.Where(i => i.Description == description).ToArray();
    }
}

public class Program
{
    public static void Main()
    {
        var order = new Order();

        order.AddRange(new OrderItem[]
        {
            new OrderItem(110072674, "Widget", 400, 45.17),
            new OrderItem(110072675, "Sprocket", 27, 5.3),
            new OrderItem(101030411, "Motor", 10, 237.5),
            new OrderItem(110072684, "Gear", 175, 5.17)
        });

        Display("Order #1", order);

        if (order.FindByPartNumber(111033401) == null)
        {
            Console.WriteLine("Order #1 doesn't have #111033401 item.\n");
        }

        order.AddRange(new OrderItem[]
        {
            new OrderItem(111033401, "Nut", 10, .5),
            new OrderItem(127700026, "Crank", 27, 5.98)
        });

        Display("Order #2", order);

        var list = order.FindByDescription("Crank");
        if (list.Count > 0)
        {
            double itemPrice = list[0].Quantity * list[0].UnitPrice;
            Console.WriteLine("Order #2 has \"Crunk\" item - price is {0:###,###.00}$.", itemPrice);
        }
    }

    private static void Display(string title, Order order)
    {
        Console.WriteLine(title);
        foreach (var item in order.OrderItemsDictionary)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();
    }
}