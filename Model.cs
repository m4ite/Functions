using System;
using System.Linq;
using System.Collections.Generic;



public static class FunctionUtil
{
  private static Linear linear = null;
  public static Function x
  {
    get
    {
      if (linear is null)
        linear = new Linear();

      return linear;
    }
  }

  private static Constant euler = null;
  public static Function e
  {
    get
    {
      if (euler is null)
        euler = new Constant(Math.E);

      return euler;
    }
  }

  public static Function sin(Function f)
  {
    Sin sin = new Sin(f);
    return sin;
  }

  public static Function cos(Function f)
  {
    Cos cos = new Cos(f);
    return cos;
  }

  public static Function ln(Function f)
  {
    Ln ln = new Ln(f);
    return ln;
  }
}


public abstract class Function
{
  public double this[double x] => get(x);

  protected abstract double get(double x);
  public abstract Function Derive();

  public static Function operator +(Function f, double n)
  {
    Sum sum = new Sum();
    sum.Add(f);
    sum.Add(new Constant(n));
    return sum;
  }
  
  public static Function operator +(Function f, Function g)
  {
    Sum sum = new Sum();
    sum.Add(f);
    sum.Add(g);
    return sum;
  }
  
  public static Function operator *(Function f, Function g)
  {
    Mult mult = new Mult();
    mult.Add(f);
    mult.Add(g);
    return mult;
  }

  public static Function operator /(Function f, Function g)
  {
    Divide divide = new Divide();
    divide.Add(f);
    divide.Add(g);
    return divide;
  }

  public static Function operator ^(Function f, Function g)
  {
    Pow pow = new Pow();
    pow.Add(f);
    pow.Add(g);
    return pow;
  }
}

public class Linear : Function
{
  protected override double get(double x) => x;
  
  public override Function Derive()
    => new Constant(1);

  public override string ToString() => "x";


  
}

public class Constant : Function
{
  private double valor;

  public Constant(double valor)
    => this.valor = valor;

  protected override double get(double x) => this.valor;
  
  public override Function Derive()
    => new Constant(0);
  
  public override string ToString() => this.valor.ToString();
}

public class Sin : Function
{
  private Function inner;
  public Sin(Function inner)
    => this.inner = inner;
  
  protected override double get(double x) 
    => Math.Sin(inner[x]);
  
  public override Function Derive()
    => null;  //Math.Cos(inner[x]) 
  
  public override string ToString() 
    => $"sin({inner})";
}

public class Cos : Function
{
  private Function inner;
  public Cos(Function inner)
    => this.inner = inner;
  
  protected override double get(double x) 
    => Math.Cos(inner[x]);
  
  public override Function Derive()
    => null; //Math.Sin(inner[x]) 
  
  public override string ToString() 
    => $"cos({inner})";
}

public class Ln : Function
{
  private Function inner;
  public Ln(Function inner)
    => this.inner = inner;
  
  protected override double get(double x) 
    => Math.Log(inner[x], FunctionUtil.e[x]);
  
  public override Function Derive()
    => inner.Derive() / inner;
  
  public override string ToString() 
    => $"ln({inner})";
}

public class Sum : Function
{
  private List<Function> funcs = new List<Function>();
  public void Add(Function func)
    => this.funcs.Add(func);

  protected override double get(double x)
    => this.funcs.Sum(f => f[x]);
  
  public override Function Derive()
  {
    Sum result = new Sum();
    
    foreach (var f in this.funcs)
      result.Add(f.Derive());
    
    return result;
  }
  
  public override string ToString()
  {
    string str = "(";

    foreach (var f in this.funcs)
      str += f.ToString() + " + ";

    return str.Substring(0, str.Length - 3) + ")";
  }
}

public class Mult : Function
{
  private List<Function> funcs = new List<Function>();
  public void Add(Function func)
    => this.funcs.Add(func);

  protected override double get(double x)
  {
    double result = 1;
    
    foreach (var f in this.funcs)
      result *= f[x];
      
    return result;
  }
  
  public override Function Derive()
  {
    return null;
  }
  
  public override string ToString()
  {
    string str = "";

    foreach (var f in this.funcs)
      str += f.ToString() + " * ";

    return str.Substring(0, str.Length - 3);
  }
}

public class Divide : Function
{
  private List<Function> funcs = new List<Function>();
  public void Add(Function func)
    => this.funcs.Add(func);

  protected override double get(double x)
  {
    double result = 1;
    
    foreach (var f in this.funcs)
      result /= f[x];
      
    return result;
  }

  public override Function Derive()
  {
    return null;
  }
  
  public override string ToString()
  {
    string str = "";

    foreach (var f in this.funcs)
      str += f.ToString() + " / ";

    return str.Substring(0, str.Length - 3);
  }
}

public class Pow : Function
{
  private List<Function> funcs = new List<Function>();
  public void Add(Function func)
    => this.funcs.Add(func);

  protected override double get(double x)
  {
    double result = 1;
    
    for (int i = 0; i < this.funcs.Count-1; i++)
    {
      var b = this.funcs[i];
      var p = this.funcs[i+1];
      Math.Pow(b[x], p[x]);
    }     
      
    return result;
  }

  public override Function Derive()
  {
    return null;
  }
  
  public override string ToString()
  {
    string str = "";

    foreach (var f in this.funcs)
      str += f.ToString() + " ^ ";

    return str.Substring(0, str.Length - 3);
  }
}