// See https://aka.ms/new-console-template for more information

public static void Print(string x, string y)
{
	Console.WriteLine($"{x} - {y}");
}

// Compilation error: 'Example.Print(int, string)': member 'Print' already defined with the same parameter types
public static void Print(string x, int y)
{
	Console.WriteLine($"{x} - {y}");
}
Console.WriteLine("Hello, World!");
