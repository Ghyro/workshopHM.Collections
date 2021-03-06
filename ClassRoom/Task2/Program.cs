﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();

	public List<OrderItem> OrderItems
	{
		get
		{
			return _orderItems;
		}
	}
}

public class Program
{
	public static void Main()
	{
        var order = new Order();

        order.OrderItems.Add(new OrderItem(110072674, "Widget", 400, 45.17));
        order.OrderItems.Add(new OrderItem(110072675, "Sprocket", 27, 5.3));
        order.OrderItems.Add(new OrderItem(101030411, "Motor", 10, 237.5));
        order.OrderItems.Add(new OrderItem(110072684, "Gear", 175, 5.17));

		Display("Order #1", order);

        var firstItem = order.OrderItems.Find(x => x.PartNumber == 111033401);

		if (firstItem is null)
		{
			Console.WriteLine("Order #1 doesn't have #111033401 item.\n");
		}

        order.OrderItems.Add(new OrderItem(111033401, "Nut", 10, .5));
        order.OrderItems.Add(new OrderItem(127700026, "Crank", 27, 5.98));

		Display("Order #2", order);

        var secondItem = order.OrderItems.Find(x => x.PartNumber == 127700026);

		if (secondItem is null)
		{
            throw new ArgumentNullException(nameof(secondItem));
		}
        else
        {
            Console.WriteLine("Order #2 has #127700026 item.");
        }
	}
	
    private static void Display(string title, Order order)
    {
        Console.WriteLine(title);
        foreach (OrderItem item in order.OrderItems)
        {
            Console.WriteLine(item);
        }
		Console.WriteLine();
    }
}