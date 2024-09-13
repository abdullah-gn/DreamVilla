using System.Runtime.InteropServices;

namespace ConsoleApp4
{
    internal class Program
    {
        public class size {

            float height = 0;
            float width = 0;

            public size(float _width, float _height)
            {
                  
                    width = _width;
                    height = _height;
            }


            public void Print(size s)
            {

                Console.WriteLine($"Width :{s.width} , Height:{s.height}");

            }



            public void IncreaseHeight(size s)
            {
                s.height += 10;
            }



        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");



            size s1 = new size(50, 100);
            size s2 = s1;
            s2.IncreaseHeight(s2);
            s2.Print(s2);

            size s3 = s1;
            s3.IncreaseHeight(s3);
            s3.Print(s3);

            s1.Print(s1);
            s2.Print(s2);
            s3.Print(s3);   













            Console.ReadLine();
        }
    }
}