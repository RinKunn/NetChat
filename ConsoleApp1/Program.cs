using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var raiser = new SomeClass();
            var rec = new ReceiverClass(raiser);

            int i = 0;
            while(true)
            {
                var res = Console.ReadLine();
                if (res == "exit") break;
                raiser.DoSomething(++i);
            }
        }
    }

    public class ReceiverClass
    {
        private SomeClass _sender;

        public ReceiverClass(SomeClass sender)
        {
            _sender = sender;
            _sender.SomeEvent += someEventHandlerAsync;
            _sender.SomeEvent += someEventHandlerAsync2;
        }

        private async Task someEventHandlerAsync(SomeEventArgs args)
        {
            Console.WriteLine($"   Start async method: {args.Index}");
            await Task.Delay(1000);
            Console.WriteLine($"   End async method: {args.Index}");
        }

        private async Task someEventHandlerAsync2(SomeEventArgs args)
        {
            try
            {
                Console.WriteLine($"   Start async method: {args.Index}");
                await Task.Delay(1500);
                throw new Exception("Error on: " + args.Index);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"   Error: {ex.Message}");
            }
        }
    }

    public class SomeEventArgs
    {
        public int Index { get; }
        public SomeEventArgs(int index)
        {
            Index = index;
        }
    }
    public delegate Task SomeEventHandler(SomeEventArgs args);

    public class SomeClass
    {
        public event SomeEventHandler SomeEvent;

        public void DoSomething(int index)
        {
            OnSomeEvent(index);
        }

        private void OnSomeEvent(int ind)
        {
            SomeEvent?.Invoke(new SomeEventArgs(ind));
            //if (SomeEvent != null)
            //{
            //    var eventListeners = SomeEvent.GetInvocationList();

            //    Console.WriteLine($"#{ind}:Raising Event: {eventListeners.Length}");
            //    for (int index = 0; index < eventListeners.Length; index++)
            //    {
            //        var methodToInvoke = (SomeEventHandler)eventListeners[index];
            //        Console.WriteLine($"#{ind}:    methodToInvoke: {methodToInvoke.Method.Name}, isAsync: {(AsyncStateMachineAttribute)methodToInvoke.Method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null}");
            //        methodToInvoke.BeginInvoke(new SomeEventArgs(ind), EndAsyncEvent, "what??");

            //    }
            //    Console.WriteLine($"#{ind}:Done Raising Event");
            //}
        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (SomeEventHandler)ar.AsyncDelegate;

            Console.WriteLine($"Ending envoke res: IsCompleted = {ar.IsCompleted}");
            
            try
            {

                invokedMethod.EndInvoke(iar).Wait();
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }
    }
}
