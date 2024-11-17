using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BenchmarkExample;

public class StringReverseBenchmark
{
    private readonly int[] intArray = GenerateRandomArray(50, 1, 1000);

    [Benchmark]
    public void FactorialIterative()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            FactorialCalculator.FactorialIterative(10);

            i += 1;
        }
    }

    [Benchmark]
    public void FactorialRecursive()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            FactorialCalculator.FactorialRecursive(10);

            i += 1;
        }
    }

    [Benchmark]
    public void FibonacciIterative()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            FibonacciCalculator.FibonacciIterative(100);

            i += 1;
        }
    }

    [Benchmark]
    public void FibonacciRecursive()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            FibonacciCalculator.FibonacciRecursive(10);

            i += 1;
        }
    }

    [Benchmark]
    public void QuickSortIterative()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            QuickSortAlgorithm.QuickSortIterative(intArray);

            i += 1;
        }
    }

    [Benchmark]
    public void QuickSortRecursive()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            QuickSortAlgorithm.QuickSortRecursive(intArray, 0, intArray.Length - 1);

            i += 1;
        }
    }

    private static int[] GenerateRandomArray(int size, int minValue, int maxValue)
    {
        var random = new Random();
        int[] array = new int[size];

        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next(minValue, maxValue + 1);
        }

        return array;
    }

    [Benchmark]
    public void SolveHanoiIterative()
    {
        var counter = 100;
        var i = 0;

        while (i < counter)
        {
            TowersOfHanoi.SolveHanoiIterative(5);

            i += 1;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddExporter(HtmlExporter.Default)
            .AddExporter(CsvExporter.Default)
            .AddExporter(RPlotExporter.Default); // Dodaje eksport wykresów

        var summary = BenchmarkRunner.Run<StringReverseBenchmark>(config);
    }
}

#region Factorial
class FactorialCalculator
{
    public static int FactorialRecursive(int n)
    {
        if (n <= 1) return 1;
        return n * FactorialRecursive(n - 1);
    }

    public static int FactorialIterative(int n)
    {
        int result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
}
#endregion

#region Fibonacci
class FibonacciCalculator
{
    public static int FibonacciRecursive(int n)
    {
        if (n <= 1) return n;
        return FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2);
    }

    public static int FibonacciIterative(int n)
    {
        if (n <= 1) return n;

        int prev = 0;
        int curr = 1;

        for (int i = 2; i <= n; i++)
        {
            int temp = curr;
            curr = prev + curr;
            prev = temp;
        }

        return curr;
    }
}
#endregion

#region QuickSort
class QuickSortAlgorithm
{
    public static void QuickSortRecursive(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(array, left, right);
            QuickSortRecursive(array, left, pivotIndex - 1);
            QuickSortRecursive(array, pivotIndex + 1, right);
        }
    }

    public static void QuickSortIterative(int[] array)
    {
        int left = 0;
        int right = array.Length - 1;
        
        var stack = new System.Collections.Generic.Stack<(int left, int right)>();
        stack.Push((left, right));

        while (stack.Count > 0)
        {
            var (start, end) = stack.Pop();

            if (start < end)
            {
                int pivotIndex = Partition(array, start, end);
                
                stack.Push((start, pivotIndex - 1));
                stack.Push((pivotIndex + 1, end));
            }
        }
    }

    private static int Partition(int[] array, int left, int right)
    {
        int pivot = array[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }
        Swap(array, i + 1, right);
        return i + 1;
    }

    private static void Swap(int[] array, int a, int b)
    {
        int temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }
}
#endregion

#region Hanoi
class TowersOfHanoi
{
    public static void SolveHanoiIterative(int n)
    {
        char source = 'A';
        char target = 'C';
        char auxiliary = 'B';

        if (n % 2 == 0)
        {
            char temp = target;
            target = auxiliary;
            auxiliary = temp;
        }

        int totalMoves = (int)Math.Pow(2, n) - 1;

        for (int i = 1; i <= totalMoves; i++)
        {
            int disk = GetDiskNumber(i);
            char fromPeg = GetFromPeg(i, n, source, target, auxiliary);
            char toPeg = GetToPeg(i, n, source, target, auxiliary);

            Console.WriteLine($"Przenieś dysk {disk} z wieży {fromPeg} na wieżę {toPeg}");
        }
    }

    private static int GetDiskNumber(int move)
    {
        int disk = 1;
        while (move % 2 == 0)
        {
            move /= 2;
            disk++;
        }
        return disk;
    }

    private static char GetFromPeg(int move, int n, char source, char target, char auxiliary)
    {
        return GetPeg(move, source, target, auxiliary);
    }

    private static char GetToPeg(int move, int n, char source, char target, char auxiliary)
    {
        return GetPeg(move, target, auxiliary, source);
    }

    private static char GetPeg(int move, char peg1, char peg2, char peg3)
    {
        switch ((move / 2) % 3)
        {
            case 0: return peg1;
            case 1: return peg2;
            case 2: return peg3;
            default: return peg1;
        }
    }
}
#endregion