using System;
using static FunctionUtil;

class Program {
  public static void Main (string[] args)
  {
    // var f = sin(x) * sin(x) + cos(x) / cos(x) + e;
    // var f = log(x);
    var f = sin(x) ^ sin(x);


    // ---- Calcula a função de x ----
    Console.WriteLine(f); // fórmula
    Console.WriteLine(f[0]); //resultado
    
  }
}