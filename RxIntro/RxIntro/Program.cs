using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace RxIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            ObservableCreate();

            //ObservableRange();

            //ObservableInterval();

            //ObservableTimer();

            //ObservableTask();

            //ObservableFromAsync();

            //ObservableDistinct();
            //ObservableDistinctUntilChanged();
        }

        private static void ObservableCreate()
        {
            var ob = Observable.Create<string>(
                            observer =>
                            {
                                var timer = new Timer();
                                timer.Interval = 1000;
                                timer.Elapsed += (s, e) => observer.OnNext($"observed {e.SignalTime}");
                                timer.Start();
                                return timer;
                            });

            var subscription = ob.Subscribe(Console.WriteLine);
            Console.ReadLine();
            subscription.Dispose();
        }

        static void ObservableRange()
        {
            var observable = Observable.Range(1, 100);

            var subscription = observable.Subscribe(value =>
            {
                Console.WriteLine($"Observed value: {value}");
            });

            Console.ReadLine();
            subscription.Dispose();
        }

        static void ObservableInterval()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1));

            var subscription = observable.Subscribe(_ =>
            {
                Console.WriteLine(DateTime.Now);
            });

            Console.ReadLine();
            subscription.Dispose();
        }

        static void ObservableTimer()
        {
            Console.WriteLine(DateTime.Now);
            var observable = Observable.Timer(TimeSpan.FromSeconds(5));
            
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var subscription = observable
            .Subscribe(_ =>
            {
                Console.WriteLine(DateTime.Now);
            });

            Console.ReadLine();
            subscription.Dispose();
        }

        static void ObservableTask()
        {
            var t = Task.Factory.StartNew(() => "Test");
            var source = t.ToObservable();
            source.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("completed"));

            Console.ReadLine();
        }

        static void ObservableFromAsync()
        {
            var wb = WebRequest.CreateHttp("https://www.google.com");
            var observable = wb.GetResponseAsync().ToObservable();
            var subs = observable.Subscribe(response =>
            {
                Console.WriteLine(response.ContentType);
            });

            Console.ReadLine();
            subs.Dispose();

        }

        static void ObservableDistinct()
        {
            var subject = new Subject<int>();
            var distinct = subject.Distinct();
            subject.Subscribe(
                i => Console.WriteLine("{0}", i),
                () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(
                i => Console.WriteLine("distinct.OnNext({0})", i),
                () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
            Console.ReadLine();
        }

        static void ObservableDistinctUntilChanged()
        {
            var subject = new Subject<int>();
            var distinct = subject.DistinctUntilChanged();
            subject.Subscribe(
                i => Console.WriteLine("{0}", i),
                () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(
                i => Console.WriteLine("distinct.OnNext({0})", i),
                () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
            Console.ReadLine();
        }

    }
}
