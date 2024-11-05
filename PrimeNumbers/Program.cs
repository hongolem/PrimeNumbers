using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

int limit = 10000000;

static bool IsPrime(int number)
{
    if (number < 2) return false;

    int boundary = (int)Math.Sqrt(number);

    for (int i = 2; i <= boundary; i++)
    {
        if (number % i == 0)
        {
            return false;
        }
    }

    return true;
}

static List<int> ClassicGeneratePrimes(int limit)
{
    List<int> primes = new List<int>();

    for (int number = 2; number <= limit; number++)
    {
        if (IsPrime(number))
        {
            primes.Add(number);
        }
    }

    return primes;
}

static List<int> ClassicGeneratePrimesParallel(int maxNumber)
{
    ConcurrentBag<int> primeNumbers = [];
    Parallel.For(2, maxNumber, x => {
        if (IsPrime(x)) primeNumbers.Add(x);
    });

    return primeNumbers.ToList();
}

static List<int> EratosthenosGeneratePrimes(int limit)
{
    // Pokud je limit menší než 2, není žádné prvočíslo
    if (limit < 2) return new List<int>();

    // Vytvoříme pole bool, kde indexy odpovídají číslům od 0 do limit
    // True znamená, že číslo je považováno za prvočíslo
    bool[] isPrime = new bool[limit + 1];
    for (int i = 2; i <= limit; i++)
    {
        isPrime[i] = true;
    }

    // Pro každé číslo od 2 do odmocniny z limitu
    for (int i = 2; i * i <= limit; i++)
    {
        if (isPrime[i])
        {
            // Označíme všechny násobky i jako složená čísla (ne-prvočísla)
            for (int j = i * i; j <= limit; j += i)
            {
                isPrime[j] = false;
            }
        }
    }

    // Seznam prvočísel
    List<int> primes = new List<int>();
    for (int i = 2; i <= limit; i++)
    {
        if (isPrime[i])
        {
            primes.Add(i);
        }
    }

    return primes;
}

static List<int> EratosthenosGeneratePrimesParallel(int maxNumber)
{
    var numBools = new bool[maxNumber];
    numBools[0] = numBools[1] = true;
    Parallel.For(2, Convert.ToInt32(Math.Sqrt(maxNumber)), i => {
        if (numBools[i]) return;
        for (int j = 2 * i; j < maxNumber; j += i)
            numBools[j] = true;
    });

    List<int> primeNumbers = new();
    for (int i = 2; i < numBools.Length; i++)
    {
        if (numBools[i] == false)
            primeNumbers.Add(i);
    }

    return primeNumbers;
}

Stopwatch stopwatch1 = Stopwatch.StartNew();
List<int> Cprimes = ClassicGeneratePrimes(limit);
stopwatch1.Stop();
Console.WriteLine("Proces trval " + stopwatch1.ElapsedMilliseconds + " ms");

Stopwatch stopwatch2 = Stopwatch.StartNew();
List<int> Eprimes = EratosthenosGeneratePrimes(limit);
stopwatch2.Stop();
Console.WriteLine("Proces trval " + stopwatch2.ElapsedMilliseconds + " ms");

Stopwatch stopwatch3 = Stopwatch.StartNew();
List<int> CEprimes = ClassicGeneratePrimesParallel(limit);
stopwatch3.Stop();
Console.WriteLine("Proces trval " + stopwatch3.ElapsedMilliseconds + " ms");

Stopwatch stopwatch4 = Stopwatch.StartNew();
List<int> EPprimes = EratosthenosGeneratePrimesParallel(limit);
stopwatch4.Stop();
Console.WriteLine("Proces trval " + stopwatch4.ElapsedMilliseconds + " ms");