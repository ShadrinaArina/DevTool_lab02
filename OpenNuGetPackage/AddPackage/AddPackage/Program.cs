using PowerCollections;

//создаю стек без размера
PowerCollections.Stack<int> stack = new ();
for (int i = 0; i < stack.Capacity; i++) {
    stack.Push(i);
}


for (int i = 0; i < stack.Capacity; i++)
{
    Console.WriteLine (stack.Pop ());
}

//создаю стек с размером
PowerCollections.Stack<int> stack2 = new PowerCollections.Stack<int>(50);
for (int i = 0; i < stack2.Capacity; i++)
{
    stack2.Push(i);
}


for (int i = 0; i < stack2.Capacity; i++)
{
    Console.WriteLine(stack2.Pop());
}

