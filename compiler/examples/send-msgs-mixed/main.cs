using System;
using System.IO;
using System.Collections.Generic;

using Cell.Automata;


class App {
  public static void Main(string[] args) {
    if (args.Length != 3) {
      Console.WriteLine("Usage: send-msgs <initial state> <message list> <final state>");
      return;
    }

    // Reading the message list, with one message per line
    // Do not split a single message over multiple lines
    string[] msgList = File.ReadAllLines(args[1]);

    // Creating the automaton
    Logins logins = new Logins();

    // Loading the initial state
    using (Stream stream = new FileStream(args[0], FileMode.Open)) {
      logins.Load(stream);
    }

    // Printing the initial state
    logins.Save(Console.OpenStandardOutput());
    Console.WriteLine();

    // Sending the messages and printing the results
    foreach (string line in msgList) {
      string msg = line.Trim();
      // Skipping lines that are empty or commented out
      if (msg != "" && !msg.StartsWith("//")) {
        try {
          logins.Execute(msg);
          Console.WriteLine("Success: {0}\n", msg);
        }
        catch (Exception) {
          Console.WriteLine("Failure: {0}\n", msg);
        }
        // Printing the state after the update
        logins.Save(Console.OpenStandardOutput());
        Console.WriteLine();
      }
    }

    // Saving the final state to the indicated file
    using (Stream stream = new FileStream(args[2], FileMode.Create)) {
      logins.Save(stream);
      stream.WriteByte((byte) '\n');
    }
  }
}
