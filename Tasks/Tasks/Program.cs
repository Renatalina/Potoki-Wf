using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            //Action action = Hello;//это делегат, вспомни
            //Task task = new Task(Hello);
            //task.Start();
            //task.Wait();

            //здесь все потоки иду параллельно 

            //вот это пулл потока, фоновый поток
            // Task task1 = Task.Run(()=>Console.WriteLine("Ok!"));
            //Task task2 = Task.Factory.StartNew(() => Console.WriteLine("Bye!"));
            //task1.Wait();
            //task2.Wait();


            //List<Task> tasks = new List<Task>
            //{
            //    new Task(Hello),
            //    new Task(()=>Console.WriteLine("Ok!")),
            //    new Task(() => Console.WriteLine("Bye!"))
            //};

            //foreach(Task item in tasks)
            //{
            //    item.Start();
            //}
            ////WaitAll принимает массив
            //Task.WaitAll(tasks.ToArray());


            /*//Task<double> task3 = new Task<double>(Summ(6.7, 4.6));//второй вариант запуска потока
            Task<double> task3 = new Task<double>(()=>Summ(5.7,6.4));
            task3.Start();
            task3.Wait();

            Console.WriteLine($"Result: {task3.Result}");*/


            //вот так работают потоки, когда сначала отработывает 4, а только потом в работу включается 5
            //Task task4 = new Task(Hello);
            //Task task5 = task4.ContinueWith(HelloTask);
            //task4.Start();
            //task5.Wait();

            //string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //string[] files = null;
            //string[] dirs = null;
            //List<Task> tasks = new List<Task>
            //{
            //    Task.Factory.StartNew(()=>files=Directory.GetFiles(directory)),
            //    Task.Factory.StartNew(()=>dirs=Directory.GetDirectories(directory))
            //};

            //Task.Factory.ContinueWhenAll(tasks.ToArray(), t =>
            //{
            //    Console.WriteLine($"Directory: {directory}");
            //    Console.WriteLine("Directories:");

            //    foreach (string item in dirs)
            //    {
            //        Console.WriteLine(item);
            //    }

            //    Console.WriteLine("Files:");
            //    foreach (string item in files)
            //    {
            //        Console.WriteLine(item);
            //    }
            //});

            //Task task6 = Method();
            //task6.Wait();

            //тоже потоки
            //Parallel.Invoke(Hello, () => Console.WriteLine("Bye!"), () => Summ(5.7, 6.4));

            //Parallel.For(3, 9, Factorial);

            //Parallel.ForEach(new int[] { 2, 5, 7, 3, 1, 4, 4, 7 }, Factorial );

            //Parallel.Invoke(
            //    () => Factorial(2),
            //    () => Factorial(5),
            //    () => Factorial(7),
            //    () => Factorial(9),
            //    () => Factorial(22),
            //    () => Factorial(5)
            //    );

            //вот это линкью, синтаксис запроса для линкью
            int[] arrInt = { 45, 87, 23, 12, 87, 64, 12 };
            //var query = from n in arrInt.AsParallel()    //использовать коллекцию как паралельный запрос
            //            where n % 2 != 0
            //            orderby n descending
            //            select n;
            //вот это точно такой же запрос как написно выше!!
            var query = arrInt.AsParallel().Where(i => i % 2 != 0).OrderByDescending(i => i).Select(i => i);

            foreach(var item in query)
            {
                Console.WriteLine(item);
            }



            Console.WriteLine("Main end");
            
            Console.ReadKey();
        }

        private static void Factorial(int obj)
        {
            int result = 1;
            for (int i=1;i<=obj; i++)
            {
                result *= i;
            }
            Console.WriteLine($"TheradID : { Thread.CurrentThread.ManagedThreadId} Result: {obj}!={result}");
        }

        //это будет асинхронный метод
        private static async Task Method()
        {
            int n = 5;
            int result = await TwoDegree(n);
            Console.WriteLine( $"2^{n}={result}");
        }

        private static Task<int> TwoDegree(int n)
        {
            int result = 2;
            return Task<int>.Run(() =>
            {
                for (int i = 0; i < n; i++)
                {
                    result *= 2;
                }
                return result;
            });
             
        }

        private static void HelloTask(Task obj)
        {
            Console.WriteLine($"HelloTask  {obj.Id}");
        }

        //private static Func<double> Summ(double v1, double v2)
        //{
        //    return () => v1 + v2;
        //}

        private static double Summ(double v1, double v2)
       {
           return v1 + v2;
       }

        private static void Hello()
        {
            Console.WriteLine("Hello");
            Console.WriteLine($"Thread: {Task.CurrentId}");
            Thread.Sleep(2000);
            Console.WriteLine("The end");

        }
    }
}
