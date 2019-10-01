using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ExploreCsharpEight
{
// static local function 
// capturing and preventing the release of resources can impact performance.Declaring local functions as static ensures this doesn't happen.
// "A static local function can't contain a reference to <variable>."
    class StaticLocalFunctions
    {
        public IEnumerable<int> Counter(int start, int end)
        {
            if (start >= end) throw new ArgumentOutOfRangeException("start must be less than end");

            return localCounter();

            IEnumerable<int> localCounter()
            {
                for (int i = start; i < end; i++)
                    yield return i;
            }
            // static IEnumerable<int> localCounter(int first, int endLocation)
            // {
            //     for (int i = first; i < endLocation; i++)
            //         yield return i;
            // }
        }
    }

//disposable ref structs and using declarations 
//the variable being declared should be disposed at the end of the enclosing scope
//the resources are disposed at the end of the block.
    internal class ResourceHog : IDisposable
    {
        private string name;
        private bool beenDisposed;

        public ResourceHog(string name) => this.name = name;

        public void Dispose()
        {
            beenDisposed = true;
            Console.WriteLine($"Disposing {name}");
        }

        internal void CopyFrom(ResourceHog src)
        {
            switch (beenDisposed, src.beenDisposed)
            {
                case (true, true): throw new ObjectDisposedException($"Resource {name} has already been disposed");
                case (true, false): throw new ObjectDisposedException($"Resource {name} has already been disposed");
                case (false, true): throw new ObjectDisposedException($"Resource {name} has already been disposed");
                default: Console.WriteLine($"Copying from {src.name} to {name}"); return;
            };

        }
    }
    internal class UsingDeclarationsRefStruct
    {
        internal int OldStyle()
        {
            #region Using_Block
            using (var src = new ResourceHog("source"))
            {
                using (var dest = new ResourceHog("destination"))
                {
                    dest.CopyFrom(src);
                }
                Console.WriteLine("After closing destination block");
            }
            Console.WriteLine("After closing source block");
            #endregion
            return 0;
        }
        internal int NewStyle()
        {
            #region Using_Declaration
            using var src = new ResourceHog("source");
            using var dest = new ResourceHog("destination");
            dest.CopyFrom(src);
            Console.WriteLine("Exiting block");
            #endregion
            return 0;
        }
    }
    

// Nullable reference types
    internal class Person
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public Person(string first, string last) =>
            (FirstName, LastName) = (first, last);

        public Person(string first, string middle, string last) =>
            (FirstName, MiddleName, LastName) = (first, middle, last);

        public override string ToString() => $"{FirstName} {MiddleName} {LastName}";
    }
    class NullableReferences
    {
    internal int NullableTestBed()
        {
            #region Nullable_Usage
            #nullable enable
            Person miguel = new Person("Miguel", "de Icaza");
            var length = GetLengthOfMiddleName(miguel);
            Console.WriteLine(length);
            #nullable restore
            //Was this tested on a person who doesn't have a middle name?
            #endregion
            return 0;
        }
// Unhandled Exception: Unhandled exception: System.NullReferenceException: Object reference not set to an instance of an object.

// Declare nullable property(Null-coalescing assignment)
        private static int GetLengthOfMiddleName(Person p)
        {
            string? middleName = p.MiddleName;
            return middleName?.Length ?? 0;// ?? -> provide a default value
        }
    }

// Asynchronous streams
    class AsyncStreams
    {
        internal async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                // every 3 elements, wait 2 seconds:
                if (i % 3 == 0)
                    await Task.Delay(3000);
                yield return i;
            }
        }
    }

//Indices and ranges
    class IndicesAndRanges
    {
        private string[] words = new string[]
        {               // index from start    index from end
            "The",      // 0                   ^9
            "quick",    // 1                   ^8
            "brown",    // 2                   ^7
            "fox",      // 3                   ^6
            "jumped",   // 4                   ^5
            "over",     // 5                   ^4
            "the",      // 6                   ^3
            "lazy",     // 7                   ^2
            "dog"       // 8                   ^1
        };              // 9 (or words.Length) ^0

        internal int Syntax()
        {
            var last = words[^1]; //contain "dog"
            Index dog = ^1;
            var last1 = words[dog];

            var quickBrownFox = words[1..4]; // contains "quick", "brown" and "fox".
            Range phrase = 1..4;
            var quickBrownFox1 = words[phrase];
            var allWords = words[..]; // contains "The" through "dog".
            var firstPhrase = words[..4]; // contains "The" through "fox"
            var lastPhrase = words[6..]; // contains "the, "lazy" and "dog"

            var lazyDog = words[^2..^0]; // contains "lazy" and "dog".

            // 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 ........85 86 87 88 89 90 91 92 93 94 95 96 97 98 99
            var numbers = Enumerable.Range(0, 100).ToArray();
            int x = 12;
            var x_x = numbers[x..^x];//Remove elements from both ends. 12-87
            return 0;
        }

    }
//Unmanaged constructed types
//stackalloc in nested expressions
//Enhancement of interpolated verbatim strings

}