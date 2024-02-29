
namespace HelloWorld
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task firstTask = new Task(() => {
                Thread.Sleep(100);
                Console.WriteLine("Task 1");

            });
            firstTask.Start();

            Task secondTask = ConsoleAfterDelayAsync("Task 2", 150);

            ConsoleAfterDelay("Delay", 75);

            Task thirdTask = ConsoleAfterDelayAsync("Task 3", 50);

            await secondTask;
            await firstTask;
            Console.WriteLine("After the Task was created");
            await thirdTask;
        }

        static void ConsoleAfterDelay(string text, int delayTime)
        {
            Thread.Sleep(delayTime);
            Console.WriteLine(text);
        }

        static async Task ConsoleAfterDelayAsync(string text, int delayTime)
        {
            await Task.Delay(delayTime);
            Console.WriteLine(text);
        }

    }
}