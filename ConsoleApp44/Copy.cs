using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp44
{
    public static class Copy
    {
        public static void CopyOperation(string from, string to, string text)
        {
            bool isCopying = false;
            bool isPaused = false;
            bool isAborted = false;

            int select = 0;
            bool isRun = true;
            List<string> operations = new List<string>();

            string intro = @"/////////////////////////////////////
//
//    THREAD MANAGEMENT
//
/////////////////////////////////////";

            while (isRun)
            {
                operations.Clear();
                if (!isCopying)
                {
                    operations.Add("Start");
                    operations.Add("\u001b[31mBack\u001b[0m");
                }
                else
                {
                    operations.Add(isPaused ? "Resume" : "Pause");
                    operations.Add("\u001b[31mAbort\u001b[0m");
                    operations.Add("\u001b[31mBack\u001b[0m");
                }


                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(intro);
                Console.ResetColor();
                Console.WriteLine();

                for (int i = 0; i < operations.Count; i++)
                {
                    if (select == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($">>  {operations[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"    {operations[i]}");
                    }
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    isAborted = true;
                    isRun = false;
                    break;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    select--;
                    if (select < 0)
                    {
                        select = operations.Count - 1;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    select++;
                    if (select >= operations.Count)
                    {
                        select = 0;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (!isCopying)
                    {
                        if (select == 0) 
                        {
                            isCopying = true;
                            isPaused = false;
                            isAborted = false;

                            Thread thread = new Thread(() =>
                            {
                                try
                                {
                                    if (!File.Exists(from))
                                    {
                                        File.WriteAllText(from, text);
                                    }

                                    using FileStream fromFile = new FileStream(from, FileMode.Open);
                                    using FileStream toFile = new FileStream(to, FileMode.Create);

                                    byte[] buffer = new byte[1024];
                                    long totalbyte = fromFile.Length;
                                    long copy = 0;
                                    int read;

                                    while ((read = fromFile.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        if (isAborted)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\u001b[31mAborted.\u001b[0m");
                                            return;
                                        }
                                        while (isPaused)
                                        {
                                            if (isAborted) return;
                                            Thread.Sleep(100);
                                        }

                                        toFile.Write(buffer, 0, read);

                                        copy += read;

                                        double percent = (double)copy / totalbyte * 100;

                                        Console.Clear();
                                        Console.Write($"\u001b[32mLoading: \u001b[0m{(int)percent}%");

                                        Thread.Sleep(100);
                                    }

                                    Console.Clear();
                                    Console.WriteLine("\u001b[32mCopied successfully!\u001b[0m");
                                    isCopying = false;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error occurred: {ex.Message}");
                                }
                                
                            });

                            thread.Start();
                        }
                        else if (select == 1) 
                        {
                            isRun = false;
                        }
                    }
                    else
                    {
                        if (select == 0) 
                        {
                            isPaused = !isPaused;
                        }
                        else if (select == 1)
                        {
                            isAborted = true;
                            isCopying = false;
                        }
                        else if (select == 2)
                        {
                            isAborted = true;
                            isRun = false;
                        }
                    }
                }
            }
        }

    }
}
