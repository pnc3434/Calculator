using System;
class Calculator
{
    private double memory = 0;
    private double currentValue = 0;
    public void MemoryAdd() => memory += currentValue;
    public void MemorySubtract() => memory -= currentValue;
    public double MemoryRecall() => memory;
    public void ClearMemory() => memory = 0;
    public double GetCurrentValue() => currentValue;
    public void SetCurrentValue(double value) => currentValue = value;
}
class Program
{
    static void Main()
    {
        Calculator calc = new Calculator();
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Текущее значение: {calc.GetCurrentValue()}");
            Console.WriteLine($"Память (M): {calc.MemoryRecall()}");
            Console.WriteLine("Доступные операции: +, -, *, /, %, 1/x, x^2, sqrt, M+, M-, MR, MC, C, ex");
            Console.Write("Введите выражение или операцию: ");
            string input = Console.ReadLine();
            if (input.ToLower() == "ex") break;
            if (string.IsNullOrWhiteSpace(input)) continue;
            try
            {
                if (IsUnaryOperation(input)) HandleUnaryOperation(input, calc);
                else if (IsMemoryOperation(input)) HandleMemoryOperation(input, calc);
                else HandleBinaryOperation(input, calc);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Неверный формат чисел!");
                Console.ReadKey();
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: Число слишком большое или слишком маленькое!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
    static bool IsUnaryOperation(string input)
    {
        string[] unaryOps = { "1/x", "x^2", "sqrt", "C" };
        return Array.Exists(unaryOps, op => input.Trim().Equals(op, StringComparison.OrdinalIgnoreCase));
    }
    static bool IsMemoryOperation(string input)
    {
        string[] memoryOps = { "M+", "M-", "MR", "MC" };
        return Array.Exists(memoryOps, op => input.Trim().Equals(op, StringComparison.OrdinalIgnoreCase));
    }
    static void HandleUnaryOperation(string operation, Calculator calc)
    {
        double originalValue = calc.GetCurrentValue();

        switch (operation.ToLower())
        {
            case "1/x":
                if (originalValue == 0)
                {
                    Console.WriteLine("Ошибка: Деление на ноль!");
                }
                else
                {
                    double result = 1 / originalValue;
                    calc.SetCurrentValue(result);
                    Console.WriteLine($"1/{originalValue} = {result}");
                }
                break;
            case "x^2":
                double squared = originalValue * originalValue;
                calc.SetCurrentValue(squared);
                Console.WriteLine($"{originalValue}² = {squared}");
                break;
            case "sqrt":
                if (originalValue < 0)
                {
                    Console.WriteLine("Ошибка: Корень из отрицательного числа!");
                }
                else
                {
                    double sqrt = Math.Sqrt(originalValue);
                    calc.SetCurrentValue(sqrt);
                    Console.WriteLine($"√{originalValue} = {sqrt}");
                }
                break;
            case "c":
                calc.SetCurrentValue(0);
                Console.WriteLine("Калькулятор очищен");
                break;
        }
        Console.ReadKey();
    }
    static void HandleMemoryOperation(string operation, Calculator calc)
    {
        switch (operation.ToUpper())
        {
            case "M+":
                calc.MemoryAdd();
                Console.WriteLine($"Добавлено в память: {calc.GetCurrentValue()}");
                break;
            case "M-":
                calc.MemorySubtract();
                Console.WriteLine($"Вычтено из памяти: {calc.GetCurrentValue()}");
                break;
            case "MR":
                double memoryValue = calc.MemoryRecall();
                calc.SetCurrentValue(memoryValue);
                Console.WriteLine($"Восстановлено из памяти: {memoryValue}");
                break;
            case "MC":
                calc.ClearMemory();
                Console.WriteLine("Память очищена");
                break;
        }
        Console.ReadKey();
    }
    static void HandleBinaryOperation(string input, Calculator calc)
    {
        int operatorPos = -1;
        char[] operators = { '+', '-', '*', '/', '%' };
        foreach (char op in operators)
        {
            int pos = input.IndexOf(op);
            if (pos > 0)
            {
                operatorPos = pos;
                break;
            }
        }
        if (operatorPos == -1)
        {
            Console.WriteLine("Ошибка: Не найден оператор (+, -, *, /, %)");
            Console.ReadKey();
            return;
        }
        string num1Str = input.Substring(0, operatorPos).Trim();
        char operation = input[operatorPos];
        string num2Str = input.Substring(operatorPos + 1).Trim();
        if (string.IsNullOrEmpty(num1Str) || string.IsNullOrEmpty(num2Str))
        {
            Console.WriteLine("Ошибка: Неверный формат выражения");
            Console.ReadKey();
            return;
        }
        double num1 = string.IsNullOrEmpty(num1Str) ? calc.GetCurrentValue() : Convert.ToDouble(num1Str);
        double num2 = Convert.ToDouble(num2Str);
        double result = 0;
        switch (operation)
        {
            case '+':
                result = num1 + num2;
                break;
            case '-':
                result = num1 - num2;
                break;
            case '*':
                result = num1 * num2;
                break;
            case '/':
                if (num2 == 0)
                {
                    Console.WriteLine("Ошибка: Деление на ноль!");
                    Console.ReadKey();
                    return;
                }
                result = num1 / num2;
                break;
            case '%':
                if (num2 == 0)
                {
                    Console.WriteLine("Ошибка: Деление на ноль!");
                    Console.ReadKey();
                    return;
                }
                result = num1 % num2;
                break;
        }
        calc.SetCurrentValue(result);
        Console.WriteLine($"{num1} {operation} {num2} = {result}");
        Console.ReadKey();
    }
}