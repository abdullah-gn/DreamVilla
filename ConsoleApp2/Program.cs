using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;
 
namespace ConsoleApp2
{
	
	internal static class Program
	{




		// func <int> equal = (e) => e==2;					or can be a  Predicate<T> exp
		public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> Source, Func<T,bool> exp )
		{
			foreach (var item in Source)
			{
				if (exp(item))
				{
					yield return item;
				}
			}
		}

		public class Example
		{
			// Method 1
			public void Print(int x, string y)
			{
				Console.WriteLine($"{x} - {y}");
			}

			// Compilation error: 'Example.Print(int, string)': member 'Print' already defined with the same parameter types
			public void Print(string x, int y)
			{
				Console.WriteLine($"{x} - {y}");
			}

         

        }


		private class mine
		{

			public void writeConsole()
			{
				      Console.WriteLine("hello");
			}
		}

		class BaseClass
		{
			public static void StaticMethod()
			{
				Console.WriteLine("BaseClass.StaticMethod");
			}
		}

		class DerivedClass : BaseClass
		{
			public static new void StaticMethod()
			{
				Console.WriteLine("DerivedClass.StaticMethod");
			}

			
		}






		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");


			Example e = new Example();

			e.Print(5, "s");



			//Interviews Questions

			List<int> x = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 2 };

			var t = x.MyWhere(i => i == 2);

			foreach (var item in t)
			{

				Console.WriteLine("what >>>>" + item);
			}



			mine a = new mine();
			a.writeConsole();


			try
			{

				string s = null;
				int.Parse(s);

			}

			catch (Exception ex) { Console.WriteLine("General Error Occured"); }

			//catch (NullReferenceException ex) { Console.WriteLine("Null reference exception Occured"); }

			finally { Console.WriteLine("Thank you"); }




			//	int employeeID = int.Parse(Request.QueryString["EmpID"]);

			BaseClass.StaticMethod();      // Calls BaseClass.StaticMethod
			DerivedClass.StaticMethod();




			//int[] nums = { 7, 1, 5, 3, 6, 4 };   //{ 7, 2, 3, 4, 5, 6, 6 }


			//int Current = 0;
			//int k = 3;

			//k = k % nums.Length;
			//Array.Reverse(nums);
			//Array.Reverse(nums, 0, k);
			//Array.Reverse(nums, k, nums.Length - k);



			/*			for (int i = 0; i < 54944; i++)
						{
							Current = nums[nums.Length - 1];
							for (int j = 1; j < nums.Length; j++)
							{

								nums[nums.Length - j] = nums[nums.Length - j - 1];

							}
							nums[0] = Current;

						}*/

			/*			int max = 0;

						for (int i = prices.Length - 1; i > 0; i--)
						{
							int curr = 0;
							for (int j = i - 1; j >= 0; j--)
							{
								curr = prices[i] - prices[j];
								if (curr > max)
									max = curr;
							}

						}*/
			int[] prices = { 3, 3, 5, 0, 0, 3, 1, 4 };   //{ 7, 2, 3, 4, 5, 6, 6 }


			int max = 0;
			int min = prices[0];

			for (int i = 1; i < prices.Length; i++)
			{
				if (prices[i] < min)
				{
					min = prices[i];
				}

				else if ((prices[i] - min) > max)
				{
					max = prices[i] - min;
				}
			}
			int profit = max;








			/*			int buy = prices[0];
						int sell = 0;
						int buyDay = 0;


						for (int i = 0; i < prices.Length-1; i++)
						{
							if (buy > prices[i])
							{
								buy = prices[i];
								buyDay = i;
							}
						}


							for (int i = buyDay; i < prices.Length; i++)
							{
								if (prices[i] > sell)
								{
									sell = prices[i];

								}
							}

							int profit  = sell - buy;*/


		}
		}
	}
