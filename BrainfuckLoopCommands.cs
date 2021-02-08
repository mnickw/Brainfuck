using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			Dictionary<int, int> loopsByStart = new Dictionary<int, int>();
			Stack<int> currentLoopsByStart = new Stack<int>();
			vm.RegisterCommand('[', b =>
			{
				if (loopsByStart.Count == 0)
				{
					Stack<int> startOfLoops = new Stack<int>();
					startOfLoops.Push(b.InstructionPointer);
					for (int tempIndex = b.InstructionPointer + 1; tempIndex != b.Instructions.Length; tempIndex++)
					{
						if (b.Instructions[tempIndex] == '[')
							startOfLoops.Push(tempIndex);
						else if (b.Instructions[tempIndex] == ']')
							loopsByStart[startOfLoops.Pop()] = tempIndex;
					}
				}
				if (b.Memory[b.MemoryPointer] == 0)
					b.InstructionPointer = loopsByStart[b.InstructionPointer];
				else
					currentLoopsByStart.Push(b.InstructionPointer);
			});
			vm.RegisterCommand(']', b =>
			{
				if (b.Memory[b.MemoryPointer] != 0)
					b.InstructionPointer = currentLoopsByStart.Peek();
				else
					currentLoopsByStart.Pop();
			});
		}
	}
}