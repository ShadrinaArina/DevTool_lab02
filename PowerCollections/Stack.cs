using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PowerCollections
{
    public class Stack<T> : IEnumerable
    {
        T[] stack;
        int count = -1; // кол-во элементов. -1 - означает отсутствие элементов
        int size; // размер стека

        public int Count { get { return count + 1; } }
        public int Capacity { get { return size; } }

        //конструктор
        public Stack(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException();

            this.size = size;
            stack = new T[size];
        }

        public Stack()
        {
            this.size = 100;
            stack = new T[size];
        }

        //добавляет на вершину стека
        public void Push(T item)
        {
            if (count + 1 == size)
                throw new InvalidOperationException("Стек полон");
            stack[++count] = item;
        }

        //снять элемент последний и вернуть его значения
        public T Pop()
        {
            if (count == -1)
                throw new InvalidOperationException("Стек пуст");
            return stack[count--];
        }

        //получить элемент последний и вернуть его значения
        public T Top()
        {
            if (count == -1)
                throw new InvalidOperationException("Стек пуст");
            return stack[count];
        }

        //реализация интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            StackEnum<T> stackEnum = new StackEnum<T>(stack, count + 1);
            return (IEnumerator)stackEnum;
        
    }

    //реализация интерфейса IEnumerator
    public class StackEnum<T> : IEnumerator
    {
        public T[] _stack;

        int position = -1;
        public StackEnum(T[] stack, int count)
        {
            _stack = new T[count];
            for (int i = count - 1; i >= 0; i--)
                _stack[i] = stack[i];
        }

        public bool MoveNext()
        {
            position++;
            return position < _stack.Length;
        }

        public void Reset()
        {
            position = -1;
        }
        object IEnumerator.Current
        {
            get
            {
                try
                {
                    return _stack[position];
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
