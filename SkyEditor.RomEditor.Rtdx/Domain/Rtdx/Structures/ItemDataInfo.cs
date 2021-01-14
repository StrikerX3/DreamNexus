using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IItemDataInfo
    {
        public IList<ItemDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ItemDataInfo : IItemDataInfo
    {
        public const int EntrySize = 0x64;

        public ItemDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public ItemDataInfo(byte[] data)
        {
            var entries = new List<Entry>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add(new Entry(data.AsSpan(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public byte[] ToByteArray()
        {
            IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize * Entries.Count]);
            int currentIndex = 0;
            foreach (var entry in Entries)
            {
                data.Write(currentIndex, entry.ToBytes());
                currentIndex += EntrySize;
            }
            return data.ReadArray();
        }

        public IList<Entry> Entries { get; }

        [DebuggerDisplay("{Name}")]
        public class Entry
        {
            public Entry()
            {
                Symbol = "";
            }

            public Entry(Span<byte> data)
            {
                Short00 = MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                Short02 = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                Short04 = MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                PrimaryActIndex = MemoryMarshal.Read<ushort>(data.Slice(0x0C, sizeof(ushort)));
                Short00 = MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                Short02 = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                Short04 = MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                BuyPrice = MemoryMarshal.Read<ushort>(data.Slice(0x06, sizeof(ushort)));
                SellPrice = MemoryMarshal.Read<ushort>(data.Slice(0x08, sizeof(ushort)));
                TaughtMove = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x0A, sizeof(ushort)));
                Short0C = MemoryMarshal.Read<ushort>(data.Slice(0x0C, sizeof(ushort)));
                PrimaryActIndex = MemoryMarshal.Read<ushort>(data.Slice(0x0E, sizeof(ushort)));
                ReviveActIndex = MemoryMarshal.Read<ushort>(data.Slice(0x10, sizeof(ushort)));

                ThrowActIndex = MemoryMarshal.Read<ushort>(data.Slice(0x14, sizeof(ushort)));
                ItemKind = (ItemKind)data[0x16];
                Byte17 = data[0x17];
                Byte18 = data[0x18];
                Byte19 = data[0x19];
                Byte1A = data[0x1A];
                Byte1B = data[0x1B];
                Byte1C = data[0x1C];
                Byte1D = data[0x1D];
                Byte1E = data[0x1E];
                Byte1F = data[0x1F];
                Byte20 = data[0x20];
                Byte21 = data[0x21];
                Symbol = Encoding.ASCII.GetString(data.Slice(0x22).ToArray()).TrimEnd('\0');
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt16(0x00, Short00);
                data.WriteUInt16(0x00, Short00);
                data.WriteUInt16(0x02, Short02);
                data.WriteUInt16(0x04, Short04);
                data.WriteUInt16(0x06, BuyPrice);
                data.WriteUInt16(0x08, SellPrice);
                data.WriteUInt16(0x0A, (ushort)TaughtMove);
                data.WriteUInt16(0x0C, Short0C);
                data.WriteUInt16(0x0E, PrimaryActIndex);
                data.WriteUInt16(0x10, ReviveActIndex);

                data.WriteUInt16(0x14, ThrowActIndex);
                data.Write(0x16, (byte)ItemKind);
                data.Write(0x17, Byte17);
                data.Write(0x18, Byte18);
                data.Write(0x19, Byte19);
                data.Write(0x1A, Byte1A);
                data.Write(0x1B, Byte1B);
                data.Write(0x1C, Byte1C);
                data.Write(0x1D, Byte1D);
                data.Write(0x1E, Byte1E);
                data.Write(0x1F, Byte1F);
                data.Write(0x20, Byte20);
                data.Write(0x21, Byte21);
                data.WriteString(0x22, Encoding.ASCII, Symbol);
                return data.ReadSpan();
            }

            public ushort Short00 { get; set; }
            public ushort Short02 { get; set; }
            public ushort Short04 { get; set; }
            public ushort BuyPrice { get; set; }
            public ushort SellPrice { get; set; }
            public WazaIndex TaughtMove { get; set; }
            public ushort Short0C { get; set; }
            public ushort PrimaryActIndex { get; set; }  // Action taken when used (items) or stepped on (traps)
            public ushort ReviveActIndex { get; set; }  // Action taken while reviving (only Tiny Reviver Seeds and Reviver Seeds)

            public ushort ThrowActIndex { get; set; }  // Action taken when the item is thrown
            public ItemKind ItemKind { get; set; }
            public byte Byte17 { get; set; }
            public byte Byte18 { get; set; }
            public byte Byte19 { get; set; }
            public byte Byte1A { get; set; }
            public byte Byte1B { get; set; }
            public byte Byte1C { get; set; }
            public byte Byte1D { get; set; }
            public byte Byte1E { get; set; }
            public byte Byte1F { get; set; }
            public byte Byte20 { get; set; }
            public byte Byte21 { get; set; }
            public string Symbol { get; set; }
        }
    }
}
