using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace PowerCollections.Tests
{
    [TestClass]
    public class StackTests
    {
        [TestMethod]
        public void TestConstructorAndProperties()
        {
            int size = 15;
            Stack<int> stack = new Stack<int>(size);

            Assert.AreEqual(size, stack.Capacity);
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow(0)]
        [DataRow(-5)]
        public void TestConstructor_ArgumentOutOfRangeException(int size)
        {
            Stack<int> stack = new Stack<int>(size);
        }

        [TestMethod]
        public void TestPush()
        {
            Stack<int> stack = new Stack<int>(4);

            stack.Push(2);

            Assert.AreEqual(2, stack.Top());

            stack.Push(0);
            stack.Push(1);
            stack.Push(9);

            Assert.AreEqual(0, stack.Top());
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPush_Exception_IfFullStack()
        {
            Stack<int> stack = new Stack<int>(3);

            stack.Push(4);
            stack.Push(3);
            stack.Push(0);
            stack.Push(7);
        }

        [TestMethod]
        public void TestPop()
        {
            Stack<string> stack = new Stack<string>(3);

            stack.Push("hello");
            stack.Push("world");
            stack.Push("!");

            Assert.AreEqual("!", stack.Pop());
            Assert.AreEqual("world", stack.Pop());
            Assert.AreEqual("hello", stack.Pop());

            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPop_InvalidOperationException()
        {
            Stack<string> stack = new Stack<string>(1);

            stack.Pop();
        }

        [TestMethod]
        public void TestTop()
        {
            Stack<double> stack = new Stack<double>(4);

            stack.Push(12.4);
            stack.Push(13.98);
            stack.Push(11);
            stack.Push(28.5);

            Assert.AreEqual(28.5, stack.Top());
            Assert.AreEqual(4, stack.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestTop_Exception_ifStackIsEmpty()
        {
            Stack<string> stack = new Stack<string>(1);

            stack.Top();
        }

        [TestMethod]
        public void TestStackEnumerable()
        {
            int[] values = { 1, 4, 76, 404 };
            Stack<int> stack = new Stack<int>(4);

            foreach (int value in values)
                stack.Push(value);

            int i = 0;
            int[] stackValues = new int[4];
            foreach (int value in stack)
                stackValues[i++] = value;

            CollectionAssert.AreEqual(values, stackValues);
        }

    }
}
