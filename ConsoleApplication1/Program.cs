using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            var context = new Model1();

            if (context.Users.ToList() == null) Console.WriteLine("no data");

            foreach (var item in context.Users.ToList())
            {
                Console.WriteLine(item.login);
            }

            Console.ReadLine();
        }
    }
}
