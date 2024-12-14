using System.Numerics;
using SharedUtils;

namespace Day9;

public class Part2 : ISolution
{
    public string Sample()
    {
        return Custom("sample");
    }

    public string Real()
    {
        return Custom("real");
    }

    public string Custom(string fileName)
    {
        var fileInput = Input.ReadSingleLine(fileName);
        return Answer(fileInput);
    }

    private static string Answer(string input)
    {
        var blocks = FillBlocks(input);
        blocks = DefragmentDrive(blocks);

        RenderMemory(blocks.ToArray());

        var checksum = CalculateChecksum(blocks);
        return checksum.ToString();
    }

    private static BigInteger CalculateChecksum(List<Block> blocks)
    {
        // Calculate Checksum
        BigInteger checksum = 0;
        for (var blockIndex = 0; blockIndex < blocks.Count; blockIndex++)
        {
            var block = blocks[blockIndex];

            if(block.Id == -1) continue;

            checksum += blockIndex * block.Id;
        }

        return checksum;
    }

    private static List<Block> FillBlocks(string input)
    {
        var blocks = new List<Block>();
        var count = 0;

        for (var index = 0; index < input.Length; index++)
        {
            var character = input[index];
            var blockSize = int.Parse(character.ToString());
            var block = new Block(-1);

            if (index % 2 == 0)
            {
                block.Id = count;

                count++;
            }

            for (var i = 0; i < blockSize; i++)
            {
                blocks.Add(block);
            }
        }

        return blocks;
    }

    private static List<Block> DefragmentDrive(List<Block> blocks)
    {
        // Break down the blocks into a list of chunks of block ids with an ID of -1
        var chunkedBlocks = ChunkDiskBlocks(blocks);

        var blocksToMove = chunkedBlocks
            .Where(t => t.Item1 != -1)
            .OrderByDescending(b => b.Item1)
            .ToList();

        var freeBlocks = chunkedBlocks.Where(t => t.Item1 == -1).ToList();

        foreach (var (id, blockList) in blocksToMove)
        {
            Console.WriteLine("Moving block: " + id);
            var freeBlockOfSuitableSize = freeBlocks.FirstOrDefault(t => t.Item2.Count >= blockList.Count);
            if(freeBlockOfSuitableSize == default)
            {
                Console.WriteLine($"Unable to find block for {id} block. Block size: {blockList.Count}");
                continue;
            }

            for (var i = 0; i < blockList.Count; i++)
            {
                blocks[freeBlockOfSuitableSize.Item2[i]] = blocks[blockList[i]];
                blocks[blockList[i]] = new Block(-1);
            }

            freeBlockOfSuitableSize.Item2.RemoveRange(0, blockList.Count);
            RenderMemory(blocks.ToArray());
        }
        return blocks;
    }

    private static List<(int, List<int>)> ChunkDiskBlocks(List<Block> blocks)
    {
        var chunkedBlocks = new List<(int,List<int>)>();
        var currentChunk = new List<int> { 0 };

        for (var index = 1; index < blocks.Count; index++)
        {
            var block = blocks[index];
            var lastBlock = blocks[index - 1];

            if(block.Id != lastBlock.Id)
            {
                chunkedBlocks.Add((lastBlock.Id, currentChunk));
                currentChunk = [];
            }

            currentChunk.Add(index);
        }
        chunkedBlocks.Add((blocks[currentChunk.Last()].Id, currentChunk));
        return chunkedBlocks;
    }

    private static void RenderMemory(Block[] memoryBlocks)
    {
        Console.SetCursorPosition(0,0);
        var maxWidth = Console.WindowWidth;

        for (var i = 0; i < memoryBlocks.Length; i++)
        {
            var block = memoryBlocks[i];

            var isFreeMemory = block.Id == -1;

            Console.ForegroundColor = isFreeMemory ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(isFreeMemory ? "." : "#");

            if(i % maxWidth == 0)
            {
                Console.WriteLine();
            }
        }
    }


    private class Block(int id)
    {
        public int Id = id;

        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }
}