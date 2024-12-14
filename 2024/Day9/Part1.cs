using System.Numerics;
using SharedUtils;

namespace Day9;

public class Part1 : ISolution
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

    private string Answer(string input)
    {
        var blocks = FillBlocks(input);
        DefragmentDrive(blocks);
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
            var block = new Block(-1, false);

            if (index % 2 == 0)
            {
                block.Id = count;
                block.IsFile = true;

                count++;
            }

            for (var i = 0; i < blockSize; i++)
            {
                blocks.Add(block);
            }
        }

        return blocks;
    }

    private static void DefragmentDrive(List<Block> blocks)
    {
        for (var blockIndex = 0; blockIndex < blocks.Count; blockIndex++)
        {
            var block = blocks[blockIndex];

            if(block.Id != -1)
            {
                continue;
            }

            var lastIndex = blocks.FindLastIndex(b => b.Id != -1);
            var lastBlock = blocks[lastIndex];

            if(blockIndex > lastIndex)
            {
                break;
            }

            blocks[blockIndex] = lastBlock;
            blocks[lastIndex] = block;
        }
    }

    private class File(int id, int[] blockIndexes)
    {
        public readonly int Id = id;
        public int[] BlockIndexes = blockIndexes;

        public int CalculateChecksum()
        {
            return BlockIndexes.Sum(block => Id * block);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Size: {BlockIndexes.Length}";
        }
    }
    private class Block(int id, bool isFile = true)
    {
        public int Id = id;
        public bool IsFile = true;

        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }
}