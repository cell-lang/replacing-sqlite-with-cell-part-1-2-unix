using System;
using System.IO;
using System.Collections.Generic;

using Cell.Automata;
using Cell.Typedefs;


public class App {
  public static void Main(string[] args) {
    if (args.Length != 1) {
      Console.WriteLine("Usage: run-northwind-queries <input dataset>");
      return;
    }

    // Creating the automaton
    Northwind northwind = new Northwind();

    // Loading the initial state
    using (Stream stream = new FileStream(args[0], FileMode.Open)) {
      northwind.Load(stream);
    }

    // Running all the queries in the medium articles

    foreach (var (id, firstName, lastName) in northwind.SortedEmployeesNames())
      Console.WriteLine("{0, 2}  {1,-8}  {2,-9}", id, firstName, lastName);
    Console.WriteLine("\n");

    int count = 0;
    foreach (var row in northwind.OrdersTotals())
      if (count++ < 20)
        Console.WriteLine("{0}  {1,8:####0.00}", row.orderId, row.total);
    if (count > 20)
      Console.WriteLine("...");
    Console.WriteLine("\n");

    count = 0;
    foreach (var (date, orderId, total) in northwind.ShippedOrdersTotals(new DateTime(1997, 1, 1), new DateTime(1997, 12, 31)))
      if (count++ < 20)
        Console.WriteLine("{0:yyyy-MM-dd}  {1}  {2,8:####0.00}", date, orderId, total);
    if (count > 20)
      Console.WriteLine("...");
    Console.WriteLine("\n");

    count = 0;
    foreach (var (categoryName, salesByProduct) in northwind.QuarterlyOrders()) {
      if (count < 80) {
        Console.WriteLine("{0}", categoryName);
        foreach (var (productName, salesByCustomer) in salesByProduct) {
          if (count < 80) {
            Console.WriteLine("  {0}", productName);
            foreach (var (companyName, salesByYear) in salesByCustomer) {
              if (count < 80) {
                bool firstYear = true;
                foreach (var (year, subtotals) in salesByYear) {
                  if (firstYear)
                    Console.Write("    {0,-34}", companyName);
                  else
                    Console.Write("                                      ");
                  Console.Write("  {0}  ", year);
                  Console.Write("  {0,8:####0.00}", subtotals[0]);
                  for (int i=1 ; i < subtotals.Length ; i++)
                    if (i == subtotals.Length - 1)
                      Console.Write("  {0,8:####0.00}", subtotals[i]);
                    else
                      Console.Write("  {0,8:###0.00}", subtotals[i]);
                  firstYear = false;
                  Console.WriteLine();
                  count++;
                }
              }
            }
          }
        }
      }
    }
    if (count >= 80)
      Console.WriteLine("...");
    Console.WriteLine("\n");

    foreach (var (categoryName, productsRevenues) in northwind.TopGrossingProductsByCategory(90)) {
      Console.WriteLine("{0}", categoryName);
      foreach (var (productName, revenues, percentage, cumulativePercentage) in productsRevenues)
        Console.WriteLine("  {0,-32}  {1,9:#####0.00}  {2:0.00}  {3:0.00}", productName, revenues, percentage, cumulativePercentage);
    }
    Console.WriteLine("\n");

    foreach (var (productName, lastCustomers) in northwind.LastOrdersForDiscontinuedProducts()) {
      Console.WriteLine("{0}", productName);
      foreach (var (customerName, lastOrders) in lastCustomers) {
        bool first = true;
        foreach (var (date, quantity, discount) in lastOrders) {
          if (first)
            Console.Write("  {0,-28}", customerName);
          else
            Console.Write("                              ");
          Console.WriteLine("  {0:yyyy-MM-dd}  {1,3}  {2:0.00}", date, quantity, discount);
          first = false;
        }
      }
    }
    Console.WriteLine("\n");

    SalesTree[] salesTrees = northwind.SalesTrees();
    foreach (var salesTree in salesTrees)
      salesTree.Print();
    Console.WriteLine("\n");

    string report = northwind.QuarterlyOrdersHtmlReport();
    File.WriteAllText("tmp/report.html", ReportHeader + report + "</body>\n</html>\n");
    Console.WriteLine("The quarterly orders report has been saved to tmp/report.html");
    Console.WriteLine("\n");

    string str = northwind.MyOwnQuery();
    Console.WriteLine(str + "\n");
  }

  //////////////////////////////////////////////////////////////////////////////

  const string ReportHeader =
    "<!DOCTYPE html>\n" +
    "<html>\n" +
    "<head>\n" +
    "  <title>Quarterly orders report</title>\n" +
    "  <style type='text/css'>\n" +
    "    table {\n" +
    "      font-family: 'Lucida Console', Monaco, monospace;\n" +
    "      border: 0px;\n" +
    "      border-spacing: 0;\n" +
    "    }\n" +
    "\n" +
    "    tr {\n" +
    "      border: 0px;\n" +
    "    }\n" +
    "\n" +
    "    td {\n" +
    "      font-style: italic;\n" +
    "      /*border: 1px solid #ccc;*/\n" +
    "      /*border: 1px;*/\n" +
    "      text-align: left;\n" +
    "      padding: 4px;\n" +
    "      white-space: nowrap;\n" +
    "    }\n" +
    "\n" +
    "    tr:nth-child(even) .year {\n" +
    "      background-color: #efefef;\n" +
    "    }\n" +
    "\n" +
    "    tr:nth-child(odd) .year {\n" +
    "      background-color: #e0eaec;\n" +
    "    }\n" +
    "\n" +
    "    tr:nth-child(even) .number {\n" +
    "      background-color: #efefef;\n" +
    "    }\n" +
    "\n" +
    "    tr:nth-child(odd) .number {\n" +
    "      background-color: #e0eaec;\n" +
    "    }\n" +
    "\n" +
    "    .category {\n" +
    "      background-color: #cfe2f3;\n" +
    "    }\n" +
    "\n" +
    "    .product {\n" +
    "      background-color: #c9daf8;\n" +
    "    }\n" +
    "\n" +
    "    .customer {\n" +
    "      background-color: #e5e0f1;\n" +
    "      padding-right: 2em;\n" +
    "    }\n" +
    "\n" +
    "    .indent {\n" +
    "      padding: 1em 2em;\n" +
    "    }\n" +
    "\n" +
    "    .year {\n" +
    "      padding-left: 1em;\n" +
    "      width: 4em;\n" +
    "    }\n" +
    "\n" +
    "    .number {\n" +
    "      text-align: right;\n" +
    "      width: 4em;\n" +
    "    }\n" +
    "  </style>\n" +
    "</head>\n" +
    "<body>";
}

////////////////////////////////////////////////////////////////////////////////

namespace Cell.Typedefs {
  public partial class SalesTree {
    public void Print() {
      Print(0);
    }

    private void Print(int indentation) {
      String nameField = new String(' ', indentation) + firstName + " " + lastName;
      Console.Write("{0,-20}  {1,10:0.00}", nameField, sales);
      if (totalSales != null)
        Console.Write("  {0,10:0.00}", totalSales);
      Console.WriteLine();
      foreach (var st in subordinates)
        st.Print(indentation + 2);
    }
  }
}
