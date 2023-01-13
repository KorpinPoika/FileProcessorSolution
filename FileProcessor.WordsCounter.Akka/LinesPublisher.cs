using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Akka;
using Akka.Actor;
using Akka.Dispatch;
using Akka.Streams.Actors;

namespace FileProcessor.WordsCounter.Akkas
{
    public class LinesPublisher: ActorPublisher<string>
    {
        private readonly string _filePath;
        private IEnumerator<string> _enumerator = null;

        public static Props Props(string filePath) => Akka.Actor.Props.Create(
            () => new LinesPublisher(filePath)     
        )
       .WithDeploy(Deploy.Local);

        public LinesPublisher(string filePath)
        {
            _filePath = filePath;
        }

        protected override void PreStart()
        {
            base.PreStart();
            _enumerator = File.ReadLines(_filePath, Encoding.GetEncoding(1251)).GetEnumerator();
        }

        protected override void PostStop()
        {
            base.PostStop();
            _enumerator?.Dispose();
        }

        protected override bool Receive(object message)
        {
            if (message is Request request)
            {
                ActorTaskScheduler.RunTask(() => ReadLinesAsync(request.Count));
        
                return true;
            }
        
            return false;
        }
        
        // protected override bool Receive(object message)        
        //     => message.Match().With<Request>(
        //             req => ActorTaskScheduler.RunTask(() => ReadLinesAsync(req.Count))
        //        )
        //       .With<Cancel>(c => OnCompleteThenStop())
        //       .WasHandled;

        private void ReadLinesAsync(long count)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    if (_enumerator.MoveNext())
                    {
                        OnNext(_enumerator.Current);
                    }
                    else
                    {
                        OnCompleteThenStop();
                    }
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }
    }
}