
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
namespace xmlpr;
public class Item
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public ShipTo ShipTo { get; set; }
    public List<Item> Items { get; set; }
}

public class ShipTo
{
    public string Name { get; set; }
    public string Street { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
}



public static class XmlParser
{
    public static Order ParseXml(string xmlString)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        XmlNode shipToNode = xmlDoc.SelectSingleNode("/shipOrder/shipTo");
        ShipTo shipTo = ParseShipTo(shipToNode);

        XmlNodeList itemNodes = xmlDoc.SelectNodes("/shipOrder/items/item");
        List<Item> items = ParseItems(itemNodes);

        return new Order
        {
            ShipTo = shipTo,
            Items = items
        };
    }

    private static ShipTo ParseShipTo(XmlNode shipToNode)
    {
        return new ShipTo
        {
            Name = shipToNode.SelectSingleNode("name").InnerText,
            Street = shipToNode.SelectSingleNode("street").InnerText,
            Address = shipToNode.SelectSingleNode("address").InnerText,
            Country = shipToNode.SelectSingleNode("country").InnerText
        };
    }

    private static List<Item> ParseItems(XmlNodeList itemNodes)
    {
        List<Item> items = new List<Item>();

        foreach (XmlNode itemNode in itemNodes)
        {
            Item item = new Item
            {
                Title = itemNode.SelectSingleNode("title").InnerText,
                Quantity = Convert.ToInt32(itemNode.SelectSingleNode("quantity").InnerText),
                Price = decimal.Parse(itemNode.SelectSingleNode("price").InnerText, CultureInfo.InvariantCulture)
            };
            items.Add(item);
        }

        return items;
    }
}
class Program
{
    static void Main()
    {
        string xmlString = @"<shipOrder>
                                <shipTo>
                                    <name>Tove Svendson</name>
                                    <street>Ragnhildvei 2</street>
                                    <address>4000 Stavanger</address>
                                    <country>Norway</country>
                                </shipTo>
                                <items>
                                    <item>
                                        <title>Empire Burlesque</title>
                                        <quantity>1</quantity>
                                        <price>10.90</price>
                                    </item>
                                    <item>
                                        <title>Hide your heart</title>
                                        <quantity>1</quantity>
                                        <price>9.90</price>
                                    </item>
                                </items>
                            </shipOrder>";

        Order order = XmlParser.ParseXml(xmlString);

        // Вывод информации о заказе
        Console.WriteLine($"ShipTo: {order.ShipTo.Name}, {order.ShipTo.Street}, {order.ShipTo.Address}, {order.ShipTo.Country}");
        Console.WriteLine("Items:");
        foreach (var item in order.Items)
        {
            Console.WriteLine($"  Title: {item.Title}, Quantity: {item.Quantity}, Price: {item.Price:C}");
        }
    }
}
